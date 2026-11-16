export const commitDetails = $state({
    hash: "",
    message: "",
    author: "",
    files: [],
    selectedFile: "",
    diffText: "",
    loading: false,
    loadingDiff: false,

    startLoad(hash) {
        this.hash = hash;
        this.message = "";
        this.author = "";
        this.files = [];
        this.selectedFile = "";
        this.diffText = "";
        this.loading = true;
        this.loadingDiff = false;
    },
    reset() {
        this.hash = ""; this.message = ""; this.author = "";
        this.files = []; this.selectedFile = "";
        this.diffText = ""; this.loading = false; this.loadingDiff = false;
    }
});