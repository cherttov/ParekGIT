<script>
    import { branch } from "../stores/branch.svelte.js";
    import { repo } from "../stores/repo.svelte.js";
    import { layout } from "../stores/layout.svelte.js";
    import RepoDropdown from "./RepoDropdown.svelte";
    import BranchDropdown from "./BranchDropdown.svelte";

    let repoOpen = $state(false);
    let branchOpen = $state(false);

    function toggleRepo(e) {
        e.stopPropagation();
        repoOpen = !repoOpen;
        if (repoOpen) branchOpen = false;
    }

    function toggleBranch(e) {
        e.stopPropagation();
        branchOpen = !branchOpen;
        if (branchOpen) repoOpen = false;
    }

    function closeAll() {
        repoOpen = false;
        branchOpen = false;
    }
</script>

<div class="topbar" id="topbar">
    <div class="topbar-logo">P.GIT</div>

    <div class="topbar-dropdown-container" id="repositories-container">
        <button class="topbar-btn" onclick={toggleRepo}>
            <div class="btn-text-group">
                <span class="topbar-dropdown btn-label">
                    Current repository:
                </span>
                <span class="topbar-dropdown btn-value">{repo.name || "None"}</span>
            </div>
            <div class="btn-arrow"></div>
        </button>
        <RepoDropdown show={repoOpen} onclose={closeAll} />
    </div>

    <div class="topbar-dropdown-container" id="branches-container" style:width="{layout.rightSidebarWidth}px">
        <button class="topbar-btn" onclick={toggleBranch}>
            <div class="btn-text-group">
                <span class="topbar-dropdown btn-label">
                    Current branch:
                </span>
                <span class="topbar-dropdown btn-value">{branch.name || "None"}</span>
            </div>
            <div class="btn-arrow"></div>
        </button>
        <BranchDropdown show={branchOpen} onclose={closeAll} />
    </div>
</div>
<!-- svelte-ignore a11y_click_events_have_key_events -->
<!-- svelte-ignore a11y_no_static_element_interactions -->
<div class="dropdown-backdrop" class:show={repoOpen || branchOpen} onclick={closeAll}></div>