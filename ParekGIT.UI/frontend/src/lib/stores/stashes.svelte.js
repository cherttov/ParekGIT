export const stashes = $state({
	list: [],
	loaded: false,

	reset() { this.list = []; this.loaded = false; }
});