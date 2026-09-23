using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class ConfigLocalGetHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		public string Action => "CONFIG_LOCAL_GET";

		// Constructor
		public ConfigLocalGetHandler(PhotinoWindow window, IGitRunner gitRunner)
		{
			_window = window;
			_gitRunner = gitRunner;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			Task<GitConfigInfo> localConfigTask = _gitRunner.GetLocalConfigAsync(repoPath);
			Task<GitConfigInfo> globalConfigTask = _gitRunner.GetGlobalConfigAsync(null);

			await Task.WhenAll(localConfigTask, globalConfigTask);

			GitConfigInfo localConfig = localConfigTask.Result;
			GitConfigInfo globalConfig = globalConfigTask.Result;

			var jsPayload = new
			{
				localName = localConfig.Name,
				localEmail = localConfig.Email,
				globalName = globalConfig.Name,
				globalEmail = globalConfig.Email
			};

			// Response
			var response = new IpcMessage
			{
				Action = "CONFIG_LOCAL_LOADED",
				Payload = JsonSerializer.SerializeToElement(jsPayload)
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
