<script>
    import Modal from "../Modal.svelte";
    import { config } from "../../stores/config.svelte";
    import { createRepo, pickFolder, loadGlobalConfig } from "../../ipc/commands";

    let { open = false, onclose } = $props();

    const gitIgnores = [
        ["angular", "Angular"], ["cpp", "C++"], ["c", "C"], ["cmake", "CMake"], ["dotnet", "Dotnet"],
        ["go", "Go"], ["godot", "Godot"], ["haskell", "Haskell"], ["java", "Java"], ["kotlin", "Kotlin"],
        ["lua", "Lua"], ["luau", "Luau"], ["nextjs", "Nextjs"], ["nim", "Nim"], ["node", "Node"],
        ["python", "Python"], ["rails", "Rails"], ["rust", "Rust"], ["sass", "Sass"], ["scala", "Scala"],
        ["swift", "Swift"], ["unity", "Unity"], ["unreal-engine", "Unreal Engine"],
        ["visual-basic", "Visual Basic"], ["visual-studio", "Visual Studio"], ["zig", "Zig"]
    ];

    const licenses = [
        ["mit", "MIT License"], ["gpl3", "GNU GPLv3"], ["apache2", "Apache License 2.0"], ["gpl2", "GNU GPLv2"]
    ];

    const blank = () => ({ name: "", path: "", gitIgnore: "None", license: "None", year: "", organization: "", project: "" });

    let form = $state(blank());
    let licenseOpen = $state(false);
    let nameInput, pathInput, anchorEl;

    const isValid = $derived(form.name.trim() !== "" && form.path.trim() !== "");
    const noLicense = $derived(form.license === "None");

    $effect(() => {
        if (!open) return;
        form = blank();
        licenseOpen = false;
        const t = setTimeout(() => nameInput?.focus(), 100);
        return () => clearTimeout(t);
    });

    $effect(() => { if (noLicense) licenseOpen = false; })

    function toggleLicense() {
        licenseOpen = !licenseOpen;
        if (licenseOpen) loadGlobalConfig();
    }

    function closeLicenseOnOutside(e) {
        if (!anchorEl?.contains(e.target)) licenseOpen = false;
    }

    function browse() {
        pickFolder((p) => { form.path = p; pathInput?.focus(); });
    }

    function confirm() {
        if (!isValid) return;
        createRepo({
            repoName: form.name.trim(),
            localPath: form.path.trim(),
            gitIgnore: form.gitIgnore,
            license: form.license,
            licenseYear: form.year.trim(),
            licenseOrganization: form.organization.trim(),
            licenseProject: form.project.trim()
        });
        onclose?.();
    }

    function handleKeydown(e) {
        if (e.key === "Enter" && !e.isComposing) confirm();
    }
</script>

<svelte:window onclickcapture={licenseOpen ? closeLicenseOnOutside : undefined} />

<Modal
    {open}
    title="Create a new repositor"
    confirmText="Create repository"
    confirmDisabled={!isValid}
    onconfirm={confirm}
    {onclose}
>
    <div class="modal-body-row">
        <label for="repo-create-modal-name">Name</label>
        <input id="repo-create-modal-name" type="text" class="modal-input input-name" placeholder="repository name"
            bind:value={form.name} bind:this={nameInput} onkeydown={handleKeydown} />
    </div>

    <div class="modal-body-row">
        <label for="repo-create-modal-path">Local path</label>
        <div class="input-button-container">
            <input id="repo-create-modal-path" type="text" class="modal-input input-path" placeholder="repository path"
                bind:value={form.path} bind:this={pathInput} onkeydown={handleKeydown} />
            <button class="browse-btn" onclick={browse}>Browse</button>
        </div>
    </div>

    <div class="modal-body-row">
        <label for="select-git-ignore">Git ignore</label>
        <select class="modal-input" id="select-git-ignore" bind:value={form.gitIgnore}>
            <option value="None">None</option>
            {#each gitIgnores as [value, label] (value)}
                <option {value}>{label}</option>
            {/each}
        </select>
    </div>

    <div class="modal-body-row">
        <label for="select-git-license">License</label>
        <div class="input-button-container">
            <select class="modal-input" id="select-git-license" bind:value={form.license}>
                <option value="None">None</option>
                {#each licenses as [value, label] (value)}
                    <option {value}>{label}</option>
                {/each}
            </select>

            <div class="license-edit-anchor" bind:this={anchorEl}>
                <button class="browse-btn disabled" class:disabled={noLicense} class:active={licenseOpen} 
                    id="license-edit-btn" disabled={noLicense} onclick={toggleLicense}>
                    Edit
                </button>

                <div class="license-edit-panel" id="license-edit-panel" class:open={licenseOpen}>
                    <div class="license-field-row">
                        <label for="license-field-year">Year</label>
                        <input type="text" class="modal-input" id="license-field-year" placeholder={new Date().getFullYear()} bind:value={form.year} />
                    </div>
                    <div class="license-field-row">
                        <label for="license-field-organization">Author</label>
                        <input type="text" class="modal-input" id="license-field-organization" placeholder={config.globalName} bind:value={form.organization} />
                    </div>
                    <div class="license-field-row">
                        <label for="license-field-project">Project</label>
                        <input type="text" class="modal-input" id="license-field-project" placeholder={form.name.trim()} bind:value={form.project} />
                    </div>
                </div>
            </div>
        </div>
    </div>
</Modal>