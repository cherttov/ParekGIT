<script>
	import Modal from "../Modal.svelte";
	import { changes } from "../../stores/changes.svelte";
	import { discardChange } from "../../ipc/commands";

	let { open = false, data = {}, onclose } = $props();
    const path = $derived(data.path ?? "");

	function confirm() {
        if (!path) return;
		discardChange(path);
		onclose?.();
	}
</script>

<Modal
	{open}
	title="Discard the change"
	confirmText="Confirm"
	onconfirm={confirm}
	{onclose}
>
	<div class="modal-body-row">
		<p class="modal-text">Are you sure you want to discard</p>
		<span class="highlight-name">{path}</span>
	</div>
</Modal>