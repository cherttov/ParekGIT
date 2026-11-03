export const repo = $state({
    name: "",
    path: "",
	remoteUrl: "",
    changes: [],
    fetching: false,
    get hasRemote() { return !!this.remoteUrl; }
});