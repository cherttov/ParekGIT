<script>
	import { modal } from "../stores/modal.svelte";
	import ErrorModal from "./modals/ErrorModal.svelte";
	import RepoCloneModal from "./modals/RepoCloneModal.svelte";
	import RepoCreateModal from "./modals/RepoCreateModal.svelte";
	import RepoAddModal from "./modals/RepoAddModal.svelte";
	import RepoRemoveModal from "./modals/RepoRemoveModal.svelte";
	import BranchNewModal from "./modals/BranchNewModal.svelte";
	import BranchFromCommitModal from "./modals/BranchFromCommitModal.svelte";
	import BranchRenameModal from "./modals/BranchRenameModal.svelte";
	import BranchDeleteModal from "./modals/BranchDeleteModal.svelte";
	import BranchForceDeleteModal from "./modals/BranchForceDeleteModal.svelte";
	import BranchMergeModal from "./modals/BranchMergeModal.svelte";
	import ChangeDiscardModal from "./modals/ChangeDiscardModal.svelte";
	import ChangeDiscardSelectedModal from "./modals/ChangeDiscardSelectedModal.svelte";
	import ChangesDiscardAllModal from "./modals/ChangesDiscardAllModal.svelte";
	import TodoModal from "./modals/TodoModal.svelte";
	import ConfigModal from "./modals/ConfigModal.svelte";
	import AccountModal from "./modals/AccountModal.svelte";
	import SettingsModal from "./modals/SettingsModal.svelte";
	import StashModal from "./modals/StashModal.svelte";

	export const modals = {
		// App
		"error": ErrorModal,

		// Repo
		"repo-clone": RepoCloneModal,
		"repo-create": RepoCreateModal,
		"repo-add": RepoAddModal,
		"repo-remove": RepoRemoveModal,
		//"repo-missing": RepoMissingModal,

		// Branch
		"branch-new": BranchNewModal,
		"branch-from-commit": BranchFromCommitModal,
		"branch-rename": BranchRenameModal,
		"branch-delete": BranchDeleteModal,
		"branch-force-delete": BranchForceDeleteModal,
		"branch-merge": BranchMergeModal,

		// Change
		"change-discard": ChangeDiscardModal,
		"changes-discard-selected": ChangeDiscardSelectedModal,
		"changes-discard-all": ChangesDiscardAllModal,

		// Todo
		"todo": TodoModal,

		// Config / Account
		"config": ConfigModal,
		"account": AccountModal,

		// Settings
		"settings": SettingsModal,

		// Stash
		"stashes": StashModal,
	};
</script>

{#each Object.entries(modals) as [id, Component] (id)}
	<Component open={modal.active === id} data={modal.data} onclose={() => modal.close()} />
{/each}