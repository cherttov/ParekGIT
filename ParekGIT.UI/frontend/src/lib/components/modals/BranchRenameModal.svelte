<script>
	import Modal from "../Modal.svelte";
	import { renameBranch } from "../../ipc/commands";

	let { open = false, data = {}, onclose } = $props();
	let newName = $state("");
	let inputEl;

	const isValid = $derived(
		newName.trim().length > 0 && 
		newName.trim() !== data.branchName && 
		!["main", "master"].includes(newName.trim().toLowerCase())
	);

	$effect(() => {
		if (!open) return;
		newName = data.branchName ?? "";
		const t = setTimeout(() => {
			inputEl?.focus();
			inputEl?.select();
		}, 100);
		return () => clearTimeout(t);
	});

	function confirm() {
		if (!isValid) return;
		renameBranch(data.branchName, newName.trim());
		onclose?.();
	}

	function handleKeydown(e) {
		if (e.key === "Enter" && !e.isComposing) confirm();
	}
</script>

<Modal 
	{open}
	title="Rename branch"
	confirmText="Rename branch"
	confirmDisabled={!isValid}
	onconfirm={confirm}
	{onclose}
>
	<div class="modal-body-row">
		<label for="branch-rename-modal-name">Name</label>
		<input id="branch-rename-modal-name" type="text" class="modal-input" placeholder="new branch name" 
			bind:value={newName} bind:this={inputEl} onkeydown={handleKeydown} />
	</div>
</Modal>