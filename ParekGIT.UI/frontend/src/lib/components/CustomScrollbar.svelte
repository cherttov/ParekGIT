<script>
    let { container } = $props();

    const scrollbarEdgePadding = 6;
    const scrollbarMinThumbHeight = 30;

    let thumbEl = $state(null);
    let thumbHeight = $state(0);
    let thumbTop = $state(scrollbarEdgePadding);
    let visible = $state(false);
    let isDragging = false;
    let startY = 0;
    let startScrollTop = 0;

    function getMetrics() {
        const containerH = container.clientHeight;
        const contentH = container.scrollHeight;
        const usableTrackHeight = containerH - scrollbarEdgePadding * 2;
        const h = Math.max((containerH / contentH) * usableTrackHeight, scrollbarMinThumbHeight);
        const maxThumbTop = usableTrackHeight - h;
        return { containerH, contentH, usableTrackHeight, thumbHeight: h, maxThumbTop }; 
    }

    function update() {
        if (!container || !thumbEl) return;
        const { containerH, contentH, thumbHeight: h, maxThumbTop } = getMetrics();

        if (contentH <= containerH) { visible = false; return; }
        visible = true;
        
        const scrollPct = container.scrollTop / (contentH - containerH);
        thumbHeight = h;
        thumbTop = scrollbarEdgePadding + scrollPct * maxThumbTop;
    }

    function onMouseDown(e) {
        if (e.button !== 0) return;
        isDragging = true;
        startY = e.clientY;
        startScrollTop = container.scrollTop;
        thumbEl.classList.add("focused");
        document.body.style.userSelect = "none";
        document.addEventListener("mousemove", onMouseMove);
        document.addEventListener("mouseup", onMouseUp);
    }

    function onMouseMove(e) {
        if (!isDragging) return;
        const { containerH, contentH, maxThumbTop } = getMetrics();
        if (maxThumbTop <= 0) return;
        const scrollRatio = (contentH - containerH) / maxThumbTop;
        container.scrollTop = startScrollTop + (e.clientY - startY) * scrollRatio;
    }

    function onMouseUp() {
        isDragging = false;
        thumbEl?.classList.remove("focused");
        document.body.style.userSelect = "";
        document.removeEventListener("mousemove", onMouseMove);
        document.removeEventListener("mouseup", onMouseUp);
    }

    $effect(() => {
        if (!container) return;
        container.addEventListener("scroll", update);
        const ro = new ResizeObserver(update);
        ro.observe(container);
        update();
        return () => {
            container.removeEventListener("scroll", update);
            ro.disconnect();
        };
    });
</script>

<!-- svelte-ignore a11y_no_static_element_interactions -->
<div
    bind:this={thumbEl}
    class="custom-scrollbar"
    style:display={visible ? "block" : "none"}
    style:height="{thumbHeight}px"
    style:transform="translateY({thumbTop}px)"
    onmousedown={onMouseDown}
></div>