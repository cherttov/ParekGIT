<script>
    import Modal from "../Modal.svelte";
    import { branches } from "../../stores/branches.svelte";
    import { branch } from "../../stores/branch.svelte";
    import { mergeBranches } from "../../ipc/commands";

    let { open = false, data = {}, onclose } = $props();
    let source = $state("");
    let target = $state("");

    let validBranches = $derived(branches.list.filter((b) => !b.isDetached));
    const isValid = $derived(source !== "" && target !== "" && source !== target);

    $effect(() => {
        if (!open) return;
        source = "";
        target = data.targetBranch ?? branch.name;
    });

    function confirm() {
        if (!isValid) return;
        mergeBranches(source, target);
        onclose?.();
    }
</script>

<Modal
    {open}
    title="Merge branches"
    confirmText="Merge branches"
    confirmDisabled={!isValid}
    onconfirm={confirm}
    {onclose}
>
    <div class="modal-body-row">
        <p class="modal-text">
            Are you sure you want to merge
            <span class="highlight-name merge-branch-inline" id="merge-modal-source-branch">{source || "{ none }"}</span>
            into 
            <span class="highlight-name merge-branch-inline" id="merge-modal-target-branch">{target}</span>
        </p>
    </div>

    <div class="modal-body-row">
        <div class="merge-branch-selector">
            <select class="modal-input modal-select" id="merge-modal-select-source" bind:value={source}>
                <option value="" disabled selected>Select source...</option>
                {#each validBranches as b (b.name)}
                    <option value={b.name}>{b.name}</option>
                {/each}
            </select>
            <div class="merge-arrow-icon"></div>
            <select class="modal-input modal-select" id="merge-modal-select-target" bind:value={target}>
                {#each validBranches as b (b.name)}
                    <option value={b.name}>{b.name}</option>
                {/each}
            </select>
        </div>
    </div>
</Modal>