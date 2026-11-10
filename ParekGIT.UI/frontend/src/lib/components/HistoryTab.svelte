<script>
    import CustomScrollbar from "./CustomScrollbar.svelte";

    let { active = false } = $props();
    let listEl = $state(null);
</script>

<div class="history-tab" id="history-tab" class:active>
    <div class="history-list-wrapper">
        <div class="history-list-container" id="history-list" bind:this={listEl}>
            
        </div>
        <CustomScrollbar container={listEl} />
    </div>
</div>