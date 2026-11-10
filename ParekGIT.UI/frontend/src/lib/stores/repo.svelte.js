export const repo = $state({
    name: "",
    path: "",
	remoteUrl: "",
    fetching: false,
    committing: false,
    get hasRemote() { return !!this.remoteUrl; }
});