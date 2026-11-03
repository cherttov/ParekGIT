<script>
    import { repo } from "../stores/repo.svelte.js";
    import { sendIpcMessage } from "../ipc/ipc.js";
    import { IpcActions } from "../ipc/actions.js";
    import { fetchRepo, publishBranch, pullRepo, pushRepo } from "../ipc/commands.js";

    const hasRepo = $derived(repo.path !== "");

    const syncState = $derived.by(() => {
        if (!hasRepo || !repo.branch || !repo.hasRemote) return "none";
        if (!repo.hasUpstream) return "publish";
        if (repo.commitsBehind > 0) return "pull";
        if (repo.commitsAhead > 0) return "push";
        return "up-to-date";
    });

    const syncTitle = $derived.by(() => {
		const plural = (n) => (n === 1 ? "" : "s");
		switch (syncState) {
			case "publish":    return "Publish branch";
			case "pull":       return `Pull ${repo.commitsBehind} commit${plural(repo.commitsBehind)}`;
			case "push":       return `Push ${repo.commitsAhead} commit${plural(repo.commitsAhead)}`;
			case "up-to-date": return "Up to date";
			default:           return "";
		}
	});

    function pushPullPublish() {
        if (syncState === "publish") publishBranch();
        else if (syncState === "pull") pullRepo();
        else if (syncState === "push") pushRepo();
    }
</script>

<div class="left-sidebar" id="left-sidebar">
    <!-- Merge Btn -->
    <button class="left-sidebar-btn btn-merge" 
            disabled={!hasRepo}
            title="Merge branch">
        <div class="btn-icon icon-merge"></div>
    </button>

    <!-- Todo Btn -->
    <button class="left-sidebar-btn btn-todo" 
            disabled={!hasRepo}
            title="Todo list">
        <div class="btn-icon icon-todo"></div>
    </button>

    <!-- Config Btn -->
    <button class="left-sidebar-btn btn-config" 
            disabled={!hasRepo}
            title="Repository config (name/email)">
        <div class="btn-icon icon-config"></div>
    </button>

    <!-- Push/Pull/Publish Btn -->
    <button class="left-sidebar-btn btn-pushpullpublish" 
            class:push={syncState === "push"}
            class:pull={syncState === "pull"}
            class:publish={syncState === "publish"}
            disabled={syncState === "none" || syncState === "up-to-date"}
            onclick={pushPullPublish}
            title={syncTitle}>
        <div class="btn-icon icon-pushpullpublish"></div>
    </button>

    <!-- Stash Btn -->
    <button class="left-sidebar-btn btn-stash" 
            disabled={!hasRepo}
            title="Stash">
        <div class="btn-icon icon-stash"></div>
    </button>

    <!-- Fetch Btn -->
    <button class="left-sidebar-btn btn-fetch"
            class:fetching={repo.fetching}
            disabled={!hasRepo}
            onclick={fetchRepo}
            title="Fetch from remote">
        <div class="btn-icon icon-fetch"></div>
    </button>

    <!-- Settings Btn -->
    <button class="left-sidebar-btn btn-settings" 
            title="Settings">
        <div class="btn-icon icon-settings"></div>
    </button>

    <!-- Account Btn -->
    <button class="left-sidebar-btn btn-account" 
            title="Account (global git config)">
        <div class="btn-icon icon-account"></div>
    </button>
</div>