<script>
    import { branches } from "../stores/branches.svelte";
    import { branch } from "../stores/branch.svelte";
    import { repo } from "../stores/repo.svelte";
    import { layout } from "../stores/layout.svelte";
    import { checkoutBranch } from "../ipc/commands";
    import { modal } from "../stores/modal.svelte";
	import CustomScrollbar from "./CustomScrollbar.svelte";

    let { show = false, onclose } = $props();
    let searchTerm = $state("");
    let listEl = $state(null);

    const filtered = $derived(
        branches.list.filter(b => b.name.toLowerCase().includes(searchTerm.toLowerCase()))
    );

    function selectBranch(b) {
        checkoutBranch(b);
        onclose?.();
    }

    function newBranch() {
        onclose?.();
        modal.open("branch-new");
    }
</script>

<div class="dropdown-panel" class:show 
    id="branch-dropdown-panel" style:width="{layout.rightSidebarWidth}px">
    <div class="filter-container" id="branches-filter">
        <div class="icon-filter"></div>
        <input id="branch-dropdown-panel-search" type="text"
                bind:value={searchTerm}
                class="filter-input"
                placeholder="Search" />

        <button class="topbar-filter-btn btn-new" onclick={newBranch}>New</button>
    </div>

    <div class="dropdown-list-wrapper">
        <div class="dropdown-list-container" id="branch-topbar-list" bind:this={listEl}>
            {#each filtered as b (b.name)}
                <!-- svelte-ignore a11y_no_static_element_interactions -->
                <!-- svelte-ignore a11y_click_events_have_key_events -->
                <div
                    class="dropdown-item"
                    class:active={branch.name === b.name}
                    onclick={() => selectBranch(b)}
                >
                    <span class="dropdown-item-text">{b.name}</span>
                    {#if repo.hasRemote && !b.isRemote && !b.isDetached && !b.remoteBranch}
                        <span class="dropdown-item-unpublished-icon"></span>
                    {/if}
                </div>
            {/each}
        </div>
        <CustomScrollbar container={listEl} />
    </div>
</div>