export const branch = $state({
	name: "",
	revision: "",
	isDetached: false,
	hasUpstream: false,
	remoteBranch: "",
	commitsAhead: 0,
	commitsBehind: 0,

	reset() {
		this.name = ""; this.revision = "";
		this.isDetached = false; this.hasUpstream = false; this.remoteBranch = "";
		this.commitsAhead = 0; this.commitsBehind = 0;
	}
});