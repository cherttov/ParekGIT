// ------- DOM ELEMENTS -------
// Topbar
const repoContainer = document.getElementById("repositories-container");
const repoDropdown = document.getElementById("repository-dropdown-panel");
const repoNewBtn = document.querySelector("#repositories-container .btn-add");
const repoBtn = document.querySelector("#repositories-container .topbar-btn");
const repoPanel = document.querySelector("#repositories-container .dropdown-panel");

const branchContainer = document.getElementById("branches-container");
const branchDropdown = document.getElementById("branch-dropdown-panel");
const branchNewBtn = document.querySelector("#branches-container .btn-new");
const branchBtn = document.querySelector("#branches-container .topbar-btn");
const branchPanel = document.querySelector("#branches-container .dropdown-panel");

const backdrop = document.getElementById("dropdown-backdrop");

// Left Sidebar
const leftSidebar = document.getElementById("left-sidebar");

// Main Content
const mainContent = document.getElementById("main-content");

// Right Sidebar + SplitContainer
const rightSidebar = document.getElementById("right-sidebar");
const resizer = document.getElementById("resizer");

const changesHeader = document.getElementById("changes-header");
const changesCountText = document.getElementById("changes-count-text");
const changesMasterCheckbox = document.getElementById("changes-master-checkbox");
const changesList = document.getElementById("changes-list");
const commitMessageInput = document.getElementById("commit-name-input");
const commitDescriptionInput = document.getElementById("commit-desc-input");
const commitBtn = document.getElementById("commit-btn");

// Modals
const modalBackdrops = document.querySelectorAll('.modal-backdrop');
const modalCloseTriggers = document.querySelectorAll('.close-modal-icon, .cancel-modal-btn');

const branchModal = document.getElementById("branch-modal");
const branchModalInputName = branchModal.querySelector(".modal-input");
const branchModalConfirmBtn = branchModal.querySelector(".confirm-modal-btn");

// const repoCloneModal = document.getElementById("repo-clone-modal");

const repoCreateModal = document.getElementById("repo-create-modal");
const repoCreateModalInputName = repoCreateModal.querySelector(".input-name");
const repoCreateModalInputPath = repoCreateModal.querySelector(".input-path");
const repoCreateModalBrowseBtn = repoCreateModal.querySelector(".browse-btn");
const repoCreateModalSelectGitIgnore = repoCreateModal.querySelector("#select-git-ignore");
const repoCreateModalSelectLicense = repoCreateModal.querySelector("#select-git-license");
const repoCreateModalConfirmBtn = repoCreateModal.querySelector(".confirm-modal-btn");

const repoAddModal = document.getElementById("repo-add-modal");
const repoAddModalInputPath = repoAddModal.querySelector(".modal-input");
const repoAddModalBrowseBtn = repoAddModal.querySelector(".browse-btn");
const repoAddModalConfirmBtn = repoAddModal.querySelector(".confirm-modal-btn");

const repoRemoveModal = document.getElementById("repo-remove-modal");
const repoRemoveModalName = document.getElementById("remove-modal-repo-name");
const repoRemoveModalLocalCheckbox = repoRemoveModal.querySelector(".ui-checkbox");
const repoRemoveModalConfirmBtn = repoRemoveModal.querySelector(".confirm-modal-btn");

// Context menus
const repoContextMenu = document.getElementById("repo-context-menu");
const repoMenuClone = repoContextMenu.querySelector(".context-menu-item.item-clone");
const repoMenuCreate = repoContextMenu.querySelector(".context-menu-item.item-create");
const repoMenuAdd = repoContextMenu.querySelector(".context-menu-item.item-add");

const repoItemContextMenu = document.getElementById("repo-item-context-menu");
const repoItemMenuTerminal = repoItemContextMenu.querySelector(".context-menu-item.item-terminal");
const repoItemMenuExplorer = repoItemContextMenu.querySelector(".context-menu-item.item-explorer");
const repoItemMenuRemove = repoItemContextMenu.querySelector(".context-menu-item.item-remove");

