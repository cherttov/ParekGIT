<script>
    import { branches } from "../stores/branches.svelte";
    import { branch } from "../stores/branch.svelte";
    import { repo } from "../stores/repo.svelte";
    import { sendIpcMessage } from "../ipc/ipc";
    import { IpcActions } from "../ipc/actions";
    import { layout } from "../stores/layout.svelte";
	import CustomScrollbar from "./CustomScrollbar.svelte";

    let { show = false, onclose, onnewbranch } = $props();
    let searchTerm = $state("");
    let listEl = $state(null);

    const filtered = $derived(
        branches.list.filter(b => b.name.toLowerCase().includes(searchTerm.toLowerCase()))
    );

    function checkoutBranch(b) {
        if (b.isDetached) return;
        if (branch.name.toLowerCase() === b.name.toLowerCase()) {
            onclose?.();
            return;
        }
        sendIpcMessage(IpcActions.BRANCH_CHECKOUT, {
            repoPath: repo.path,
            branchName: b.name,
            isRemote: b.isRemote
        });
        onclose?.();
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

        <button class="topbar-filter-btn btn-new" onclick={() => onnewbranch?.()}>New</button>
    </div>

    <div class="dropdown-list-wrapper">
        <div class="dropdown-list-container" id="branch-topbar-list" bind:this={listEl}>
            {#each filtered as b}
                <!-- svelte-ignore a11y_no_static_element_interactions -->
                <!-- svelte-ignore a11y_click_events_have_key_events -->
                <div
                    class="dropdown-item"
                    class:active={branch.name === b.name}
                    onclick={() => checkoutBranch(b)}
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