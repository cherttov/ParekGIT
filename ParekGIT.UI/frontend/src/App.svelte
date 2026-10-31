<script>
	import LeftSidebar from "./lib/components/LeftSidebar.svelte";

	import { onMount } from "svelte";
	import { startIpc, sendIpcMessage } from "./lib/ipc/ipc.js";
	import { registerHandlers } from "./lib/ipc/handlers.js";
	import { IpcActions } from "./lib/ipc/actions.js";
	import { repo } from "./lib/stores/repo.svelte.js";
	import { app } from "./lib/stores/app.svelte.js";
	import { repositories } from "./lib/stores/repositories.svelte";
	import Topbar from "./lib/components/Topbar.svelte";

	// Registering handlers & starting Ipc listening
	registerHandlers();
	startIpc();

	// Keep '<body class="loading">' in sync with app stace
	$effect(() => {
		document.body.classList.toggle("loading", !app.ready);
	});
	
	onMount(() => {
		sendIpcMessage(IpcActions.APP_READY);
	});
</script>

<Topbar />
<div class="app-container">
	<LeftSidebar />
</div>