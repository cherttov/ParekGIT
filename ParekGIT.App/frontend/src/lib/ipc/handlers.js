import { IpcActions } from "./actions.js";
import { onIpc } from "./ipc.js";
import { repo } from "../stores/repo.svelte.js";
import { app } from "../stores/app.svelte.js";
import { repositories } from "../stores/repositories.svelte.js";
import { branch } from "../stores/branch.svelte.js";
import { branches } from "../stores/branches.svelte.js";
import {
	openRepo, loadStashes, refreshRepoState,
	resolveFolderPick, saveSettings, loadRepoStatus, loadCommitFileDiff,
	loadTodos, fetchRepo, loadBranches
} from "./commands.js";
import { config } from "../stores/config.svelte.js";
import { modal } from "../stores/modal.svelte.js";
import { todos } from "../stores/todos.svelte.js";
import { stashes } from "../stores/stashes.svelte.js";
import { changes } from "../stores/changes.svelte.js";
import { view } from "../stores/view.svelte.js";
import { commit } from "../stores/commit.svelte.js";
import { history } from "../stores/history.svelte.js";
import { commitDetails } from "../stores/commitDetails.svelte.js";
import { diff } from "../stores/diff.svelte.js";

export function registerHandlers() {
	// App
	onIpc(IpcActions.APP_INITIALIZED, (p) => {
		app.theme = p.settings.theme;
		app.themes = p.themes;
		repositories.list = p.repositories;
		if (p.settings.lastRepoPath) {
			const lastRepo = p.repositories.find(
				(r) => r.absolutePath === p.settings.lastRepoPath
			);
			if (lastRepo) openRepo(lastRepo);
		}
		app.ready = true;
	});
	onIpc(IpcActions.APP_ERROR, (p) => {
		commit.committing = false;
		modal.open("error", { message: p.message });
	});

	// Repo
	onIpc(IpcActions.REPO_OPENED, (p) => {
		commit.switchRepo(repo.path, p.repo.absolutePath);
		repo.name = p.repo.name;
		repo.path = p.repo.absolutePath;
		repo.remoteUrl = p.repo.remoteUrl;
		branches.list = p.branches;

		config.resetLocal();
		stashes.reset();
		changes.reset();
		history.reset();
		diff.reset();
		commitDetails.reset();
		todos.reset();
		view.reset();
		applyBranch(p.branch);

		refreshRepoState();
		loadTodos();
	});
	onIpc(IpcActions.REPO_CLONED, (p) => {
		appendRepo(p.repo);
	});
	onIpc(IpcActions.REPO_CREATED, (p) => {
		appendRepo(p.repo);
	});
	onIpc(IpcActions.REPO_ADDED, (p) => {
		appendRepo(p.repo);
	});
	onIpc(IpcActions.REPO_REMOVED, (p) => {
		repositories.list = repositories.list.filter(r => r.absolutePath !== p.repoPath);
		if (repo.path === p.repoPath) {
			commit.clear();
			commit.forget(p.repoPath);

			repo.reset();
			branch.reset();
			branches.reset();
			stashes.reset();
			changes.reset();
			history.reset();
			diff.reset();
			commitDetails.reset();
			todos.reset();
			view.reset();
			config.resetLocal();
		}
	});
	onIpc(IpcActions.REPO_FETCHED, () => {
		repo.fetching = false;
		refreshRepoState();
	});
	onIpc(IpcActions.REPO_PULLED, () => {
		fetchRepo();
	});
	onIpc(IpcActions.REPO_PUSHED, () => {
		fetchRepo();
	});
	onIpc(IpcActions.REPO_STATUS_LOADED, (p) => {
		changes.setFiles(p.files);
	});
	onIpc(IpcActions.REPO_FILES_CHANGED, (p) => {
		if (p.repoPath === repo.path) loadRepoStatus();
	});
	onIpc(IpcActions.REPO_PATH_MISSING, (p) => {
		modal.open("error", { message: `Repository not found at path: ${p.repoPath}` });
	});
	onIpc(IpcActions.REPO_SYNC_STATUS_CHANGED, (p) => {
		branch.commitsBehind = p.commitsBehind ?? 0;
		branch.commitsAhead = p.commitsAhead ?? 0;
	});

	// Branch
	onIpc(IpcActions.BRANCH_CHECKED_OUT, (p) => {
		branches.list = p.branches;

		changes.reset();
		history.reset();
		diff.reset();
		commitDetails.reset();
		todos.reset();
		view.reset();
		applyBranch(p.branch);

		refreshRepoState();
		loadTodos();
	});
	onIpc(IpcActions.BRANCH_LIST_LOADED, (p) => {
		branches.list = p.branches;
		refreshRepoState();
	});
	onIpc(IpcActions.BRANCH_DELETED, (p) => {
		branches.list = branches.list.filter((b) => b.name !== p.branchName);
	});
	onIpc(IpcActions.BRANCH_FORCE_DELETE_REQUESTED, (p) => {
		modal.open("branch-force-delete", { branchName: p.branchName, unmergedCount: p.unmergedCount });
	});
	onIpc(IpcActions.BRANCH_RENAMED, (p) => {
		if (branch.name === p.oldName) branch.name = p.newName;
		loadBranches();
	});
	onIpc(IpcActions.BRANCH_MERGED, () => {
		loadBranches();
		refreshRepoState();
	});
	onIpc(IpcActions.BRANCH_PUBLISHED, () => {
		branch.hasUpstream = true;
		loadBranches();
		refreshRepoState();
	});
	onIpc(IpcActions.BRANCH_HISTORY_LOADED, (p) => {
		history.setCommits(p.commits);
	});
	onIpc(IpcActions.BRANCH_HISTORY_APPENDED, (p) => {
		history.appendCommits(p.commits);
	});

	// Commit
	onIpc(IpcActions.COMMIT_DETAILS_LOADED, (p) => {
		commitDetails.message = p.message ?? "";
		commitDetails.author = p.author ?? "";
		commitDetails.files = p.files ?? [];
		commitDetails.selectedFile = "";
		commitDetails.diffText = "";
		commitDetails.loading = false;

		// Autoselect 1st file
		if (commitDetails.files.length > 0) {
			loadCommitFileDiff(commitDetails.hash, commitDetails.files[0].path);
		}
	});
	onIpc(IpcActions.COMMIT_FILE_DIFF_LOADED, (p) => {
		commitDetails.diffText = p.diffText ?? "";
		commitDetails.loadingDiff = false;
	});
	onIpc(IpcActions.COMMIT_CHECKED_OUT, () => { // FINISH
		loadBranches();
	});
	onIpc(IpcActions.COMMIT_REVERTED, () => {
		refreshRepoState();
	});

	// Change
	onIpc(IpcActions.CHANGE_DIFF_LOADED, (p) => {
		diff.diffText = p.diffText ?? "";
		diff.loading = false;
	});
	onIpc(IpcActions.CHANGE_ALL_DISCARDED, () => {
		loadRepoStatus();
	});
	onIpc(IpcActions.CHANGE_DISCARDED, () => {
		loadRepoStatus();
	});
	onIpc(IpcActions.CHANGE_IGNORED, () => {
		loadRepoStatus();
	});
	onIpc(IpcActions.CHANGES_COMMITTED, () => {
		commit.committing = false;
		commit.clear();
		refreshRepoState();
	});

	// Todo
	onIpc(IpcActions.TODO_LOADED, (p) => {
		todos.list = p?.todos ?? [];
	});
	onIpc(IpcActions.TODO_SAVED, () => { });

	// Config
	onIpc(IpcActions.CONFIG_LOCAL_LOADED, (p) => {
		config.localName = p.localName ?? "";
		config.localEmail = p.localEmail ?? "";
		config.localLoaded = true;
		config.globalName = p.globalName ?? "";
		config.globalEmail = p.globalEmail ?? "";
	});
	onIpc(IpcActions.CONFIG_LOCAL_SAVED, () => { });
	onIpc(IpcActions.CONFIG_GLOBAL_LOADED, (p) => {
		config.globalName = p.globalName ?? "";
		config.globalEmail = p.globalEmail ?? "";
		config.globalLoaded = true;
	});
	onIpc(IpcActions.CONFIG_GLOBAL_SAVED, () => { });

	// Settings / Theme
	onIpc(IpcActions.SETTINGS_SAVED, (p) => {
		app.theme = p.settings.theme;
	});
	onIpc(IpcActions.THEME_LIST_LOADED, (p) => {
		app.themes = p.themes;
		if (!p.themes.some((t) => t.id === app.theme)) {
			app.theme = "catppuccin-mocha";
			saveSettings(app.theme);
		}
	});

	// Explorer
	onIpc(IpcActions.EXPLORER_FOLDER_PICKED, (p) => {
		resolveFolderPick(p.path);
	});

	// Stash
	onIpc(IpcActions.STASH_LIST_LOADED, (p) => {
		stashes.list = p.stashes ?? [];
		stashes.loaded = true;
	});
	onIpc(IpcActions.STASH_SAVED, () => {
		loadStashes();
		refreshRepoState();
	});
	onIpc(IpcActions.STASH_POPPED, () => {
		loadStashes();
		refreshRepoState();
	});
	onIpc(IpcActions.STASH_DROPPED, () => {
		loadStashes();
	});

	// Logs
	onIpc(IpcActions.LOGS_CLEARED, () => { });
}

// Helpers
function appendRepo(r) {
	repositories.list = [...repositories.list, r];
	if (r.isValid) openRepo(r);
}

function applyBranch(b) {
	branch.reset();
	if (!b) return;
	branch.name = b.name;
	branch.revision = b.isDetached ? b.targetCommitHash : b.name;
	branch.isDetached = b.isDetached;
	branch.hasUpstream = !!b.remoteBranch;
	branch.remoteBranch = b.remoteBranch ?? "";
}

