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