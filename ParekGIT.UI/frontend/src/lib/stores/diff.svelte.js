export const diff = $state({
	filePath: "",
	diffText: "",
	loading: false,

	reset() { this.filePath = ""; this.diffText = ""; this.loading = false; }
});