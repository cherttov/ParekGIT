<script>
	import Modal from "../Modal.svelte";
	import { addRepo, removeRepo, pickFolder } from "../../ipc/commands";

	let { open = false, data = {}, onclose } = $props();
	let path = $state("");
	let inputEl;

	const isValid = $derived(path.trim() !== "");

	$effect(() => {
		if (!open) return;
		path = data.path ?? "";
		const t = setTimeout(() => inputEl?.focus(), 100);
		return () => clearTimeout(t);
	});

	function browse() {
		pickFolder((p) => { path = p; inputEl?.focus(); });
	}

	function confirm() {
		if (!isValid) return;
		removeRepo(data.oldPath, false);
		addRepo(path.trim());
		onclose?.();
	}

	function handleKeydown(e) {
		if (e.key === "Enter" && !e.isComposing) confirm();
	}
</script>

<Modal
	{open}
	title="Locate repository"
	confirmText="Locate"
	confirmDisabled={!isValid}
	onconfirm={confirm}
	{onclose}
>
	<div class="modal-body-row">
		<label for="repo-missing-add-modal-path">Local path</label>
		<div class="input-button-container">
			<input id="repo-missing-add-modal-path" type="text" class="modal-input" placeholder="repository path"
				bind:value={path} bind:this={inputEl} onkeydown={handleKeydown} />
			<button class="browse-btn" onclick={browse}>Browse</button>
		</div>
	</div>
</Modal>