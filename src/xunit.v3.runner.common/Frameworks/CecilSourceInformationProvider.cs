using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using Xunit.Internal;
using Xunit.Runner.Common;

namespace Xunit;

/// <summary>
/// An implementation of <see cref="ISourceInformationProvider"/> backed by <c>Mono.Cecil</c>.
/// </summary>
public sealed class CecilSourceInformationProvider : ISourceInformationProvider
{
	readonly static HashSet<byte[]> PublicKeyTokensToSkip = new(
	[
		[0x31, 0xbf, 0x38, 0x56, 0xad, 0x36, 0x4e, 0x35],  // Windows
		[0xb0, 0x3f, 0x5f, 0x7f, 0x11, 0xd5, 0x0a, 0x3a],  // .NET Framework
		[0xb7, 0x7a, 0x5c, 0x56, 0x19, 0x34, 0xe0, 0x89],  // .NET
		[0xcc, 0x7b, 0x13, 0xff, 0xcd, 0x2d, 0xdd, 0x51],  // netstandard
		[0x50, 0xce, 0xbf, 0x1c, 0xce, 0xb9, 0xd0, 0x5e],  // Mono
		[0x8d, 0x05, 0xb1, 0xbb, 0x7a, 0x6f, 0xdb, 0x6c],  // xUnit.net
	], ByteArrayComparer.Instance);

	readonly List<ModuleDefinition> moduleDefinitions;
	readonly Dictionary<string, TypeDefinition> typeDefinitions = [];

	CecilSourceInformationProvider(List<ModuleDefinition> moduleDefinitions)
	{
		this.moduleDefinitions = moduleDefinitions;

		foreach (var moduleDefinition in moduleDefinitions)
			foreach (var typeDefinition in moduleDefinition.Types.Where(t => t.IsPublic))
				typeDefinitions[typeDefinition.FullName] = typeDefinition;
	}

	/// <summary>
	/// Creates a source provider for the given test assembly, and any <c>*.dll</c> file that exists
	/// in the same folder.
	/// </summary>
	/// <remarks>
	/// If the symbols are valid and readable, this will return an instance of <see cref="CecilSourceInformationProvider"/>.
	/// If there are no symbols, or the symbols do not match the binary, then this will return an
	/// instance of <see cref="NullSourceInformationProvider"/>.
	/// </remarks>
	/// <param name="assemblyFileName">The test assembly filename</param>
	public static ISourceInformationProvider Create(string? assemblyFileName)
	{
		if (!RunSettingsUtility.CollectSourceInformation)
			return NullSourceInformationProvider.Instance;

		var folder = Path.GetDirectoryName(assemblyFileName);
		if (assemblyFileName is null || folder is null || !Directory.Exists(folder))
			return NullSourceInformationProvider.Instance;

		try
		{
			var symbolProvider = new DefaultSymbolReaderProvider(throwIfNoSymbol: false);
			var moduleDefinitions =
				Directory
					.GetFiles(folder, "*.dll")
					.Concat([assemblyFileName])
					.Distinct()
					.Select(file =>
					{
						try
						{
							if (!File.Exists(file))
								return null;

							var moduleDefinition = ModuleDefinition.ReadModule(file);

							// Exclude non-.NET assemblies
							if (moduleDefinition.Assembly is null)
								return null;

							// Exclude things with known public keys
							var name = moduleDefinition.Assembly.Name;
							if (name.HasPublicKey == true && PublicKeyTokensToSkip.Contains(name.PublicKeyToken))
								return null;

							using var symbolReader = symbolProvider.GetSymbolReader(moduleDefinition, moduleDefinition.FileName);
							if (symbolReader is null)
								return null;

							moduleDefinition.ReadSymbols(symbolReader, throwIfSymbolsAreNotMaching: false);
							if (moduleDefinition.HasSymbols)
								return moduleDefinition;
						}
						catch { }

						return null;
					})
					.WhereNotNull()
					.ToList();

			if (moduleDefinitions.Count != 0)
				return new CecilSourceInformationProvider(moduleDefinitions);
		}
		catch { }

		return NullSourceInformationProvider.Instance;
	}

	/// <inheritdoc/>
	public ValueTask DisposeAsync()
	{
		foreach (var moduleDefinition in moduleDefinitions)
			moduleDefinition.SafeDispose();

		return default;
	}

	/// <inheritdoc/>
	public SourceInformation GetSourceInformation(
		string? testClassName,
		string? testMethodName)
	{
		if (testClassName is null || testMethodName is null)
			return SourceInformation.Null;

		try
		{
			var testClassNamePieces = testClassName.Split('+');

			if (typeDefinitions.TryGetValue(testClassNamePieces[0], out var typeDefinition))
			{
				foreach (var nestedClassName in testClassNamePieces.Skip(1))
				{
					typeDefinition = typeDefinition.NestedTypes.FirstOrDefault(t => t.Name == nestedClassName);
					if (typeDefinition is null)
						return SourceInformation.Null;
				}

				var methodDefinitions = typeDefinition.GetMethods().Where(m => m.Name == testMethodName && m.IsPublic).ToList();
				if (methodDefinitions.Count == 1)
				{
					var debugInformation = typeDefinition.Module.SymbolReader.Read(methodDefinitions[0]);
					// 0xFEEFEE marks a "hidden" line, per https://mono-cecil.narkive.com/gFuvydFp/trouble-with-sequencepoint
					var sequencePoint = debugInformation.SequencePoints.FirstOrDefault(sp => sp.StartLine != 0xFEEFEE);
					if (sequencePoint is not null)
						return new(sequencePoint.Document.Url, sequencePoint.StartLine);
				}
			}
		}
		catch { }

		return SourceInformation.Null;
	}

	sealed class ByteArrayComparer : IEqualityComparer<byte[]>
	{
		public static ByteArrayComparer Instance { get; } = new();

		public bool Equals(byte[]? x, byte[]? y)
		{
			if (x is null)
				return y is null;
			if (y is null)
				return false;
			if (x.Length != y.Length)
				return false;

			return ((IStructuralEquatable)x).Equals(y, EqualityComparer<byte>.Default);
		}

		public int GetHashCode(byte[] obj) =>
			((IStructuralEquatable)obj).GetHashCode(EqualityComparer<byte>.Default);
	}
}
