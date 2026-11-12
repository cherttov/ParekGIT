import { modal } from "../../stores/modal.svelte";
import { repo } from "../../stores/repo.svelte";
import { openTerminal, showInExplorer } from "../../ipc/commands";

// Dropdown ADD button + top part of repo-topbar context menu
export const addRepoItems = [
    { 
        label: "Clone repository...", 
        onclick: () => modal.open("repo-clone") 
    },
    { 
        label: "Create new repository...", 
        onclick: () => modal.open("repo-create") 
    },
    { 
        label: "Add existing repository...", 
        onclick: () => modal.open("repo-add") 
    }
];

// Tools, same for both repo context-menus
function toolItems(path, name, isInvalid) {
    return [
        { 
            label: "Open in terminal", 
            disabled: isInvalid, 
            onclick: () => openTerminal(path) 
        },
        { 
            label: "Show in explorer", 
            disabled: isInvalid, 
            onclick: () => showInExplorer(path) 
        },
        "divider",
        { 
            label: "Remove...", 
            onclick: () => modal.open("repo-remove", { repoPath: path, repoName: name, isInvalid }) 
        }
    ];
}

// RMB on repo-dropdown item
export const repoMenuItems = (r) => toolItems(r.absolutePath, r.name, !r.isValid);

// RMB on repo-topbar
export function topbarRepoMenuItems(r) {
    if (!r.path) return addRepoItems;
    return [
        ...addRepoItems, 
        "divider", 
        ...toolItems(r.path, r.name, false)
    ];
}