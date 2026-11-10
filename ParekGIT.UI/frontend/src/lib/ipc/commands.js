import { sendIpcMessage } from "./ipc";
import { IpcActions } from "./actions";
import { repo } from "../stores/repo.svelte";
import { branch } from "../stores/branch.svelte";
import { config } from "../stores/config.svelte";
import { app } from "../stores/app.svelte";
import { commit } from "../stores/commit.svelte";
import { changes } from "../stores/changes.svelte";

export function checkoutRepo(r) {
    if (!r.isValid || repo.path === r.absolutePath) return;
    sendIpcMessage(IpcActions.REPO_CHECKOUT, { repoPath: r.absolutePath });
}

export function addRepo(repoPath) {
    sendIpcMessage(IpcActions.REPO_ADD, { repoPath });
}

export function cloneRepo(p) {
    sendIpcMessage(IpcActions.REPO_CLONE, p);
}

export function createRepo(p) {
    sendIpcMessage(IpcActions.REPO_CREATE, p);
}

export function removeRepo(repoPath, deleteLocal = false) {
    sendIpcMessage(IpcActions.REPO_REMOVE, { repoPath, deleteLocal });
}

export function checkoutBranch(b) {
    if (!repo.path || b.isDetached || branch.name === b.name) return;
    sendIpcMessage(IpcActions.BRANCH_CHECKOUT, { repoPath: repo.path, branchName: b.name, isRemote: b.isRemote });
}

export function createBranch(branchName) {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.BRANCH_CREATE, { repoPath: repo.path, branchName });
}

export function deleteBranch(branchName, remoteBranch = "") {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.BRANCH_DELETE, { repoPath: repo.path, branchName, isRemote: false });
    if (remoteBranch) {
        sendIpcMessage(IpcActions.BRANCH_DELETE, { repoPath: repo.path, branchName: remoteBranch, isRemote: true });
    }
}

export function forceDeleteBranch(branchName) {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.BRANCH_FORCE_DELETE, { repoPath: repo.path, branchName });
}

export function refreshRepoState() {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.REPO_STATUS_LOAD, { repoPath: repo.path });
    if (branch.revision) {
        sendIpcMessage(IpcActions.BRANCH_HISTORY_LOAD, { repoPath: repo.path, branchName: branch.revision });
        sendIpcMessage(IpcActions.TODO_LOAD, { repoPath: repo.path, branchName: branch.revision });
    }
}

export function fetchRepo() {
    if (!repo.path || repo.fetching) return;
    repo.fetching = true;
    sendIpcMessage(IpcActions.REPO_FETCH, { repoPath: repo.path });
}

export function publishBranch() {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.REPO_PUBLISH, { repoPath: repo.path, branchName: branch.name });
}

export function pullRepo() {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.REPO_PULL, { repoPath: repo.path });
}

export function pushRepo() {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.REPO_PUSH, { repoPath: repo.path });
}

export function openTerminal(repoPath) {
    sendIpcMessage(IpcActions.REPO_TERMINAL, { repoPath });
}

export function showInExplorer(path) {
    sendIpcMessage(IpcActions.EXPLORER_SHOW, { path });
}

export function pickFolder(onpicked) {
    app.pendingFolderPick = onpicked;
    sendIpcMessage(IpcActions.EXPLORER_PICK_FOLDER);
}

export function resolveFolderPick(path) {
    app.pendingFolderPick?.(path);
    app.pendingFolderPick = null;
}

export function mergeBranches(sourceBranch, targetBranch) {
    if (!repo.path || !sourceBranch || !targetBranch || sourceBranch === targetBranch) return;
    sendIpcMessage(IpcActions.BRANCH_MERGE, { repoPath: repo.path, sourceBranch, targetBranch });
}

export function loadTodos() {
    if (!repo.path || !branch.revision) return;
    sendIpcMessage(IpcActions.TODO_LOAD, { repoPath: repo.path, branchName: branch.revision });
}

export function saveTodos(todoList) {
    if (!repo.path || !branch.revision) return;
    sendIpcMessage(IpcActions.TODO_SAVE, { repoPath: repo.path, branchName: branch.revision, todos: todoList });
}

export function loadLocalConfig() {
    if (!repo.path) return;
    config.localName = "";
    config.localEmail = "";
    config.localLoaded = false;
    sendIpcMessage(IpcActions.CONFIG_LOCAL_LOAD, { repoPath: repo.path });
}

export function saveLocalConfig(name, email) {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.CONFIG_LOCAL_SAVE, { repoPath: repo.path, name, email });
}

export function loadGlobalConfig() {
    sendIpcMessage(IpcActions.CONFIG_GLOBAL_LOAD);
}

export function saveGlobalConfig(name, email) {
    sendIpcMessage(IpcActions.CONFIG_GLOBAL_SAVE, { name, email });
}

export function loadThemes() {
    sendIpcMessage(IpcActions.THEME_LIST_LOAD);
}

export function saveSettings(theme) {
    sendIpcMessage(IpcActions.SETTINGS_SAVE, { theme });
}

export function clearLogs() {
    sendIpcMessage(IpcActions.LOGS_CLEAR);
}

export function loadStashes() {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.STASH_LIST_LOAD, { repoPath: repo.path });
}

export function saveStash(message, files) {
    if (!repo.path || files.length === 0) return;
    sendIpcMessage(IpcActions.STASH_SAVE, { repoPath: repo.path, message, files });
}

export function popStash(selector) {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.STASH_POP, { repoPath: repo.path, selector });
}

export function dropStash(selector) {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.STASH_DROP, { repoPath: repo.path, selector });
}

export function loadStatus() {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.REPO_STATUS_LOAD, { repoPath: repo.path });
}

export function commitChanges() {
    const message = commit.message.trim();
    const files = changes.checkedPaths;
    if (!repo.path || !message || files.length === 0 || commit.committing) return;

    commit.committing = true;
    sendIpcMessage(IpcActions.REPO_COMMIT, { 
        repoPath: repo.path, 
        message, 
        description: commit.description.trim(), 
        files 
    });
}

export function discardChange(filePath) {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.CHANGE_DISCARD, { repoPath: repo.path, filePath });
}

export function discardSelected(paths) { paths.forEach(discardChange); }

export function discardAll() {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.CHANGE_ALL_DISCARD, { repoPath: repo.path });
}