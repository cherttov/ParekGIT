<script>
    import { repositories } from "../stores/repositories.svelte";
    import { repo } from "../stores/repo.svelte";
    import { branch } from "../stores/branch.svelte";
    import { branches } from "../stores/branches.svelte";
    import { sendIpcMessage } from "../ipc/ipc";
    import { IpcActions } from "../ipc/actions";
    import CustomScrollbar from "./CustomScrollbar.svelte";

    let { show = false, onclose } = $props();
    let searchTerm = $state("");
    let listEl = $state(null);

    const filtered = $derived(
        repositories.list.filter(r => r.Name.toLowerCase().includes(searchTerm.toLowerCase()))
    );

    function selectRepo(r) {
        if (!r.IsValid) return;
        if (repo.path === r.AbsolutePath) { onclose?.(); return; }

        repo.name = r.Name;
        repo.path = r.AbsolutePath;
        repo.hasRemote = !!r.RemoteUrl;
        repo.changes = [];
        branch.name = "";
        branches.list = [];

        sendIpcMessage(IpcActions.REPO_SELECTED, { absolutePath: r.AbsolutePath });
        sendIpcMessage(IpcActions.REPO_STATUS_LOAD, { repoPath: r.AbsolutePath });
        onclose?.();
    }
</script>

<div class="dropdown-panel" 
    class:show
    id="repository-dropdown-panel">
    <div class="filter-container" id="repositories-filter">
        <div class="icon-filter"></div>
        <input id="repo-search-input" type="text"
                bind:value={searchTerm}
                class="filter-input"
                placeholder="Search" />

        <button class="topbar-filter-btn btn-add">
            Add
            <div class="btn-arrow"></div>
        </button>
    </div>

    <div class="dropdown-list-wrapper">
        <div class="dropdown-list-container" id="repo-topbar-list" bind:this={listEl}>
            {#each filtered as r}
                <!-- svelte-ignore a11y_no_static_element_interactions -->
                <!-- svelte-ignore a11y_click_events_have_key_events -->
                <div
                    class="dropdown-item"
                    class:active={repo.path === r.AbsolutePath}
                    class:invalid={!r.IsValid}
                    class:remote={!!r.RemoteUrl}
                    title={!r.IsValid ? "Repository folder not found" : ""}
                    onclick={() => selectRepo(r)}
                >
                    <span class="dropdown-item-text">{r.Name}</span>
                </div>
            {/each}
        </div>
        <CustomScrollbar container={listEl} />
    </div>
</div>