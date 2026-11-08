<script>
    import Modal from "../Modal.svelte";
    import { addRepo, pickFolder } from "../../ipc/commands";

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
        addRepo(path.trim());
        onclose?.();
    }

    function handleKeydown(e) {
        if (e.key === "Enter" && !e.isComposing) confirm();
    }
</script>

<Modal
    {open}
    title="Add local repository"
    confirmText="Add repository"
    confirmDisabled={!isValid}
    onconfirm={confirm}
    {onclose}
>
    <div class="modal-body-row">
        <label for="repo-add-modal-path">Local path</label>
        <div class="input-button-container">
            <input id="repo-add-modal-path" type="text" class="modal-input" placeholder="repository path"
                bind:value={path} bind:this={inputEl} onkeydown={handleKeydown} />
            <button class="browse-btn" onclick={browse}>Browse</button>
        </div>
    </div>
</Modal>