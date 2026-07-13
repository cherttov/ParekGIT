// ------- DOM ELEMENTS -------
// Topbar
const repoContainer = document.getElementById("repositories-container");
const repoDropdown = document.getElementById("repository-dropdown-panel");
const repoNewBtn = document.querySelector("#repositories-container .btn-add");
const repoBtn = document.querySelector("#repositories-container .topbar-btn");
const repoPanel = document.querySelector("#repositories-container .dropdown-panel");
const repoList = document.getElementById("repo-topbar-list");
const repoScrollbar = document.getElementById("repo-topbar-scrollbar");

const branchContainer = document.getElementById("branches-container");
const branchDropdown = document.getElementById("branch-dropdown-panel");
const branchNewBtn = document.querySelector("#branches-container .btn-new");
const branchBtn = document.querySelector("#branches-container .topbar-btn");
const branchPanel = document.querySelector("#branches-container .dropdown-panel");
const branchList = document.getElementById("branch-topbar-list");
const branchScrollbar = document.getElementById("branch-topbar-scrollbar");

const backdrop = document.getElementById("dropdown-backdrop");

// Left Sidebar
const leftSidebar = document.getElementById("left-sidebar");

const fileBtn = leftSidebar.querySelector(".btn-file");
const mergeBtn = leftSidebar.querySelector(".btn-merge");
const branchesBtn = leftSidebar.querySelector(".btn-branches");
const analyticsBtn = leftSidebar.querySelector(".btn-analytics");
const todoBtn = leftSidebar.querySelector(".btn-todo");
const configBtn = leftSidebar.querySelector(".btn-config");
const fetchBtn = leftSidebar.querySelector(".btn-fetch");
const settingsBtn = leftSidebar.querySelector(".btn-settings");
const accountBtn = leftSidebar.querySelector(".btn-account");

// Main Content
const mainContent = document.getElementById("main-content");

const diffViewer = document.getElementById("diff-viewer");
const diffFilename = document.getElementById("diff-filename");
const diffContent = document.getElementById("diff-content");
const diffBodyWrapper = document.getElementById("diff-body-wrapper");
const diffScrollbar = document.getElementById("diff-scrollbar");

const detailsViewer = document.getElementById("details-viewer");
const detailsCommitMessage = document.getElementById("details-commit-message");
const detailsCommitStats = document.getElementById("details-commit-stats");
const detailsContent = document.getElementById("details-content");
const detailsBodyWrapper = document.getElementById("details-body-wrapper");
const detailsScrollbar = document.getElementById("details-scrollbar");
const detailsBtn = document.querySelector(".details-footer-btn");
const detailsBtnValue = detailsBtn.querySelector(".btn-value");
const detailsPanel = document.getElementById("details-footer-panel");
const detailsFileList = document.getElementById("details-file-list");
const detailsFileScrollbar = document.getElementById("details-file-scrollbar");

// Right Sidebar + SplitContainer
const rightSidebar = document.getElementById("right-sidebar");
const resizer = document.getElementById("resizer");

const tabBtnChanges = document.getElementById("tab-btn-changes");
const tabBtnHistory = document.getElementById("tab-btn-history");
const tabChanges = document.getElementById("changes-tab");
const tabHistory = document.getElementById("history-tab");

const changesHeader = document.getElementById("changes-header");
const changesCountText = document.getElementById("changes-count-text");
const changesMasterCheckbox = document.getElementById("changes-master-checkbox");
const changesList = document.getElementById("changes-list");
const changesScrollbar = document.getElementById("changes-scrollbar");
const commitMessageInput = document.getElementById("commit-name-input");
const commitDescriptionInput = document.getElementById("commit-desc-input");
const commitBtn = document.getElementById("commit-btn");

const historyList = document.getElementById("history-list");
const historyScrollbar = document.getElementById("history-scrollbar");

// Modals
const modalBackdrops = document.querySelectorAll('.modal-backdrop');
const modalCloseTriggers = document.querySelectorAll('.close-modal-icon, .cancel-modal-btn');

const branchNewModal = document.getElementById("branch-new-modal");
const branchNewModalInputName = branchNewModal.querySelector(".modal-input");
const branchNewModalConfirmBtn = branchNewModal.querySelector(".confirm-modal-btn");

const branchRenameModal = document.getElementById("branch-rename-modal");
const branchRenameModalInputName = branchRenameModal.querySelector(".modal-input");
const branchRenameModalConfirmBtn = branchRenameModal.querySelector(".confirm-modal-btn");

const branchDeleteModal = document.getElementById("branch-delete-modal");
const branchDeleteModalConfirmBtn = branchDeleteModal.querySelector(".confirm-modal-btn");

const branchMergeModal = document.getElementById("branch-merge-modal");
const branchMergeModalSourceLabel = document.getElementById("merge-modal-source-branch");
const branchMergeModalTargetLabel = document.getElementById("merge-modal-target-branch");
const branchMergeModalSelectSource = document.getElementById("merge-modal-select-source");
const branchMergeModalSelectTarget = document.getElementById("merge-modal-select-target");
const branchMergeModalConfirmBtn = branchMergeModal.querySelector(".confirm-modal-btn");

const repoCloneModal = document.getElementById("repo-clone-modal");
const repoCloneModalConfirmBtn = repoCloneModal.querySelector(".confirm-modal-btn");

const repoCreateModal = document.getElementById("repo-create-modal");
const repoCreateModalInputName = repoCreateModal.querySelector(".input-name");
const repoCreateModalInputPath = repoCreateModal.querySelector(".input-path");
const repoCreateModalBrowseBtn = repoCreateModal.querySelector(".browse-btn");
const repoCreateModalSelectGitIgnore = repoCreateModal.querySelector("#select-git-ignore");
const repoCreateModalSelectLicense = repoCreateModal.querySelector("#select-git-license");
const repoCreateModalLicenseEdit = repoCreateModal.querySelector("#license-edit-btn");
const repoCreateModalLicenseEditPanel = repoCreateModal.querySelector("#license-edit-panel");
const repoCreateModalConfirmBtn = repoCreateModal.querySelector(".confirm-modal-btn");

const repoAddModal = document.getElementById("repo-add-modal");
const repoAddModalInputPath = repoAddModal.querySelector(".modal-input");
const repoAddModalBrowseBtn = repoAddModal.querySelector(".browse-btn");
const repoAddModalConfirmBtn = repoAddModal.querySelector(".confirm-modal-btn");

const repoRemoveModal = document.getElementById("repo-remove-modal");
const repoRemoveModalName = document.getElementById("remove-modal-repo-name");
const repoRemoveModalLocalCheckbox = repoRemoveModal.querySelector(".ui-checkbox");
const repoRemoveModalConfirmBtn = repoRemoveModal.querySelector(".confirm-modal-btn");

const settingsModal = document.getElementById("settings-modal");
const settingsModalThemeSelect = document.getElementById("settings-theme-select");
const settingsModalLogsView = document.getElementById("settings-logs-view");
const settingsModalLogsClear = document.getElementById("settings-logs-clear");
const settingsModalConfirmBtn = settingsModal.querySelector(".confirm-modal-btn");

const accountModal = document.getElementById("account-modal");
const accountModalInputName = accountModal.querySelector(".input-name");
const accountModalInputEmail = accountModal.querySelector(".input-email");
const accountModalConfirmBtn = accountModal.querySelector(".confirm-modal-btn");

const todoModal = document.getElementById("todo-modal");
const todoModalRowsContainer = document.getElementById("todo-rows-container");
const todoScrollbar = document.getElementById("todo-scrollbar");
const todoModalConfirmBtn = todoModal.querySelector(".confirm-modal-btn");

const configModal = document.getElementById("config-modal");
const configModalName = configModal.querySelector(".input-name");
const configModalEmail = configModal.querySelector(".input-email");
const configModalConfirmBtn = configModal.querySelector(".confirm-modal-btn");

const errorModal = document.getElementById("error-modal");
const errorModalMessage = document.getElementById("error-modal-message")

const repoMissingModal = document.getElementById("repo-missing-modal");
const repoMissingModalConfirmBtn = repoMissingModal.querySelector(".confirm-modal-btn");
const repoMissingModalCancelBtn = repoMissingModal.querySelector(".cancel-modal-btn");

// Context menus
const repoContextMenu = document.getElementById("repo-context-menu");
const repoMenuClone = repoContextMenu.querySelector(".context-menu-item.item-clone");
const repoMenuCreate = repoContextMenu.querySelector(".context-menu-item.item-create");
const repoMenuAdd = repoContextMenu.querySelector(".context-menu-item.item-add");

const repoItemContextMenu = document.getElementById("repo-item-context-menu");
const repoItemMenuTerminal = repoItemContextMenu.querySelector(".context-menu-item.item-terminal");
const repoItemMenuExplorer = repoItemContextMenu.querySelector(".context-menu-item.item-explorer");
const repoItemMenuRemove = repoItemContextMenu.querySelector(".context-menu-item.item-remove");

const branchItemContextMenu = document.getElementById("branch-item-context-menu");
const branchItemMenuRename = branchItemContextMenu.querySelector(".context-menu-item.item-rename");
const branchItemMenuCopy = branchItemContextMenu.querySelector(".context-menu-item.item-copy");
const branchItemMenuMerge = branchItemContextMenu.querySelector(".context-menu-item.item-merge");
const branchItemMenuDelete = branchItemContextMenu.querySelector(".context-menu-item.item-delete");

const topbarRepoContextMenu = document.getElementById("topbar-repo-context-menu");
const topbarRepoMenuClone = topbarRepoContextMenu.querySelector(".context-menu-item.item-clone");
const topbarRepoMenuCreate = topbarRepoContextMenu.querySelector(".context-menu-item.item-create");
const topbarRepoMenuAdd = topbarRepoContextMenu.querySelector(".context-menu-item.item-add");
const topbarRepoMenuTerminal = topbarRepoContextMenu.querySelector(".context-menu-item.item-terminal");
const topbarRepoMenuExplorer = topbarRepoContextMenu.querySelector(".context-menu-item.item-explorer");
const topbarRepoMenuRemove = topbarRepoContextMenu.querySelector(".context-menu-item.item-remove");