// ------- APP STATE -------
let currentRepoPath = "";
let activeBrowseInput = null;
let currentChangesCount = 0;

// ------- IPC COMMUNICATION -------
const sendIpcMessage = (action, payload = {}) => {
    const envelope = {
        Action: action,
        Payload: payload
    };
    window.external.sendMessage(JSON.stringify(envelope));
};

window.external.receiveMessage(message => {
    const data = JSON.parse(message);

    switch (data.Action) {
        case "LOAD_REPOSITORIES":
            loadRepositoriesIntoDropdown(data.Payload);
            break;

        case "BRANCHES_LOADED":
            loadBranchesIntoDropdown(data.Payload);
            break;

        case "BRANCH_LOADED":
            loadBranchesIntoDropdown(data.Payload); // temporary
            break;

        case "REPO_CREATED":
            addRepositoryToDropdown(data.Payload);
            break;

        case "REPO_ADDED":
            addRepositoryToDropdown(data.Payload);
            break;

        case "REPO_REMOVED":
            deleteRepoFromDropdown(data.Payload);
            break;

        case "FOLDER_SELECTED":
            folderSelected(data.Payload);
            break;

        case "REPO_STATUS_LOADED":
            renderChangedFiles(data.Payload);
            break;

        default:
            console.warn("Unknown action received received: ", data.Action);
    }
})

// ------- UI FUNCTIONS & HELPERS -------
let minRightWidth = 0, maxRightWidth = 0;
let isResizing = false;

// Keep panel widths in sync (match their parents)
const updatePanelWidths = () => {
    repoPanel.style.width = `${repoContainer.offsetWidth}px`;
    repoPanel.style.left = `${repoContainer.offsetLeft}px`;
    branchPanel.style.width = window.getComputedStyle(rightSidebar).width;
};

// Context menu position
const placeContextMenu = (event, contextMenu) => {
    let mouseX = event.clientX;
    let mouseY = event.clientY;

    const menuWidth = contextMenu.offsetWidth;
    const menuHeight = contextMenu.offsetHeight;

    if (mouseX + menuWidth > window.innerWidth) {
        mouseX = mouseX - menuWidth;
    }
    if (mouseY + menuHeight > window.innerHeight) {
        mouseY = mouseY - menuHeight;
    }

    contextMenu.style.left = `${mouseX}px`;
    contextMenu.style.top = `${mouseY}px`;
};

// Close everything
const closeDropdowns = () => {
    repoPanel.classList.remove('show');
    branchPanel.classList.remove('show');
    backdrop.classList.remove('show');
    repoContextMenu.classList.remove('show');
    repoItemContextMenu.classList.remove('show');
};

const closeAndClearModal = (modalElement) => {
    modalElement.classList.remove('show');
    const inputs = modalElement.querySelectorAll('input');
    inputs.forEach(input => input.value = "");
};

const toggleDropdown = (toShow, toHide, event) => {
    event.stopPropagation();
    updatePanelWidths();
    toHide.classList.remove('show');

    const isOpening = toShow.classList.toggle('show');
    backdrop.classList.toggle('show', isOpening);
};

// Commit button disabling/enabling
const toggleCommitButton = () => {
    if (commitMessageInput.value.trim() === "" || currentChangesCount === 0) {
        commitBtn.disabled = true;
        commitBtn.classList.add("disabled");
    } else {
        commitBtn.disabled = false;
        commitBtn.classList.remove("disabled");
    }
};

// Changes header checkbox updater
const updateMasterCheckboxState = () => {
    const allFileCheckboxes = Array.from(document.querySelectorAll(".changes-item-checkbox"));
    if (allFileCheckboxes.length === 0) { return; }
    const allChecked = allFileCheckboxes.every(box => box.checked);
    changesMasterCheckbox.checked = allChecked;
}

