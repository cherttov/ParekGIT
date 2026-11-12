import { modal } from "../../stores/modal.svelte";
import { checkoutCommit, revertCommit } from "../../ipc/commands";

// RMB on history item (revert disabled if not the latest commit)
export function historyMenuItems(c, isLatest) {
    return [
        { 
            label: "Checkout commit", 
            onclick: () => checkoutCommit(c.hash)
        },
        { 
            label: "Revert commit", 
            disabled: !isLatest, 
            onclick: () => revertCommit(c.hash)
        },
        "divider",
        { 
            label: "Create branch...", 
            onclick: () => modal.open("branch-from-commit", { hash: c.hash })
        },
        { 
            label: "Copy SHA", 
            onclick: () => navigator.clipboard.writeText(c.hash)
        }
    ];
}