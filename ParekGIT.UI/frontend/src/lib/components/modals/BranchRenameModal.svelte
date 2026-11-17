<script>
	import Modal from "../Modal.svelte";
    import { createBranch, renameBranch } from "../../ipc/commands";

    let { open = false, data = {}, onclose } = $props();
    let newName = $state("");
    let inputEl;

    const isValid = $derived(newName.trim().length > 0);

    $effect(() => {
        if (!open) return;
        newName = "";
        const t = setTimeout(() => inputEl?.focus(), 100);
        return () => clearTimeout(t);
    });

    function confirm() {
        if (!isValid) return;
        renameBranch(data.branchName, newName);
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