import { modal } from "../../stores/modal.svelte";

const protectedBranches = ["main", "master"];
export const isProtected = (b) => b.isDetached || protectedBranches.includes(b.name.toLowerCase());

export function branchMenuItems(b) {
    return [
        { 
            label: "Rename...", disabled: isProtected(b),
            onclick: () => modal.open("branch-rename", { branchName: b.name })
        },
        { 
            label: "Copy branch name",
            onclick: () => navigator.clipboard.writeText(b.name)
        },
        { 
            label: "Merge branch...", disabled: isProtected(b),
            onclick: () => modal.open("branch-merge", { branchName: b.name })
        },
        { 
            label: "Delete...", disabled: isProtected(b),
            onclick: () => modal.open("branch-delete", { branchName: b.name, remoteBranch: b.remoteBranch })
        },
    ];
}