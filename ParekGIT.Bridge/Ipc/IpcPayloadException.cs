namespace ParekGIT.Bridge.Models
{
	internal sealed class IpcPayloadException : Exception
	{
		public IpcPayloadException(string fieldName)
			: base($"Missing or invalid required field: '{fieldName}'") { }

		public IpcPayloadException(string fieldName, string reason)
			: base($"Invalid field '{fieldName}': {reason}") { }
	}
}