const topbarBranchContextMenu = document.getElementById("topbar-branch-context-menu");
const topbarBranchMenuNew = topbarBranchContextMenu.querySelector(".context-menu-item.item-new");
const topbarBranchMenuRename = topbarBranchContextMenu.querySelector(".context-menu-item.item-rename");
const topbarBranchMenuCopy = topbarBranchContextMenu.querySelector(".context-menu-item.item-copy");
const topbarBranchMenuMerge = topbarBranchContextMenu.querySelector(".context-menu-item.item-merge");
const topbarBranchMenuDelete = topbarBranchContextMenu.querySelector(".context-menu-item.item-delete");

const changesItemContextMenu = document.getElementById("changes-item-context-menu");
const changesItemMenuDiscard = changesItemContextMenu.querySelector(".context-menu-item.item-discard");
const changesItemMenuIgnoreFile = changesItemContextMenu.querySelector(".context-menu-item.item-ignore-file");
const changesItemMenuCopyAbsPath = changesItemContextMenu.querySelector(".context-menu-item.item-copy-abs-path");
const changesItemMenuCopyRelPath = changesItemContextMenu.querySelector(".context-menu-item.item-copy-rel-path");
const changesItemMenuExplorer = changesItemContextMenu.querySelector(".context-menu-item.item-explorer");

const historyItemContextMenu = document.getElementById("history-item-context-menu");
const historyItemMenuCheckout = historyItemContextMenu.querySelector(".context-menu-item.item-checkout");
const historyItemMenuRevert = historyItemContextMenu.querySelector(".context-menu-item.item-revert");
const historyItemMenuCreateBranch = historyItemContextMenu.querySelector(".context-menu-item.item-create-branch");
const historyItemMenuCopySHA = historyItemContextMenu.querySelector(".context-menu-item.item-copy-sha");

const protectedBranches = ["main", "master"];
const binaryExts = ['.exe', '.dll', '.bin', '.obj', '.pdb', '.png', '.jpg', '.jpeg', '.gif', '.ico', '.zip', '.tar', '.gz', '.7z', '.pdf'];

// ------- APP STATE -------
let currentRepoPath = "";
let currentBranch = "";
let activeHistoryHash = "";
let activeBrowseInput = null;
let activeDetailsFile = "";
let currentChangesCount = 0;
let repoDrafts = {};
let fetchStopRequested = false;
let currentTheme = "catppuccin-mocha";
let draftTodos = [];
let activeTodos = [];
let currentHistorySkip = 0;
let historyTake = 50;
let isFetchingHistory = false;
let hasReachedEndOfHistory = false;

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
        case "APP_INITIALIZED":
            appInit(data.Payload);
            break;

        case "BRANCHES_LOADED":
            loadBranchesIntoDropdown(data.Payload);
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

        case "REPO_COMMITTED":
            processCommit();
            break;

        case "BRANCH_HISTORY_LOADED":
            currentHistorySkip = 0;
            isFetchingHistory = false;
            hasReachedEndOfHistory = data.Payload.length < historyTake;
            renderHistory(data.Payload, false);
            break;

        case "BRANCH_HISTORY_APPENDED":
            isFetchingHistory = false;
            if (data.Payload.length < historyTake) { hasReachedEndOfHistory = true; }
            renderHistory(data.Payload, true);
            break;

        case "FILE_DIFF_LOADED":
            renderFileDiff(data.Payload.diffText, diffContent, diffBodyWrapper, diffScrollbar);
            break;

        case "BRANCH_DELETED":
            removeBranchFromDropdown(data.Payload);
            break; 

        case "BRANCH_RENAMED":
            renameBranchInDropdown(data.Payload);
            break;

        case "REPO_FETCHED":
            fetchRepo(data.Payload);
            break;

        case "REPO_FILES_CHANGED":
            processFileChanges(data.Payload);
            break;

        case "COMMIT_DETAILS_LOADED":
            loadCommitDetails(data.Payload);
            break;

        case "HISTORY_FILE_DIFF_LOADED":
            renderFileDiff(data.Payload.diffText, detailsContent, detailsBodyWrapper, detailsScrollbar);
            break;

        case "CHANGE_DISCARDED": // FINISH
            break;

        case "CHANGE_IGNORED":
            processFileChanges(data.Payload); // REVISIT
            break;

        case "HISTORY_CHECKED_OUT": // FINISH
            break;

        case "HISTORY_REVERT_RESULT": // FINISH
            break;

        case "BRANCH_MERGED":
            processBranchesMerged();
            break;

        case "SETTINGS_SAVED":
            applyTheme(data.Payload.Theme)
            break;

        case "TODO_LOADED":
            loadTodos(data.Payload.todos);
            break;

        case "TODO_SAVED":
            break;

        case "APP_ERROR":
            showErrorModal(data.Payload.message);
            break;

        case "CONFIG_LOCAL_LOADED":
            processLocalConfigLoad(data.Payload);
            break;

        case "CONFIG_LOCAL_SAVED":
            break;

        case "CONFIG_GLOBAL_LOADED":
            processGlobalConfigLoad(data.Payload);
            break;

        case "CONFIG_GLOBAL_SAVED":
            break;

        case "REPO_PATH_MISSING":
            processMissingRepo(data.Payload.absolutePath);
            break;

        default:
            console.warn("Unknown action received: ", data.Action);
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

const stopFetchAnimation = () => {
    const icon = fetchBtn.querySelector('.icon-fetch');
    if (!icon || !fetchBtn.classList.contains("fetching")) { return; }

    const currentTransform = window.getComputedStyle(icon).transform;
    fetchBtn.classList.remove("fetching");
    icon.style.transform = currentTransform;
    void icon.offsetWidth;

    icon.style.transition = "transform 0.5s ease-out, background-color 0.3s ease";
    icon.style.transform = "rotate(-45deg)";

    setTimeout(() => {
        icon.style.transform = "";
        icon.style.transition = "";
    }, 500);
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
    branchItemContextMenu.classList.remove('show');
    topbarRepoContextMenu.classList.remove('show');
    topbarBranchContextMenu.classList.remove('show');
    changesItemContextMenu.classList.remove('show');
    historyItemContextMenu.classList.remove('show');
    detailsPanel.classList.remove('show');

    document.querySelectorAll('.context-active').forEach(el => el.classList.remove('context-active'));
};

const closeAndClearModal = (modalElement) => {
    modalElement.classList.remove('show');

    // Clear all text inputs
    const inputs = modalElement.querySelectorAll('input');
    inputs.forEach(input => input.value = "");

    // Reset all select inputs
    const selects = modalElement.querySelectorAll('select');
    selects.forEach(select => {
        select.selectedIndex = 0;
        select.dispatchEvent(new Event('change'));
    });
};

const toggleDropdown = (toShow, toHide, event) => {
    event.stopPropagation();
    updatePanelWidths();
    toHide.classList.remove('show');
    detailsPanel.classList.remove('show');

    document.querySelectorAll('.context-active').forEach(el => el.classList.remove('context-active'));

    const isOpening = toShow.classList.toggle('show');
    backdrop.classList.toggle('show', isOpening);

    if (isOpening) {
        setTimeout(() => {
            if (toShow === repoPanel) { updateCustomScrollbar(repoList, repoScrollbar); }
            if (toShow === branchPanel) { updateCustomScrollbar(branchList, branchScrollbar); }
        }, 10);
    }
};

// Commit button disabling/enabling
const toggleCommitButton = () => {
    const checkedCount = document.querySelectorAll(".changes-item-checkbox:checked").length;

    // Wait for C# to return the branch name
    if (currentBranch === "") {
        commitBtn.disabled = true;
        commitBtn.classList.add("disabled");
        commitBtn.textContent = "Loading...";
        return;
    }

    // Commit state
    const commitText = checkedCount > 0
        ? `Commit ${checkedCount} file${checkedCount === 1 ? '' : 's'} to ${currentBranch}`
        : `Commit`;

    if (commitMessageInput.value.trim() === "" || checkedCount === 0) {
        commitBtn.disabled = true;
        commitBtn.classList.add("disabled");
        commitBtn.textContent = commitText;
    } else {
        commitBtn.disabled = false;
        commitBtn.classList.remove("disabled");
        commitBtn.textContent = commitText;
    }
};

// Context menu validators
const validateBranchContextMenu = (branchName, renameBtn, deleteBtn) => {
    const isProtected = protectedBranches.includes(branchName.toLowerCase());

    renameBtn.classList.toggle("disabled", isProtected);
    renameBtn.disabled = isProtected;

    deleteBtn.classList.toggle("disabled", isProtected);
    deleteBtn.disabled = isProtected;
};

const validateRepoContextMenu = (repoName, terminalBtn, explorerBtn, removeBtn, isInvalid = false) => {
    const isRepoEmpty = !repoName || repoName.trim() === "";

    const disableTools = isRepoEmpty || isInvalid;

    terminalBtn.classList.toggle("disabled", disableTools);
    terminalBtn.disabled = disableTools;

    explorerBtn.classList.toggle("disabled", disableTools);
    explorerBtn.disabled = disableTools;

    removeBtn.classList.toggle("disabled", isRepoEmpty);
    removeBtn.disabled = isRepoEmpty;
};

// Modal input boxes validators
const validateBranchNewModal = () => {
    const isValid = branchNewModalInputName.value.trim() !== "";
    branchNewModalConfirmBtn.disabled = !isValid;
    branchNewModalConfirmBtn.classList.toggle("disabled", !isValid);
};

const validateBranchRenameModal = () => {
    const newName = branchRenameModalInputName.value.trim();
    const oldName = branchRenameModal.dataset.targetName || currentBranch;

    const isValid = newName !== ""
                    && newName !== oldName
                    && !protectedBranches.includes(newName);

    branchRenameModalConfirmBtn.disabled = !isValid;
    branchRenameModalConfirmBtn.classList.toggle("disabled", !isValid);
};