// C# - Load repositories
function loadRepositoriesIntoDropdown(repositories) {
    const existingItems = repoDropdown.querySelectorAll('.dropdown-item');
    existingItems.forEach(item => item.remove());

    if (repositories.length === 0) { return; }

    repositories.forEach(repo => {
        const item = document.createElement("div");
        item.className = "dropdown-item";

        item.dataset.path = repo.AbsolutePath;

        item.innerHTML = `${repo.Name}`;

        // LMB - select
        item.addEventListener("click", () => {
            currentRepoPath = repo.AbsolutePath;
            sendIpcMessage("REPO_SELECTED", { absolutePath: repo.AbsolutePath });
            sendIpcMessage("GET_REPO_STATUS", { repoPath: repo.AbsolutePath });

            branchBtn.classList.remove("disabled");
        });

        // RMB - context menu
        item.addEventListener("contextmenu", (event) => {
            event.preventDefault();
            event.stopPropagation();

            repoContextMenu.classList.remove("show");

            repoItemContextMenu.dataset.targetPath = repo.AbsolutePath;
            repoItemContextMenu.dataset.targetName = repo.Name;

            repoItemContextMenu.classList.add("show");

            placeContextMenu(event, repoItemContextMenu);
        });

        repoDropdown.appendChild(item);
    });
}

// C# - Load branches
function loadBranchesIntoDropdown(branches) {
    const existingItems = branchDropdown.querySelectorAll('.dropdown-item');
    existingItems.forEach(item => item.remove());

    const branchBtnValue = document.querySelector('#branches-container .btn-value');

    if (branches.length === 0) {
        branchBtnValue.textContent = "No branches";
        return;
    }

    let currentBranchName = "None";

    branches.forEach(branch => {
        const item = document.createElement("div");

        if (branch.IsCurrent) {
            item.className = "dropdown-item active";
            currentBranchName = branch.Name;
        }
        else {
            item.className = "dropdown-item";
        }

        item.innerHTML = `${branch.Name}`;

        item.addEventListener("click", () => {
            sendIpcMessage("BRANCH_SELECTED", {
                absolutePath: currentRepoPath,
                branchName: branch.Name,
                isRemote: branch.IsRemote
            });
        });

        branchDropdown.appendChild(item);
    });

    branchBtnValue.textContent = currentBranchName;

    // Load changes
    if (currentRepoPath) {
        sendIpcMessage("GET_REPO_STATUS", { repoPath: currentRepoPath });
    }
}

// C# - Delete repo 
function deleteRepoFromDropdown(repository) {
    const removedPath = repository.absolutePath;
    if (!removedPath) { return; }

    const cssPath = removedPath.replace(/\\/g, '\\\\');
    const itemToRemove = repoDropdown.querySelector(`.dropdown-item[data-path="${cssPath}"]`)

    if (itemToRemove) { itemToRemove.remove(); }

    if (currentRepoPath === removedPath) {
        currentRepoPath = "";

        const repoBtnValue = repoBtn.querySelector('.btn-value');
        if (repoBtnValue) { repoBtnValue.textContent = "None"; }

        branchBtn.classList.add("disabled");
        const branchBtnValue = branchBtn.querySelector('.btn-value');
        if (branchBtnValue) { branchBtnValue.textContent = "None"; }

        const existingBranches = branchDropdown.querySelectorAll('.dropdown-item');
        existingBranches.forEach(item => item.remove());
    }
}

// C# - Folder selected
function folderSelected(directory) {
    if (activeBrowseInput) {
        activeBrowseInput.value = directory.path;
        activeBrowseInput.focus();
        activeBrowseInput = null;
    }
}

