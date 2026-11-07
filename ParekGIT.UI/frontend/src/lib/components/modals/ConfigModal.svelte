<script>
	import Modal from "../Modal.svelte";
    import { config } from "../../stores/config.svelte";
    import { loadLocalConfig, saveLocalConfig } from "../../ipc/commands";

    let { open = false, data = {}, onclose } = $props();
    let name = $state("");
    let email = $state("");
    let input;

    $effect(() => {
        if (!open) return;
        name = "";
        email = "";
        loadLocalConfig();
        const t = setTimeout(() => input?.focus(), 100);
        return () => clearTimeout(t);
    });

    $effect(() => {
        if (!open || !config.localLoaded) return;
        name = config.localName;
        email = config.localEmail;
    });

    function confirm() {
        if (!config.localLoaded) return;
        saveLocalConfig(name.trim(), email.trim());
        onclose?.();
    }

    function handleKeydown(e) {
        if (e.key === "Enter" && !e.isComposing) confirm();
    }
</script>


<Modal
    {open}
    title="Config"
    confirmText="Save"
    confirmDisabled={!config.localLoaded}
    onconfirm={confirm}
    {onclose}
>
    <div class="modal-body-row">
        <label for="config-modal-name">Name</label>
        <input id="config-modal-name" type="text" class="modal-input input-name" 
            placeholder={config.globalName || "Not set"} bind:value={name} onkeydown={handleKeydown} />
    </div>

    <div class="modal-body-row"> 
        <label for="config-modal-email">Email</label>
        <input id="config-modal-email" type="text" class="modal-input input-email" 
            placeholder={config.globalEmail || "Not set"} bind:value={email} onkeydown={handleKeydown} />
    </div>
</Modal>