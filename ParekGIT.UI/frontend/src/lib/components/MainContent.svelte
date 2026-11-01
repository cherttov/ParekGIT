<script>
    let { el = $bindable() } = $props();
</script>

<div class="main-content" id="main-content" bind:this={el}>
</div>