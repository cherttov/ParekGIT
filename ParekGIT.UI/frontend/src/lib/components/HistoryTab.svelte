<script>
    import CustomScrollbar from "./CustomScrollbar.svelte";
    import { history } from "../stores/history.svelte";
    import { contextMenu } from "../stores/contextMenu.svelte";
    import { historyMenuItems } from "./menus/historyContextMenu";
    import { loadMoreHistory, loadCommitDetails } from "../ipc/commands";

    let { active = false } = $props();
    let listEl = $state(null);

    const NEAR_BOTTOM_PX = 30;
    
    function onScroll() {
        if (!listEl) return;
        const nearBottom = listEl.scrollTop + listEl.clientHeight >= listEl.scrollHeight - NEAR_BOTTOM_PX;
        if (nearBottom) loadMoreHistory();
    }

    function onSelect(c) {
        if (history.selectedHash === c.hash) return;
        history.select(c.hash);
        loadCommitDetails(c.hash);
    }
</script>

<div class="history-tab" id="history-tab" class:active>
    <div class="history-list-wrapper">
        <div class="history-list-container" id="history-list" bind:this={listEl} onscroll={onScroll}>
            {#each history.commits as c, i (c.hash)}
                <!-- svelte-ignore a11y_click_events_have_key_events -->
                <!-- svelte-ignore a11y_no_static_element_interactions -->
                <div class="history-item" class:selected={history.selectedHash === c.hash}
                    onclick={() => onSelect(c)} oncontextmenu={(e) => contextMenu.open(e, historyMenuItems(c, i === 0), c.hash)}>
                        <div class="history-message">{c.message}</div>
                        <div class="history-meta">
                            <div class="history-author">{c.author}</div>
                            <div class="history-time">{c.timeAgo}</div>
                        </div>
                </div>
            {/each}
        </div>
        <CustomScrollbar container={listEl} />
    </div>
</div>