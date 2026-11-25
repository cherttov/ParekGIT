export const todos = $state({
	list: [],

	reset() { this.list = []; }
});