<script>
    let { code, untracked = "?" } = $props();

    const status = $derived(
        code.includes(untracked) ? { letter: "U", name: "untracked", title: "Untracked" } :
        code.includes("A") ? { letter: "A", name: "added", title: "Added" } :
        code.includes("D") ? { letter: "D", name: "deleted", title: "Deleted" } :
        code.includes("R") ? { letter: "R", name: "renamed", title: "Renamed" } :
        { letter: "M", name: "modified", title: "Modified" }
    );
</script>

<span class="change-status status-{status.name}" title={status.title}>{status.letter}</span>