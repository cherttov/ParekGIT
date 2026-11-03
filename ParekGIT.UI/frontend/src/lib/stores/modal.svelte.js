export const modal = $state({
    active: null,
    data: {},

    open(id, data = {}) {
        this.active = id;
        this.data = data;
    },
    close() {
        this.active = null;
        this.data = {};
    }
});