const validateRepoCreateModal = () => {
    const isValid = repoCreateModalInputName.value.trim() !== ""
        && repoCreateModalInputPath.value.trim() !== "";
    repoCreateModalConfirmBtn.disabled = !isValid;
    repoCreateModalConfirmBtn.classList.toggle("disabled", !isValid);
};

const validateRepoAddModal = () => {
    const isValid = repoAddModalInputPath.value.trim() !== "";
    repoAddModalConfirmBtn.disabled = !isValid;
    repoAddModalConfirmBtn.classList.toggle("disabled", !isValid);
};

const validateBranchMergeModal = () => {
    const sourceBranch = branchMergeModalSelectSource.value;
    const targetBranch = branchMergeModalSelectTarget.value;

    const isValid = sourceBranch !== "" && targetBranch !== "" && sourceBranch !== targetBranch;

    branchMergeModalConfirmBtn.classList.toggle("disabled", !isValid);
    branchMergeModalConfirmBtn.disabled = !isValid;
};

// Changes header checkbox updater
const updateMasterCheckboxState = () => {
    const allFileCheckboxes = Array.from(document.querySelectorAll(".changes-item-checkbox"));
    if (allFileCheckboxes.length === 0) { return; }

    const allChecked = allFileCheckboxes.every(box => box.checked);
    changesMasterCheckbox.checked = allChecked;

    toggleCommitButton();
};

// Tab switching helpers
const switchToChangesTab = () => {
    tabBtnChanges.classList.add("active");
    tabBtnHistory.classList.remove("active");
    tabChanges.classList.add("active");
    tabHistory.classList.remove("active");

    diffViewer.style.display = "flex";
    detailsViewer.style.display = "none";
};
const switchToHistoryTab = () => {
    tabBtnChanges.classList.remove("active");
    tabBtnHistory.classList.add("active");
    tabChanges.classList.remove("active");
    tabHistory.classList.add("active");

    diffViewer.style.display = "none";
    detailsViewer.style.display = "flex";
};

// Update custom scrollbar
const updateCustomScrollbar = (container, scrollbar) => {
    // If should be visible
    if (!container || !scrollbar) { return; }

    const containerH = container.clientHeight;
    const contentH = container.scrollHeight;

    if (contentH <= containerH) {
        scrollbar.style.display = "none";
        return;
    }

    scrollbar.style.display = "block";

    const EDGE_PADDING = 6;

    const usableTrackHeight = containerH - (EDGE_PADDING * 2);

    // Style math
    const heightRatio = containerH / contentH;
    const thumbHeight = Math.max(heightRatio * usableTrackHeight, 30);

    const scrollPercentage = container.scrollTop / (contentH - containerH);
    const maxThumbTop = usableTrackHeight - thumbHeight;
    const thumbTop = EDGE_PADDING + (scrollPercentage * maxThumbTop);

    // Apply style
    scrollbar.style.height = `${thumbHeight}px`;
    scrollbar.style.transform = `translateY(${thumbTop}px)`;
};

const interactCustomScrollbar = (container, scrollbar) => {
    let isDragging = false;
    let startY;
    let startScrollTop;

    scrollbar.addEventListener('mousedown', (e) => {
        if (e.button !== 0) { return; }

        isDragging = true;
        startY = e.clientY;
        startScrollTop = container.scrollTop;

        scrollbar.classList.add('focused');

        document.body.style.userSelect = 'none';
        document.addEventListener('mousemove', onMouseMove);
        document.addEventListener('mouseup', onMouseUp);
    });

    const onMouseMove = (e) => {
        if (!isDragging) { return; }

        const deltaY = e.clientY - startY;

        const containerH = container.clientHeight;
        const contentH = container.scrollHeight;
        const EDGE_PADDING = 6;
        const usableTrackHeight = containerH - (EDGE_PADDING * 2);

        const heightRatio = containerH / contentH;
        const thumbHeight = Math.max(heightRatio * usableTrackHeight, 30);
        const maxThumbTop = usableTrackHeight - thumbHeight;

        if (maxThumbTop <= 0) { return; }

        const scrollRatio = (contentH - containerH) / maxThumbTop;
        container.scrollTop = startScrollTop + (deltaY * scrollRatio);
    };

    const onMouseUp = () => {
        isDragging = false;
        scrollbar.classList.remove('focused');
        document.body.style.userSelect = '';
        document.removeEventListener('mousemove', onMouseMove);
        document.removeEventListener('mouseup', onMouseUp);
    }
};

// Apply app theme
const applyTheme = (themeName) => {
    document.body.className = document.body.className.replace(/theme-\S+/g, '');
    if (themeName) { document.body.classList.add(`theme-${themeName}`); }
}

// Draft memory helpers
const saveDraft = () => {
    if (!currentRepoPath) { return; }

    repoDrafts[currentRepoPath] = {
        message: commitMessageInput.value,
        description: commitDescriptionInput.value
    };
};

const loadDraft = () => {
    if (!currentRepoPath) { return; }

    const draft = repoDrafts[currentRepoPath];

    if (draft) {
        commitMessageInput.value = draft.message;
        commitDescriptionInput.value = draft.description;
    } else {
        commitMessageInput.value = "";
        commitDescriptionInput.value = "";
    }
};

const resetDiffViewer = () => {
    diffFilename.textContent = "Select a file to view changes";
    diffContent.textContent = "";
    updateCustomScrollbar(diffBodyWrapper, diffScrollbar);
};

const resetDetailsViewer = () => {
    detailsCommitMessage.textContent = "Select a commit to view details";
    detailsCommitStats.textContent = "";
    detailsContent.textContent = "";

    detailsBtnValue.textContent = "No commit selected";
    detailsBtn.disabled = true;
    detailsBtn.classList.add("disabled");
    detailsFileList.innerHTML = "";
};

const escapeHtml = (unsafeText) => {
    return unsafeText
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039");
};

// Merge modal helper
const setupMergeModal = (preselectedTarget) => {
    branchMergeModalSelectSource.innerHTML = "";
    branchMergeModalSelectTarget.innerHTML = "";

    const defaultSourceOption = document.createElement("option");
    defaultSourceOption.value = "";
    defaultSourceOption.textContent = "Select source...";
    defaultSourceOption.disabled = true;
    defaultSourceOption.selected = true;
    branchMergeModalSelectSource.appendChild(defaultSourceOption);

    const branchItems = branchDropdown.querySelectorAll('.dropdown-item');
    branchItems.forEach(item => {
        const branchName = item.dataset.branchName;
        if (!branchName) { return; }

        const optionSource = document.createElement("option");
        optionSource.value = branchName;
        optionSource.textContent = branchName;
        branchMergeModalSelectSource.appendChild(optionSource);

        const optionTarget = document.createElement("option");
        optionTarget.value = branchName;
        optionTarget.textContent = branchName;
        branchMergeModalSelectTarget.appendChild(optionTarget);
    });

    branchMergeModalSelectTarget.value = preselectedTarget || currentBranch;

    branchMergeModalSourceLabel.textContent = "{ none }";
    branchMergeModalTargetLabel.textContent = branchMergeModalSelectTarget.value;

    branchMergeModalSelectSource.onchange = (e) => {
        branchMergeModalSourceLabel.textContent = e.target.value;
        validateBranchMergeModal();
    };
    branchMergeModalSelectTarget.onchange = (e) => {
        branchMergeModalTargetLabel.textContent = e.target.value;
        validateBranchMergeModal();
    };

    validateBranchMergeModal();
};

// Todo modal container list 
const renderTodoList = () => {
    todoModalRowsContainer.innerHTML = "";

    // Render all existing todo items
    draftTodos.forEach((todo, index) => {
        const row = document.createElement("div");
        row.className = "todo-row";

        // Checkbox
        const checkbox = document.createElement("input");
        checkbox.type = "checkbox";
        checkbox.className = "ui-checkbox todo-checkbox";
        checkbox.checked = todo.isCompleted;
        checkbox.addEventListener("click", () => {
            todo.isCompleted = checkbox.checked;

            if (todo.isCompleted) { label.classList.add("done"); }
            else { label.classList.remove("done"); }
        });

        // Label
        const label = document.createElement("span");
        label.className = `todo-label ${todo.isCompleted ? "done" : ""}`;
        label.textContent = todo.text;

        // Delete
        const deleteBtn = document.createElement("button");
        deleteBtn.className = "todo-remove-btn";
        deleteBtn.innerHTML = "&times;";
        deleteBtn.addEventListener("click", () => {
            draftTodos.splice(index, 1);
            renderTodoList();
        });

        row.appendChild(checkbox);
        row.appendChild(label);
        row.appendChild(deleteBtn);
        todoModalRowsContainer.appendChild(row);
    });

    // Render constant new task row
    const newRow = document.createElement("div");
    newRow.className = "todo-row";

    const newInput = document.createElement("input");
    newInput.type = "text";
    newInput.className = "modal-input";
    newInput.placeholder = "Add new task (press enter)";

    newInput.addEventListener("keyup", (e) => {
        if (e.key === "Enter" && newInput.value.trim() !== "") {
            draftTodos.push({
                text: newInput.value.trim(),
                isCompleted: false
            });
            renderTodoList();

            // Refocus on new task input
            const newInputs = todoModalRowsContainer.querySelectorAll(".todo-row .modal-input");
            if (newInputs.length > 0) {
                newInputs[newInputs.length - 1].focus();
            }
        }
    });

    newRow.appendChild(newInput);
    todoModalRowsContainer.appendChild(newRow);

    setTimeout(() => {
        updateCustomScrollbar(todoModalRowsContainer, todoScrollbar);
    }, 10);
};

// Repo dropdown item invalid helper
function markRepoDropdownItemInvalid(repoPath) {
    const items = repoPanel.querySelectorAll(".dropdown-item");
    items.forEach(item => {
        if (item.dataset.path.toLowerCase() === repoPath.toLowerCase()) {
            item.classList.add("invalid");
            item.title = "Repository folder not found";
        }
    });
}

