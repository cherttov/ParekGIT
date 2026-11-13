<script>
    import Modal from "../Modal.svelte";
	import { changes } from "../../stores/changes.svelte";
    import { discardSelected } from "../../ipc/commands";

    let { open = false, onclose } = $props();
    const count = $derived(changes.checkedPaths.length);

    function confirm() {
        const paths = changes.checkedPaths;
        if (paths.length === 0) return;
        discardSelected(paths);
        onclose?.();
    }
</script>

<Modal
    {open}
    title="Discard selected changes"
    confirmText="Confirm"
    onconfirm={confirm}
    {onclose}
>
    <div class="modal-body-row">
        <p class="modal-text">Are you sure you want to discard</p>
        <span class="highlight-name" id="discard-selected-modal-count">{count} changed file{count === 1 ? "" : "s"}</span>
    </div>
</Modal>