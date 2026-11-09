export const repo = $state({
    name: "",
    path: "",
	remoteUrl: "",
    changes: [],
    fetching: false,
    committing: false,
    get hasRemote() { return !!this.remoteUrl; }
});