// C# - Load diff text
function renderFileDiff(diffText, contentTarget, wrapperTarget, scrollbarTarget) {
    if (!diffText || diffText.trim() === "") {
        contentTarget.textContent = "(No changes or binary file)";
        updateCustomScrollbar(wrapperTarget, scrollbarTarget);
        return;
    }

    // Intercept binary files
    if (diffText.includes("Binary files ")) {
        contentTarget.innerHTML = `<span class="diff-chunk">Binary file changed (no preview available)</span>`;
        updateCustomScrollbar(wrapperTarget, scrollbarTarget);
        return;
    }
    
    const lines = diffText.split('\n');
    const formattedLines = [];
    let hitFirstChunk = !diffText.startsWith('diff --git');

    for (let line of lines) {
        // skip git diff headers
        if (!hitFirstChunk) {
            if (line.startsWith('@@')) { hitFirstChunk = true }
            else { continue; }
        }

        if (line.startsWith('\\')) { continue; }

        let safeLine = escapeHtml(line);

        // Apply classes based on character (1 spaces)
        if (safeLine.startsWith('+')) {
            formattedLines.push(`<span class="diff-add">+ ${safeLine.substring(1)}</span>`);
        } else if (safeLine.startsWith('-')) {
            formattedLines.push(`<span class="diff-remove">- ${safeLine.substring(1)}</span>`);
        } else if (safeLine.startsWith('@@')) {
            formattedLines.push(`<span class="diff-chunk">${safeLine.substring(1)}</span>`);
        } else if (safeLine.startsWith(' ')) {
            formattedLines.push(`<span class="diff-normal">  ${safeLine.substring(1)}</span>`);
        } else {
            formattedLines.push(`<span class="diff-normal">  ${safeLine.substring(1)}</span>`);
        }
    }

    // Inject colored html
    if (formattedLines.length === 0) {
        contentTarget.innerHTML = `<span class="diff-chunk">Empty file (no content to preview)</span>`;
    } else {
        contentTarget.innerHTML = formattedLines.join('');
    }

    updateCustomScrollbar(wrapperTarget, scrollbarTarget);
}

// C# - App init
function appInit(data) {
    // Visuals
    currentTheme = data.Settings.Theme;
    applyTheme(data.Settings.Theme);
    settingsModalThemeSelect.value = data.Settings.Theme;

    // Data
    loadRepositoriesIntoDropdown(data.Repositories);

    // Load last repo
    if (data.Settings.LastRepoPath) {
        const cssPath = data.Settings.LastRepoPath.replace(/\\/g, '\\\\');

        const lastRepoItem = repoDropdown.querySelector(`.dropdown-item[data-path="${cssPath}"]`);

        if (lastRepoItem) {
            lastRepoItem.click();
        }
    }

    document.body.classList.remove("loading");
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

        // If repoPath is invalid
        if (!repo.IsValid) {
            item.classList.add("invalid");
            item.title = "Repository folder not found";
        }

        item.innerHTML = `<span class="dropdown-item-text">${repo.Name}</span>`;

        // LMB - select
        item.addEventListener("click", () => {
            // Item invalid || already selected
            if (item.classList.contains("invalid")) { return; }
            if (currentRepoPath.toLowerCase() === repo.AbsolutePath.toLowerCase()) { return; }

            currentRepoPath = repo.AbsolutePath;
            currentBranch = "";

            loadDraft();
            toggleCommitButton();
            switchToChangesTab();
            resetDiffViewer();
            resetDetailsViewer();
            activeTodos = [];

            sendIpcMessage("REPO_SELECTED", { absolutePath: repo.AbsolutePath });
            sendIpcMessage("GET_REPO_STATUS", { repoPath: repo.AbsolutePath });
            sendIpcMessage("TODO_LOAD", { repoPath: repo.AbsolutePath });

            branchBtn.classList.remove("disabled");
            branchBtn.disabled = false;
            mergeBtn.classList.remove("disabled");
            todoBtn.classList.remove("disabled");
            configBtn.classList.remove("disabled");
            fetchBtn.classList.remove("disabled");
        });

        // RMB - context menu
        item.addEventListener("contextmenu", (event) => {
            event.preventDefault();
            event.stopPropagation();

            repoContextMenu.classList.remove("show");
            repoDropdown.querySelectorAll('.context-active').forEach(el => el.classList.remove('context-active'));
            item.classList.add("context-active");

            repoItemContextMenu.dataset.targetPath = repo.AbsolutePath;
            repoItemContextMenu.dataset.targetName = repo.Name;

            const isInvalid = item.classList.contains("invalid");

            validateRepoContextMenu(
                repo.AbsolutePath,
                repoItemMenuTerminal,
                repoItemMenuExplorer,
                repoItemMenuRemove,
                isInvalid
            );

            repoItemContextMenu.classList.add("show");

            placeContextMenu(event, repoItemContextMenu);
        });

        repoList.appendChild(item);
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
            currentBranch = branch.Name;
        }
        else {
            item.className = "dropdown-item";
        }

        item.dataset.branchName = branch.Name;

        item.innerHTML = `<span class="dropdown-item-text">${branch.Name}</span>`;

        // LMB - select
        item.addEventListener("click", () => {
            if (currentBranch.toLowerCase() === branch.Name.toLowerCase()) { return; }

            resetDiffViewer();
            resetDetailsViewer();

            historyList.innerHTML = "";
            changesList.innerHTML = "";

            sendIpcMessage("BRANCH_SELECTED", {
                absolutePath: currentRepoPath,
                branchName: branch.Name,
                isRemote: branch.IsRemote
            });
        });

        // RMB - context menu
        item.addEventListener("contextmenu", (event) => {
            event.preventDefault();
            event.stopPropagation();

            branchItemContextMenu.dataset.targetPath = currentRepoPath;
            branchItemContextMenu.dataset.targetName = branch.Name;

            validateBranchContextMenu(branch.Name, branchItemMenuRename, branchItemMenuDelete);

            branchItemContextMenu.classList.add("show");
            branchDropdown.querySelectorAll('.context-active').forEach(el => el.classList.remove('context-active'));
            item.classList.add("context-active");

            placeContextMenu(event, branchItemContextMenu);
        });

        branchList.appendChild(item);
    });

    branchBtnValue.textContent = currentBranchName;

    toggleCommitButton();

    // Load changes & history
    if (currentRepoPath) {
        sendIpcMessage("GET_REPO_STATUS", { repoPath: currentRepoPath });

        if (currentBranch) {
            sendIpcMessage("GET_BRANCH_HISTORY", {
                repoPath: currentRepoPath,
                branchName: currentBranch
            });
        }
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
        branchBtn.disabled = true;
        mergeBtn.classList.add("disabled");
        todoBtn.classList.add("disabled");
        configBtn.classList.add("disabled");
        fetchBtn.classList.add("disabled");

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
        activeBrowseInput.dispatchEvent(new Event('input'));
        activeBrowseInput.focus();
        activeBrowseInput = null;
    }
}

// C# - Add repository to dropdown
function addRepositoryToDropdown(repo) {
    const item = document.createElement("div");

    item.className = "dropdown-item";
    item.dataset.path = repo.AbsolutePath; 
    item.innerHTML = `<span class="dropdown-item-text">${repo.Name}</span>`;

    // If repoPath is invalid
    if (!repo.IsValid) {
        item.classList.add("invalid");
        item.title = "Repository folder not found";
    }

    // LMB - select
    item.addEventListener("click", () => {
        // Item invalid || already selected
        if (item.classList.contains("invalid")) { return; }
        if (currentRepoPath.toLowerCase() === repo.AbsolutePath.toLowerCase()) { return; }

        currentRepoPath = repo.AbsolutePath;
        currentBranch = "";

        toggleCommitButton();
        resetDiffViewer();
        resetDetailsViewer();
        activeTodos = [];

        sendIpcMessage("REPO_SELECTED", { absolutePath: repo.AbsolutePath });
        sendIpcMessage("GET_REPO_STATUS", { repoPath: repo.AbsolutePath });
        sendIpcMessage("TODO_LOAD", { repoPath: repo.AbsolutePath });

        branchBtn.classList.remove("disabled");
        branchBtn.disabled = false;
        mergeBtn.classList.remove("disabled");
        todoBtn.classList.remove("disabled");
        configBtn.classList.remove("disabled");
        fetchBtn.classList.remove("disabled");
    });

    // RMB - context menu
    item.addEventListener("contextmenu", (event) => {
        event.preventDefault();
        event.stopPropagation();

        repoContextMenu.classList.remove("show");
        repoDropdown.querySelectorAll('.context-active').forEach(el => el.classList.remove('context-active'));
        item.classList.add("context-active");

        repoItemContextMenu.dataset.targetPath = repo.AbsolutePath;
        repoItemContextMenu.dataset.targetName = repo.Name;

        const isInvalid = item.classList.contains("invalid");

        validateRepoContextMenu(
            repo.AbsolutePath,
            repoItemMenuTerminal,
            repoItemMenuExplorer,
            repoItemMenuRemove,
            isInvalid
        );

        repoItemContextMenu.classList.add("show");

        placeContextMenu(event, repoItemContextMenu);
    });

    repoList.appendChild(item);

    if (repo.IsValid) {
        item.click();
    }
}

