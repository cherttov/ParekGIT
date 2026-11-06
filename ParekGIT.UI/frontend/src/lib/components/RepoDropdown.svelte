<script>
    import { repo } from "../stores/repo.svelte";
    import { IpcActions } from "../ipc/actions";
	import { repositories } from "../stores/repositories.svelte";
    import { checkoutRepo } from "../ipc/commands";
    import { contextMenu } from "../stores/contextMenu.svelte";
    import { repoMenuItems } from "./contextMenus/repoContextMenu";
    import { addRepoItems } from "./contextMenus/repoContextMenu";
    import CustomScrollbar from "./CustomScrollbar.svelte";

    let { show = false, onclose } = $props();
    let searchTerm = $state("");
    let listEl = $state(null);

    $effect(() => { if (show) searchTerm = ""; });

    const filtered = $derived(
        repositories.list.filter(r => r.name.toLowerCase().includes(searchTerm.toLowerCase()))
    );

    function selectRepo(r) {
        checkoutRepo(r);
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

        <button class="topbar-filter-btn btn-add" onclick={(e) => contextMenu.open(e, addRepoItems)}>
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
                    class:active={repo.path === r.absolutePath}
                    class:invalid={!r.isValid}
                    class:remote={!!r.remoteUrl}
                    title={!r.isValid ? "Repository folder not found" : ""}
                    onclick={() => selectRepo(r)}
                    oncontextmenu={(e) => contextMenu.open(e, repoMenuItems(r))}
                >
                    <span class="dropdown-item-text">{r.name}</span>
                </div>
            {/each}
        </div>
        <CustomScrollbar container={listEl} />
    </div>
</div>