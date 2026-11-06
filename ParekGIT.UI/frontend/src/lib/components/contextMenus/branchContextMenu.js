import { modal } from "../../stores/modal.svelte";

const protectedBranches = ["main", "master"];
const isProtected = (b) => b.isDetached || protectedBranches.includes(b.name.toLowerCase());

// RMB on branch-dropdown item
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
            label: "Merge branch...", disabled: b.isDetached,
            onclick: () => modal.open("branch-merge", { branchName: b.name })
        },
        "divider",
        { 
            label: "Delete...", disabled: isProtected(b),
            onclick: () => modal.open("branch-delete", { branchName: b.name, remoteBranch: b.remoteBranch })
        },
    ];
}

// RMB on topbar branch
export const topbarBranchMenuItems = (b) => [
    { 
        label: "New...", onclick: () => modal.open("branch-new") 
    },
    "divider",
    ...branchMenuItems(b)
];