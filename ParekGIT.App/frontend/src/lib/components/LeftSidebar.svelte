<script>
	import { repo } from "../stores/repo.svelte.js";
	import { sendIpcMessage } from "../ipc/ipc.js";
	import { IpcActions } from "../ipc/actions.js";
	import { fetchRepo, publishBranch, pullRepo, pushRepo } from "../ipc/commands.js";
	import { modal } from "../stores/modal.svelte.js";
	import { branch } from "../stores/branch.svelte.js";

	const hasRepo = $derived(repo.path !== "");

	const syncState = $derived.by(() => {
		if (!hasRepo || !branch.name || branch.isDetached || !repo.hasRemote) return "none";
		if (!branch.hasUpstream) return "publish";
		if (branch.commitsBehind > 0) return "pull";
		if (branch.commitsAhead > 0) return "push";
		return "up-to-date";
	});

	const syncTitle = $derived.by(() => {
		const plural = (n) => (n === 1 ? "" : "s");
		switch (syncState) {
			case "publish":    return "Publish branch";
			case "pull":       return `Pull ${branch.commitsBehind} commit${plural(branch.commitsBehind)}`;
			case "push":       return `Push ${branch.commitsAhead} commit${plural(branch.commitsAhead)}`;
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

<div class="left-sidebar">
	<!-- Merge Btn -->
	<button class="left-sidebar-btn btn-merge" title="Merge branch"
			disabled={!hasRepo || !branch.name || branch.isDetached}
			onclick={() => modal.open("branch-merge", { targetBranch: branch.name })}>
		<div class="btn-icon icon-merge"></div>
	</button>

	<!-- Todo Btn -->
	<button class="left-sidebar-btn btn-todo" title="Todo list"
			disabled={!hasRepo} onclick={() => modal.open("todo")}>
		<div class="btn-icon icon-todo"></div>
	</button>

	<!-- Config Btn -->
	<button class="left-sidebar-btn btn-config" title="Repository config (name/email)"
			disabled={!hasRepo} onclick={() => modal.open("config")}>
		<div class="btn-icon icon-config"></div>
	</button>

	<!-- Push/Pull/Publish Btn -->
	<button class="left-sidebar-btn btn-pushpullpublish" title={syncTitle}
			class:push={syncState === "push"}
			class:pull={syncState === "pull"}
			class:publish={syncState === "publish"}
			disabled={syncState === "none" || syncState === "up-to-date"}
			onclick={pushPullPublish}>
		<div class="btn-icon icon-pushpullpublish"></div>
	</button>

	<!-- Stash Btn -->
	<button class="left-sidebar-btn btn-stash" title="Stash"
			disabled={!hasRepo} onclick={() => modal.open("stashes")}>
		<div class="btn-icon icon-stash"></div>
	</button>

	<!-- Fetch Btn -->
	<button class="left-sidebar-btn btn-fetch" title="Fetch from remote"
			class:fetching={repo.fetching} disabled={!hasRepo} onclick={fetchRepo}>
		<div class="btn-icon icon-fetch"></div>
	</button>

	<!-- Settings Btn -->
	<button class="left-sidebar-btn btn-settings" title="Settings"
			onclick={() => modal.open("settings")}>
		<div class="btn-icon icon-settings"></div>
	</button>

	<!-- Account Btn -->
	<button class="left-sidebar-btn btn-account" title="Account (global git config)"
			onclick={() => modal.open("account")}>
		<div class="btn-icon icon-account"></div>
	</button>
</div>