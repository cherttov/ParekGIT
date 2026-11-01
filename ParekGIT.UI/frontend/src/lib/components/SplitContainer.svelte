<script>
    import MainContent from "./MainContent.svelte";
    import RightSidebar from "./RightSidebar.svelte";
    import { layout } from "../stores/layout.svelte";

    let splitContainerEl = $state(null);
    let rightSidebarEl = $state(null);
    let mainContentEl = $state(null);
    let resizerEl = $state(null);

    let isResizing = false;
    let minRightWidth = 0;
    let maxRightWidth = 0;
    
    function onResizerMouseDown(e) {
        if (e.button !== 0) return;
        if (!splitContainerEl || !rightSidebarEl || !mainContentEl || !resizerEl) return;
        isResizing = true;

        minRightWidth = parseInt(getComputedStyle(rightSidebarEl).minWidth) || 192;
        const minMainWidth = parseInt(getComputedStyle(mainContentEl).minWidth) || 360;
        maxRightWidth = splitContainerEl.offsetWidth - minMainWidth - resizerEl.offsetWidth;

        document.addEventListener("mousemove", onMouseMove);
        document.addEventListener("mouseup", onMouseUp);
        document.body.style.userSelect = "none";
    }

    function onMouseMove(e) {
        if (!isResizing) return;
        const targetWidth = window.innerWidth - e.clientX;
        const newWidth = Math.max(minRightWidth, Math.min(targetWidth, maxRightWidth));
        layout.rightSidebarWidth = newWidth;
    }

    function onMouseUp() {
        isResizing = false;
        document.removeEventListener("mousemove", onMouseMove);
        document.removeEventListener("mouseup", onMouseUp);
        document.body.style.userSelect = "";
    }

    $effect(() => {
        function onWindowResize() {
            if (!splitContainerEl || !rightSidebarEl || !mainContentEl || !resizerEl) return;
            const minMainWidth = parseInt(getComputedStyle(mainContentEl).minWidth) || 360;
            const fallbackMinRight = parseInt(getComputedStyle(rightSidebarEl).minWidth) || 192;
            const currentMaxRight = splitContainerEl.offsetWidth - minMainWidth - resizerEl.offsetWidth;
            const currentRight = parseInt(getComputedStyle(rightSidebarEl).width);
            if (layout.rightSidebarWidth > currentMaxRight) {
                layout.rightSidebarWidth = Math.max(fallbackMinRight, currentMaxRight);
            }
        }
        window.addEventListener("resize", onWindowResize);
        return () => window.removeEventListener("resize", onWindowResize);
    });
</script>

<div class="split-container" bind:this={splitContainerEl}>
    <MainContent bind:el={mainContentEl}/>
    <!-- svelte-ignore a11y_no_static_element_interactions -->
    <div class="resizer" id="resizer" bind:this={resizerEl} onmousedown={onResizerMouseDown}></div>
    <RightSidebar bind:el={rightSidebarEl}/>
</div>