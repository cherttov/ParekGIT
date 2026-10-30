import { IpcActions } from "./actions.js";

// Frontend -> C#
export function sendIpcMessage(action, payload = {}) {
    window.external.sendMessage(JSON.stringify({ Action: action, Payload: payload }));
}

// C# -> Frontend
const handlers = new Map();

export function onIpc(action, handler) {
    handlers.set(action, handler);
}

export function startIpc() {
	window.external.receiveMessage((message) => {
		const data = JSON.parse(message);
		const handler = handlers.get(data.Action);
		if (!handler) {
			console.warn("Unknown action received: ", data.Action);
			return;
		}
		handler(data.Payload);
	});
}