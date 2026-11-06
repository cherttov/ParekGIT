<script>
    import Modal from "../Modal.svelte";
    import { deleteBranch } from "../../ipc/commands";

    let { open = false, data = {}, onclose } = $props();
    let deleteRemote = $state(false);

    $effect(() => { if (open) deleteRemote = false; });

    function confirm() {
        deleteBranch(data.branchName, deleteRemote ? data.remoteBranch : "");
        onclose?.();
    }
</script>

<Modal
    {open}
    title="Delete branch"
    confirmText="Delete branch"
    onconfirm={confirm}
    {onclose}
>
    <div class="modal-body-row">
        <p class="modal-text">Are you sure you want to delete this branch?</p>
        <span class="highlight-name">{data.branchName}</span>
    </div>

    {#if data.remoteBranch}
        <div class="modal-body-row checkbox-row">
            <input id="branch-delete-remote" class="ui-checkbox" type="checkbox" bind:checked={deleteRemote} />
            <label for="branch-delete-remote">Also delete on remote</label>
        </div>
    {/if}
</Modal>