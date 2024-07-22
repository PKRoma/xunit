using System.Collections.Generic;
using Xunit.Internal;
using Xunit.Sdk;

namespace Xunit.Runner.Common;

public partial class TestClassFinished
{
	/// <inheritdoc/>
	/// <remarks>
	/// Note: Will be <c>0</c> if there was no value provided during deserialization.
	/// </remarks>
	public required decimal ExecutionTime { get; set; }

	/// <inheritdoc/>
	/// <remarks>
	/// Note: Will be <c>0</c> if there was no value provided during deserialization.
	/// </remarks>
	public required int TestsFailed { get; set; }

	/// <inheritdoc/>
	/// <remarks>
	/// Note: Will be <c>0</c> if there was no value provided during deserialization.
	/// </remarks>
	public required int TestsNotRun { get; set; }

	/// <inheritdoc/>
	/// <remarks>
	/// Note: Will be <c>0</c> if there was no value provided during deserialization.
	/// </remarks>
	public required int TestsSkipped { get; set; }

	/// <inheritdoc/>
	/// <remarks>
	/// Note: Will be <c>0</c> if there was no value provided during deserialization.
	/// </remarks>
	public required int TestsTotal { get; set; }

	/// <inheritdoc/>
	protected override void Deserialize(IReadOnlyDictionary<string, object?> root)
	{
		Guard.ArgumentNotNull(root);

		base.Deserialize(root);

		ExecutionTime = JsonDeserializer.TryGetDecimal(root, nameof(ExecutionTime)) ?? ExecutionTime;
		TestsFailed = JsonDeserializer.TryGetInt(root, nameof(TestsFailed)) ?? TestsFailed;
		TestsNotRun = JsonDeserializer.TryGetInt(root, nameof(TestsNotRun)) ?? TestsNotRun;
		TestsSkipped = JsonDeserializer.TryGetInt(root, nameof(TestsSkipped)) ?? TestsSkipped;
		TestsTotal = JsonDeserializer.TryGetInt(root, nameof(TestsTotal)) ?? TestsTotal;
	}
}