// C# - Add repository to dropdown
function addRepositoryToDropdown(repo) {
    const item = document.createElement("div");
    item.className = "dropdown-item";
    item.dataset.path = repo.AbsolutePath;
    item.innerHTML = `${repo.Name}`;

    // LMB - select
    item.addEventListener("click", () => {
        currentRepoPath = repo.AbsolutePath;
        sendIpcMessage("REPO_SELECTED", { absolutePath: repo.AbsolutePath });
        sendIpcMessage("GET_REPO_STATUS", { repoPath: repo.AbsolutePath });
        branchBtn.classList.remove("disabled");
    });

    // RMB - context menu
    item.addEventListener("contextmenu", (event) => {
        event.preventDefault();
        event.stopPropagation();

        repoContextMenu.classList.remove("show");
        repoItemContextMenu.dataset.targetPath = repo.AbsolutePath;
        repoItemContextMenu.classList.add("show");

        placeContextMenu(event, repoItemContextMenu);
    });

    repoDropdown.appendChild(item);
    item.click();
}

// C# - Load changed files to right sidebar
function renderChangedFiles(files) {
    changesList.innerHTML = "";

    currentChangesCount = files.length;

    changesCountText.textContent = `${files.length} changed file${files.length === 1 ? '' : 's'}`;

    toggleCommitButton();

    if (files.length === 0) { return; }

    files.forEach(file => {
        const item = document.createElement("div");
        item.className = "change-item";

        let statusLetter = "M";
        let statusClass = "status-modified";

        if (file.StatusCode.includes("?")) {
            statusLetter = "U";
            statusClass = "status-untracked";
        } else if (file.StatusCode.includes("A")) {
            statusLetter = "A";
            statusClass = "status-added";
        } else if (file.StatusCode.includes("D")) {
            statusLetter = "D";
            statusClass = "status-deleted";
        } else if (file.StatusCode.includes("R")) {
            statusLetter = "R";
            statusClass = "status-renamed";
        }

        item.innerHTML = `
            <div class="change-item-left">
                <div class="change-status ${statusClass}">${statusLetter}</div>
                <div class="change-path">${file.Path}</div>
            </div>
            <input type="checkbox" class="ui-checkbox changes-item-checkbox" checked/>
        `

        const checkbox = item.querySelector(".changes-item-checkbox");
        checkbox.addEventListener("change", () => {
            updateMasterCheckboxState();
        });

        changesList.appendChild(item);
    });
}

// ------- EVENT LISTENERS -------
// Global overrides
document.addEventListener('wheel', (event) => {
    if (event.ctrlKey || event.metaKey) {
        event.preventDefault();
    }
}, { passive: false });

// Toggles (dropdowns)
repoBtn.addEventListener('click', (event) => toggleDropdown(repoPanel, branchPanel, event));
branchBtn.addEventListener('click', (event) => toggleDropdown(branchPanel, repoPanel, event));

// Close triggers (dropdowns)
backdrop.addEventListener('click', closeDropdowns);
window.addEventListener('click', closeDropdowns);

// Close triggers (modals)
modalBackdrops.forEach(backdrop => {
    backdrop.addEventListener('click', (event) => {
        if (event.target === backdrop) { closeAndClearModal(backdrop); }
    });
});
modalCloseTriggers.forEach(trigger => {
    trigger.addEventListener('click', (event) => {
        const parentModal = event.target.closest('.modal-backdrop');
        if (parentModal) { closeAndClearModal(parentModal); }
    });
});

// Prevent closing when inside (dropdowns)
document.querySelectorAll('.dropdown-panel').forEach(panel => {
    panel.addEventListener('click', (event) => event.stopPropagation());
});

// Resizer logic (split-container)
window.addEventListener('resize', updatePanelWidths);

resizer.addEventListener("mousedown", (event) => {
    isResizing = true;

    const rightCSS = window.getComputedStyle(rightSidebar);
    const mainCSS = window.getComputedStyle(mainContent);

    minRightWidth = parseInt(rightCSS.minWidth) || 192;
    const minMainWidth = parseInt(mainCSS.minWidth) || 360;

    maxRightWidth = window.innerWidth - leftSidebar.offsetWidth - minMainWidth - resizer.offsetWidth;

    document.addEventListener("mousemove", resize);
    document.addEventListener("mouseup", stopResize);
    document.body.style.userSelect = "none";
});

