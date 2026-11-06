<script>
    import { modal } from "../stores/modal.svelte";
    import BranchNewModal from "./modals/BranchNewModal.svelte";
    import BranchDeleteModal from "./modals/BranchDeleteModal.svelte";
    import RepoAddModal from "./modals/RepoAddModal.svelte";
    import RepoCloneModal from "./modals/RepoCloneModal.svelte";
    import RepoCreateModal from "./modals/RepoCreateModal.svelte";
    import RepoRemoveModal from "./modals/RepoRemoveModal.svelte";

    export const modals = {
        "branch-new": BranchNewModal,
        "branch-delete": BranchDeleteModal,
        "repo-add": RepoAddModal,
        "repo-clone": RepoCloneModal,
        "repo-create": RepoCreateModal,
        "repo-remove": RepoRemoveModal
    };
</script>

{#each Object.entries(modals) as [id, Component] (id)}
    <Component open={modal.active === id} data={modal.data} onclose={() => modal.close()} />
{/each}