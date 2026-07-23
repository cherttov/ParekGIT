namespace ParekGIT.Bridge.Models
{
	public class IpcPayloadException : Exception
	{
		public IpcPayloadException(string fieldName)
			: base($"Missing or invalid required filed: '{fieldName}'") { }

		public IpcPayloadException(string fieldName, string reason)
			: base($"Invalid field '{fieldName}': {reason}") { }
	}
}
