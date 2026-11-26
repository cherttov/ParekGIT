<script>
    import Modal from "../Modal.svelte";
    import { modal } from "../../stores/modal.svelte";
	import { removeRepo } from "../../ipc/commands";

	let { open = false, data = {}, onclose } = $props();

	function confirm() {
		modal.open("repo-missing-add", { path: data.repoPath, oldPath: data.repoPath });
	}

    function remove() {
        removeRepo(data.repoPath, false);
    }
</script>

<Modal
	{open}
	title="Repository not found"
	confirmText="Locate..."
    cancelText="Remove"
	onconfirm={confirm}
    oncancel={remove}
	{onclose}
>
	<div class="modal-body-row">
        <p class="modal-text">Repository not found at path: </p>
        <span class="highlight-name">{data.repoPath}</span>
    </div>
</Modal>