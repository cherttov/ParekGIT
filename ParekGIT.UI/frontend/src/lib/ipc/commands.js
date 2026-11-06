import { sendIpcMessage } from "./ipc";
import { IpcActions } from "./actions";
import { repo } from "../stores/repo.svelte";
import { branch } from "../stores/branch.svelte";
import { app } from "../stores/app.svelte";

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
    if (b.isDetached || branch.name === b.name) return;
    sendIpcMessage(IpcActions.BRANCH_CHECKOUT, { repoPath: repo.path, branchName: b.name, isRemote: b.isRemote });
}

export function createBranch(branchName) {
    if (!repo.path) return;
    sendIpcMessage(IpcActions.BRANCH_CREATE, { repoPath: repo.path, branchName });
}

export function deleteBranch(branchName, remoteBranch = "") {
    sendIpcMessage(IpcActions.BRANCH_DELETE, { repoPath: repo.path, branchName, isRemote: false });
    if (remoteBranch) {
        sendIpcMessage(IpcActions.BRANCH_DELETE, { repoPath: repo.path, branchName: remoteBranch, isRemote: true });
    }
}

export function forceDeleteBranch(branchName) {
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
    sendIpcMessage(IpcActions.REPO_PUBLISH, { repoPath: repo.path, branchName: branch.name });
}

export function pullRepo() {
    sendIpcMessage(IpcActions.REPO_PULL, { repoPath: repo.path });
}

export function pushRepo() {
    sendIpcMessage(IpcActions.REPO_PUSH, { repoPath: repo.path });
}

export function openTerminal(repoPath) {
    sendIpcMessage(IpcActions.REPO_TERMINAL, { repoPath });
}

export function showInExplorer(path) {
    sendIpcMessage(IpcActions.EXPLORER_SHOW, { path });
}

export function loadGlobalConfig() {
    sendIpcMessage(IpcActions.CONFIG_GLOBAL_LOAD, {});
}

export function pickFolder(onpicked) {
    app.pendingFolderPick = onpicked;
    sendIpcMessage(IpcActions.EXPLORER_PICK_FOLDER);
}

export function resolveFolderPick(path) {
    app.pendingFolderPick?.(path);
    app.pendingFolderPick = null;
}