import { IpcActions } from "./actions.js";
import { onIpc, sendIpcMessage } from "./ipc.js";
import { repo } from "../stores/repo.svelte.js";
import { app } from "../stores/app.svelte.js";
import { repositories } from "../stores/repositories.svelte.js";
import { branch } from "../stores/branch.svelte.js";
import { branches } from "../stores/branches.svelte.js";
import { checkoutRepo, refreshRepoState } from "./commands.js";

export function registerHandlers() {
    // App
    onIpc(IpcActions.APP_INITIALIZED, (payload) => {
        app.theme = payload.settings.theme;
        app.themes = payload.themes;
        repositories.list = payload.repositories;
        if (payload.settings.lastRepoPath) {
            const lastRepo = payload.repositories.find(
                (r) => r.absolutePath === payload.settings.lastRepoPath
            );
            if (lastRepo) checkoutRepo(lastRepo);
        }
        app.ready = true;
    });

    onIpc(IpcActions.APP_ERROR, (payload) => {
        // showErrorModal
    });

	// Repo
	onIpc(IpcActions.REPO_CHECKED_OUT, (payload) => {
        repo.name = payload.repo.name,
        repo.path = payload.repo.absolutePath,
        repo.remoteUrl = payload.repo.remoteUrl
        repo.changes = [];
        branches.list = payload.branches;
        applyBranch(payload.branch);
        refreshRepoState();
	});
    onIpc(IpcActions.REPO_CLONED, addRepo);
    onIpc(IpcActions.REPO_CREATED, addRepo);
    onIpc(IpcActions.REPO_ADDED, addRepo);
    onIpc(IpcActions.REPO_REMOVED, (payload) => {
        repositories.list = repositories.list.filter(r => r.absolutePath !== payload.repoPath);
        if (repo.path === payload.repoPath) {
            repo.name = "";
            repo.path = "";
            repo.remoteUrl = "";
            branch.name = "";
            branches.list = [];
        }
    });
    // 	[IpcActions.REPO_COMMITTED]: processCommit,
    onIpc(IpcActions.REPO_FETCHED, () => {
        repo.fetching = false;
    });
    // 	[IpcActions.REPO_PULLED]: processRepoPulled,
    // 	[IpcActions.REPO_PUSHED]: processRepoPushed,
    // 	[IpcActions.REPO_PUBLISHED]: processRepoPublished,
    onIpc(IpcActions.REPO_STATUS_LOADED, (payload) => {
        repo.changes = payload;
    });
    // 	[IpcActions.REPO_FILES_CHANGED]: processFileChanges,
    // 	[IpcActions.REPO_PATH_MISSING]: processMissingRepo,
    onIpc(IpcActions.REPO_SYNC_STATUS_CHANGED, (payload) => {
        branch.commitsBehind = payload.commitsBehind || 0;
        branch.commitsAhead = payload.commitsAhead || 0;
    });

    // Branch
    onIpc(IpcActions.BRANCH_CHECKED_OUT, (payload) => {
        branches.list = payload.branches;
        applyBranch(payload.branch);
        refreshRepoState();
    })
    onIpc(IpcActions.BRANCH_LIST_LOADED, (payload) => {
        applyBranch(payload.branches);
        refreshRepoState();
    });
    // 	[IpcActions.BRANCH_DELETED]: removeBranchFromDropdown,
    // 	[IpcActions.BRANCH_FORCE_DELETE_REQUESTED]: processBranchForceDeleteRequested,
    // 	[IpcActions.BRANCH_RENAMED]: renameBranchInDropdown,
    // 	[IpcActions.BRANCH_MERGED]: processBranchesMerged,
    // 	[IpcActions.BRANCH_HISTORY_LOADED]: processBranchHistoryLoaded,
    // 	[IpcActions.BRANCH_HISTORY_APPENDED]: processBranchHistoryAppended,

    // Commit / File
    // 	[IpcActions.COMMIT_DETAILS_LOADED]: loadCommitDetails,
    // 	[IpcActions.COMMIT_FILE_DIFF_LOADED]: processCommitFileDiffLoaded,
    // 	[IpcActions.COMMIT_CHECKED_OUT]: processCommitCheckedOut, // FINISH
    // 	[IpcActions.COMMIT_REVERTED]: processCommitReverted,
    // 	[IpcActions.FILE_DIFF_LOADED]: processFileDiffLoaded,

    // Change
    // 	[IpcActions.CHANGE_ALL_DISCARDED]: noop,
    // 	[IpcActions.CHANGE_DISCARDED]: noop,
    // 	[IpcActions.CHANGE_IGNORED]: processFileChanges, // REVISIT

    // Todo
    // 	[IpcActions.TODO_LOADED]: loadTodos,
    // 	[IpcActions.TODO_SAVED]: noop,

    // Config
    // 	[IpcActions.CONFIG_LOCAL_LOADED]: processLocalConfigLoad,
    // 	[IpcActions.CONFIG_LOCAL_SAVED]: noop,
    // 	[IpcActions.CONFIG_GLOBAL_LOADED]: processGlobalConfigLoad,
    // 	[IpcActions.CONFIG_GLOBAL_SAVED]: noop,

    // Settings / Theme
    // 	[IpcActions.SETTINGS_SAVED]: processSettingsSaved,
    // 	[IpcActions.THEME_LIST_LOADED]: processThemesLoaded,

    // Explorer
    // 	[IpcActions.EXPLORER_FOLDER_PICKED]: folderSelected,

    // Stash
    // 	[IpcActions.STASH_LIST_LOADED]: loadStashes,
    // 	[IpcActions.STASH_SAVED]: processStashChange,
    // 	[IpcActions.STASH_POPPED]: processStashChange,
    // 	[IpcActions.STASH_DROPPED]: processStashDropped,

    // Logs
    // 	[IpcActions.LOGS_CLEARED]: noop,
}

// Helpers
function addRepo(r) {
    repositories.list = [...repositories.list, r];
}

function applyBranch(b) {
    if (!b) { branch.name = ""; branch.revision = ""; return; }
    branch.name = b.name;
    branch.revision = b.isDetached ? b.targetCommitHash : b.name;
    branch.isDetached = b.isDetached;
    branch.hasUpstream = !!b.remoteBranch;
    branch.remoteBranch = b.remoteBranch || "";
}

