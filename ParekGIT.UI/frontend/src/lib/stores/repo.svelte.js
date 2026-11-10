export const repo = $state({
    name: "",
    path: "",
	remoteUrl: "",
    fetching: false,
    get hasRemote() { return !!this.remoteUrl; }
});