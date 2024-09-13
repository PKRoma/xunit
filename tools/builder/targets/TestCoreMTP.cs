using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(
	BuildTarget.TestCoreMTP,
	BuildTarget.Build
)]
public static class TestCoreMTP
{
	static readonly string refSubPath = Path.DirectorySeparatorChar + "ref" + Path.DirectorySeparatorChar;

	public static async Task OnExecute(BuildContext context)
	{
		await RunTests(context, "net6.0");
		await RunTests(context, "net8.0");
	}

	static async Task RunTests(
		BuildContext context,
		string framework)
	{
		// ------------- AnyCPU -------------

		context.BuildStep($"Running .NET tests ({framework}, AnyCPU, via 'dotnet test' with Microsoft.Testing.Platform)");

		await RunTestAssemblies(context, "dotnet", framework, x86: false);

		// ------------- Forced x86 -------------

		// Only Windows supports side-by-side 64- and 32-bit installs of .NET SDK
		if (!context.NeedMono)
		{
			// Only run 32-bit .NET Core tests if 32-bit .NET Core is installed
			var programFilesX86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
			if (programFilesX86 is not null)
			{

				var x86Dotnet = Path.Combine(programFilesX86, "dotnet", "dotnet.exe");
				if (File.Exists(x86Dotnet))
				{

					context.BuildStep($"Running .NET tests ({framework}, x86, via 'dotnet test' with Microsoft.Testing.Platform)");

					await RunTestAssemblies(context, x86Dotnet, framework, x86: true);
				}
			}
		}

		// Clean out all the 'dotnet test' log files, because if we got this far everything succeeded

		foreach (var logFile in Directory.GetFiles(context.TestOutputFolder, "*.log"))
			File.Delete(logFile);
	}

	static async Task RunTestAssemblies(
		BuildContext context,
		string dotnetPath,
		string framework,
		bool x86)
	{
		var binSubPath = Path.Combine("bin", context.ConfigurationText, framework);
		var testAssemblies =
			Directory
				.GetFiles(context.BaseFolder, "xunit.v3.*.tests.dll", SearchOption.AllDirectories)
				.Where(x => x.Contains(binSubPath) && !x.Contains(refSubPath) && (x.Contains(".x86") == x86))
				.OrderBy(x => x)
				.Select(x => x.Substring(context.BaseFolder.Length + 1));

		foreach (var testAssembly in testAssemblies)
		{
			var outputFileName = $"{Path.GetFileNameWithoutExtension(testAssembly)}-{framework}-{(x86 ? "x86" : "AnyCPU")}-mtp";
			var projectFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(testAssembly))));

			await context.Exec(dotnetPath, $"test {projectFolder} --configuration {context.ConfigurationText} --framework {framework} --no-build --no-restore -- {context.TestFlagsParallelMTP}--pre-enumerate-theories on --results-directory \"{context.TestOutputFolder}\" --report-xunit --report-xunit-filename \"{outputFileName}.xml\" --report-xunit-html --report-xunit-html-filename \"{outputFileName}.html\" --report-ctrf --report-ctrf-filename \"{outputFileName}.ctrf\"", workingDirectory: context.BaseFolder);
		}
	}
}