<script>
    import { contextMenu } from "../stores/contextMenu.svelte";

    let el = $state(null);
    let w = $state(0), h = $state(0);
    let vw = $state(0), vh = $state(0);

    const place = (pos, size, max) => (pos + size > max ? Math.max(0, pos - size) : pos);

    function run(item) {
        contextMenu.close();
        item.onclick?.();
    }
</script>

<svelte:window 
    bind:innerWidth={vw} 
    bind:innerHeight={vh}
    onpointerdown={(e) => { if (contextMenu.visible && e.button === 0 && !el?.contains(e.target)) contextMenu.close() }}
    onkeydown={(e) => { if (contextMenu.visible && e.key === "Escape") { e.preventDefault(); contextMenu.close(); }}}
    onresize={() => contextMenu.close() }
    oncontextmenu={(e) => e.preventDefault()}
/>

<div class="context-menu" class:show={contextMenu.visible} bind:this={el}
    bind:offsetWidth={w} bind:offsetHeight={h}
    style:left="{place(contextMenu.x, w, vw)}px" style:top="{place(contextMenu.y, h, vh)}px"
>
    {#each contextMenu.items as item}
        {#if item === "divider"}
            <div class="context-menu-divider"></div>
        {:else}
            <button class="context-menu-item" disabled={item.disabled} onclick={() => run(item)}>
                {item.label}
            </button>
        {/if}
    {/each}
</div>