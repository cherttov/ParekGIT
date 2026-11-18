export const config = $state({
	localName: "",
	localEmail: "",
	localLoaded: false,
	globalName: "",
	globalEmail: "",
	globalLoaded: false,

	resetLocal() { this.localName = ""; this.localEmail = ""; this.localLoaded = false; },
	resetGlobal() { this.globalName = ""; this.globalEmail = ""; this.globalLoaded = false; },
	reset() {
		this.localName = ""; this.localEmail = "", this.localLoaded = false;
		this.globalEmail = ""; this.globalName = ""; this.globalLoaded = false;
	}
});