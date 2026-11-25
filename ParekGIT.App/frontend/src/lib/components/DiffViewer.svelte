<script>
	import { diff } from "../stores/diff.svelte";
	import { parseDiff, isBinary } from "../utils/parseDiff";
	import CustomScrollbar from "./CustomScrollbar.svelte";

	let wrapperEl = $state(null);
	
	const lines = $derived(diff.diffText ? parseDiff(diff.diffText) : null);
	const message = $derived.by(() => {
		if (!diff.filePath) return "";
		if (isBinary(diff.filePath)) return "Binary file changed (no preview available)";
		if (diff.loading) return "Loading changes...";
		if (lines === null) return "(No changes or binary file)";
		if (lines === "binary") return "Binary file changed (no preview available)";
		if (lines === "empty") return "Empty file (no content to preview)";
		return "";
	});
</script>

<div class="diff-viewer">
	<div class="diff-header">
		<span id="diff-filename">{diff.filePath || "Select a file to view changes"}</span>
	</div>

	<div class="diff-body-wrapper"bind:this={wrapperEl}>
		<pre class="diff-content">{#if message
			}<span class="diff-chunk">{message}</span>{:else if Array.isArray(lines)
			}{#each lines as line}<span class="diff-{line.type}">{line.text}</span>{/each
			}{/if}</pre>
		<CustomScrollbar container={wrapperEl} />
	</div>
</div>