// C# - Load changed files to right sidebar
function renderChangedFiles(files) {
    changesList.innerHTML = "";

    currentChangesCount = files.length;
    changesCountText.textContent = `${files.length} changed file${files.length === 1 ? '' : 's'}`;

    if (files.length === 0) {
        toggleCommitButton();
        return;
    }

    files.forEach(file => {
        const item = document.createElement("div");
        item.className = "change-item";
        item.dataset.path = file.Path;

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

        // LMB - Change item clicking
        item.addEventListener("click", (event) => {
            if (event.target.classList.contains("changes-item-checkbox")) { return; }
            if (item.classList.contains("selected")) { return; }

            document.querySelectorAll(".change-item").forEach(el => el.classList.remove("selected"));
            item.classList.add("selected");

            diffFilename.textContent = file.Path;

            // Check for binary extensions
            const ext = file.Path.substring(file.Path.lastIndexOf('.')).toLowerCase();
            if (binaryExts.includes(ext)) {
                diffContent.textContent = "Binary file changed (no preview available)";
                return;
            }

            diffContent.textContent = "Loading changes...";

            sendIpcMessage("GET_FILE_DIFF", {
                repoPath: currentRepoPath,
                filePath: file.Path
            });
        });

        const checkbox = item.querySelector(".changes-item-checkbox");
        checkbox.addEventListener("change", () => {
            updateMasterCheckboxState();
        });

        // RMB - Change item context menu
        item.addEventListener("contextmenu", (event) => {
            event.preventDefault();
            event.stopPropagation();
            closeDropdowns();

            changesItemContextMenu.dataset.targetPath = file.Path;

            document.querySelectorAll(".change-item").forEach(el => el.classList.remove("context-active"));
            item.classList.add("context-active");

            changesItemContextMenu.classList.add("show");
            placeContextMenu(event, changesItemContextMenu);
        });

        changesList.appendChild(item);
    });

    toggleCommitButton();
    updateCustomScrollbar(changesList, changesScrollbar);
}

// C# - Repo committed handler
function processCommit() {
    commitMessageInput.value = "";
    commitDescriptionInput.value = "";

    saveDraft();

    resetDiffViewer();
    toggleCommitButton();
    resetDetailsViewer();

    if (currentRepoPath) {
        sendIpcMessage("GET_REPO_STATUS", { repoPath: currentRepoPath });

        if (currentBranch) {
            sendIpcMessage("GET_BRANCH_HISTORY", {
                repoPath: currentRepoPath,
                branchName: currentBranch
            });
        }
    }
}

// C# - Load/append commit history into history-tab
function renderHistory(commits, isAppending = false) {
    if (!isAppending) {
        historyList.innerHTML = "";
    }

    if (!commits || commits.length === 0) {
        updateCustomScrollbar(historyList, historyScrollbar);
        return;
    }

    commits.forEach(commit => {
        const item = document.createElement("div");
        item.className = "history-item";

        item.dataset.hash = commit.Hash;

        item.innerHTML = `
            <div class="history-message">${commit.Message}</div>
            <div class="history-meta">
                <div class="history-author">${commit.Author}</div>
                <div class="history-time"">${commit.TimeAgo}</div>
            </div>
        `;

        // LMB - Show commit history details
        item.addEventListener("click", () => {
            if (item.classList.contains("selected")) { return; }

            document.querySelectorAll(".history-item").forEach(el => el.classList.remove("selected"));
            item.classList.add("selected");

            activeHistoryHash = commit.Hash;
            detailsCommitMessage.textContent = `Loading details...`;
            detailsCommitStats.textContent = ``;
            detailsContent.textContent = "";

            detailsBtnValue.textContent = "Loading...";
            detailsBtn.disabled = true;
            detailsBtn.classList.add("disabled");

            sendIpcMessage("GET_COMMIT_DETAILS", {
                repoPath: currentRepoPath,
                commitHash: commit.Hash
            });
        });

        // RMB - History item context menu
        item.addEventListener("contextmenu", (event) => {
            event.preventDefault();
            event.stopPropagation();
            closeDropdowns();

            historyItemContextMenu.dataset.targetHash = commit.Hash;

            document.querySelectorAll(".history-item").forEach(el => el.classList.remove("context-active"));
            item.classList.add("context-active");

            historyItemContextMenu.classList.add("show");
            placeContextMenu(event, historyItemContextMenu);
        });

        historyList.appendChild(item);
    });

    updateCustomScrollbar(historyList, historyScrollbar);
}

// C# - Remove branch from dropdown
function removeBranchFromDropdown(payload) {
    const items = branchDropdown.querySelectorAll('.dropdown-item');
    items.forEach(item => {
        if (item.dataset.branchName === payload.branchName) { item.remove(); }
    });
}

// C# - Rename branch in dropdown
function renameBranchInDropdown(payload) {
    const oldName = payload.oldName;
    const newName = payload.newName;

    const items = branchDropdown.querySelectorAll('.dropdown-item');
    items.forEach(item => {
        if (item.dataset.branchName === oldName) {
            item.querySelector('.dropdown-item-text').textContent = newName;
            item.dataset.branchName = newName;
        }
    });

    if (currentBranch === oldName) {
        currentBranch = newName;
        document.querySelector('#branches-container .btn-value').textContent = newName;
        toggleCommitButton();
    }

    if (currentRepoPath) {
        sendIpcMessage("REPO_SELECTED", { absolutePath: currentRepoPath });
    }
}

// C# - Repo fetching
function fetchRepo(repo) {
    stopFetchAnimation();

    if (currentRepoPath) {
        sendIpcMessage("GET_REPO_STATUS", { repoPath: currentRepoPath });
        if (currentBranch) {
            sendIpcMessage("GET_BRANCH_HISTORY", {
                repoPath: currentRepoPath,
                branchName: currentBranch
            });
        }
    }
}

// C# - Repo files changed
function processFileChanges(repo) {
    if (currentRepoPath === repo.repoPath) {
        sendIpcMessage("GET_REPO_STATUS", { repoPath: currentRepoPath });
    } else {
        console.warn("RepoWatcher is watching unnecessary repositories.");
    }
}

// C# - History commit details
function loadCommitDetails(details) {
    activeDetailsFile = "";

    // Header info
    detailsCommitMessage.innerHTML = `${details.message}`;
    detailsCommitStats.textContent = `${details.files.length} file${details.files.length === 1 ? '' : 's'} changed`;

    detailsFileList.innerHTML = "";

    if (!details.files || details.files.length === 0) {
        detailsBtnValue.textContent = "No files available";
        detailsBtn.disabled = true;
        detailsBtn.classList.add("disabled");
        return;
    }

    detailsBtn.disabled = false;
    detailsBtn.classList.remove("disabled");

    details.files.forEach((file, index) => {
        const item = document.createElement("div");
        item.className = "dropdown-item";

        let statusClass = "status-modified";
        if (file.StatusCode.startsWith("A")) { statusClass = "status-added"; }
        else if (file.StatusCode.startsWith("D")) { statusClass = "status-deleted"; }
        else if (file.StatusCode.startsWith("R")) { statusClass = "status-renamed"; }
        else if (file.StatusCode.startsWith("U")) { statusClass = "status-untracked"; }

        item.innerHTML = `<span class="change-status ${statusClass}">${file.StatusCode[0]}</span>
                          <span class="change-path">${file.Path}</span>`;

        // LMB
        item.addEventListener("click", (event) => {
            if (file.Path === activeDetailsFile) { closeDropdowns(); return; }
            activeDetailsFile = file.Path;

            event.stopPropagation();
            detailsFileList.querySelectorAll('.dropdown-item').forEach(el => el.classList.remove("active"));
            item.classList.add('active');

            detailsBtnValue.innerHTML = `<span class="change-status ${statusClass}">${file.StatusCode[0]}</span>
                                         <span class="change-path">${file.Path}</span>`;
            closeDropdowns();

            const ext = file.Path.substring(file.Path.lastIndexOf('.').toLowerCase());
            if (binaryExts.includes(ext)) {
                detailsContent.innerHTML = `<span class="diff-chunk">Binary file changes (no preview available)</span>`;
                updateCustomScrollbar(detailsBodyWrapper, detailsScrollbar);
                return;
            }

            detailsContent.textContent = "Loading diff...";

            sendIpcMessage("GET_HISTORY_FILE_DIFF", {
                repoPath: currentRepoPath,
                commitHash: activeHistoryHash,
                filePath: file.Path
            });
        });

        detailsFileList.appendChild(item);

        if (index === 0) { item.click(); }
    });

    updateCustomScrollbar(detailsFileList, detailsFileScrollbar);
}

// C# - Branches merged handler
function processBranchesMerged() {
    resetDiffViewer();
    resetDetailsViewer();

    if (currentRepoPath) {
        sendIpcMessage("GET_BRANCHES", { absolutePath: currentRepoPath });
        sendIpcMessage("GET_REPO_STATUS", { repoPath: currentRepoPath });

        if (currentBranch) {
            sendIpcMessage("GET_BRANCH_HISTORY", {
                repoPath: currentRepoPath,
                branchName: currentBranch
            });
        }
    }
}

// C# - Load todos
function loadTodos(todos) {
    activeTodos = todos || [];
}

// C# - Handle backend errors
function showErrorModal(message) {
    errorModalMessage.textContent = message || "An unknown error occured.";
    errorModal.classList.add("show");
}

// C# - On local config load
function processLocalConfigLoad(configs) {
    configModalName.value = configs.localName || "";
    configModalEmail.value = configs.localEmail || "";

    configModalName.placeholder = configs.globalName || "Not set";
    configModalEmail.placeholder = configs.globalEmail || "Not set";

    configModal.classList.add("show");
}

// C# - On global config load
function processGlobalConfigLoad(config) {
    accountModalInputName.value = config.globalName || "";
    accountModalInputEmail.value = config.globalEmail || "";

    accountModalInputName.placeholder = config.globalName || "";
    accountModalInputEmail.placeholder = config.globalEmail || "";

    accountModal.classList.add("show");
}

// C# - Repo not found (path is invalid)
function processMissingRepo(repoPath) {
    document.getElementById("missing-modal-repo-path").textContent = repoPath;
    repoMissingModal.dataset.targetPath = repoPath;
    markRepoDropdownItemInvalid(repoPath);
    repoMissingModal.classList.add("show");

    // Unselect invalid repo if it's current
    if (currentRepoPath === repoPath) {
        currentRepoPath = "";
        currentBranch = "";

        // Reset Topbar
        const repoBtnValue = repoBtn.querySelector('.btn-value');
        if (repoBtnValue) { repoBtnValue.textContent = "None"; }

        const branchBtnValue = branchBtn.querySelector('.btn-value');
        if (branchBtnValue) { branchBtnValue.textContent = "None"; }

        // Disable buttons
        branchBtn.classList.add("disabled");
        branchBtn.disabled = true;
        mergeBtn.classList.add("disabled");
        todoBtn.classList.add("disabled");
        configBtn.classList.add("disabled");
        fetchBtn.classList.add("disabled");

        // Clear UI panels
        const existingBranches = branchDropdown.querySelectorAll('.dropdown-item');
        existingBranches.forEach(item => item.remove());

        changesList.innerHTML = "";
        historyList.innerHTML = "";
        currentChangesCount = 0;
        changesCountText.textContent = "0 changed files";

        resetDiffViewer();
        resetDetailsViewer();
        toggleCommitButton();
    }
}

