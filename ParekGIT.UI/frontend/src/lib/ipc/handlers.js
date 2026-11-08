import { IpcActions } from "./actions.js";
import { onIpc, sendIpcMessage } from "./ipc.js";
import { repo } from "../stores/repo.svelte.js";
import { app } from "../stores/app.svelte.js";
import { repositories } from "../stores/repositories.svelte.js";
import { branch } from "../stores/branch.svelte.js";
import { branches } from "../stores/branches.svelte.js";
import { checkoutRepo, loadStashes, refreshRepoState, resolveFolderPick, saveSettings } from "./commands.js";
import { config } from "../stores/config.svelte.js";
import { modal } from "../stores/modal.svelte.js";
import { todos } from "../stores/todos.svelte.js";
import { stashes } from "../stores/stashes.svelte.js";

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

    onIpc(IpcActions.APP_ERROR, (p) => { modal.open("error", { message: p.message }); });

	// Repo
	onIpc(IpcActions.REPO_CHECKED_OUT, (p) => {
        repo.name = p.repo.name;
        repo.path = p.repo.absolutePath;
        repo.remoteUrl = p.repo.remoteUrl;
        repo.changes = [];
        branches.list = p.branches;
        config.localLoaded = false;
        stashes.list = [];
        stashes.loaded = false;
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
            stashes.list = [];
            stashes.loaded = false;
        }
    });
    // 	[IpcActions.REPO_COMMITTED]: processCommit,
    onIpc(IpcActions.REPO_FETCHED, () => { repo.fetching = false; });
    // 	[IpcActions.REPO_PULLED]: processRepoPulled,
    // 	[IpcActions.REPO_PUSHED]: processRepoPushed,
    // 	[IpcActions.REPO_PUBLISHED]: processRepoPublished,
    onIpc(IpcActions.REPO_STATUS_LOADED, (p) => { repo.changes = p; });
    // 	[IpcActions.REPO_FILES_CHANGED]: processFileChanges,
    // 	[IpcActions.REPO_PATH_MISSING]: processMissingRepo,
    onIpc(IpcActions.REPO_SYNC_STATUS_CHANGED, (p) => {
        branch.commitsBehind = p.commitsBehind ?? 0;
        branch.commitsAhead = p.commitsAhead ?? 0;
    });

    // Branch
    onIpc(IpcActions.BRANCH_CHECKED_OUT, (p) => {
        branches.list = p.branches;
        applyBranch(p.branch);
        refreshRepoState();
    });
    onIpc(IpcActions.BRANCH_LIST_LOADED, (p) => {
        branches.list = p.branches;
        refreshRepoState();
    });
    onIpc(IpcActions.BRANCH_DELETED, (p) => { branches.list = branches.list.filter((b) => b.name !== p.branchName); });
    onIpc(IpcActions.BRANCH_FORCE_DELETE_REQUESTED, (p) => {
        modal.open("branch-force-delete", { branchName: p.branchName, unmergedCount: p.unmergedCount });
    });
    // 	[IpcActions.BRANCH_RENAMED]: renameBranchInDropdown,
    onIpc(IpcActions.BRANCH_MERGED, () => {
        sendIpcMessage(IpcActions.BRANCH_LIST_LOAD, { repoPath: repo.path });
        refreshRepoState();
    });
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
    onIpc(IpcActions.TODO_LOADED, (p) => { todos.list = p?.todos ?? []; });
    // 	[IpcActions.TODO_SAVED]: noop,

    // Config
    onIpc(IpcActions.CONFIG_LOCAL_LOADED, (p) => {
        config.localName = p.localName ?? "";
        config.localEmail = p.localEmail ?? "";
        config.localLoaded = true;
        config.globalName = p.globalName ?? "";
        config.globalEmail = p.globalEmail ?? "";
    });
    // 	[IpcActions.CONFIG_LOCAL_SAVED]: noop,
    onIpc(IpcActions.CONFIG_GLOBAL_LOADED, (p) => {
        config.globalName = p.globalName ?? "";
        config.globalEmail = p.globalEmail ?? "";
    });
    // 	[IpcActions.CONFIG_GLOBAL_SAVED]: noop,

    // Settings / Theme
    onIpc(IpcActions.SETTINGS_SAVED, (p) => { app.theme = p.settings.theme; });
    onIpc(IpcActions.THEME_LIST_LOADED, (p) => {
        app.themes = p.themes;
        if (!p.themes.some((t) => t.id === app.theme)) {
            app.theme = "catppuccin-mocha";
            saveSettings(app.theme);
        }
    });

    // Explorer
    onIpc(IpcActions.EXPLORER_FOLDER_PICKED, (p) => resolveFolderPick(p.path));

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
    onIpc(IpcActions.STASH_DROPPED, () => { loadStashes(); });

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
    branch.remoteBranch = b.remoteBranch ?? "";
}

