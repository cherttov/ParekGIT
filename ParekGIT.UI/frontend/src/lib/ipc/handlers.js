import { IpcActions } from "./actions.js";
import { onIpc } from "./ipc.js";
import { repo } from "../stores/repo.svelte.js";
import { app } from "../stores/app.svelte.js";
import { repositories } from "../stores/repositories.svelte.js";
import { branch } from "../stores/branch.svelte.js";
import { branches } from "../stores/branches.svelte.js";
import { checkoutRepo, refreshRepoState, resolveFolderPick } from "./commands.js";
import { config } from "../stores/config.svelte.js";
import { modal } from "../stores/modal.svelte.js";

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
            if (lastRepo) checkoutRepo(lastRepo);
        }
        app.ready = true;
    });

    onIpc(IpcActions.APP_ERROR, (p) => {
        // showErrorModal
    });

	// Repo
	onIpc(IpcActions.REPO_CHECKED_OUT, (p) => {
        repo.name = p.repo.name;
        repo.path = p.repo.absolutePath;
        repo.remoteUrl = p.repo.remoteUrl;
        repo.changes = [];
        branches.list = p.branches;
        applyBranch(p.branch);
        refreshRepoState();
	});
    onIpc(IpcActions.REPO_CLONED, (p) => { appendRepo(p.repo) });
    onIpc(IpcActions.REPO_CREATED, (p) => { appendRepo(p.repo) });
    onIpc(IpcActions.REPO_ADDED, (p) => { appendRepo(p.repo) });
    onIpc(IpcActions.REPO_REMOVED, (p) => {
        repositories.list = repositories.list.filter(r => r.absolutePath !== p.repoPath);
        if (repo.path === p.repoPath) {
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
    onIpc(IpcActions.REPO_STATUS_LOADED, (p) => {
        repo.changes = p;
    });
    // 	[IpcActions.REPO_FILES_CHANGED]: processFileChanges,
    // 	[IpcActions.REPO_PATH_MISSING]: processMissingRepo,
    onIpc(IpcActions.REPO_SYNC_STATUS_CHANGED, (p) => {
        branch.commitsBehind = p.commitsBehind || 0;
        branch.commitsAhead = p.commitsAhead || 0;
    });

    // Branch
    onIpc(IpcActions.BRANCH_CHECKED_OUT, (p) => {
        branches.list = p.branches;
        applyBranch(p.branch);
        refreshRepoState();
    })
    onIpc(IpcActions.BRANCH_LIST_LOADED, (p) => {
        branches.list = p.branches;
        refreshRepoState();
    });
    onIpc(IpcActions.BRANCH_DELETED, (p) => {
        branches.list = branches.list.filter((b) => b.name !== p.branchName);
    });
    onIpc(IpcActions.BRANCH_FORCE_DELETE_REQUESTED, (p) => {
        modal.open("branch-force-delete", { branchName: p.branchName, unmergedCount: p.unmergedCount });
    })
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
    onIpc(IpcActions.CONFIG_GLOBAL_LOADED, (p) => {
        config.globalName = p.globalName || "";
        config.globalEmail = p.globalEmail || "";
    });
    // 	[IpcActions.CONFIG_GLOBAL_SAVED]: noop,

    // Settings / Theme
    // 	[IpcActions.SETTINGS_SAVED]: processSettingsSaved,
    // 	[IpcActions.THEME_LIST_LOADED]: processThemesLoaded,

    // Explorer
    onIpc(IpcActions.EXPLORER_FOLDER_PICKED, (p) => resolveFolderPick(p.path));

    // Stash
    // 	[IpcActions.STASH_LIST_LOADED]: loadStashes,
    // 	[IpcActions.STASH_SAVED]: processStashChange,
    // 	[IpcActions.STASH_POPPED]: processStashChange,
    // 	[IpcActions.STASH_DROPPED]: processStashDropped,

    // Logs
    // 	[IpcActions.LOGS_CLEARED]: noop,
}

// Helpers
function appendRepo(r) {
    repositories.list = [...repositories.list, r];
    if (r.isValid) checkoutRepo(r);
}

function applyBranch(b) {
    if (!b) { branch.name = ""; branch.revision = ""; return; }
    branch.name = b.name;
    branch.revision = b.isDetached ? b.targetCommitHash : b.name;
    branch.isDetached = b.isDetached;
    branch.hasUpstream = !!b.remoteBranch;
    branch.remoteBranch = b.remoteBranch || "";
}

