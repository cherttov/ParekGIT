<script>
    import { modal } from "../stores/modal.svelte";
    import BranchNewModal from "./modals/BranchNewModal.svelte";
    import BranchDeleteModal from "./modals/BranchDeleteModal.svelte";

    export const modals = {
        "branch-new": BranchNewModal,
        "branch-delete": BranchDeleteModal
    };
</script>

{#each Object.entries(modals) as [id, Component] (id)}
    <Component open={modal.active === id} data={modal.data} onclose={() => modal.close()} />
{/each}