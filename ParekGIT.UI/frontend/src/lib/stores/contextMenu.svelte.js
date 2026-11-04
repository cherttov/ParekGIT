export const contextMenu = $state({
    visible: false,
    items: [],
    x: 0,
    y: 0,
    key: null, // highlighted item (for .context-active)

    open(event, items, key = null) {
        event.preventDefault();
        event.stopPropagation();
        this.items = items;
        this.x = event.clientX;
        this.y = event.clientY;
        this.key = key;
        this.visible = true;
    },
    close() {
        this.visible = false;
        this.key = null;
    }
})