// ------- EVENT LISTENERS -------
// Global overrides
document.addEventListener('wheel', (event) => {
    if (event.ctrlKey || event.metaKey) {
        event.preventDefault();
    }
}, { passive: false });

document.addEventListener("keydown", (event) => {
    if (event.key === "Escape") {
        const activeModal = document.querySelector(".modal-backdrop.show");
        if (activeModal) {
            closeAndClearModal(activeModal);
            return;
        }

        closeDropdowns();
    }
});

// Left sidebar buttons (left-sidebar)
mergeBtn.addEventListener("click", () => {
    if (!currentRepoPath || !currentBranch) { return; }

    setupMergeModal(currentBranch);
    branchMergeModal.dataset.targetName = currentBranch;
    branchMergeModal.classList.add("show");
});

todoBtn.addEventListener("click", () => {
    if (!currentRepoPath) { return; }

    draftTodos = JSON.parse(JSON.stringify(activeTodos));

    renderTodoList();
    todoModal.classList.add("show");
});

configBtn.addEventListener("click", () => {
    if (!currentRepoPath) { return; }

    sendIpcMessage("CONFIG_LOCAL_GET", { repoPath: currentRepoPath });
});

fetchBtn.addEventListener("click", () => {
    if (!currentRepoPath || fetchBtn.classList.contains("fetching")) { return; }

    fetchBtn.classList.add("fetching");

    sendIpcMessage("REPO_FETCH", {
        repoPath: currentRepoPath
    });
});

settingsBtn.addEventListener("click", () => {
    settingsModalThemeSelect.value = currentTheme;
    settingsModal.classList.add("show");
});

accountBtn.addEventListener("click", () => {
    sendIpcMessage("CONFIG_GLOBAL_GET", {});
});

// Scrollbars (topbar)
repoList.addEventListener("scroll", () => updateCustomScrollbar(repoList, repoScrollbar));
branchList.addEventListener("scroll", () => updateCustomScrollbar(branchList, branchScrollbar));

// Todo list scrollbar (scrollbar/modals)
todoModalRowsContainer.addEventListener("scroll", () => {
    updateCustomScrollbar(todoModalRowsContainer, todoScrollbar);
});

// Settings modal (modals)
settingsModalLogsView.addEventListener("click", () => {
    sendIpcMessage("EXPLORER_OPEN", {
        path: "%APP_DATA%"
    });
});

settingsModalLogsClear.addEventListener("click", () => {
    sendIpcMessage("LOGS_CLEAR", {});
});

settingsModalConfirmBtn.addEventListener("click", () => {
    const updatedSettings = {
        Id: 1,
        Theme: settingsModalThemeSelect.value
    };

    sendIpcMessage("SETTINGS_SAVE", updatedSettings);

    currentTheme = updatedSettings.Theme;

    applyTheme(updatedSettings.Theme);
    closeAndClearModal(settingsModal);
});

// Diff page (main-content)
diffBodyWrapper.addEventListener("scroll", () => updateCustomScrollbar(diffBodyWrapper, diffScrollbar));

// History details page (main-content)
detailsBodyWrapper.addEventListener("scroll", () => updateCustomScrollbar(detailsBodyWrapper, detailsScrollbar));
detailsFileList.addEventListener("scroll", () => updateCustomScrollbar(detailsFileList, detailsFileScrollbar));
detailsBtn.addEventListener("click", (event) => {
    if (detailsBtn.disabled || detailsBtn.classList.contains("disabled")) { return; }

    event.stopPropagation();
    repoPanel.classList.remove('show');
    branchPanel.classList.remove('show');
    const isOpening = detailsPanel.classList.toggle("show");
    backdrop.classList.toggle("show", isOpening);

    if (isOpening) {
        setTimeout(() => updateCustomScrollbar(detailsFileList, detailsFileScrollbar));
    }
});

// Toggles (dropdowns)
repoBtn.addEventListener('click', (event) => toggleDropdown(repoPanel, branchPanel, event));
repoBtn.addEventListener("contextmenu", (event) => {
    event.preventDefault();
    event.stopPropagation();
    closeDropdowns();

    validateRepoContextMenu(
        currentRepoPath,
        topbarRepoMenuTerminal,
        topbarRepoMenuExplorer,
        topbarRepoMenuRemove
    );

    topbarRepoContextMenu.classList.add("show");
    placeContextMenu(event, topbarRepoContextMenu);
});

branchBtn.addEventListener('click', (event) => toggleDropdown(branchPanel, repoPanel, event));
branchBtn.addEventListener("contextmenu", (event) => {
    event.preventDefault();
    event.stopPropagation();

    if (branchBtn.classList.contains("disabled")) { return; }
    closeDropdowns();

    validateBranchContextMenu(currentBranch, topbarBranchMenuRename, topbarBranchMenuDelete);

    topbarBranchContextMenu.classList.add("show");
    placeContextMenu(event, topbarBranchContextMenu);
});

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
document.querySelectorAll('.dropdown-panel, .context-menu').forEach(panel => {
    panel.addEventListener('click', (event) => event.stopPropagation());
});

// Resizer logic (split-container)
window.addEventListener('resize', () => {
    const mainCSS = window.getComputedStyle(mainContent);
    const minMainWidth = parseInt(mainCSS.minWidth) || 360;

    const rightCSS = window.getComputedStyle(rightSidebar);
    const fallbackMinRight = parseInt(rightCSS.minWidth) || 192;

    const currentMaxRightWidth = window.innerWidth - leftSidebar.offsetWidth - minMainWidth - resizer.offsetWidth;
    let currentRightWidth = parseInt(rightCSS.width);

    if (currentRightWidth > currentMaxRightWidth) {
        const safeWidth = Math.max(fallbackMinRight, currentMaxRightWidth);
        rightSidebar.style.width = `${safeWidth}px`;
        branchContainer.style.width = `${safeWidth}px`;
    }

    updatePanelWidths();
    closeDropdowns();
});

resizer.addEventListener("mousedown", (event) => {
    isResizing = true;
    closeDropdowns();

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
    if (!isResizing) { return; }

    let targetWidth = window.innerWidth - event.clientX;
    let newWidth = Math.max(minRightWidth, Math.min(targetWidth, maxRightWidth));

    // Calculate always visible
    rightSidebar.style.width = `${newWidth}px`;
    branchContainer.style.width = `${newWidth}px`;

    if (branchPanel.classList.contains('show')) {
        branchPanel.style.width = `${newWidth}px`;
    }

    if (repoPanel.classList.contains('show')) {
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

// Sidebar tab switching (right-sidebar/tab-selector)
tabBtnChanges.addEventListener("click", switchToChangesTab);

tabBtnHistory.addEventListener("click", switchToHistoryTab);

// Master checkbox logic (right-sidebar/changes-header)
changesMasterCheckbox.addEventListener("change", (event) => {
    const isChecked = event.target.checked;
    const allFileCheckboxes = document.querySelectorAll(".changes-item-checkbox");
    allFileCheckboxes.forEach(box => {
        box.checked = isChecked
    });

    toggleCommitButton();
});

// Commit section (right-sidebar)
commitMessageInput.addEventListener("input", () => {
    saveDraft();
    toggleCommitButton();
});

commitDescriptionInput.addEventListener("input", () => {
    saveDraft();
});

commitBtn.addEventListener("click", () => {
    const message = commitMessageInput.value.trim();
    const description = commitDescriptionInput.value.trim();

    if (message === "" || currentRepoPath === "" || currentChangesCount === 0) { return; }

    const selectedFiles = [];
    const allFileCheckboxes = document.querySelectorAll(".changes-item-checkbox:checked");

    allFileCheckboxes.forEach(checkbox => {
        const itemRow = checkbox.closest('.change-item');
        const filePath = itemRow.dataset.path;
        selectedFiles.push(filePath);
    });

    if (selectedFiles.length === 0) { return; }

    commitBtn.disabled = true;
    commitBtn.classList.add("disabled");

    sendIpcMessage("REPO_COMMIT", {
        repoPath: currentRepoPath,
        message: message,
        description: description,
        files: selectedFiles
    });
});

// Scrollbar (right-sidebar)
changesList.addEventListener("scroll", () => { updateCustomScrollbar(changesList, changesScrollbar); });
historyList.addEventListener("scroll", () => {
    updateCustomScrollbar(historyList, historyScrollbar);

    if (isFetchingHistory || hasReachedEndOfHistory || !currentBranch) { return; }

    const threshold = 30; // 30px
    const isNearBottom = historyList.scrollTop + historyList.clientHeight >= historyList.scrollHeight - threshold;

    if (isNearBottom) {
        isFetchingHistory = true;
        currentHistorySkip += historyTake;

        sendIpcMessage("GET_BRANCH_HISTORY", {
            repoPath: currentRepoPath,
            branchName: currentBranch,
            skip: currentHistorySkip,
            take: historyTake
        });
    }
});

window.addEventListener("resize", () => {
    updateCustomScrollbar(changesList, changesScrollbar);
    updateCustomScrollbar(historyList, historyScrollbar);
});

const listResizeObserver = new ResizeObserver(() => {
    updateCustomScrollbar(changesList, changesScrollbar);
    updateCustomScrollbar(historyList, historyScrollbar);
});

if (changesList) { listResizeObserver.observe(changesList); }
if (historyList) { listResizeObserver.observe(historyList); }

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

    branchNewModal.classList.add("show");
    validateBranchNewModal();
    setTimeout(() => { branchNewModalInputName.focus(); }, 100);
});

// Context menu options (context-menu)
topbarRepoMenuClone.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();
    repoCloneModal.classList.add("show");
    // validateRepoCloneModal();
    // setTimeout(() => { ...; }, 100);
});

