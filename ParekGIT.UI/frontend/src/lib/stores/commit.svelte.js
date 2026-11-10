const drafts = {};
export const commit = $state({
    message: "",
    description: "",
    committing: false,

    switchRepo(fromPath, toPath) {
        if (fromPath) drafts[fromPath] = { message: this.message, description: this.description };
        const d = drafts[toPath];
        this.message = d?.message ?? "";
        this.description = d?.description ?? "";
        this.committing = false;
    },
    clear() { this.message = ""; this.description = ""; },
    forget(path) { delete drafts[path]; }
});