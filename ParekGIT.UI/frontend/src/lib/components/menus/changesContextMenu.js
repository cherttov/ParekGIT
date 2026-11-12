import { modal } from "../../stores/modal.svelte";
import { repo } from "../../stores/repo.svelte";
import { changes } from "../../stores/changes.svelte";
import { ignoreFile, showInExplorer } from "../../ipc/commands";

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
            label: "Discard changes...", 
            onclick: () => modal.open("change-discard", { path: file.path }) 
        },
        { 
            label: "Ignore file", 
            onclick: () => ignoreFile(file.path)
        },
        "divider",
        { 
            label: "Copy absolute path", 
            onclick: () => navigator.clipboard.writeText(toAbsolute(file.path).abs)
        },
        { 
            label: "Copy relative path", 
            onclick: () => navigator.clipboard.writeText(file.path) 
        },
        "divider",
        {
            label: "Show in Explorer",
            onclick: () => { const { abs, sep } = toAbsolute(file.path); showInExplorer(abs.substring(0, abs.lastIndexOf(sep))); }
        }
    ];
}

// RMB on the changes header
export function changesMasterMenuItems() {
    const total = changes.files.length;
    const paths = changes.checkedPaths;
    return [
        {
            label: "Discard all changes...",
            disabled: total === 0,
            onclick: () => modal.open("changes-dicard-all", { count: total })
        },
        {
            label: "Discard selected changes...", 
            disabled: total === 0,
            onclick: () => modal.open("changes-discard-selected", { paths: [...paths] })
        }
    ];
}