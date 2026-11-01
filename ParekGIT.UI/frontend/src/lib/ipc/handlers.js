import { IpcActions } from "./actions.js";
import { onIpc } from "./ipc.js";
import { repo } from "../stores/repo.svelte.js";
import { app } from "../stores/app.svelte.js";
import { repositories } from "../stores/repositories.svelte.js";
import { branch } from "../stores/branch.svelte.js";
import { branches } from "../stores/branches.svelte.js";

export function registerHandlers() {
    // App
    onIpc(IpcActions.APP_INITIALIZED, (payload) => {
        app.theme = payload.settings.theme;
        app.themes = payload.themes;
        repositories.list = payload.repositories;
        app.ready = true;
    });

    onIpc(IpcActions.APP_ERROR, (payload) => {
        // showErrorModal
    });

    // Repo
    onIpc(IpcActions.REPO_CLONED, addRepo);
    onIpc(IpcActions.REPO_CREATED, addRepo);
    onIpc(IpcActions.REPO_ADDED, addRepo);
    onIpc(IpcActions.REPO_REMOVED, (payload) => {
        repositories.list = repositories.list.filter(r => r.absolutePath !== payload.repoPath);
        if (repo.path === payload.repoPath) {
            repo.name = "";
            repo.path = "";
            repo.hasRemote = false;
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
    onIpc(IpcActions.BRANCH_LIST_LOADED, (payload) => {
        branches.list = payload;
        const current = payload.find((b) => b.isCurrent);
        if (current) {
            branch.name = current.isDetached ? current.targetCommitHash : current.name;
            branch.revision = current.isDetached ? current.targetCommitHash : current.name;
            branch.isDetached = current.isDetached;
            branch.hasUpstream = !!current.remoteBranch;
            branch.remoteBranch = current.remoteBranch || "";
        }
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