function resize(event) {
    if (isResizing) {
        let targetWidth = window.innerWidth - event.clientX;
        let newWidth = Math.max(minRightWidth, Math.min(targetWidth, maxRightWidth));

        rightSidebar.style.width = `${newWidth}px`;
        branchContainer.style.width = `${newWidth}px`;
        branchPanel.style.width = `${newWidth}px`;

        repoPanel.style.width = `${repoContainer.offsetWidth}px`;
        repoPanel.style.left = `${repoContainer.offsetLeft}px`;
    }
}

function stopResize() {
    isResizing = false;
    document.removeEventListener("mousemove", resize);
    document.removeEventListener("mouseup", stopResize);
    document.body.style.userSelect = "";
}

// Master checkbox logic (right-sidebar/changes-header)
changesMasterCheckbox.addEventListener("change", (event) => {
    const isChecked = event.target.checked;
    const allFileCheckboxes = document.querySelectorAll(".changes-item-checkbox");
    allFileCheckboxes.forEach(box => {
        box.checked = isChecked
    });
});

// Commit section (right-sidebar)
commitMessageInput.addEventListener("input", toggleCommitButton);

// Add/new buttons (dropdowns)
repoNewBtn.addEventListener("click", (event) => {
    event.stopPropagation();

    repoContextMenu.classList.add('show');

    repoItemContextMenu.classList.remove("show");

    placeContextMenu(event, repoContextMenu);
});

branchNewBtn.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();

    branchModal.classList.add("show");
    setTimeout(() => {
        branchModalInputName.focus();
    }, 100);
});

// Context menu options (context-menu)
repoMenuClone.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();
});

repoMenuCreate.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();

    repoCreateModal.classList.add("show");
    setTimeout(() => {
        repoCreateModalInputName.focus();
    }, 100);
});

repoMenuAdd.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();

    repoAddModal.classList.add("show");
    setTimeout(() => {
        repoAddModalInputPath.focus();
    }, 100);
});

repoItemMenuTerminal.addEventListener("click", (event) => {
    event.stopPropagation();

    const pathToRepo = repoItemContextMenu.dataset.targetPath;
    if (pathToRepo) {
        sendIpcMessage("REPO_TERMINAL", {
            "repoPath": pathToRepo
        });
    }

    repoItemContextMenu.classList.remove("show");
});

repoItemMenuExplorer.addEventListener("click", (event) => {
    event.stopPropagation();

    const pathToRepo = repoItemContextMenu.dataset.targetPath;
    if (pathToRepo) {
        sendIpcMessage("REPO_EXPLORER", {
            "repoPath": pathToRepo
        });
    }

    repoItemContextMenu.classList.remove("show");
});

repoItemMenuRemove.addEventListener("click", (event) => {
    event.stopPropagation();

    const pathToRepo = repoItemContextMenu.dataset.targetPath;
    const repoName = repoItemContextMenu.dataset.targetName;

    if (pathToRepo) {
        repoRemoveModal.dataset.targetPath = pathToRepo;

        repoRemoveModalName.textContent = repoName;

        repoRemoveModal.classList.add("show");
    }

    repoItemContextMenu.classList.remove("show");
    closeDropdowns();
});

// Force close (context-menu)
window.addEventListener("click", (event) => {
    if (repoContextMenu.classList.contains("show")) {
        if (!repoContextMenu.contains(event.target) && !repoNewBtn.contains(event.target)) {
            repoContextMenu.classList.remove("show");
        }
    }

    if (repoItemContextMenu.classList.contains("show")) {
        if (!repoItemContextMenu.contains(event.target)) {
            repoItemContextMenu.classList.remove("show");
        }
    }
}, true);

