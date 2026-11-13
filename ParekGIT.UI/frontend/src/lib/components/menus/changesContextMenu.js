import { modal } from "../../stores/modal.svelte";
import { repo } from "../../stores/repo.svelte";
import { changes } from "../../stores/changes.svelte";
import { discardChange, ignoreFile, showInExplorer } from "../../ipc/commands";

// Builds an absolute path based on repo path separators (Windows/Unix)
function toAbsolute(relPath) {
    const sep = repo.path.includes("\\") ? "\\" : "/";
    const base = repo.path.replace(/[\\/]+$/, "");
    return { abs: base + sep + relPath.replace(/[\\/]+/g, sep), sep };
}

// RMB on a changed file
export function changeMenuItems(file) {
    return [
        { 
            label: "Discard changes", 
            onclick: () => discardChange(file.path)
        },
        "divider",
        { 
            label: "Ignore file (add to .gitignore)", 
            onclick: () => ignoreFile(file.path)
        },
        "divider",
        { 
            label: "Copy absolute file path", 
            onclick: () => navigator.clipboard.writeText(toAbsolute(file.path).abs)
        },
        { 
            label: "Copy relative file path", 
            onclick: () => navigator.clipboard.writeText(file.path) 
        },
        "divider",
        {
            label: "Show in explorer",
            onclick: () => { const { abs, sep } = toAbsolute(file.path); showInExplorer(abs.substring(0, abs.lastIndexOf(sep))); }
        }
    ];
}

// RMB on the changes header
export function changesMasterMenuItems() {
    return [
        {
            label: "Discard all changes...",
            disabled: changes.files.length === 0,
            onclick: () => modal.open("changes-dicard-all")
        },
        {
            label: "Discard selected changes...", 
            disabled: changes.files.length === 0,
            onclick: () => modal.open("changes-discard-selected")
        }
    ];
}