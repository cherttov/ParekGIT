<script>
    import CustomScrollbar from "./CustomScrollbar.svelte";

    let { el = $bindable() } = $props();
    let activeTab = $state("changes"); // "changes" || "history"

    let changesListEl = $state(null);
    let historyListEl = $state(null);
</script>

<div class="right-sidebar" id="right-sidebar" bind:this={el}>
    <div class="tab-selector">
        <button class="tab-btn" id="tab-btn-changes"
            class:active={activeTab === "changes"}
            onclick={() => activeTab = "changes"}>
            Changes
        </button>
        <button class="tab-btn" id="tab-btn-history"
            class:active={activeTab === "history"}
            onclick={() => activeTab = "changes"}>
            History
        </button>
    </div>

    <div class="history-tab" id="history-tab" class:active={activeTab === "history"}>
        <div class="history-list-wrapper">
            <div class="history-list-container" id="history-list" bind:this={historyListEl}></div>
            <CustomScrollbar container={historyListEl} />
        </div>
    </div>

    <div class="changes-tab" id="changes-tab" class:active={activeTab === "changes"}>
        <div class="changes-header" id="changes-header">
            <span id="changes-count-text">0 changed files</span>
            <input type="checkbox" class="ui-checkbox" id="changes-master-checkbox" checked />
        </div>

        <div class="changes-list-wrapper">
            <div class="changes-list-container" id="changes-list" bind:this={changesListEl}></div>
            <CustomScrollbar container={changesListEl} />
        </div>

        <div class="right-sidebar-bottom">
            <input type="text" class="commit-input message-input" id="commit-name-input" placeholder="Message (required)" />
            <textarea class="commit-input description-input" id="commit-desc-input" placeholder="Description"></textarea>
            <button class="commit-btn" id="commit-btn">
                Commit X files to <strong>[branch]</strong>
            </button>
        </div>
    </div>
</div>