// Confirm buttons (modals)
branchModalConfirmBtn.addEventListener("click", () => {
    const newBranchName = branchModalInputName.value.trim();

    if (newBranchName === "") { return; }

    sendIpcMessage("BRANCH_CREATE", {
        absolutePath: currentRepoPath,
        branchName: newBranchName
    });

    closeAndClearModal(branchModal);
});

repoCreateModalConfirmBtn.addEventListener("click", () => {
    const repoName = repoCreateModalInputName.value.trim();
    const localPath = repoCreateModalInputPath.value.trim();
    const gitIgnore = repoCreateModalSelectGitIgnore.value.trim() ?? "None";
    const gitLicense = repoCreateModalSelectLicense.value.trim() ?? "None";

    sendIpcMessage("REPO_CREATE", {
        "repoName": repoName,
        "localPath": localPath,
        "gitIgnore": gitIgnore,
        "gitLicense": gitLicense
    });

    closeAndClearModal(repoCreateModal);
});

repoAddModalConfirmBtn.addEventListener("click", () => {
    const repoPath = repoAddModalInputPath.value.trim();

    sendIpcMessage("REPO_ADD", {
        "repoPath": repoPath
    });

    closeAndClearModal(repoAddModal);
});

repoRemoveModalConfirmBtn.addEventListener("click", () => {
    const repoPath = repoRemoveModal.dataset.targetPath;

    if (repoPath) {
        const deleteLocal = repoRemoveModalLocalCheckbox.checked;

        sendIpcMessage("REPO_REMOVE", {
            "repoPath": repoPath,
            "deleteLocal": deleteLocal
        });
    }

    closeAndClearModal(repoRemoveModal);
});

// Input boxes (modals)
branchModalInputName.addEventListener("keyup", (event) => {
    if (event.key === "Enter") { branchModalConfirmBtn.click(); }
});

// Browse buttons (modals)
repoCreateModalBrowseBtn.addEventListener("click", (event) => {
    event.preventDefault();

    activeBrowseInput = repoCreateModalInputPath;

    sendIpcMessage("OPEN_EXPLORER_DIALOG");
});

repoAddModalBrowseBtn.addEventListener("click", (event) => {
    event.preventDefault();

    activeBrowseInput = repoAddModalInputPath;

    sendIpcMessage("OPEN_EXPLORER_DIALOG");
});

// Search & Selection Setup (dropdowns)
const setupFilter = (inputSelector, panelSelector) => {
    const searchInput = document.querySelector(inputSelector);

    searchInput.addEventListener('input', (event) => {
        const searchTerm = event.target.value.toLowerCase();

        const dropdownItems = document.querySelectorAll(`${panelSelector} .dropdown-item`);

        dropdownItems.forEach(item => {
            const itemText = item.textContent.toLowerCase();

            if (itemText.includes(searchTerm)) {
                item.style.display = '';
            } else {
                item.style.display = 'none';
            }
        });
    });
};

const setupSelection = (panelSelector, btnValueSelector) => {
    const panel = document.querySelector(panelSelector);
    const valueDisplay = document.querySelector(btnValueSelector);

    panel.addEventListener('click', (event) => {
        const item = event.target.closest('.dropdown-item');

        if (item) {
            valueDisplay.textContent = item.textContent;

            panel.querySelectorAll('.dropdown-item').forEach(elem => {
                elem.classList.remove('active');
            });

            item.classList.add('active');

            closeDropdowns();
        }
    });
};

setupFilter('#repositories-filter .filter-input', '#repository-dropdown-panel');
setupFilter('#branches-filter .filter-input', '#branch-dropdown-panel');

setupSelection('#repository-dropdown-panel', '#repositories-container .btn-value');
setupSelection('#branch-dropdown-panel', '#branches-container .btn-value');

// ------- APP INIT -------
updatePanelWidths();

branchBtn.classList.add("disabled"); // change to load last selected repo automatically
toggleCommitButton();

window.addEventListener('DOMContentLoaded', () => {
    sendIpcMessage("APP_READY");
});