topbarRepoMenuCreate.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();
    repoCreateModal.classList.add("show");
    validateRepoCreateModal();
    setTimeout(() => { repoCreateModalInputName.focus(); }, 100);
});

topbarRepoMenuAdd.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();
    repoAddModal.classList.add("show");
    validateRepoAddModal();
    setTimeout(() => { repoAddModalInputPath.focus(); }, 100);
});

topbarRepoMenuTerminal.addEventListener("click", (event) => {
    event.stopPropagation();
    if (currentRepoPath) {
        sendIpcMessage("REPO_TERMINAL", {
            "repoPath": currentRepoPath
        });
    }
    closeDropdowns();
});

topbarRepoMenuExplorer.addEventListener("click", (event) => {
    event.stopPropagation();
    if (currentRepoPath) {
        sendIpcMessage("EXPLORER_OPEN", {
            "path": currentRepoPath
        });
    }
    closeDropdowns();
});

topbarRepoMenuRemove.addEventListener("click", (event) => {
    event.stopPropagation();
    const pathToRepo = currentRepoPath;
    if (pathToRepo) {
        repoRemoveModal.dataset.targetPath = currentRepoPath;
        repoRemoveModalName.textContent = document.querySelector('#repositories-container .btn-value').textContent;
        repoRemoveModalLocalCheckbox.checked = false;
        repoRemoveModal.classList.add("show");
    }
    closeDropdowns();
});

topbarBranchMenuNew.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();
    branchNewModal.classList.add("show");
    validateBranchNewModal();
    setTimeout(() => { branchNewModalInputName.focus(); }, 100);
});

topbarBranchMenuCopy.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();
    if (currentBranch) {
        navigator.clipboard.writeText(currentBranch).then(() => {
            console.log(`Copied '${currentBranch}' to clipboard.`);
        }).catch(err => {
            console.error("Failed to copy text: ", err);
        });
    }
});

topbarBranchMenuMerge.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();
    if (!currentRepoPath || !currentBranch) { return; }

    setupMergeModal(currentBranch);
    branchMergeModal.dataset.targetName = currentBranch;
    branchMergeModal.classList.add("show");
});

topbarBranchMenuRename.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();

    if (!currentRepoPath || !currentBranch) { return; }

    branchRenameModal.dataset.targetName = currentBranch;
    branchRenameModalInputName.value = currentBranch;

    branchRenameModal.classList.add("show");
    validateBranchRenameModal();

    setTimeout(() => {
        branchRenameModalInputName.focus();
        branchRenameModalInputName.select();
    }, 100);
});

topbarBranchMenuDelete.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();

    if (!currentRepoPath || !currentBranch) { return; }

    branchDeleteModal.dataset.targetName = currentBranch;
    document.getElementById("delete-modal-branch-name").textContent = currentBranch;

    branchDeleteModal.classList.add("show");
});

branchItemMenuRename.addEventListener("click", (event) => {
    event.stopPropagation();
    const branchName = branchItemContextMenu.dataset.targetName;

    if (branchName) {
        branchRenameModal.dataset.targetName = branchName;
        branchRenameModalInputName.value = branchName;

        branchRenameModal.classList.add("show");
        validateBranchRenameModal();

        setTimeout(() => {
            branchRenameModalInputName.focus();
            branchRenameModalInputName.select();
        }, 100);
    }

    branchItemContextMenu.classList.remove("show");
    closeDropdowns();
});

branchItemMenuCopy.addEventListener("click", (event) => {
    event.stopPropagation();
    const branchName = branchItemContextMenu.dataset.targetName;

    if (branchName) {
        navigator.clipboard.writeText(branchName).then(() => {
            console.log(`Copied '${branchName}' to clipboard.`);
        }).catch(err => {
            console.error("Failed to copy text: ", err);
        });
    }

    branchItemContextMenu.classList.remove("show");
    closeDropdowns();
});

branchItemMenuMerge.addEventListener("click", (event) => {
    event.stopPropagation();
    const branchName = branchItemContextMenu.dataset.targetName;

    if (branchName) {
        setupMergeModal(branchName);
        branchMergeModal.dataset.targetName = branchName;
        branchMergeModal.classList.add("show");
    }

    branchItemContextMenu.classList.remove("show");
    closeDropdowns();
});

branchItemMenuDelete.addEventListener("click", (event) => {
    event.stopPropagation();
    const branchName = branchItemContextMenu.dataset.targetName;

    if (branchName) {
        branchDeleteModal.dataset.targetName = branchName;

        document.getElementById("delete-modal-branch-name").textContent = branchName;
        branchDeleteModal.classList.add("show");
    }

    branchItemContextMenu.classList.remove("show");
    closeDropdowns();
});

repoMenuClone.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();

    repoCloneModal.classList.add("show");
    // validateRepoCloneModal();
    // setTimeout(() => { ...; }, 100);
});

repoMenuCreate.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();

    repoCreateModal.classList.add("show");
    validateRepoCreateModal();
    setTimeout(() => { repoCreateModalInputName.focus(); }, 100);
});

repoMenuAdd.addEventListener("click", (event) => {
    event.stopPropagation();
    closeDropdowns();

    repoAddModal.classList.add("show");
    validateRepoAddModal();
    setTimeout(() => { repoAddModalInputPath.focus(); }, 100);
});

repoItemMenuTerminal.addEventListener("click", (event) => {
    event.stopPropagation();

    const pathToRepo = repoItemContextMenu.dataset.targetPath;
    if (pathToRepo) {
        sendIpcMessage("REPO_TERMINAL", {
            "repoPath": pathToRepo
        });
    }
    closeDropdowns();
});

repoItemMenuExplorer.addEventListener("click", (event) => {
    event.stopPropagation();

    const pathToRepo = repoItemContextMenu.dataset.targetPath;
    if (pathToRepo) {
        sendIpcMessage("EXPLORER_OPEN", {
            "path": pathToRepo
        });
    }
    closeDropdowns();
});

repoItemMenuRemove.addEventListener("click", (event) => {
    event.stopPropagation();

    const pathToRepo = repoItemContextMenu.dataset.targetPath;
    const repoName = repoItemContextMenu.dataset.targetName;

    if (pathToRepo) {
        repoRemoveModal.dataset.targetPath = pathToRepo;

        repoRemoveModalName.textContent = repoName;
        repoRemoveModalLocalCheckbox.checked = false;

        repoRemoveModal.classList.add("show");
    }
    closeDropdowns();
});

changesItemMenuDiscard.addEventListener("click", (event) => {
    event.stopPropagation();
    const filePath = changesItemContextMenu.dataset.targetPath;
    if (filePath) {
        sendIpcMessage("CHANGE_DISCARD", {
            repoPath: currentRepoPath,
            filePath: filePath
        });
    }
    closeDropdowns();
});

changesItemMenuIgnoreFile.addEventListener("click", (event) => {
    event.stopPropagation();
    const filePath = changesItemContextMenu.dataset.targetPath;
    if (filePath) {
        sendIpcMessage("CHANGE_IGNORE", {
            repoPath: currentRepoPath,
            filePath: filePath,
            ignoreType: "file"
        });
    }
    closeDropdowns();
});

changesItemMenuCopyAbsPath.addEventListener("click", (event) => {
    event.stopPropagation();
    const filePath = changesItemContextMenu.dataset.targetPath;
    if (filePath) {
        let absPath = `${currentRepoPath}/${filePath}`;

        const isWindows = currentRepoPath.includes('\\');

        if (isWindows) {
            absPath = absPath.replace(/\//g, '\\').replace(/\\\\/g, '\\');
        } else {
            absPath = absPath.replace(/\\/g, '/').replace(/\/\//g, '/');
        }

        navigator.clipboard.writeText(absPath).then(() => console.log(`Copied '${absPath}'`));
    }
    closeDropdowns();
});

changesItemMenuCopyRelPath.addEventListener("click", (event) => {
    event.stopPropagation();
    const filePath = changesItemContextMenu.dataset.targetPath;
    if (filePath) {
        navigator.clipboard.writeText(filePath).then(() => console.log(`Copied '${filePath}'`));
    }
    closeDropdowns();
});

changesItemMenuExplorer.addEventListener("click", (event) => {
    event.stopPropagation();
    const filePath = changesItemContextMenu.dataset.targetPath;
    if (filePath) {
        let rawPath = `${currentRepoPath}/${filePath}`;
        let lastSlashIndex = rawPath.lastIndexOf('/');
        let dirPath = rawPath.substring(0, lastSlashIndex);

        sendIpcMessage("EXPLORER_OPEN", {
            path: dirPath
        });
    }
    closeDropdowns();
});

historyItemMenuCheckout.addEventListener("click", (event) => {
    event.stopPropagation();
    const hash = historyItemContextMenu.dataset.targetHash;
    if (hash) {
        sendIpcMessage("HISTORY_CHECKOUT", {
            repoPath: currentRepoPath,
            commitHash: hash
        });
    }
    closeDropdowns();
});

historyItemMenuRevert.addEventListener("click", (event) => {
    event.stopPropagation();
    const hash = historyItemContextMenu.dataset.targetHash;
    if (hash) {
        sendIpcMessage("HISTORY_REVERT", {
            repoPath: currentRepoPath,
            commitHash: hash
        });
    }
    closeDropdowns();
});

historyItemMenuCreateBranch.addEventListener("click", (event) => { // FINISH
    event.stopPropagation();
    const hash = historyItemContextMenu.dataset.targetHash;
    if (hash) {
        sendIpcMessage("HISTORY_BRANCH_CREATE", { // CHANGE: CONNECT TO MODAL THEN TO BACKEND
            repoPath: currentRepoPath,
            commitHash: hash
        });
    }
    closeDropdowns();
});

historyItemMenuCopySHA.addEventListener("click", (event) => {
    event.stopPropagation();
    const hash = historyItemContextMenu.dataset.targetHash;
    if (hash) { navigator.clipboard.writeText(hash).then(() => console.log(`Copied '${hash}'`));  }
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
            document.querySelectorAll('.context-active').forEach(el => el.classList.remove('context-active'));
        }
    }

    if (branchItemContextMenu.classList.contains("show")) {
        if (!branchItemContextMenu.contains(event.target)) {
            branchItemContextMenu.classList.remove("show");
            document.querySelectorAll('.context-active').forEach(el => el.classList.remove('context-active'));
        }
    }

    if (topbarRepoContextMenu.classList.contains("show")) {
        if (!topbarRepoContextMenu.contains(event.target)) {
            topbarRepoContextMenu.classList.remove("show");
            document.querySelectorAll('.context-active').forEach(el => el.classList.remove('context-active'));
        }
    }

    if (topbarBranchContextMenu.classList.contains("show")) {
        if (!topbarBranchContextMenu.contains(event.target)) {
            topbarBranchContextMenu.classList.remove("show");
            document.querySelectorAll('.context-active').forEach(el => el.classList.remove('context-active'));
        }
    }

    if (repoCreateModalLicenseEditPanel.classList.contains("open")) {
        if (!repoCreateModalLicenseEditPanel.contains(event.target) && !repoCreateModalLicenseEdit.contains(event.target)) {
            repoCreateModalLicenseEditPanel.classList.remove("open");
            repoCreateModalLicenseEdit.classList.remove("active");
        }
    }
}, true);

