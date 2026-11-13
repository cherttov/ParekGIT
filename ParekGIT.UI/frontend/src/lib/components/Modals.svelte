<script>
    import { modal } from "../stores/modal.svelte";
    import BranchNewModal from "./modals/BranchNewModal.svelte";
    import BranchDeleteModal from "./modals/BranchDeleteModal.svelte";
    import BranchMergeModal from "./modals/BranchMergeModal.svelte";
    import BranchFromCommitModal from "./modals/BranchFromCommitModal.svelte";
    import RepoAddModal from "./modals/RepoAddModal.svelte";
    import RepoCloneModal from "./modals/RepoCloneModal.svelte";
    import RepoCreateModal from "./modals/RepoCreateModal.svelte";
    import RepoRemoveModal from "./modals/RepoRemoveModal.svelte";
    import ChangeDiscardSelectedModal from "./modals/ChangeDiscardSelectedModal.svelte";
    import ChangesDiscardAllModal from "./modals/ChangesDiscardAllModal.svelte";
    import TodoModal from "./modals/TodoModal.svelte";
    import ConfigModal from "./modals/ConfigModal.svelte";
    import StashModal from "./modals/StashModal.svelte";
    import SettingsModal from "./modals/SettingsModal.svelte";
    import ErrorModal from "./modals/ErrorModal.svelte";

    export const modals = {
        "branch-new": BranchNewModal,
        "branch-delete": BranchDeleteModal,
        "branch-merge": BranchMergeModal,
        "branch-from-commit": BranchFromCommitModal,
        "repo-add": RepoAddModal,
        "repo-clone": RepoCloneModal,
        "repo-create": RepoCreateModal,
        "repo-remove": RepoRemoveModal,
        "changes-discard-selected": ChangeDiscardSelectedModal,
        "changes-discard-all": ChangesDiscardAllModal,
        "todo": TodoModal,
        "config": ConfigModal,
        "stashes": StashModal,
        "settings": SettingsModal,
        "error": ErrorModal
    };
</script>

{#each Object.entries(modals) as [id, Component] (id)}
    <Component open={modal.active === id} data={modal.data} onclose={() => modal.close()} />
{/each}