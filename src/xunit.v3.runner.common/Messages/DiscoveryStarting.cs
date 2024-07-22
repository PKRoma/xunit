using System.Collections.Generic;
using Xunit.Internal;
using Xunit.Sdk;

namespace Xunit.Runner.Common;

public partial class DiscoveryStarting
{
	/// <inheritdoc/>
	/// <remarks>
	/// Note: Will be <see cref="MessageSinkMessage.UnsetStringPropertyValue"/> if there was no value provided during deserialization.
	/// </remarks>
	public required string AssemblyName { get; set; } = UnsetStringPropertyValue;

	/// <inheritdoc/>
	/// <remarks>
	/// Note: Will be <see cref="MessageSinkMessage.UnsetStringPropertyValue"/> if there was no value provided during deserialization.
	/// </remarks>
	public required string AssemblyPath { get; set; } = UnsetStringPropertyValue;

	/// <inheritdoc/>
	/// <remarks>
	/// Note: Will be <c>null</c> if there was no value provided during deserialization.
	/// </remarks>
	public required string? ConfigFilePath { get; set; }

	/// <inheritdoc/>
	protected override void Deserialize(IReadOnlyDictionary<string, object?> root)
	{
		Guard.ArgumentNotNull(root);

		base.Deserialize(root);

		AssemblyName = JsonDeserializer.TryGetString(root, nameof(AssemblyName)) ?? AssemblyName;
		AssemblyPath = JsonDeserializer.TryGetString(root, nameof(AssemblyPath)) ?? AssemblyPath;
		ConfigFilePath = JsonDeserializer.TryGetString(root, nameof(ConfigFilePath));
	}
}
