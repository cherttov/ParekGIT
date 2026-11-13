<script>
    import Modal from "../Modal.svelte";
    import CustomScrollbar from "../CustomScrollbar.svelte";
    import { stashes } from "../../stores/stashes.svelte";
    import { changes } from "../../stores/changes.svelte";
    import { loadStashes, saveStash, popStash, dropStash } from "../../ipc/commands";

    let { open = false, onclose } = $props();
    let message = $state("");
    let listEl = $state(null);
    let inputEl;

    const canSave = $derived(changes.checkedPaths.length > 0);

    $effect(() => {
        if (!open) return;
        message = "";
        stashes.list = [];
        stashes.loaded = false;
        loadStashes();
        const t = setTimeout(() => inputEl.focus(), 100);
        return () => clearTimeout(t);
    });

    function displayMessage(raw) {
        const i = raw.indexOf(": ");
        return i >= 0 ? raw.slice(i + 2) : raw;
    }

    function save() {
        if (!canSave) return;
        saveStash(message.trim(), $state.snapshot(changes.checkedPaths));
        message = "";
    }

    function handleKeydown(e) {
        if (e.key === "Enter" && !e.isComposing) save();
    }
</script>

<Modal
    {open}
    title="Stashes"
    confirmText={null}
    cancelText="Close"
    bodyStyle="padding: 0; gap: 0"
    {onclose}
>
    <div class="modal-list-wrapper">
        <div class="stash-new-row">
            <input type="text" class="modal-input" id="stash-modal-input-message" placeholder="Stash message (optional)"
                bind:value={message} bind:this={inputEl} onkeydown={handleKeydown} />
            <button class="stash-action-btn stash-save-btn" id="stash-modal-save-btn" disabled={!canSave} onclick={save}>Stash</button>
        </div>

        <div class="modal-list-rows" id="stash-rows-container" bind:this={listEl}>
            {#if stashes.loaded && stashes.list.length === 0}
                <div class="stash-empty">No stashes in this repository</div>
            {:else}
                {#each stashes.list as stash (stash.commitHash)}
                    <div class="stash-modal-row">
                        <div class="stash-info">
                            <div class="stash-message">{displayMessage(stash.message)}</div>
                            <div class="stash-meta">
                                <span class="stash-branch">{stash.branchName}</span>
                                <span class="stash-hash">{stash.commitHash.substring(0, 7)}</span>
                            </div>
                        </div>
                        <div class="stash-actions">
                            <button class="stash-action-btn stash-pop-btn" onclick={() => popStash(stash.selector)}>Pop</button>
                            <button class="stash-action-btn stash-drop-btn" onclick={() => dropStash(stash.selector)}>Drop</button>
                        </div>
                    </div>
                {/each}
            {/if}
        </div>
        
        <CustomScrollbar container={listEl}></CustomScrollbar>
    </div>
</Modal>