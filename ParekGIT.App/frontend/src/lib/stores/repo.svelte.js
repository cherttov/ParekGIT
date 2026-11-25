export const repo = $state({
	name: "",
	path: "",
	remoteUrl: "",
	fetching: false,

	get hasRemote() { return !!this.remoteUrl; },

	reset() {
		this.name = ""; this.path = "";
		this.remoteUrl = ""; this.fetching = false;
	}
});