export const view = $state({
	tab: "changes", // "changes" || "history"

	reset() { this.tab = "changes"; }
});