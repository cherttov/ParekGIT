<script>
    import Modal from "../Modal.svelte";
	import { changes } from "../../stores/changes.svelte";
    import { discardAll } from "../../ipc/commands";

    let { open = false, onclose } = $props();
    const count = $derived(changes.files.length);

    function confirm() {
        if (count === 0) return;
        discardAll();
        onclose?.();
    }
</script>

<Modal
    {open}
    title="Discard all changes"
    confirmText="Confirm"
    onconfirm={confirm}
    {onclose}
>
    <div class="modal-body-row">
        <p class="modal-text">Are you sure you want to discard</p>
        <span class="highlight-name" id="discard-all-modal-count">{count} changed file{count === 1 ? "" : "s"}</span>
    </div>
</Modal>