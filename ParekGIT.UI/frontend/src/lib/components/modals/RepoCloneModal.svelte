<script>
    import Modal from "../Modal.svelte";
    import { pickFolder, cloneRepo } from "../../ipc/commands";

    let { open = false, onclose } = $props();
    let url = $state("");
    let asLocal = $state(false);
    let path = $state("");
    let urlInput, pathInput;

    const isValid = $derived(url.trim() !== "" && path.trim() !== "");

    $effect(() => {
        if (!open) return;
        url = "";
        asLocal = false;
        path = "";
        const t = setTimeout(() => urlInput?.focus(), 100);
        return () => clearTimeout(t);
    });

    function browse() {
        pickFolder((p) => { path = p; pathInput?.focus(); });
    }

    function confirm() {
        if (!isValid) return;
        cloneRepo({ repoUrl: url.trim(), asLocal, localPath: path.trim() });
        onclose?.();
    }

    function handleKeydown(e) {
        if (e.key === "Enter" && !e.isComposing) confirm();
    }
</script>

<Modal
    {open}
    title="Clone a repository"
    confirmText="Clone repository"
    confirmDisabled={!isValid}
    onconfirm={confirm}
    {onclose}
>
    <div class="modal-body-row">
        <label for="repo-clone-modal-url">Repository URL</label>
        <input id="repo-clone-modal-url" type="text" class="modal-input input-url" placeholder="repository url"
                bind:value={url} bind:this={urlInput} onkeydown={handleKeydown} />
    </div>

    <div class="modal-body-row checkbox-row">
        <input id="clone-modal-clone-local" type="checkbox" class="ui-checkbox" bind:checked={asLocal} />
        <label for="clone-modal-clone-local">Clone as local</label>
    </div>

    <div class="modal-body-row">
        <label for="repo-clone-modal-path">Local path</label>
        <div class="input-button-container">
            <input id="repo-clone-modal-path" type="text" class="modal-input input-path" placeholder="local path"
                    bind:value={path} bind:this={pathInput} onkeydown={handleKeydown} />
            <button class="browse-btn" onclick={browse}>Browse</button>
        </div>
    </div>
</Modal>