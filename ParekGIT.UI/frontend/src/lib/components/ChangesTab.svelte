<script>
    import { changes } from "../stores/changes.svelte";
    import { branch } from "../stores/branch.svelte";
    import { commit } from "../stores/commit.svelte";
    import { contextMenu } from "../stores/contextMenu.svelte";
    import { changeMenuItems, changesMasterMenuItems } from "./menus/changesContextMenu";
    import { commitChanges } from "../ipc/commands";
    import CustomScrollbar from "./CustomScrollbar.svelte";
    import StatusBadge from "./StatusBadge.svelte";

    let { active = false } = $props();
    let listEl = $state(null);
    const count = $derived(changes.files.length);

    const checkedCount = $derived(changes.checkedPaths.length);
    const canCommit = $derived(branch.name !== "" && commit.message.trim() !== "" && checkedCount > 0 && !commit.committing);
</script>

<div class="changes-tab" id="changes-tab" class:active>
    <!-- svelte-ignore a11y_no_static_element_interactions -->
    <div class="changes-header" id="changes-header" oncontextmenu={(e) => contextMenu.open(e, changesMasterMenuItems())}>
        <span id="changes-count-text">{count} changed file{count === 1 ? "" : "s"}</span>
        <input type="checkbox" class="ui-checkbox" id="changes-master-checkbox" 
            checked={changes.allChecked} onchange={(e) => changes.setAllChecked(e.currentTarget.checked)} />
    </div>

    <div class="changes-list-wrapper">
        <div class="changes-list-container" id="changes-list" bind:this={listEl}>
            {#each changes.files as file (file.path)}
                <!-- svelte-ignore a11y_no_static_element_interactions -->
                <!-- svelte-ignore a11y_click_events_have_key_events -->
                <div class="change-item" class:selected={changes.selectedPath === file.path}
                    onclick={() => changes.select(file.path)}
                    oncontextmenu={(e) => contextMenu.open(e, changeMenuItems(file), file.path)}>
                    <div class="change-item-left">
                        <StatusBadge code={file.statusCode} />
                        <div class="change-path">{file.path}</div>
                    </div>
                    <input type="checkbox" class="ui-checkbox changes-item-checkbox"
                        bind:checked={file.checked} onclick={(e) => e.stopPropagation()} />
                </div>
            {/each}
        </div>
        <CustomScrollbar container={listEl} />
    </div>

    <div class="right-sidebar-bottom">
        <input type="text" class="commit-input message-input" id="commit-name-input" placeholder="Message (required)" bind:value={commit.message} />
        <textarea class="commit-input description-input" id="commit-desc-input" placeholder="Description" bind:value={commit.description}></textarea>
        <button class="commit-btn" id="commit-btn" disabled={!canCommit} onclick={commitChanges}>
            {#if !branch.name}
                Loading...
            {:else if checkedCount > 0}
                Commit {checkedCount} file{checkedCount === 1 ? "" : "s"} to <strong>{branch.name}</strong>
            {:else}
                Commit
            {/if}
        </button>
    </div>
</div>