import { sendIpcMessage } from "./ipc";
import { IpcActions } from "./actions";
import { repo } from "../stores/repo.svelte";
import { branch } from "../stores/branch.svelte";

export function checkoutRepo(r) {
    if (!r.isValid || repo.path === r.absolutePath) return;
    sendIpcMessage(IpcActions.REPO_CHECKOUT, { repoPath: r.absolutePath });
}

export function checkoutBranch(b) {
    if (b.isDetached || branch.name === b.name) return;
    sendIpcMessage(IpcActions.BRANCH_CHECKOUT, {
        repoPath: repo.path,
        branchName: b.name,
        isRemote: b.isRemote
    });
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