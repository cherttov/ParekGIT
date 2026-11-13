<script>
    import { untrack } from "svelte";
	import Modal from "../Modal.svelte";
    import { app } from "../../stores/app.svelte";
    import { loadThemes, saveSettings, showInExplorer, clearLogs } from "../../ipc/commands";

    let { open = false, onclose } = $props();
    let theme = $state("");

    $effect(() => {
        if (!open) return;
        loadThemes();
        theme = untrack(() => app.theme);
    });

    function confirm() {
        app.theme = theme;
        saveSettings(theme);
        onclose?.();
    }
</script>


<Modal
    {open}
    title="Settings"
    confirmText="Save"
    onconfirm={confirm}
    {onclose}
>
    <div class="modal-body-row">
        <label for="settings-theme-select">Theme: </label>
        <select class="modal-input" id="settings-theme-select" bind:value={theme}>
            {#each app.themes as t (t.id)}
                <option value={t.id}>{t.name}</option>
            {/each}
        </select>
    </div>

    <div class="modal-body-row">
        <p class="modal-text">Logs</p>
        <div class="button-button-container">
            <button class="browse-btn view-logs" id="settings-logs-view" onclick={() => showInExplorer("%APP_DATA%")}>
                View logs
            </button>
            <button class="browse-btn clear-logs" id="settings-logs-clear" onclick={clearLogs}>
                Clear logs
            </button>
        </div>
    </div>
</Modal>