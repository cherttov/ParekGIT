<script>
	import { view } from "../stores/view.svelte";
	import DiffViewer from "./DiffViewer.svelte";
	import DetailsViewer from "./DetailsViewer.svelte";

	let { el = $bindable() } = $props();
</script>

<div class="main-content" id="main-content" bind:this={el}>
	{#if view.tab === "changes"}
		<DiffViewer />
	{:else if view.tab === "history"}
		<DetailsViewer />
	{/if}
</div>