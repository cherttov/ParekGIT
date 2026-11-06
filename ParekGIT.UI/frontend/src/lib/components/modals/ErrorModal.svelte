<script>
	import Modal from "../Modal.svelte";

    let { open = false, data = {}, onclose } = $props();
</script>

<Modal
    {open}
    title="Error"
    confirmText={null}
    cancelText="Close"
    {onclose}
>
    <div class="modal-body-row">
        <pre id="error-modal-message">{data.message}</pre>
    </div>
</Modal>