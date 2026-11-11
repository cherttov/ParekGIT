<script>
	import LeftSidebar from "./lib/components/LeftSidebar.svelte";
	import ContextMenu from "./lib/components/ContextMenu.svelte";
	import Modals from "./lib/components/Modals.svelte";
	import Topbar from "./lib/components/Topbar.svelte";
	import SplitContainer from "./lib/components/SplitContainer.svelte";
	import { onMount } from "svelte";
	import { startIpc, sendIpcMessage } from "./lib/ipc/ipc.js";
	import { registerHandlers } from "./lib/ipc/handlers.js";
	import { IpcActions } from "./lib/ipc/actions.js";
	import { app } from "./lib/stores/app.svelte.js";

	// Registering handlers & starting Ipc listening
	registerHandlers();
	startIpc();

	// Keep '<body class="loading">' in sync with app state
	$effect(() => {
		document.body.classList.toggle("loading", !app.ready);
	});

	// Apply theme visually on app.theme/app.themes change
	$effect(() => {
		const activeTheme = app.themes.find((t) => t.id === app.theme);
		if (!activeTheme?.colors) return;

		for (const [key, value] of Object.entries(activeTheme.colors)) {
			document.documentElement.style.setProperty(`--${key}`, value);
		}
	});
	
	onMount(() => {
		sendIpcMessage(IpcActions.APP_READY);
	});
</script>

<Topbar />
<div class="app-container">
	<LeftSidebar />
	<SplitContainer />
</div>
<Modals />
<ContextMenu />