export const changes = $state({
    files: [],
    selectedPath: "",

    get checkedPaths() { return this.files.filter((f) => f.checked).map((f) => f.path); },
    get allChecked() { return this.files.every((f) => f.checked); },

    setFiles(files) {
        const wasChecked = new Map(this.files.map((f) => [f.path, f.checked]));
        this.files = files.map((f) => ({ ...f, checked: wasChecked.get(f.path) ?? true }));
        if (!this.files.some((f) => f.path === this.selectedPath)) this.selectedPath = "";
    },
    setAllChecked(value) { this.files.forEach((f) => (f.checked = value)); },
    select(path) { this.selectedPath = path; },
    reset() { this.files = []; this.selectedPath = ""; }
});