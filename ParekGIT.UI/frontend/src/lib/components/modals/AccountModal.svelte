<script>
	import Modal from "../Modal.svelte";
    import { config } from "../../stores/config.svelte";
    import { loadGlobalConfig, saveGlobalConfig } from "../../ipc/commands";

    let { open = false, onclose } = $props();
    let name = $state("");
    let email = $state("");

    const providers = [
        { id: "github", label: "Github" },
        { id: "codeberg", label: "Codeberg" },
        { id: "gitlab", label: "GitLab" }
    ];

    $effect(() => {
        if (!open) return;
        name = "";
        email = "";
        loadGlobalConfig();
    });

    $effect(() => {
        if (!open || !config.globalLoaded) return;
        name = config.globalName;
        email = config.globalEmail;
    })

    function confirm() {
        if (!config.globalLoaded) return;
        saveGlobalConfig(name.trim(), email.trim());
        onclose?.();
    }
</script>


<Modal
    {open}
    title="Account"
    confirmText="Save"
    onconfirm={confirm}
    confirmDisabled={!config.globalLoaded}
    {onclose}
>
    <div class="modal-body-row">
        <label for="account-modal-name">Name</label>
        <input id="account-modal-name" type="text" class="modal-input input-name" placeholder="Not set" bind:value={name} />
    </div>

    <div class="modal-body-row">
        <label for="account-modal-email">Email</label>
        <input id="account-modal-email" type="text" class="modal-input input-email" placeholder="Not set" bind:value={email} />
    </div>

    <div class="modal-body-divider"></div>

    <div class="modal-body-row">
        <p class="modal-text">Linked accounts</p>

        {#each providers as p (p.id)}
            <div class="linked-account-row" data-provider={p.id}>
                <span class="linked-account-name">{p.label}</span>
                <button class="link-account-btn" disabled>Link</button>
            </div>
        {/each}
    </div>
</Modal>