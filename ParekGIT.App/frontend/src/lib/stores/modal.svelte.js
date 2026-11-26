export const modal = $state({
	previous: null,
	active: null,
	data: {},

	open(id, data = {}) {
		this.previous = this.active;
		this.active = id;
		this.data = data;
	},
	close() {
		this.previous = null;
		this.active = null;
	},

	reset() {
		this.previous = null;
		this.active = null;
		this.data = {};
	}
});