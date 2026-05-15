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

// Modals
const modalBackdrops = document.querySelectorAll('.modal-backdrop');
const modalCloseTriggers = document.querySelectorAll('.close-modal-icon, .cancel-modal-btn');

const branchModal = document.getElementById("branch-modal");
const branchModalInputName = branchModal.querySelector(".modal-input");
const branchModalConfirmBtn = branchModal.querySelector(".confirm-modal-btn");

// const repoCloneModal = document.getElementById("repo-clone-modal");

const repoCreateModal = document.getElementById("repo-create-modal");
const repoCreateModalInputName = repoCreateModal.querySelector(".input-name");
const repoCreateModalInputDescription = repoCreateModal.querySelector(".input-description");
const repoCreateModalInputPath = repoCreateModal.querySelector(".input-path");
const repoCreateModalSelectGitIgnore = repoCreateModal.querySelector("#select-git-ignore");
const repoCreateModalSelectLicense = repoCreateModal.querySelector("#select-git-license");
const repoCreateModalConfirmBtn = repoCreateModal.querySelector(".confirm-modal-btn");

const repoAddModal = document.getElementById("repo-add-modal");
const repoAddModalInputPath = repoAddModal.querySelector(".modal-input");
const repoAddModalConfirmBtn = repoAddModal.querySelector(".confirm-modal-btn");

// Context menus
const repoContextMenu = document.getElementById("repo-context-menu");
const repoMenuClone = repoContextMenu.querySelector(".context-menu-item.item-clone");
const repoMenuCreate = repoContextMenu.querySelector(".context-menu-item.item-create");
const repoMenuAdd = repoContextMenu.querySelector(".context-menu-item.item-add");

// ------- APP STATE -------
let currentRepoPath = "";

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

        case "REPO_CREATED": // finish
            break;

        case "REPO_ADDED": // finish
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

// Close everything
const closeDropdowns = () => {
    repoPanel.classList.remove('show');
    branchPanel.classList.remove('show');
    backdrop.classList.remove('show');
    repoContextMenu.classList.remove('show');
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

        item.addEventListener("click", () => {
            currentRepoPath = repo.AbsolutePath;
            sendIpcMessage("REPO_SELECTED", { absolutePath: repo.AbsolutePath });

            branchBtn.classList.remove("disabled");
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

// Add/new buttons (dropdowns)
repoNewBtn.addEventListener("click", (event) => {
    event.stopPropagation();

    repoContextMenu.classList.add('show');

    // Get mouse pos & limit it
    let mouseX = event.clientX;
    let mouseY = event.clientY;

    const menuWidth = repoContextMenu.offsetWidth;
    const menuHeight = repoContextMenu.offsetHeight;

    if (mouseX + menuWidth > window.innerWidth) {
        mouseX = mouseX - menuWidth;
    }
    if (mouseY + menuHeight > window.innerHeight) {
        mouseY = mouseY - menuHeight;
    }

    repoContextMenu.style.left = `${mouseX}px`;
    repoContextMenu.style.top = `${mouseY}px`;
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

// Force close (context-menu)
window.addEventListener("click", (event) => {
    if (repoContextMenu.classList.contains("show")) {
        if (!repoContextMenu.contains(event.target) && !repoNewBtn.contains(event.target)) {
            repoContextMenu.classList.remove("show");
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
    const description = repoCreateModalInputDescription.value.trim() ?? "";
    const localPath = repoCreateModalInputPath.value.trim();
    const gitIgnore = repoCreateModalSelectGitIgnore.value.trim() ?? "None";
    const gitLicense = repoCreateModalSelectLicense.value.trim() ?? "None";

    sendIpcMessage("REPO_CREATE", {
        "repoName": repoName,
        "description": description,
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

// Input boxes (modals)
branchModalInputName.addEventListener("keyup", (event) => {
    if (event.key === "Enter") { branchModalConfirmBtn.click(); }
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

window.addEventListener('DOMContentLoaded', () => {
    sendIpcMessage("APP_READY");
});