// Confirm buttons (modals)
branchNewModalConfirmBtn.addEventListener("click", () => {
    const newBranchName = branchNewModalInputName.value.trim();

    if (newBranchName === "") { return; }

    sendIpcMessage("BRANCH_CREATE", {
        repoPath: currentRepoPath,
        branchName: newBranchName
    });

    closeAndClearModal(branchNewModal);
});

branchRenameModalConfirmBtn.addEventListener("click", () => {
    const newBranchName = branchRenameModalInputName.value.trim();
    const oldBranchName = branchRenameModal.dataset.targetName;

    if (newBranchName === "" || !oldBranchName) { return; }

    sendIpcMessage("BRANCH_RENAME", {
        repoPath: currentRepoPath,
        oldName: oldBranchName,
        newName: newBranchName
    });

    closeAndClearModal(branchRenameModal);
});

branchDeleteModalConfirmBtn.addEventListener("click", () => {
    const branchName = branchDeleteModal.dataset.targetName;

    if (!branchName) { return; }

    sendIpcMessage("BRANCH_DELETE", {
        repoPath: currentRepoPath,
        branchName: branchName
    });

    closeAndClearModal(branchDeleteModal);
});

branchMergeModalConfirmBtn.addEventListener("click", () => {
    const sourceBranch = branchMergeModalSelectSource.value;
    const targetBranch = branchMergeModalSelectTarget.value;

    if (!sourceBranch || !targetBranch || !currentRepoPath || sourceBranch === targetBranch) { return; }

    sendIpcMessage("BRANCH_MERGE", {
        repoPath: currentRepoPath,
        sourceBranch: sourceBranch,
        targetBranch: targetBranch
    });

    closeAndClearModal(branchMergeModal);
});

repoCloneModalConfirmBtn.addEventListener("click", () => {
    closeAndClearModal(repoCloneModal);
});

repoCreateModalConfirmBtn.addEventListener("click", () => {
    const repoName = repoCreateModalInputName.value.trim();
    const localPath = repoCreateModalInputPath.value.trim();
    const gitIgnore = repoCreateModalSelectGitIgnore.value.trim() ?? "None";
    const license = repoCreateModalSelectLicense.value.trim() ?? "None";
    const licenseYear = document.getElementById("license-field-year").value.trim();
    const licenseOrganization = document.getElementById("license-field-organization").value.trim();
    const licenseProject = document.getElementById("license-field-project").value.trim();

    sendIpcMessage("REPO_CREATE", {
        repoName: repoName,
        localPath: localPath,
        gitIgnore: gitIgnore,
        license: license,
        licenseYear: licenseYear,
        licenseOrganization: licenseOrganization,
        licenseProject: licenseProject
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

repoMissingModalConfirmBtn.addEventListener("click", () => {
    const stalePath = repoMissingModal.dataset.targetPath;

    closeAndClearModal(repoMissingModal);

    repoAddModalInputPath.value = stalePath || "";
    repoAddModal.classList.add("show");
});

repoMissingModalCancelBtn.addEventListener("click", () => {
    const stalePath = repoMissingModal.dataset.targetPath;

    if (stalePath) {
        sendIpcMessage("REPO_REMOVE", {
            "repoPath": stalePath,
            "deleteLocal": false
        });
    }

    closeAndClearModal(repoMissingModal);
})

todoModalConfirmBtn.addEventListener("click", () => {
    if (!currentRepoPath) { return; }

    activeTodos = JSON.parse(JSON.stringify(draftTodos));

    sendIpcMessage("TODO_SAVE", {
        repoPath: currentRepoPath,
        todos: activeTodos
    });

    closeAndClearModal(todoModal);
});

configModalConfirmBtn.addEventListener("click", () => {
    if (!currentRepoPath) { return; }

    sendIpcMessage("CONFIG_LOCAL_SAVE", {
        repoPath: currentRepoPath,
        name: configModalName.value.trim(),
        email: configModalEmail.value.trim()
    });

    closeAndClearModal(configModal);
});

accountModalConfirmBtn.addEventListener("click", () => {
    sendIpcMessage("CONFIG_GLOBAL_SAVE", {
        name: accountModalInputName.value.trim(),
        email: accountModalInputEmail.value.trim()
    });

    closeAndClearModal(accountModal);
});

// Input boxes (modals)
branchNewModalInputName.addEventListener("keyup", (event) => {
    if (event.key === "Enter") { branchNewModalConfirmBtn.click(); }
});
branchRenameModalInputName.addEventListener("keyup", (event) => {
    if (event.key === "Enter") { branchRenameModalConfirmBtn.click(); }
});
repoCreateModalInputName.addEventListener("keyup", (event) => {
    if (event.key === "Enter") { repoCreateModalConfirmBtn.click(); }
});
repoCreateModalInputPath.addEventListener("keyup", (event) => {
    if (event.key === "Enter") { repoCreateModalConfirmBtn.click(); }
});
repoAddModalInputPath.addEventListener("keyup", (event) => {
    if (event.key === "Enter") { repoAddModalConfirmBtn.click(); }
});

// Confirm toggle (modals)
branchNewModalInputName.addEventListener("input", validateBranchNewModal);
branchRenameModalInputName.addEventListener("input", validateBranchRenameModal);
repoCreateModalInputName.addEventListener("input", validateRepoCreateModal);
repoCreateModalInputPath.addEventListener("input", validateRepoCreateModal);
repoAddModalInputPath.addEventListener("input", validateRepoAddModal);

// Browse buttons (modals)
repoCreateModalBrowseBtn.addEventListener("click", (event) => {
    event.preventDefault();

    activeBrowseInput = repoCreateModalInputPath;

    sendIpcMessage("EXPLORER_OPEN_DIALOG");
});

repoAddModalBrowseBtn.addEventListener("click", (event) => {
    event.preventDefault();

    activeBrowseInput = repoAddModalInputPath;

    sendIpcMessage("EXPLORER_OPEN_DIALOG");
});

// Repo create modal edit btn (modals/repo-create-modal)
repoCreateModalLicenseEdit.addEventListener("click", (event) => {
    event.stopPropagation();
    repoCreateModalLicenseEditPanel.classList.toggle("open");
    repoCreateModalLicenseEdit.classList.toggle("active");
});

repoCreateModalSelectLicense.addEventListener("change", (event) => {
    const isNone = event.target.value === "None";

    repoCreateModalLicenseEdit.disabled = isNone;
    repoCreateModalLicenseEdit.classList.toggle("disabled", isNone);

    if (isNone) {
        repoCreateModalLicenseEditPanel.classList.remove("open");
        repoCreateModalLicenseEdit.classList.remove("active");
    }
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

        updateCustomScrollbar(repoList, repoScrollbar);
        updateCustomScrollbar(branchList, branchScrollbar);
    });
};

const setupSelection = (panelSelector, btnValueSelector) => {
    const panel = document.querySelector(panelSelector);
    const valueDisplay = document.querySelector(btnValueSelector);

    panel.addEventListener('click', (event) => {
        const item = event.target.closest('.dropdown-item');

        if (item && !item.classList.contains('invalid')) {
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

interactCustomScrollbar(repoList, repoScrollbar);
interactCustomScrollbar(branchList, branchScrollbar);
interactCustomScrollbar(diffBodyWrapper, diffScrollbar);
interactCustomScrollbar(detailsFileList, detailsFileScrollbar);
interactCustomScrollbar(detailsBodyWrapper, detailsScrollbar);
interactCustomScrollbar(changesList, changesScrollbar);
interactCustomScrollbar(historyList, historyScrollbar);
interactCustomScrollbar(todoModalRowsContainer, todoScrollbar);

fileBtn.classList.add("disabled"); // FINISH
mergeBtn.classList.add("disabled");
branchesBtn.classList.add("disabled"); // FINISH
analyticsBtn.classList.add("disabled"); // FINISH
todoBtn.classList.add("disabled");
configBtn.classList.add("disabled");
fetchBtn.classList.add("disabled");

branchBtn.classList.add("disabled");
branchBtn.disabled = true;

// FINISH
repoMenuClone.classList.add("disabled");
repoMenuClone.disabled = true;
topbarRepoMenuClone.classList.add("disabled");
topbarRepoMenuClone.disabled = true;

// FINISH
historyItemMenuCreateBranch.classList.add("disabled");
historyItemMenuCreateBranch.disabled = true;

switchToChangesTab();
toggleCommitButton();
resetDetailsViewer();

window.addEventListener('DOMContentLoaded', () => {
    sendIpcMessage("APP_READY");
});