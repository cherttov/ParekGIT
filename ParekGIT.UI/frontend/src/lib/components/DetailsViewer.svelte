<script>
	import { commitDetails } from "../stores/commitDetails.svelte";
    import { loadCommitFileDiff } from "../ipc/commands";
    import { parseDiff, isBinary } from "../utils/parseDiff";
    import CustomScrollbar from "./CustomScrollbar.svelte";
    import StatusBadge from "./StatusBadge.svelte";

    let wrapperEl = $state(null);
    let fileListEl = $state(null);
    let panelOpen = $state(false);

    const lines = $derived(commitDetails.diffText ? parseDiff(commitDetails.diffText) : null);
    const selectedFile = $derived(commitDetails.files.find((f) => f.path === commitDetails.selectedFile) ?? null);
    const fileCount = $derived(commitDetails.files.length);

    const headerMessage = $derived(commitDetails.loading ? "Loading details..." : (commitDetails.message || "Select a commit to view details"));
    const headerStats = $derived(commitDetails.loading || !commitDetails.hash ? "" : `${fileCount} file${fileCount === 1 ? "" : "s"} changed`);
    const footerText = $derived.by(() => {
        if (commitDetails.loading) return "Loading...";
        if (!commitDetails.hash) return "No commit selected";
        if (fileCount === 0) return "No files available";
        return null;
    });

    const message = $derived.by(() => {
        if (commitDetails.loading || !commitDetails.selectedFile) return "";
        if (commitDetails.loadingDiff) return "Loading diff...";
        if (isBinary(commitDetails.selectedFile)) return "Binary file changed (no preview available)";
        if (lines === null) return "(No changes or binary file)";
        if (lines === "binary") return "Binary file changed (no preview available)";
        if (lines === "empty") return "Empty file (no content to preview)";
        return "";
    });

    $effect(() => {
        if (!commitDetails.selectedFile && commitDetails.files && commitDetails.files.length > 0) {
            selectFile(commitDetails.files[0]);
        }
    });

    function selectFile(file) {
        panelOpen = false;
        if (commitDetails.selectedFile === file.path) return;
        commitDetails.selectedFile = file.path;
        commitDetails.diffText = "";

        if (isBinary(file.path)) {
            commitDetails.loadingDiff = true;
            return;
        }
        commitDetails.loadingDiff = true;
        loadCommitFileDiff(commitDetails.hash, file.path);
    }
</script>

<svelte:window onclick={() => panelOpen = false} />

<div class="details-viewer" id="details-viewer">
    <div class="details-header" id="details-header">
        <div class="details-header-left">
            <span id="details-commit-message">{headerMessage}</span>
        </div>

        <div class="details-header-right">
            <span id="details-commit-stats">{headerStats}</span>
        </div>
    </div>

    <div class="details-body-wrapper" id="details-body-wrapper" bind:this={wrapperEl}>
        <pre class="diff-content" id="details-content">{#if message
            }<span class="diff-chunk">{message}</span>{:else if Array.isArray(lines) && commitDetails.selectedFile && !commitDetails.loading
            }{#each lines as line}<span class="diff-{line.type}">{line.text}</span>{/each}{/if}</pre>
        <CustomScrollbar id="details-scrollbar" container={wrapperEl} />
    </div>

    <div class="details-footer" id="details-footer">
        <div class="details-footer-container" id="details-footer-container">
            <button class="topbar-btn details-footer-btn" 
                disabled={commitDetails.loading || fileCount === 0}
                onclick={(e) => { e.stopPropagation(); panelOpen = !panelOpen; }}>
                <div class="btn-text-group">
                    <span class="topbar-dropdown btn-value">
                        {#if footerText}
                            {footerText}
                        {:else if selectedFile}
                            <StatusBadge code={selectedFile.statusCode} />
                            <span class="change-path">{selectedFile.path}</span>
                        {:else}
                            No files available
                        {/if}
                    </span>
                </div>
                <div class="btn-arrow"></div>
            </button>

            <!-- svelte-ignore a11y_no_static_element_interactions -->
            <!-- svelte-ignore a11y_click_events_have_key_events -->
            <div class="dropdown-panel" class:show={panelOpen} id="details-footer-panel" 
                onclick={(e) => e.stopPropagation()}>
                <div class="dropdown-list-wrapper">
                    <div class="dropdown-list-container" id="details-file-list" bind:this={fileListEl}>
                        {#each commitDetails.files as file (file.path)}
                            <!-- svelte-ignore a11y_click_events_have_key_events -->
                            <div class="dropdown-item" class:active={commitDetails.selectedFile === file.path} onclick={() => selectFile(file)}>
                                <StatusBadge code={file.statusCode} />
                                <span>{file.path}</span>
                            </div>
                        {/each}
                    </div>
                    <CustomScrollbar container={fileListEl} />
                </div>
            </div>
        </div>
    </div>
</div>
<!-- svelte-ignore a11y_click_events_have_key_events -->
<!-- svelte-ignore a11y_no_static_element_interactions -->
<div class="dropdown-backdrop" class:show={panelOpen} onclick={() => panelOpen = false}></div>