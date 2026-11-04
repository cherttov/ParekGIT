<script>
	import Modal from "../Modal.svelte";
    import { createBranch } from "../../ipc/commands";

    let { open = false, onclose } = $props();
    let branchName = $state("");
    let input;

    const isValid = $derived(branchName.trim().length > 0);

    $effect(() => {
        if (!open) return;
        branchName = "";
        const t = setTimeout(() => input?.focus(), 100);
        return () => clearTimeout(t);
    });

    function submit() {
        if (!isValid) return;
        createBranch(branchName.trim());
        onclose?.();
    }

    function handleKeydown(e) {
        if (e.key === "Enter" && !e.isComposing) createBranch();
    }
</script>

<Modal 
    {open}
    title="Create new branch"
    confirmText="Create branch"
    confirmDisabled={!isValid}
    onconfirm={submit}
    {onclose}
>
    <div class="modal-body-row">
        <label for="branch-new-name">Name</label>
        <input id="branch-new-name" type="text"
            class="modal-input" placeholder="new branch name"
            bind:value={branchName}  bind:this={input}
            onkeydown={handleKeydown}
        />
    </div>
</Modal>