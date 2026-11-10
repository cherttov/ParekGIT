<script>
    import ChangesTab from "./ChangesTab.svelte";
    import HistoryTab from "./HistoryTab.svelte";
    import { layout } from "../stores/layout.svelte";
    import { view } from "../stores/view.svelte";

    let { el = $bindable() } = $props();
</script>

<div class="right-sidebar" id="right-sidebar" bind:this={el} style:width="{layout.rightSidebarWidth}px">
    <div class="tab-selector">
        <button class="tab-btn" id="tab-btn-changes"
            class:active={view.tab === "changes"} onclick={() => view.tab = "changes"}>
            Changes
        </button>
        <button class="tab-btn" id="tab-btn-history"
            class:active={view.tab === "history"} onclick={() => view.tab = "history"}>
            History
        </button>
    </div>

    <ChangesTab active={view.tab === "changes"} />
    <HistoryTab active={view.tab === "history"} />
</div>