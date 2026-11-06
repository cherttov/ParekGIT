<script>
	import Modal from "../Modal.svelte";
    import { removeRepo } from "../../ipc/commands";

    let { open = false, data = {}, onclose } = $props();
    let deleteLocal = $state(false);

    $effect(() => { if (open) deleteLocal = false; });

    function confirm() {
        removeRepo(data.repoPath, deleteLocal)
        onclose?.();
    }
</script>

<Modal
    {open}
    title="Remove repository"
    confirmText="Remove"
    onconfirm={confirm}
    {onclose}
>
    <div class="modal-body-row">
        <p class="modal-text">Are you sure you want to remove this repository from ParekGIT?</p>
        <span class="highlight-name" id="remove-modal-repo-name">{data.repoName}</span>
    </div>

    {#if !data.isInvalid}
        <div class="modal-body-row checkbox-row">
            <input id="repo-remove-modal-recycle-checkbox" type="checkbox" class="ui-checkbox" bind:checked={deleteLocal}/>
            <label for="repo-remove-modal-recycle-checkbox">Also move this repository to recycle bin</label>
        </div>
    {/if}
</Modal>