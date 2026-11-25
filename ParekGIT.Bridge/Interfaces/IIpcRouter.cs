using Photino.NET;

namespace ParekGIT.Bridge.Interfaces
{
	public interface IIpcRouter
	{
		void RegisterHandler(IMessageHandler handler);
		void HandleMessage(object sender, WebMessageReceivedEventArgs e);
	}
}