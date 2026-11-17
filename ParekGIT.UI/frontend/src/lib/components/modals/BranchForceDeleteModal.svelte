<script>
    import Modal from "../Modal.svelte";
    import { forceDeleteBranch } from "../../ipc/commands";

    let { open = false, data = {}, onclose } = $props();

    function confirm() {
        forceDeleteBranch(data.branchName);
        onclose?.();
    }
</script>

<Modal
    {open}
    title="Force delete branch"
    confirmText="Confirm"
    onconfirm={confirm}
    {onclose}
>
    <div class="modal-body-row">
        <p class="modal-text">
            This branch has
            <span class="highlight-name merge-branch-inline" id="unmerged-changes-count">{data.unmergedCount}</span>
            changes that have not been merged.
        </p>
    </div>

    <div class="modal-body-row">
        <p class="modal-text">Are you sure you want to permanently delete it?</p>
        <span class="highlight-name" id="delete-unmerged-modal-branch-name">{data.branchName}</span>
    </div>
</Modal>