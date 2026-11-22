import { modal } from "../../stores/modal.svelte";
import { repo } from "../../stores/repo.svelte";
import { branch } from "../../stores/branch.svelte";

const protectedBranches = ["main", "master"];
const isProtected = (b) => b.isDetached || protectedBranches.includes(b.name.toLowerCase());
const isCurrent = (b) => !b.isDetached && b.name === branch.name;

// "New" item in contextMenu
const newBranchItem = { label: "New...", onclick: () => modal.open("branch-new") };

// RMB on branch-dropdown item
export function branchMenuItems(b) {
	return [
		{
			label: "Rename...",
			disabled: isProtected(b),
			onclick: () => modal.open("branch-rename", { branchName: b.name })
		},
		{
			label: "Copy branch name",
			onclick: () => navigator.clipboard.writeText(b.name)
		},
		{
			label: "Merge into this branch...",
			disabled: b.isDetached,
			onclick: () => modal.open("branch-merge", { targetBranch: b.name })
		},
		"divider",
		{ 
			label: "Delete...",
			disabled: isProtected(b) || isCurrent(b),
			onclick: () => modal.open("branch-delete", { branchName: b.name, remoteBranch: b.remoteBranch })
		}
	];
}

// RMB on topbar branch
export function topbarBranchMenuItems(b) {
	if (!repo.path) return [];
	if (!b.name && !b.isDetached) return [newBranchItem];
	return [newBranchItem, "divider", ...branchMenuItems(b)];
}