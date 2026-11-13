<script>
	import Modal from "../Modal.svelte";
    import { createBranchFromCommit } from "../../ipc/commands";

    let { open = false, data = {}, onclose } = $props();
    let branchName = $state("");
    let inputEl;

    const hash = $derived(data?.hash ?? "");
    const isValid = $derived(branchName.trim().length > 0 && hash !== "");

    $effect(() => {
        if (!open) return;
        branchName = "";
        const t = setTimeout(() => inputEl?.focus(), 100);
        return () => clearTimeout(t);
    });

    function confirm() {
        if (!isValid) return;
        createBranchFromCommit(branchName.trim(), hash);
        onclose?.();
    }

    function handleKeydown(e) {
        if (e.key === "Enter" && !e.isComposing) confirm();
    }
</script>

<Modal 
    {open}
    title="Create branch"
    confirmText="Create branch"
    confirmDisabled={!isValid}
    onconfirm={confirm}
    {onclose}
>
    <div class="modal-body-row">
            <p class="modal-text">Create a branch from commit:</p>
            <span class="highlight-name" id="branch-history-modal-commit-name">{hash.substring(0, 7)}</span>
    </div>

    <div class="modal-body-row">
        <label for="branch-history-new-modal-name">Name</label>
        <input id="branch-history-new-modal-name" type="text" class="modal-input" placeholder="new branch name"
            bind:value={branchName} bind:this={inputEl} onkeydown={handleKeydown} />
    </div>
</Modal>