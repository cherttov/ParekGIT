import { modal } from "../../stores/modal.svelte";
import { checkoutCommit, revertCommit } from "../../ipc/commands";

export const historyItemMenuItems = (commit, isLatest) => [
        { 
            label: "Checkout commit", disabled: isProtected(b),
            onclick: () => revertCommit(commit.hash))
        },
        { 
            label: "Revert changes in commit", disabled: !isLatest,
            onclick: () => revertCommit(commit.hash)
        },
        { 
            label: "Create branch from commit...", disabled: isProtected(b),
            onclick: () => modal.open("branch-merge", { branchName: b.name })
        },
        { 
            label: "Delete...", disabled: isProtected(b),
            onclick: () => modal.open("branch-delete", { branchName: b.name, remoteBranch: b.remoteBranch })
        },
];