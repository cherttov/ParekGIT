// ======================== DOM ELEMENTS ========================
// Topbar repo
const repoContainer = document.getElementById("repositories-container");
const repoDropdown = document.getElementById("repository-dropdown-panel");
const repoNewBtn = document.querySelector("#repositories-container .btn-add");
const repoBtn = document.querySelector("#repositories-container .topbar-btn");
const repoPanel = document.querySelector("#repositories-container .dropdown-panel");
const repoList = document.getElementById("repo-topbar-list");
const repoScrollbar = document.getElementById("repo-topbar-scrollbar");

// Topbar branch
const branchContainer = document.getElementById("branches-container");
const branchDropdown = document.getElementById("branch-dropdown-panel");
const branchNewBtn = document.querySelector("#branches-container .btn-new");
const branchBtn = document.querySelector("#branches-container .topbar-btn");
const branchPanel = document.querySelector("#branches-container .dropdown-panel");
const branchList = document.getElementById("branch-topbar-list");
const branchScrollbar = document.getElementById("branch-topbar-scrollbar");

// Backdrop for dropdowns (darkened background)
const dropdownBackdrop = document.getElementById("dropdown-backdrop");

// LeftSidebar
const leftSidebar = document.getElementById("left-sidebar");

// LeftSidebar buttons
const fileBtn = leftSidebar.querySelector(".btn-file");
const mergeBtn = leftSidebar.querySelector(".btn-merge");
const branchesBtn = leftSidebar.querySelector(".btn-branches");
const analyticsBtn = leftSidebar.querySelector(".btn-analytics");
const todoBtn = leftSidebar.querySelector(".btn-todo");
const configBtn = leftSidebar.querySelector(".btn-config");
const fetchBtn = leftSidebar.querySelector(".btn-fetch");
const settingsBtn = leftSidebar.querySelector(".btn-settings");
const accountBtn = leftSidebar.querySelector(".btn-account");

// MainContent
const mainContent = document.getElementById("main-content");

// MainContent file diff viewer
const diffViewer = document.getElementById("diff-viewer");
const diffFilename = document.getElementById("diff-filename");
const diffContent = document.getElementById("diff-content");
const diffBodyWrapper = document.getElementById("diff-body-wrapper");
const diffScrollbar = document.getElementById("diff-scrollbar");

// MainContent commit details viewer
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

// RightSidebar + SplitContainer
const rightSidebar = document.getElementById("right-sidebar");
const resizer = document.getElementById("resizer");

// RightSidebar header
const tabBtnChanges = document.getElementById("tab-btn-changes");
const tabBtnHistory = document.getElementById("tab-btn-history");
const tabChanges = document.getElementById("changes-tab");
const tabHistory = document.getElementById("history-tab");

// RightSidebar changes tab
const changesHeader = document.getElementById("changes-header");
const changesCountText = document.getElementById("changes-count-text");
const changesMasterCheckbox = document.getElementById("changes-master-checkbox");
const changesList = document.getElementById("changes-list");
const changesScrollbar = document.getElementById("changes-scrollbar");
const commitMessageInput = document.getElementById("commit-name-input");
const commitDescriptionInput = document.getElementById("commit-desc-input");
const commitBtn = document.getElementById("commit-btn");

// RightSidebar history tab
const historyList = document.getElementById("history-list");
const historyScrollbar = document.getElementById("history-scrollbar");

// Backdrop for modals (darkened background)
const modalBackdrops = document.querySelectorAll(".modal-backdrop");

// Uniform Modal Close Triggers (CancelButton and 'X' symbol top-right)
const modalCloseTriggers = document.querySelectorAll(".close-modal-icon, .cancel-modal-btn");

// New Branch Modal
const branchNewModal = document.getElementById("branch-new-modal");
const branchNewModalInputName = branchNewModal.querySelector(".modal-input");
const branchNewModalConfirmBtn = branchNewModal.querySelector(".confirm-modal-btn");

// New Branch from Commit Modal
const branchHistoryNewModal = document.getElementById("branch-history-new-modal");
const branchHistoryNewModalName = document.getElementById("branch-history-modal-commit-name");
const branchHistoryNewModalInputName = branchHistoryNewModal.querySelector(".modal-input");
const branchHistoryNewModalConfirmBtn = branchHistoryNewModal.querySelector(".confirm-modal-btn");

// Rename Branch Modal
const branchRenameModal = document.getElementById("branch-rename-modal");
const branchRenameModalInputName = branchRenameModal.querySelector(".modal-input");
const branchRenameModalConfirmBtn = branchRenameModal.querySelector(".confirm-modal-btn");

// Delete Branch Modal
const branchDeleteModal = document.getElementById("branch-delete-modal");
const branchDeleteModalName = document.getElementById("delete-modal-branch-name");
const branchDeleteModalConfirmBtn = branchDeleteModal.querySelector(".confirm-modal-btn");

// Merge Branch Modal
const branchMergeModal = document.getElementById("branch-merge-modal");
const branchMergeModalSourceLabel = document.getElementById("merge-modal-source-branch");
const branchMergeModalTargetLabel = document.getElementById("merge-modal-target-branch");
const branchMergeModalSelectSource = document.getElementById("merge-modal-select-source");
const branchMergeModalSelectTarget = document.getElementById("merge-modal-select-target");
const branchMergeModalConfirmBtn = branchMergeModal.querySelector(".confirm-modal-btn");

// Clone Repo Modal
const repoCloneModal = document.getElementById("repo-clone-modal");
const repoCloneModalRepoUrl = repoCloneModal.querySelector(".input-url");
const repoCloneModalAsLocal = repoCloneModal.querySelector(".ui-checkbox");
const repoCloneModalInputPath = repoCloneModal.querySelector(".input-path");
const repoCloneModalBrowseBtn = repoCloneModal.querySelector(".browse-btn");
const repoCloneModalConfirmBtn = repoCloneModal.querySelector(".confirm-modal-btn");

// Create Repo Modal
const repoCreateModal = document.getElementById("repo-create-modal");
const repoCreateModalInputName = repoCreateModal.querySelector(".input-name");
const repoCreateModalInputPath = repoCreateModal.querySelector(".input-path");
const repoCreateModalBrowseBtn = repoCreateModal.querySelector(".browse-btn");
const repoCreateModalSelectGitIgnore = repoCreateModal.querySelector("#select-git-ignore");
const repoCreateModalSelectLicense = repoCreateModal.querySelector("#select-git-license");
const repoCreateModalLicenseEdit = repoCreateModal.querySelector("#license-edit-btn");
const repoCreateModalLicenseEditPanel = repoCreateModal.querySelector("#license-edit-panel");
const repoCreateModalConfirmBtn = repoCreateModal.querySelector(".confirm-modal-btn");

// Add Repo Modal
const repoAddModal = document.getElementById("repo-add-modal");
const repoAddModalInputPath = repoAddModal.querySelector(".modal-input");
const repoAddModalBrowseBtn = repoAddModal.querySelector(".browse-btn");
const repoAddModalConfirmBtn = repoAddModal.querySelector(".confirm-modal-btn");

// Remove/Delete Repo Modal
const repoRemoveModal = document.getElementById("repo-remove-modal");
const repoRemoveModalName = document.getElementById("remove-modal-repo-name");
const repoRemoveModalLocalCheckbox = repoRemoveModal.querySelector(".ui-checkbox");
const repoRemoveModalConfirmBtn = repoRemoveModal.querySelector(".confirm-modal-btn");

// Settings Modal
const settingsModal = document.getElementById("settings-modal");
const settingsModalThemeSelect = document.getElementById("settings-theme-select");
const settingsModalLogsView = document.getElementById("settings-logs-view");
const settingsModalLogsClear = document.getElementById("settings-logs-clear");
const settingsModalConfirmBtn = settingsModal.querySelector(".confirm-modal-btn");

// Account Modal
const accountModal = document.getElementById("account-modal");
const accountModalInputName = accountModal.querySelector(".input-name");
const accountModalInputEmail = accountModal.querySelector(".input-email");
const accountModalConfirmBtn = accountModal.querySelector(".confirm-modal-btn");

// Todo Modal
const todoModal = document.getElementById("todo-modal");
const todoModalRowsContainer = document.getElementById("todo-rows-container");
const todoScrollbar = document.getElementById("todo-scrollbar");
const todoModalConfirmBtn = todoModal.querySelector(".confirm-modal-btn");

// Config Modal
const configModal = document.getElementById("config-modal");
const configModalName = configModal.querySelector(".input-name");
const configModalEmail = configModal.querySelector(".input-email");
const configModalConfirmBtn = configModal.querySelector(".confirm-modal-btn");

// Uniform Error Modal
const errorModal = document.getElementById("error-modal");
const errorModalMessage = document.getElementById("error-modal-message");

// Repo Missing Error Modal
const repoMissingModal = document.getElementById("repo-missing-modal");
const repoMissingModalConfirmBtn = repoMissingModal.querySelector(".confirm-modal-btn");
const repoMissingModalCancelBtn = repoMissingModal.querySelector(".cancel-modal-btn");

// Dropdown 'ADD' Button ContextMenu
const repoContextMenu = document.getElementById("repo-context-menu");
const repoMenuClone = repoContextMenu.querySelector(".context-menu-item.item-clone");
const repoMenuCreate = repoContextMenu.querySelector(".context-menu-item.item-create");
const repoMenuAdd = repoContextMenu.querySelector(".context-menu-item.item-add");

// Repo Dropdown ContextMenu
const repoItemContextMenu = document.getElementById("repo-item-context-menu");
const repoItemMenuTerminal = repoItemContextMenu.querySelector(".context-menu-item.item-terminal");
const repoItemMenuExplorer = repoItemContextMenu.querySelector(".context-menu-item.item-explorer");
const repoItemMenuRemove = repoItemContextMenu.querySelector(".context-menu-item.item-remove");

// Branch Dropdown ContextMenu
const branchItemContextMenu = document.getElementById("branch-item-context-menu");
const branchItemMenuRename = branchItemContextMenu.querySelector(".context-menu-item.item-rename");
const branchItemMenuCopy = branchItemContextMenu.querySelector(".context-menu-item.item-copy");
const branchItemMenuMerge = branchItemContextMenu.querySelector(".context-menu-item.item-merge");
const branchItemMenuDelete = branchItemContextMenu.querySelector(".context-menu-item.item-delete");

// Current Repo Topbar ContextMenu
const topbarRepoContextMenu = document.getElementById("topbar-repo-context-menu");
const topbarRepoMenuClone = topbarRepoContextMenu.querySelector(".context-menu-item.item-clone");
const topbarRepoMenuCreate = topbarRepoContextMenu.querySelector(".context-menu-item.item-create");
const topbarRepoMenuAdd = topbarRepoContextMenu.querySelector(".context-menu-item.item-add");
const topbarRepoMenuTerminal = topbarRepoContextMenu.querySelector(".context-menu-item.item-terminal");
const topbarRepoMenuExplorer = topbarRepoContextMenu.querySelector(".context-menu-item.item-explorer");
const topbarRepoMenuRemove = topbarRepoContextMenu.querySelector(".context-menu-item.item-remove");

// Current Branch Topbar ContextMenu
const topbarBranchContextMenu = document.getElementById("topbar-branch-context-menu");
const topbarBranchMenuNew = topbarBranchContextMenu.querySelector(".context-menu-item.item-new");
const topbarBranchMenuRename = topbarBranchContextMenu.querySelector(".context-menu-item.item-rename");
const topbarBranchMenuCopy = topbarBranchContextMenu.querySelector(".context-menu-item.item-copy");
const topbarBranchMenuMerge = topbarBranchContextMenu.querySelector(".context-menu-item.item-merge");
const topbarBranchMenuDelete = topbarBranchContextMenu.querySelector(".context-menu-item.item-delete");

// Change Item LeftSidebar ChangesTab ContextMenu
const changesItemContextMenu = document.getElementById("changes-item-context-menu");
const changesItemMenuDiscard = changesItemContextMenu.querySelector(".context-menu-item.item-discard");
const changesItemMenuIgnoreFile = changesItemContextMenu.querySelector(".context-menu-item.item-ignore-file");
const changesItemMenuCopyAbsPath = changesItemContextMenu.querySelector(".context-menu-item.item-copy-abs-path");
const changesItemMenuCopyRelPath = changesItemContextMenu.querySelector(".context-menu-item.item-copy-rel-path");
const changesItemMenuExplorer = changesItemContextMenu.querySelector(".context-menu-item.item-explorer");

// Commit Item LeftSidebar HistoryTab ContextMenu
const historyItemContextMenu = document.getElementById("history-item-context-menu");
const historyItemMenuCheckout = historyItemContextMenu.querySelector(".context-menu-item.item-checkout");
const historyItemMenuRevert = historyItemContextMenu.querySelector(".context-menu-item.item-revert");
const historyItemMenuCreateBranch = historyItemContextMenu.querySelector(".context-menu-item.item-create-branch");
const historyItemMenuCopySHA = historyItemContextMenu.querySelector(".context-menu-item.item-copy-sha");

// ContextMenu list, ProtectedBranches list, Known BinaryExtensions list
const contextMenus = [repoContextMenu, repoItemContextMenu, branchItemContextMenu, topbarRepoContextMenu, topbarBranchContextMenu];
const protectedBranches = ["main", "master"];
const binaryExts = [
	".exe", ".dll", ".bin", ".obj",
	".pdb", ".png", ".jpg", ".jpeg",
	".gif", ".ico", ".zip", ".tar",
	".gz", ".7z", ".pdf",
];

// ======================== APP STATE ========================
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
let isPullRequired = false;
let commitsBehind = 0;

let minRightWidth = 0, maxRightWidth = 0;
let isResizing = false;

// ======================== IPC ACTIONS ========================
const IpcActions = {
	// Outgoing (frontend -> C#)
	APP_READY: "APP_READY",
	REPO_SELECTED: "REPO_SELECTED",
	GET_REPO_STATUS: "GET_REPO_STATUS",
	GET_BRANCH_HISTORY: "GET_BRANCH_HISTORY",
	GET_BRANCHES: "GET_BRANCHES",
	GET_FILE_DIFF: "GET_FILE_DIFF",
	GET_HISTORY_FILE_DIFF: "GET_HISTORY_FILE_DIFF",
	GET_COMMIT_DETAILS: "GET_COMMIT_DETAILS",
	BRANCH_SELECTED: "BRANCH_SELECTED",
	BRANCH_CREATE: "BRANCH_CREATE",
	BRANCH_HISTORY_CREATE: "BRANCH_HISTORY_CREATE",
	BRANCH_RENAME: "BRANCH_RENAME",
	BRANCH_DELETE: "BRANCH_DELETE",
	BRANCH_MERGE: "BRANCH_MERGE",
	REPO_FETCH: "REPO_FETCH",
	REPO_PULL: "REPO_PULL",
	REPO_COMMIT: "REPO_COMMIT",
	REPO_CLONE: "REPO_CLONE",
	REPO_CREATE: "REPO_CREATE",
	REPO_ADD: "REPO_ADD",
	REPO_REMOVE: "REPO_REMOVE",
	REPO_TERMINAL: "REPO_TERMINAL",
	CHANGE_DISCARD: "CHANGE_DISCARD",
	CHANGE_IGNORE: "CHANGE_IGNORE",
	HISTORY_CHECKOUT: "HISTORY_CHECKOUT",
	HISTORY_REVERT: "HISTORY_REVERT",
	TODO_LOAD: "TODO_LOAD",
	TODO_SAVE: "TODO_SAVE",
	CONFIG_LOCAL_GET: "CONFIG_LOCAL_GET",
	CONFIG_LOCAL_SAVE: "CONFIG_LOCAL_SAVE",
	CONFIG_GLOBAL_GET: "CONFIG_GLOBAL_GET",
	CONFIG_GLOBAL_SAVE: "CONFIG_GLOBAL_SAVE",
	SETTINGS_SAVE: "SETTINGS_SAVE",
	EXPLORER_OPEN: "EXPLORER_OPEN",
	EXPLORER_OPEN_DIALOG: "EXPLORER_OPEN_DIALOG",
	LOGS_CLEAR: "LOGS_CLEAR",

	// Incoming (C# -> frontend)
	APP_INITIALIZED: "APP_INITIALIZED",
	APP_ERROR: "APP_ERROR",
	FOLDER_SELECTED: "FOLDER_SELECTED",
	REPO_CLONED: "REPO_CLONED",
	REPO_CREATED: "REPO_CREATED",
	REPO_ADDED: "REPO_ADDED",
	REPO_REMOVED: "REPO_REMOVED",
	REPO_COMMITTED: "REPO_COMMITTED",
	REPO_FETCHED: "REPO_FETCHED",
	REPO_PULLED: "REPO_PULLED",
	REPO_STATUS_LOADED: "REPO_STATUS_LOADED",
	REPO_FILES_CHANGED: "REPO_FILES_CHANGED",
	REPO_PATH_MISSING: "REPO_PATH_MISSING",
	REMOTE_SYNC_STATUS: "REMOTE_SYNC_STATUS",
	BRANCHES_LOADED: "BRANCHES_LOADED",
	BRANCH_DELETED: "BRANCH_DELETED",
	BRANCH_RENAMED: "BRANCH_RENAMED",
	BRANCH_MERGED: "BRANCH_MERGED",
	BRANCH_HISTORY_LOADED: "BRANCH_HISTORY_LOADED",
	BRANCH_HISTORY_APPENDED: "BRANCH_HISTORY_APPENDED",
	FILE_DIFF_LOADED: "FILE_DIFF_LOADED",
	HISTORY_FILE_DIFF_LOADED: "HISTORY_FILE_DIFF_LOADED",
	COMMIT_DETAILS_LOADED: "COMMIT_DETAILS_LOADED",
	CHANGE_DISCARDED: "CHANGE_DISCARDED",
	CHANGE_IGNORED: "CHANGE_IGNORED",
	HISTORY_CHECKED_OUT: "HISTORY_CHECKED_OUT",
	HISTORY_REVERT_RESULT: "HISTORY_REVERT_RESULT",
	TODO_LOADED: "TODO_LOADED",
	TODO_SAVED: "TODO_SAVED",
	CONFIG_LOCAL_LOADED: "CONFIG_LOCAL_LOADED",
	CONFIG_LOCAL_SAVED: "CONFIG_LOCAL_SAVED",
	CONFIG_GLOBAL_LOADED: "CONFIG_GLOBAL_LOADED",
	CONFIG_GLOBAL_SAVED: "CONFIG_GLOBAL_SAVED",
	SETTINGS_SAVED: "SETTINGS_SAVED",
};

// ======================== IPC COMMUNICATION ========================
// Wraps a payload in the {Action, Payload} envelope and sends it to C# over the IPC Bridge
const sendIpcMessage = (action, payload = {}) => {
	const envelope = {
		Action: action,
		Payload: payload,
	};
	window.external.sendMessage(JSON.stringify(envelope));
};

// Dispatches every incoming C# message to its handler (based on the 'Action')
window.external.receiveMessage((message) => {
	const data = JSON.parse(message);

	switch (data.Action) {
		case IpcActions.APP_INITIALIZED:
			appInit(data.Payload);
			break;

		case IpcActions.APP_ERROR:
			showErrorModal(data.Payload.message);
			break;

		case IpcActions.FOLDER_SELECTED:
			folderSelected(data.Payload);
			break;

		case IpcActions.REPO_CLONED:
			addRepositoryToDropdown(data.Payload);
			break;

		case IpcActions.REPO_CREATED:
			addRepositoryToDropdown(data.Payload);
			break;

		case IpcActions.REPO_ADDED:
			addRepositoryToDropdown(data.Payload);
			break;

		case IpcActions.REPO_REMOVED:
			deleteRepoFromDropdown(data.Payload);
			break;

		case IpcActions.REPO_COMMITTED:
			processCommit();
			break;

		case IpcActions.REPO_FETCHED:
			fetchRepo(data.Payload);
			break;

		case IpcActions.REPO_PULLED: // MOVE TO DEDICATED METHOD
			isPullRequired = false;
			commitsBehind = 0;
			refreshRepoState();
			break;

		case IpcActions.REPO_STATUS_LOADED:
			renderChangedFiles(data.Payload);
			break;

		case IpcActions.REPO_FILES_CHANGED:
			processFileChanges(data.Payload);
			break;

		case IpcActions.REPO_PATH_MISSING:
			processMissingRepo(data.Payload.absolutePath);
			break;

		case IpcActions.REMOTE_SYNC_STATUS:
			commitsBehind = data.Payload.commitsBehind;
			isPullRequired = commitsBehind > 0;
			toggleCommitButton();
			break;

		case IpcActions.BRANCHES_LOADED:
			loadBranchesIntoDropdown(data.Payload);
			break;

		case IpcActions.BRANCH_DELETED:
			removeBranchFromDropdown(data.Payload);
			break;

		case IpcActions.BRANCH_RENAMED:
			renameBranchInDropdown(data.Payload);
			break;

		case IpcActions.BRANCH_MERGED:
			processBranchesMerged();
			break;

		case IpcActions.BRANCH_HISTORY_LOADED:
			processBranchHistoryLoaded(data.Payload);
			break;

		case IpcActions.BRANCH_HISTORY_APPENDED:
			processBranchHistoryAppended(data.Payload);
			break;

		case IpcActions.FILE_DIFF_LOADED:
			renderFileDiff(data.Payload.diffText, diffContent, diffBodyWrapper, diffScrollbar);
			break;

		case IpcActions.HISTORY_FILE_DIFF_LOADED:
			renderFileDiff(data.Payload.diffText, detailsContent, detailsBodyWrapper, detailsScrollbar);
			break;

		case IpcActions.COMMIT_DETAILS_LOADED:
			loadCommitDetails(data.Payload);
			break;

		case IpcActions.CHANGE_DISCARDED: // FINISH
			break;

		case IpcActions.CHANGE_IGNORED:
			processFileChanges(data.Payload); // REVISIT
			break;

		case IpcActions.HISTORY_CHECKED_OUT: // FINISH
			break;

		case IpcActions.HISTORY_REVERT_RESULT: // FINISH
			break;

		case IpcActions.TODO_LOADED:
			loadTodos(data.Payload.todos);
			break;

		case IpcActions.TODO_SAVED:
			break;

		case IpcActions.CONFIG_LOCAL_LOADED:
			processLocalConfigLoad(data.Payload);
			break;

		case IpcActions.CONFIG_LOCAL_SAVED:
			break;

		case IpcActions.CONFIG_GLOBAL_LOADED:
			processGlobalConfigLoad(data.Payload);
			break;

		case IpcActions.CONFIG_GLOBAL_SAVED:
			break;

		case IpcActions.SETTINGS_SAVED:
			applyTheme(data.Payload.Theme);
			break;

		default:
			console.warn("Unknown action received: ", data.Action);
	}
});

// ======================== GENERIC UI HELPERS ========================
// UI methods with no single owner.

// Updates width of Topbar dropdowns
const updatePanelWidths = () => {
	repoPanel.style.width = `${repoContainer.offsetWidth}px`;
	repoPanel.style.left = `${repoContainer.offsetLeft}px`;
	branchPanel.style.width = window.getComputedStyle(rightSidebar).width;
};

// Stops LeftSidebar fetchBtn animation
const stopFetchAnimation = () => {
	const icon = fetchBtn.querySelector(".icon-fetch");
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

// Places context menu to stay in viewport (based on position)
const placeContextMenu = (event, contextMenu) => {
	let mouseX = event.clientX;
	let mouseY = event.clientY;

	const menuWidth = contextMenu.offsetWidth;
	const menuHeight = contextMenu.offsetHeight;

	if (mouseX + menuWidth > window.innerWidth) { mouseX = mouseX - menuWidth; }
	if (mouseY + menuHeight > window.innerHeight) { mouseY = mouseY - menuHeight; }

	contextMenu.style.left = `${mouseX}px`;
	contextMenu.style.top = `${mouseY}px`;
};

// Closes all dropdowns
const closeDropdowns = () => {
	repoPanel.classList.remove("show");
	branchPanel.classList.remove("show");
	dropdownBackdrop.classList.remove("show");
	repoContextMenu.classList.remove("show");
	repoItemContextMenu.classList.remove("show");
	branchItemContextMenu.classList.remove("show");
	topbarRepoContextMenu.classList.remove("show");
	topbarBranchContextMenu.classList.remove("show");
	changesItemContextMenu.classList.remove("show");
	historyItemContextMenu.classList.remove("show");
	detailsPanel.classList.remove("show");

	document.querySelectorAll(".context-active").forEach((el) => el.classList.remove("context-active"));
};

// Closes and clears given modal
const closeAndClearModal = (modalElement) => {
	modalElement.classList.remove("show");

	// Clear all text inputs
	const inputs = modalElement.querySelectorAll("input");
	inputs.forEach((input) => (input.value = ""));

	// Reset all select inputs
	const selects = modalElement.querySelectorAll("select");
	selects.forEach((select) => {
		select.selectedIndex = 0;
		select.dispatchEvent(new Event("change"));
	});
};

// Toggles 1st dropdown to visible & 2nd to hidden
const toggleDropdown = (toShow, toHide, event) => {
	event.stopPropagation();
	updatePanelWidths();
	toHide.classList.remove("show");
	detailsPanel.classList.remove("show");

	document.querySelectorAll(".context-active").forEach((el) => el.classList.remove("context-active"));

	const isOpening = toShow.classList.toggle("show");
	dropdownBackdrop.classList.toggle("show", isOpening);

	if (isOpening) {
		setTimeout(() => {
			if (toShow === repoPanel) { updateCustomScrollbar(repoList, repoScrollbar); }
			if (toShow === branchPanel) { updateCustomScrollbar(branchList, branchScrollbar); }
		}, 10);
	}
};

// Updates CustomScrollbar height & range
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

	const usableTrackHeight = containerH - EDGE_PADDING * 2;

	// Style math
	const heightRatio = containerH / contentH;
	const thumbHeight = Math.max(heightRatio * usableTrackHeight, 30);

	const scrollPercentage = container.scrollTop / (contentH - containerH);
	const maxThumbTop = usableTrackHeight - thumbHeight;
	const thumbTop = EDGE_PADDING + scrollPercentage * maxThumbTop;

	// Apply style
	scrollbar.style.height = `${thumbHeight}px`;
	scrollbar.style.transform = `translateY(${thumbTop}px)`;
};

// Allows to scroll using the scrollbar
const interactCustomScrollbar = (container, scrollbar) => {
	let isDragging = false;
	let startY;
	let startScrollTop;

	scrollbar.addEventListener("mousedown", (e) => {
		if (e.button !== 0) { return; }

		isDragging = true;
		startY = e.clientY;
		startScrollTop = container.scrollTop;

		scrollbar.classList.add("focused");

		document.body.style.userSelect = "none";
		document.addEventListener("mousemove", onMouseMove);
		document.addEventListener("mouseup", onMouseUp);
	});

	const onMouseMove = (e) => {
		if (!isDragging) { return; }

		const deltaY = e.clientY - startY;

		const containerH = container.clientHeight;
		const contentH = container.scrollHeight;
		const EDGE_PADDING = 6;
		const usableTrackHeight = containerH - EDGE_PADDING * 2;

		const heightRatio = containerH / contentH;
		const thumbHeight = Math.max(heightRatio * usableTrackHeight, 30);
		const maxThumbTop = usableTrackHeight - thumbHeight;

		if (maxThumbTop <= 0) { return; }

		const scrollRatio = (contentH - containerH) / maxThumbTop;
		container.scrollTop = startScrollTop + deltaY * scrollRatio;
	};

	const onMouseUp = () => {
		isDragging = false;
		scrollbar.classList.remove("focused");
		document.body.style.userSelect = "";
		document.removeEventListener("mousemove", onMouseMove);
		document.removeEventListener("mouseup", onMouseUp);
	};
};

// Swaps the active `theme-*` class in <body> to the given theme name (config)
const applyTheme = (themeName) => {
	document.body.className = document.body.className.replace(/theme-\S+/g, "");
	if (themeName) { document.body.classList.add(`theme-${themeName}`); }
};

// Escapes HTML-significant characters for safe injection via innerHTML
const escapeHtml = (unsafeText) => {
	return unsafeText
		.replace(/&/g, "&amp;")
		.replace(/</g, "&lt;")
		.replace(/>/g, "&gt;")
		.replace(/"/g, "&quot;")
		.replace(/'/g, "&#039");
};

// Switches from HistoryTab to ChangesTab in the RightSidebar
const switchToChangesTab = () => {
	tabBtnChanges.classList.add("active");
	tabBtnHistory.classList.remove("active");
	tabChanges.classList.add("active");
	tabHistory.classList.remove("active");

	diffViewer.style.display = "flex";
	detailsViewer.style.display = "none";
};

// Switches from ChangesTab to HistoryTab in the RightSidebar
const switchToHistoryTab = () => {
	tabBtnChanges.classList.remove("active");
	tabBtnHistory.classList.add("active");
	tabChanges.classList.remove("active");
	tabHistory.classList.add("active");

	diffViewer.style.display = "none";
	detailsViewer.style.display = "flex";
};

// Opens the given modal, validates its input and focuses the InputBox + selects text if set to true
const openModalWithFocus = (modal, validator, focusInput, selectText = false) => {
	closeDropdowns();
	modal.classList.add("show");
	if (validator) { validator(); }
	setTimeout(() => {
		focusInput.focus();
		if (selectText) { focusInput.select(); }
	}, 100);
};

// Copies text to clipboard
const copyToClipboard = (text) => {
	navigator.clipboard.writeText(text)
		.then(() => console.log(`Copied '${text}' to clipboard.`))
		.catch((err) => console.error("Failed to copy text: ", err));
};

// Closes ContextMenu when clicking outside of it
const closeContextMenusOnOutsideClick = (event) => {
	if (repoContextMenu.classList.contains("show") && !repoContextMenu.contains(event.target) && !repoNewBtn.contains(event.target)) {
		repoContextMenu.classList.remove("show");
	}

	contextMenus.forEach((menu) => {
		if (menu.classList.contains("show") && !menu.contains(event.target)) {
			menu.classList.remove("show");
			document.querySelectorAll(".context-active").forEach((el) => el.classList.remove("context-active"));
		}
	});
};

// ======================== MODAL VALIDATORS ========================
// Enable/disable modal's confirm button based on whether its input is valid.

// Enables the NewBranchModal confirmBtn once a name is entered
const validateBranchNewModal = () => {
	const isValid = branchNewModalInputName.value.trim() !== "";
	branchNewModalConfirmBtn.disabled = !isValid;
	branchNewModalConfirmBtn.classList.toggle("disabled", !isValid);
};

// Enables the NewBranchModal (from commit) confirmBtn once a name is entered
const validateBranchHistoryNewModal = () => {
	const isValid = branchHistoryNewModalInputName.value.trim() !== "";
	branchHistoryNewModalConfirmBtn.disabled = !isValid;
	branchHistoryNewModalConfirmBtn.classList.toggle("disabled", !isValid);
};

// Enables the BranchRenameModal confirmBtn once the new name is entered
const validateBranchRenameModal = () => {
	const newName = branchRenameModalInputName.value.trim();
	const oldName = branchRenameModal.dataset.targetName || currentBranch;

	const isValid = newName !== ""
		&& newName !== oldName
		&& !protectedBranches.includes(newName);

	branchRenameModalConfirmBtn.disabled = !isValid;
	branchRenameModalConfirmBtn.classList.toggle("disabled", !isValid);
};

// Enables the CloneRepoModal confirmBtn once the URL and localPath are entered 
const validateRepoCloneModal = () => {
	const isValid = repoCloneModalRepoUrl.value.trim() !== ""
		&& repoCloneModalInputPath.value.trim() !== "";
	repoCloneModalConfirmBtn.disabled = !isValid;
	repoCloneModalConfirmBtn.classList.toggle("disabled", !isValid);
};

// Enables the CreateRepoModal confirmBtn once the name and localPath are entered
const validateRepoCreateModal = () => {
	const isValid = repoCreateModalInputName.value.trim() !== ""
		&& repoCreateModalInputPath.value.trim() !== "";
	repoCreateModalConfirmBtn.disabled = !isValid;
	repoCreateModalConfirmBtn.classList.toggle("disabled", !isValid);
};

// Enables the AddRepoModal confirmBtn once a path is entered
const validateRepoAddModal = () => {
	const isValid = repoAddModalInputPath.value.trim() !== "";
	repoAddModalConfirmBtn.disabled = !isValid;
	repoAddModalConfirmBtn.classList.toggle("disabled", !isValid);
};

// Enables the MergeBranchModal confirmBtn once a source is selected and different from the target
const validateBranchMergeModal = () => {
	const sourceBranch = branchMergeModalSelectSource.value;
	const targetBranch = branchMergeModalSelectTarget.value;

	const isValid = sourceBranch !== ""
		&& targetBranch !== ""
		&& sourceBranch !== targetBranch;

	branchMergeModalConfirmBtn.classList.toggle("disabled", !isValid);
	branchMergeModalConfirmBtn.disabled = !isValid;
};

// ======================== CONTEXT MENU VALIDATORS ========================
// Enable/disable individual ContextMenu options based on state (e.g. protected branch, invalid repo).

// Disables Rename/Delete on a branch's context menu if it's protected (main/master)
const validateBranchContextMenu = (branchName, renameBtn, deleteBtn) => {
	const isProtected = protectedBranches.includes(branchName.toLowerCase());

	renameBtn.classList.toggle("disabled", isProtected);
	renameBtn.disabled = isProtected;

	deleteBtn.classList.toggle("disabled", isProtected);
	deleteBtn.disabled = isProtected;
};

// Disables Terminal/Explorer if the repo is empty/invalid + Remove if the repo is empty
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

// ======================== REPO HELPERS ========================
// Functions and handlers scoped to repository selection, RepoDropdown and repo lifecycle (add/clone/create/remove).

// Enable/disable repo left-sidebar tools
function setRepoToolsEnabled(enabled) {
	branchBtn.classList.toggle("disabled", !enabled);
	branchBtn.disabled = !enabled;
	mergeBtn.classList.toggle("disabled", !enabled);
	todoBtn.classList.toggle("disabled", !enabled);
	configBtn.classList.toggle("disabled", !enabled);
	fetchBtn.classList.toggle("disabled", !enabled);
}

// Build a repo dropdown item
function createRepoDropdownItem(repo) {
	const item = document.createElement("div");
	item.className = "dropdown-item";
	item.dataset.path = repo.AbsolutePath;
	item.innerHTML = `<span class="dropdown-item-text">${repo.Name}</span>`;

	if (!repo.IsValid) {
		item.classList.add("invalid");
		item.title = "Repository folder not found";
	}

	if (repo.IsRemote) { item.classList.add("remote"); }

	// LMB - select
	item.addEventListener("click", () => {
		if (item.classList.contains("invalid")) { return; }
		if (currentRepoPath.toLowerCase() === repo.AbsolutePath.toLowerCase()) { return; }

		currentRepoPath = repo.AbsolutePath;
		currentBranch = "";

		loadDraft();
		toggleCommitButton();
		switchToChangesTab();
		resetViewers();
		activeTodos = [];

		sendIpcMessage(IpcActions.REPO_SELECTED, { absolutePath: repo.AbsolutePath });
		sendIpcMessage(IpcActions.GET_REPO_STATUS, { repoPath: repo.AbsolutePath });
		sendIpcMessage(IpcActions.TODO_LOAD, { repoPath: repo.AbsolutePath });

		setRepoToolsEnabled(true);
	});

	// RMB - context menu
	item.addEventListener("contextmenu", (event) => {
		event.preventDefault();
		event.stopPropagation();

		repoContextMenu.classList.remove("show");
		repoDropdown.querySelectorAll(".context-active").forEach((el) => el.classList.remove("context-active"));
		item.classList.add("context-active");

		repoItemContextMenu.dataset.targetPath = repo.AbsolutePath;
		repoItemContextMenu.dataset.targetName = repo.Name;

		const isInvalid = item.classList.contains("invalid");

		validateRepoContextMenu(repo.AbsolutePath, repoItemMenuTerminal, repoItemMenuExplorer, repoItemMenuRemove, isInvalid);

		repoItemContextMenu.classList.add("show");

		placeContextMenu(event, repoItemContextMenu);
	});

	return item;
}

// C# - Load repositories
function loadRepositoriesIntoDropdown(repositories) {
	const existingItems = repoDropdown.querySelectorAll(".dropdown-item");
	existingItems.forEach((item) => item.remove());

	if (repositories.length === 0) { return; }

	repositories.forEach((repo) => {
		const item = createRepoDropdownItem(repo);
		repoList.appendChild(item);
	});
}

// C# - Add repository to dropdown
function addRepositoryToDropdown(repo) {
	const item = createRepoDropdownItem(repo);
	repoList.appendChild(item);

	if (repo.IsValid) { item.click(); }
}

// C# - Delete repo
function deleteRepoFromDropdown(repository) {
	const removedPath = repository.absolutePath;
	if (!removedPath) { return; }

	const cssPath = removedPath.replace(/\\/g, "\\\\");
	const itemToRemove = repoDropdown.querySelector(`.dropdown-item[data-path="${cssPath}"]`);

	if (itemToRemove) { itemToRemove.remove(); }

	if (currentRepoPath === removedPath) {
		currentRepoPath = "";

		const repoBtnValue = repoBtn.querySelector(".btn-value");
		if (repoBtnValue) { repoBtnValue.textContent = "None"; }

		setRepoToolsEnabled(false);

		const branchBtnValue = branchBtn.querySelector(".btn-value");
		if (branchBtnValue) { branchBtnValue.textContent = "None"; }

		const existingBranches = branchDropdown.querySelectorAll(".dropdown-item");
		existingBranches.forEach((item) => item.remove());
	}
}

// Marks a repo in the Topbar dropdown as invalid
function markRepoDropdownItemInvalid(repoPath) {
	const items = repoPanel.querySelectorAll(".dropdown-item");
	items.forEach((item) => {
		if (item.dataset.path.toLowerCase() === repoPath.toLowerCase()) {
			item.classList.add("invalid");
			item.title = "Repository folder not found";
		}
	});
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
		const repoBtnValue = repoBtn.querySelector(".btn-value");
		if (repoBtnValue) { repoBtnValue.textContent = "None"; }

		const branchBtnValue = branchBtn.querySelector(".btn-value");
		if (branchBtnValue) { branchBtnValue.textContent = "None"; }

		// Disable buttons
		setRepoToolsEnabled(false);

		// Clear UI panels
		const existingBranches = branchDropdown.querySelectorAll(".dropdown-item");
		existingBranches.forEach((item) => item.remove());

		changesList.innerHTML = "";
		historyList.innerHTML = "";
		currentChangesCount = 0;
		changesCountText.textContent = "0 changed files";

		resetViewers();
		toggleCommitButton();
	}
}

// C# - Folder selected
function folderSelected(directory) {
	if (activeBrowseInput) {
		activeBrowseInput.value = directory.path;
		activeBrowseInput.dispatchEvent(new Event("input"));
		activeBrowseInput.focus();
		activeBrowseInput = null;
	}
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
		const cssPath = data.Settings.LastRepoPath.replace(/\\/g, "\\\\");

		const lastRepoItem = repoDropdown.querySelector(`.dropdown-item[data-path="${cssPath}"]`);

		if (lastRepoItem) { lastRepoItem.click(); }
	}

	document.body.classList.remove("loading");
}

// ======================== BRANCH HELPERS ========================
// Functions and handlers scoped to branch selection, BranchDropdown and branch lifecycle (create/rename/delete/merge).

// C# - Load branches
function loadBranchesIntoDropdown(branches) {
	const existingItems = branchDropdown.querySelectorAll(".dropdown-item");
	existingItems.forEach((item) => item.remove());

	const branchBtnValue = document.querySelector("#branches-container .btn-value");

	if (branches.length === 0) {
		branchBtnValue.textContent = "No branches";
		return;
	}

	let currentBranchName = "None";

	branches.forEach((branch) => {
		const item = document.createElement("div");

		if (branch.IsCurrent) {
			item.className = "dropdown-item active";
			currentBranchName = branch.Name;
			currentBranch = branch.Name;
		} else {
			item.className = "dropdown-item";
		}

		item.dataset.branchName = branch.Name;

		item.innerHTML = `<span class="dropdown-item-text">${branch.Name}</span>`;

		// LMB - select
		item.addEventListener("click", () => {
			if (currentBranch.toLowerCase() === branch.Name.toLowerCase()) { return; }

			resetViewers();

			historyList.innerHTML = "";
			changesList.innerHTML = "";

			sendIpcMessage(IpcActions.BRANCH_SELECTED, {
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
			branchDropdown.querySelectorAll(".context-active").forEach((el) => el.classList.remove("context-active"));
			item.classList.add("context-active");

			placeContextMenu(event, branchItemContextMenu);
		});

		branchList.appendChild(item);
	});

	branchBtnValue.textContent = currentBranchName;

	toggleCommitButton();

	refreshRepoState();
}

// C# - Remove branch from dropdown
function removeBranchFromDropdown(payload) {
	const items = branchDropdown.querySelectorAll(".dropdown-item");
	items.forEach((item) => {
		if (item.dataset.branchName === payload.branchName) {
			item.remove();
		}
	});
}

// C# - Rename branch in dropdown
function renameBranchInDropdown(payload) {
	const oldName = payload.oldName;
	const newName = payload.newName;

	const items = branchDropdown.querySelectorAll(".dropdown-item");
	items.forEach((item) => {
		if (item.dataset.branchName === oldName) {
			item.querySelector(".dropdown-item-text").textContent = newName;
			item.dataset.branchName = newName;
		}
	});

	if (currentBranch === oldName) {
		currentBranch = newName;
		document.querySelector("#branches-container .btn-value").textContent = newName;
		toggleCommitButton();
	}

	if (currentRepoPath) {
		sendIpcMessage(IpcActions.REPO_SELECTED, { absolutePath: currentRepoPath });
	}
}

// Sets up the MergeModal (doesn't make it visible)
function setupMergeModal(preselectedTarget) {
	branchMergeModalSelectSource.innerHTML = "";
	branchMergeModalSelectTarget.innerHTML = "";

	const defaultSourceOption = document.createElement("option");
	defaultSourceOption.value = "";
	defaultSourceOption.textContent = "Select source...";
	defaultSourceOption.disabled = true;
	defaultSourceOption.selected = true;
	branchMergeModalSelectSource.appendChild(defaultSourceOption);

	const branchItems = branchDropdown.querySelectorAll(".dropdown-item");
	branchItems.forEach((item) => {
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
}

// C# - Branches merged handler
function processBranchesMerged() {
	resetViewers();

	if (currentRepoPath) {
		sendIpcMessage(IpcActions.GET_BRANCHES, { absolutePath: currentRepoPath });
	}

	refreshRepoState();
}

// ======================== CHANGES & COMMIT HELPERS ========================
// Functions scoped to the working-tree changes list, commit drafting and the commit/pull button.

// Toggles CommitButton if a commit is possible || acts as a PullButton if there are changes on remote
function toggleCommitButton() {
	// Wait for C# to return the branch name
	if (currentBranch === "") {
		commitBtn.disabled = true;
		commitBtn.classList.add("disabled");
		commitBtn.textContent = "Loading...";
		return;
	}

	if (isPullRequired) {
		commitBtn.disabled = false;
		commitBtn.classList.remove("disabled");
		commitBtn.textContent = `Pull (${commitsBehind} commit${commitsBehind === 1 ? "" : "s"} behind)`;
		return;
	}

	const checkedCount = document.querySelectorAll(".changes-item-checkbox:checked",).length;

	// Commit state
	const commitText = checkedCount > 0
		? `Commit ${checkedCount} file${checkedCount === 1 ? "" : "s"} to ${currentBranch}`
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
}

// Updates MasterCheckbox in the ChangesTab in RightSidebar
function updateMasterCheckboxState() {
	const allFileCheckboxes = Array.from(document.querySelectorAll(".changes-item-checkbox"));
	if (allFileCheckboxes.length === 0) { return; }

	const allChecked = allFileCheckboxes.every((box) => box.checked);
	changesMasterCheckbox.checked = allChecked;

	toggleCommitButton();
}

// C# - Load changed files to right sidebar
function renderChangedFiles(files) {
	changesList.innerHTML = "";

	currentChangesCount = files.length;
	changesCountText.textContent = `${files.length} changed file${files.length === 1 ? "" : "s"}`;

	if (files.length === 0) {
		toggleCommitButton();
		return;
	}

	files.forEach((file) => {
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
		`;

		// LMB - Change item clicking
		item.addEventListener("click", (event) => {
			if (event.target.classList.contains("changes-item-checkbox")) { return; }
			if (item.classList.contains("selected")) { return; }

			document.querySelectorAll(".change-item").forEach((el) => el.classList.remove("selected"));
			item.classList.add("selected");

			diffFilename.textContent = file.Path;

			// Check for binary extensions
			const ext = file.Path.substring(file.Path.lastIndexOf(".")).toLowerCase();
			if (binaryExts.includes(ext)) {
				diffContent.textContent = "Binary file changed (no preview available)";
				return;
			}

			diffContent.textContent = "Loading changes...";

			sendIpcMessage(IpcActions.GET_FILE_DIFF, {
				repoPath: currentRepoPath,
				filePath: file.Path
			});
		});

		const checkbox = item.querySelector(".changes-item-checkbox");
		checkbox.addEventListener("change", () => { updateMasterCheckboxState(); });

		// RMB - Change item context menu
		item.addEventListener("contextmenu", (event) => {
			event.preventDefault();
			event.stopPropagation();
			closeDropdowns();

			changesItemContextMenu.dataset.targetPath = file.Path;

			document.querySelectorAll(".change-item").forEach((el) => el.classList.remove("context-active"));
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

	resetViewers();
	toggleCommitButton();

	refreshRepoState();
}

// Refresh changes + history for the current repo/branch
function refreshRepoState() {
	if (!currentRepoPath) { return; }

	sendIpcMessage(IpcActions.GET_REPO_STATUS, { repoPath: currentRepoPath });

	if (currentBranch) {
		sendIpcMessage(IpcActions.GET_BRANCH_HISTORY, {
			repoPath: currentRepoPath,
			branchName: currentBranch
		});
	}
}

// C# - Repo fetching
function fetchRepo(repo) {
	stopFetchAnimation();
	refreshRepoState();
	toggleCommitButton();
}

// C# - Repo files changed
function processFileChanges(repo) {
	if (currentRepoPath === repo.repoPath) {
		sendIpcMessage(IpcActions.GET_REPO_STATUS, { repoPath: currentRepoPath });
	} else {
		console.warn("RepoWatcher is watching unnecessary repositories.");
	}
}

// Saves Commit message & description 
function saveDraft() {
	if (!currentRepoPath) { return; }

	repoDrafts[currentRepoPath] = {
		message: commitMessageInput.value,
		description: commitDescriptionInput.value
	};
}

// Loads saved Commit message & description
function loadDraft() {
	if (!currentRepoPath) { return; }

	const draft = repoDrafts[currentRepoPath];

	if (draft) {
		commitMessageInput.value = draft.message;
		commitDescriptionInput.value = draft.description;
	} else {
		commitMessageInput.value = "";
		commitDescriptionInput.value = "";
	}
}

// ======================== HISTORY & DIFF HELPERS ========================
// Functions scoped to commit history, commit details and rendering file diffs.

// C# - Load/append commit history into history-tab
function renderHistory(commits, isAppending = false) {
	if (!isAppending) {
		historyList.innerHTML = "";
	}

	if (!commits || commits.length === 0) {
		updateCustomScrollbar(historyList, historyScrollbar);
		return;
	}

	commits.forEach((commit) => {
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

			document.querySelectorAll(".history-item").forEach((el) => el.classList.remove("selected"));
			item.classList.add("selected");

			activeHistoryHash = commit.Hash;
			detailsCommitMessage.textContent = `Loading details...`;
			detailsCommitStats.textContent = ``;
			detailsContent.textContent = "";

			detailsBtnValue.textContent = "Loading...";
			detailsBtn.disabled = true;
			detailsBtn.classList.add("disabled");

			sendIpcMessage(IpcActions.GET_COMMIT_DETAILS, {
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

			document.querySelectorAll(".history-item").forEach((el) => el.classList.remove("context-active"));
			item.classList.add("context-active");

			historyItemContextMenu.classList.add("show");
			placeContextMenu(event, historyItemContextMenu);
		});

		historyList.appendChild(item);
	});

	updateCustomScrollbar(historyList, historyScrollbar);
}

// C# - Initial branch history load
function processBranchHistoryLoaded(commits) {
	currentHistorySkip = 0;
	isFetchingHistory = false;
	hasReachedEndOfHistory = commits.length < historyTake;
	renderHistory(commits, false);
}

// C# - Paginated branch history load (infinite scroll)
function processBranchHistoryAppended(commits) {
	isFetchingHistory = false;
	if (commits.length < historyTake) {
		hasReachedEndOfHistory = true;
	}
	renderHistory(commits, true);
}

// C# - History commit details
function loadCommitDetails(details) {
	activeDetailsFile = "";

	// Header info
	detailsCommitMessage.innerHTML = `${details.message}`;
	detailsCommitStats.textContent = `${details.files.length} file${details.files.length === 1 ? "" : "s"} changed`;

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
		if (file.StatusCode.startsWith("A")) {
			statusClass = "status-added";
		} else if (file.StatusCode.startsWith("D")) {
			statusClass = "status-deleted";
		} else if (file.StatusCode.startsWith("R")) {
			statusClass = "status-renamed";
		} else if (file.StatusCode.startsWith("U")) {
			statusClass = "status-untracked";
		}

		item.innerHTML = `<span class="change-status ${statusClass}">${file.StatusCode[0]}</span>
						  <span class="change-path">${file.Path}</span>`;

		// LMB
		item.addEventListener("click", (event) => {
			if (file.Path === activeDetailsFile) {
				closeDropdowns();
				return;
			}
			activeDetailsFile = file.Path;

			event.stopPropagation();
			detailsFileList.querySelectorAll(".dropdown-item").forEach((el) => el.classList.remove("active"));
			item.classList.add("active");

			detailsBtnValue.innerHTML = `<span class="change-status ${statusClass}">${file.StatusCode[0]}</span>
										 <span class="change-path">${file.Path}</span>`;
			closeDropdowns();

			const ext = file.Path.substring(file.Path.lastIndexOf(".")).toLowerCase();
			if (binaryExts.includes(ext)) {
				detailsContent.innerHTML = `<span class="diff-chunk">Binary file changes (no preview available)</span>`;
				updateCustomScrollbar(detailsBodyWrapper, detailsScrollbar);
				return;
			}

			detailsContent.textContent = "Loading diff...";

			sendIpcMessage(IpcActions.GET_HISTORY_FILE_DIFF, {
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

	const lines = diffText.split("\n");
	const formattedLines = [];
	let hitFirstChunk = !diffText.startsWith("diff --git");

	for (let line of lines) {
		// skip git diff headers
		if (!hitFirstChunk) {
			if (line.startsWith("@@")) { hitFirstChunk = true; }
			else { continue; }
		}

		if (line.startsWith("\\")) { continue; }

		let safeLine = escapeHtml(line);

		// Apply classes based on character (1 spaces)
		if (safeLine.startsWith("+")) {
			formattedLines.push(
				`<span class="diff-add">+ ${safeLine.substring(1)}</span>`,
			);
		} else if (safeLine.startsWith("-")) {
			formattedLines.push(
				`<span class="diff-remove">- ${safeLine.substring(1)}</span>`,
			);
		} else if (safeLine.startsWith("@@")) {
			formattedLines.push(
				`<span class="diff-chunk">${safeLine.substring(1)}</span>`,
			);
		} else if (safeLine.startsWith(" ")) {
			formattedLines.push(
				`<span class="diff-normal">  ${safeLine.substring(1)}</span>`,
			);
		} else {
			formattedLines.push(
				`<span class="diff-normal">  ${safeLine.substring(1)}</span>`,
			);
		}
	}

	// Inject colored html
	if (formattedLines.length === 0) {
		contentTarget.innerHTML = `<span class="diff-chunk">Empty file (no content to preview)</span>`;
	} else {
		contentTarget.innerHTML = formattedLines.join("");
	}

	updateCustomScrollbar(wrapperTarget, scrollbarTarget);
}

// Resets the file-diff viewer to its empty/placeholder state
function resetDiffViewer() {
	diffFilename.textContent = "Select a file to view changes";
	diffContent.textContent = "";
	updateCustomScrollbar(diffBodyWrapper, diffScrollbar);
}

// Resets the commit-details viewer to its empty/placeholder state
function resetDetailsViewer() {
	detailsCommitMessage.textContent = "Select a commit to view details";
	detailsCommitStats.textContent = "";
	detailsContent.textContent = "";

	detailsBtnValue.textContent = "No commit selected";
	detailsBtn.disabled = true;
	detailsBtn.classList.add("disabled");
	detailsFileList.innerHTML = "";
}

// Resets both main-content viewers together
function resetViewers() {
	resetDiffViewer();
	resetDetailsViewer();
}

// ======================== TODO HELPERS ========================
// Functions scoped to the per-repo TodoList modal.

// Rebuilds the TodoModal task list + extra "add new task" row at the end/bottom
function renderTodoList() {
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
			if (newInputs.length > 0) { newInputs[newInputs.length - 1].focus(); }
		}
	});

	newRow.appendChild(newInput);
	todoModalRowsContainer.appendChild(newRow);

	setTimeout(() => {
		updateCustomScrollbar(todoModalRowsContainer, todoScrollbar);
	}, 10);
}

// C# - Load todos
function loadTodos(todos) {
	activeTodos = todos || [];
}

// ======================== CONFIG & SETTINGS HELPERS ========================
// Functions scoped to local/global git config and app settings.

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

// C# - Handle backend errors
function showErrorModal(message) {
	errorModalMessage.textContent = message || "An unknown error occurred.";
	errorModal.classList.add("show");
}

// ======================== GLOBAL & GENERIC LISTENERS ========================
// Listeners with no single domain owner, window/document, dropdown/modal mechanics, scrollbars, app-wide shortcuts.

// Global overrides (prevent scroll-zoom & close modals/context-menus on ESC key)
document.addEventListener("wheel", (event) => {
	if (event.ctrlKey || event.metaKey) { event.preventDefault(); }
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

// Toggles (dropdowns)
repoBtn.addEventListener("click", (event) =>
	toggleDropdown(repoPanel, branchPanel, event)
);

repoBtn.addEventListener("contextmenu", (event) => {
	event.preventDefault();
	event.stopPropagation();
	closeDropdowns();

	validateRepoContextMenu(currentRepoPath, topbarRepoMenuTerminal, topbarRepoMenuExplorer, topbarRepoMenuRemove);

	topbarRepoContextMenu.classList.add("show");
	placeContextMenu(event, topbarRepoContextMenu);
});

branchBtn.addEventListener("click", (event) =>
	toggleDropdown(branchPanel, repoPanel, event)
);

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
dropdownBackdrop.addEventListener("click", closeDropdowns);

window.addEventListener("click", closeDropdowns);

// Close triggers (modals)
modalBackdrops.forEach((backdrop) => {
	backdrop.addEventListener("click", (event) => {
		if (event.target === backdrop) { closeAndClearModal(backdrop); }
	});
});

modalCloseTriggers.forEach((trigger) => {
	trigger.addEventListener("click", (event) => {
		const parentModal = event.target.closest(".modal-backdrop");
		if (parentModal) { closeAndClearModal(parentModal); }
	});
});

// Prevent closing when inside (dropdowns/context-menus)
document.querySelectorAll(".dropdown-panel, .context-menu").forEach((panel) => {
	panel.addEventListener("click", (event) => event.stopPropagation());
});

// Resizer logic (split-container)
window.addEventListener("resize", () => {
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

	if (branchPanel.classList.contains("show")) {
		branchPanel.style.width = `${newWidth}px`;
	}

	if (repoPanel.classList.contains("show")) {
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
	allFileCheckboxes.forEach((box) => {
		box.checked = isChecked;
	});

	toggleCommitButton();
});

// Scrollbars (topbar)
repoList.addEventListener("scroll", () =>
	updateCustomScrollbar(repoList, repoScrollbar)
);

branchList.addEventListener("scroll", () =>
	updateCustomScrollbar(branchList, branchScrollbar)
);

// Todo list scrollbar (scrollbar/modals)
todoModalRowsContainer.addEventListener("scroll", () =>
	updateCustomScrollbar(todoModalRowsContainer, todoScrollbar)
);

// Diff page (main-content)
diffBodyWrapper.addEventListener("scroll", () => updateCustomScrollbar(diffBodyWrapper, diffScrollbar));

// History details page (main-content)
detailsBodyWrapper.addEventListener("scroll", () =>
	updateCustomScrollbar(detailsBodyWrapper, detailsScrollbar)
);

detailsFileList.addEventListener("scroll", () =>
	updateCustomScrollbar(detailsFileList, detailsFileScrollbar)
);

detailsBtn.addEventListener("click", (event) => {
	if (detailsBtn.disabled || detailsBtn.classList.contains("disabled")) { return; }

	event.stopPropagation();
	repoPanel.classList.remove("show");
	branchPanel.classList.remove("show");
	const isOpening = detailsPanel.classList.toggle("show");
	dropdownBackdrop.classList.toggle("show", isOpening);

	if (isOpening) {
		setTimeout(() => updateCustomScrollbar(detailsFileList, detailsFileScrollbar));
	}
});

// Scrollbar (right-sidebar)
changesList.addEventListener("scroll", () => {
	updateCustomScrollbar(changesList, changesScrollbar);
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

// Force close (context-menu)
window.addEventListener("click", closeContextMenusOnOutsideClick, true);

window.addEventListener("click", (event) => {
	if (repoCreateModalLicenseEditPanel.classList.contains("open")) {
		if (!repoCreateModalLicenseEditPanel.contains(event.target) && !repoCreateModalLicenseEdit.contains(event.target)) {
			repoCreateModalLicenseEditPanel.classList.remove("open");
			repoCreateModalLicenseEdit.classList.remove("active");
		}
	}
}, true);

// Search & Selection Setup (dropdowns)
const setupFilter = (inputSelector, panelSelector) => {
	const searchInput = document.querySelector(inputSelector);

	searchInput.addEventListener("input", (event) => {
		const searchTerm = event.target.value.toLowerCase();

		const dropdownItems = document.querySelectorAll(`${panelSelector} .dropdown-item`);

		dropdownItems.forEach((item) => {
			const itemText = item.textContent.toLowerCase();

			if (itemText.includes(searchTerm)) { item.style.display = ""; }
			else { item.style.display = "none"; }
		});

		updateCustomScrollbar(repoList, repoScrollbar);
		updateCustomScrollbar(branchList, branchScrollbar);
	});
};

const setupSelection = (panelSelector, btnValueSelector) => {
	const panel = document.querySelector(panelSelector);
	const valueDisplay = document.querySelector(btnValueSelector);

	panel.addEventListener("click", (event) => {
		const item = event.target.closest(".dropdown-item");

		if (item && !item.classList.contains("invalid")) {
			valueDisplay.textContent = item.textContent;

			panel.querySelectorAll(".dropdown-item").forEach((elem) => { elem.classList.remove("active"); });

			item.classList.add("active");

			closeDropdowns();
		}
	});
};

setupFilter("#repositories-filter .filter-input", "#repository-dropdown-panel");
setupFilter("#branches-filter .filter-input", "#branch-dropdown-panel");

setupSelection("#repository-dropdown-panel", "#repositories-container .btn-value");
setupSelection("#branch-dropdown-panel", "#branches-container .btn-value");

// ======================== REPO LISTENERS ========================
// Click/Input listeners scoped to the RepoDropdown items, repo ContextMenus and RepoModals.

// New button (dropdown)
repoNewBtn.addEventListener("click", (event) => {
	event.stopPropagation();

	repoContextMenu.classList.add("show");

	repoItemContextMenu.classList.remove("show");

	placeContextMenu(event, repoContextMenu);
});

// New button context-menu options (dropdown/context-menu)
repoMenuClone.addEventListener("click", (event) => {
	event.stopPropagation();
	openModalWithFocus(repoCloneModal, validateRepoCloneModal, repoCloneModalRepoUrl);
});

repoMenuCreate.addEventListener("click", (event) => {
	event.stopPropagation();
	openModalWithFocus(repoCreateModal, validateRepoCreateModal, repoCreateModalInputName);
});

repoMenuAdd.addEventListener("click", (event) => {
	event.stopPropagation();
	openModalWithFocus(repoAddModal, validateRepoAddModal, repoAddModalInputPath);
});

// Topbar context-menu options (topbar/context-menu)
topbarRepoMenuClone.addEventListener("click", (event) => {
	event.stopPropagation();
	openModalWithFocus(repoCloneModal, validateRepoCloneModal, repoCloneModalRepoUrl);
});

topbarRepoMenuCreate.addEventListener("click", (event) => {
	event.stopPropagation();
	openModalWithFocus(repoCreateModal, validateRepoCreateModal, repoCreateModalInputName);
});

topbarRepoMenuAdd.addEventListener("click", (event) => {
	event.stopPropagation();
	openModalWithFocus(repoAddModal, validateRepoAddModal, repoAddModalInputPath);
});

topbarRepoMenuTerminal.addEventListener("click", (event) => {
	event.stopPropagation();
	if (currentRepoPath) {
		sendIpcMessage(IpcActions.REPO_TERMINAL, {
			repoPath: currentRepoPath
		});
	}
	closeDropdowns();
});

topbarRepoMenuExplorer.addEventListener("click", (event) => {
	event.stopPropagation();
	if (currentRepoPath) {
		sendIpcMessage(IpcActions.EXPLORER_OPEN, {
			path: currentRepoPath
		});
	}
	closeDropdowns();
});

topbarRepoMenuRemove.addEventListener("click", (event) => {
	event.stopPropagation();
	const pathToRepo = currentRepoPath;
	if (pathToRepo) {
		repoRemoveModal.dataset.targetPath = currentRepoPath;
		repoRemoveModalName.textContent = document.querySelector("#repositories-container .btn-value").textContent;
		repoRemoveModalLocalCheckbox.checked = false;
		repoRemoveModal.classList.add("show");
	}
	closeDropdowns();
});

// Dropdown repo item context-menu options (dropdown-items/context-menu)
repoItemMenuTerminal.addEventListener("click", (event) => {
	event.stopPropagation();

	const pathToRepo = repoItemContextMenu.dataset.targetPath;
	if (pathToRepo) {
		sendIpcMessage(IpcActions.REPO_TERMINAL, {
			repoPath: pathToRepo
		});
	}
	closeDropdowns();
});

repoItemMenuExplorer.addEventListener("click", (event) => {
	event.stopPropagation();

	const pathToRepo = repoItemContextMenu.dataset.targetPath;
	if (pathToRepo) {
		sendIpcMessage(IpcActions.EXPLORER_OPEN, {
			path: pathToRepo
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

// Repo related modals confirm buttons (modals)
repoCloneModalConfirmBtn.addEventListener("click", () => {
	const repoUrl = repoCloneModalRepoUrl.value;
	const asLocal = repoCloneModalAsLocal.checked;
	const localPath = repoCloneModalInputPath.value;

	if (!repoUrl || !localPath) { return; }

	sendIpcMessage(IpcActions.REPO_CLONE, {
		repoUrl: repoUrl,
		asLocal: asLocal,
		localPath: localPath
	});

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

	sendIpcMessage(IpcActions.REPO_CREATE, {
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

	sendIpcMessage(IpcActions.REPO_ADD, {
		repoPath: repoPath
	});

	closeAndClearModal(repoAddModal);
});

repoRemoveModalConfirmBtn.addEventListener("click", () => {
	const repoPath = repoRemoveModal.dataset.targetPath;

	if (repoPath) {
		const deleteLocal = repoRemoveModalLocalCheckbox.checked;

		sendIpcMessage(IpcActions.REPO_REMOVE, {
			repoPath: repoPath,
			deleteLocal: deleteLocal
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

// Custom cancel button for 'repo-missing-modal' (modals)
repoMissingModalCancelBtn.addEventListener("click", () => {
	const stalePath = repoMissingModal.dataset.targetPath;

	if (stalePath) {
		sendIpcMessage(IpcActions.REPO_REMOVE, {
			repoPath: stalePath,
			deleteLocal: false
		});
	}

	closeAndClearModal(repoMissingModal);
});

// Repo related modals 'enter' shortcuts (modals)
repoCreateModalInputName.addEventListener("keyup", (event) => {
	if (event.key === "Enter") { repoCreateModalConfirmBtn.click(); }
});

repoCreateModalInputPath.addEventListener("keyup", (event) => {
	if (event.key === "Enter") { repoCreateModalConfirmBtn.click(); }
});

repoAddModalInputPath.addEventListener("keyup", (event) => {
	if (event.key === "Enter") { repoAddModalConfirmBtn.click(); }
});

// Repo related modals 'input -> validator' listeners (modals)
repoCloneModalRepoUrl.addEventListener("input", validateRepoCloneModal);
repoCloneModalInputPath.addEventListener("input", validateRepoCloneModal);
repoCreateModalInputName.addEventListener("input", validateRepoCreateModal);
repoCreateModalInputPath.addEventListener("input", validateRepoCreateModal);
repoAddModalInputPath.addEventListener("input", validateRepoAddModal);

// Browse buttons (modals)
repoCloneModalBrowseBtn.addEventListener("click", (event) => {
	event.preventDefault();

	activeBrowseInput = repoCloneModalInputPath;

	sendIpcMessage(IpcActions.EXPLORER_OPEN_DIALOG);
});

repoCreateModalBrowseBtn.addEventListener("click", (event) => {
	event.preventDefault();

	activeBrowseInput = repoCreateModalInputPath;

	sendIpcMessage(IpcActions.EXPLORER_OPEN_DIALOG);
});

repoAddModalBrowseBtn.addEventListener("click", (event) => {
	event.preventDefault();

	activeBrowseInput = repoAddModalInputPath;

	sendIpcMessage(IpcActions.EXPLORER_OPEN_DIALOG);
});

// RepoCreateModal edit button (modals/repo-create-modal)
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

// ======================== BRANCH LISTENERS ========================
// Click/Input listeners scoped to the BranchDropdown items, branch ContextMenus and BranchModals.

// New button (dropdown)
branchNewBtn.addEventListener("click", (event) => {
	event.stopPropagation();
	openModalWithFocus(branchNewModal, validateBranchNewModal, branchNewModalInputName);
});

// Topbar context-menu options (topbar/context-menu)
topbarBranchMenuNew.addEventListener("click", (event) => {
	event.stopPropagation();
	openModalWithFocus(branchNewModal, validateBranchNewModal, branchNewModalInputName);
});

topbarBranchMenuCopy.addEventListener("click", (event) => {
	event.stopPropagation();
	closeDropdowns();
	if (currentBranch) { copyToClipboard(currentBranch); }
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

	if (!currentRepoPath || !currentBranch) { return; }

	branchRenameModal.dataset.targetName = currentBranch;
	branchRenameModalInputName.value = currentBranch;

	openModalWithFocus(branchRenameModal, validateBranchRenameModal, branchRenameModalInputName, true);
});

topbarBranchMenuDelete.addEventListener("click", (event) => {
	event.stopPropagation();
	closeDropdowns();

	if (!currentRepoPath || !currentBranch) { return; }

	branchDeleteModal.dataset.targetName = currentBranch;
	branchDeleteModalName.textContent = currentBranch;

	branchDeleteModal.classList.add("show");
});

// Dropdown branch item context-menu options (dropdown-items/context-menu)
branchItemMenuRename.addEventListener("click", (event) => {
	event.stopPropagation();
	const branchName = branchItemContextMenu.dataset.targetName;

	if (branchName) {
		branchRenameModal.dataset.targetName = branchName;
		branchRenameModalInputName.value = branchName;

		openModalWithFocus(branchRenameModal, validateBranchRenameModal, branchRenameModalInputName, true);
	}

	branchItemContextMenu.classList.remove("show");
	closeDropdowns();
});

branchItemMenuCopy.addEventListener("click", (event) => {
	event.stopPropagation();
	const branchName = branchItemContextMenu.dataset.targetName;

	if (branchName) { copyToClipboard(branchName); }

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

		branchDeleteModalName.textContent = branchName;
		branchDeleteModal.classList.add("show");
	}

	branchItemContextMenu.classList.remove("show");
	closeDropdowns();
});

// Left sidebar branch related button (left-sidebar)
mergeBtn.addEventListener("click", () => {
	if (!currentRepoPath || !currentBranch) { return; }

	setupMergeModal(currentBranch);
	branchMergeModal.dataset.targetName = currentBranch;
	branchMergeModal.classList.add("show");
});

// Branch related modals confirm buttons (modals)
branchNewModalConfirmBtn.addEventListener("click", () => {
	const newBranchName = branchNewModalInputName.value.trim();

	if (newBranchName === "") { return; }

	sendIpcMessage(IpcActions.BRANCH_CREATE, {
		repoPath: currentRepoPath,
		branchName: newBranchName
	});

	closeAndClearModal(branchNewModal);
});

branchHistoryNewModalConfirmBtn.addEventListener("click", () => {
	const newBranchName = branchHistoryNewModalInputName.value.trim();
	const commitHash = branchHistoryNewModal.dataset.targetHash;

	if (newBranchName === "" || !commitHash) { return; }

	sendIpcMessage(IpcActions.BRANCH_HISTORY_CREATE, {
		repoPath: currentRepoPath,
		branchName: newBranchName,
		commitHash: commitHash
	});

	closeAndClearModal(branchHistoryNewModal);
});

branchRenameModalConfirmBtn.addEventListener("click", () => {
	const newBranchName = branchRenameModalInputName.value.trim();
	const oldBranchName = branchRenameModal.dataset.targetName;

	if (newBranchName === "" || !oldBranchName) { return; }

	sendIpcMessage(IpcActions.BRANCH_RENAME, {
		repoPath: currentRepoPath,
		oldName: oldBranchName,
		newName: newBranchName
	});

	closeAndClearModal(branchRenameModal);
});

branchDeleteModalConfirmBtn.addEventListener("click", () => {
	const branchName = branchDeleteModal.dataset.targetName;

	if (!branchName) { return; }

	sendIpcMessage(IpcActions.BRANCH_DELETE, {
		repoPath: currentRepoPath,
		branchName: branchName
	});

	closeAndClearModal(branchDeleteModal);
});

branchMergeModalConfirmBtn.addEventListener("click", () => {
	const sourceBranch = branchMergeModalSelectSource.value;
	const targetBranch = branchMergeModalSelectTarget.value;

	if (!sourceBranch || !targetBranch || !currentRepoPath || sourceBranch === targetBranch) { return; }

	sendIpcMessage(IpcActions.BRANCH_MERGE, {
		repoPath: currentRepoPath,
		sourceBranch: sourceBranch,
		targetBranch: targetBranch
	});

	closeAndClearModal(branchMergeModal);
});

// Branch related modals 'enter' shortcuts (modals)
branchNewModalInputName.addEventListener("keyup", (event) => {
	if (event.key === "Enter") { branchNewModalConfirmBtn.click(); }
});

branchHistoryNewModal.addEventListener("keyup", (event) => {
	if (event.key === "Enter") { branchHistoryNewModalConfirmBtn.click(); }
});

branchRenameModalInputName.addEventListener("keyup", (event) => {
	if (event.key === "Enter") { branchRenameModalConfirmBtn.click(); }
});

// Branch related modals 'input -> validator' listeners (modals)
branchNewModalInputName.addEventListener("input", validateBranchNewModal);
branchHistoryNewModalInputName.addEventListener("input", validateBranchHistoryNewModal);
branchRenameModalInputName.addEventListener("input", validateBranchRenameModal);

// ======================== CHANGES & COMMIT LISTENERS ========================
// Click/Input listeners scoped to the commit draft inputs, commit/pull button and changed-file ContextMenu.

// Commit section (right-sidebar)
commitMessageInput.addEventListener("input", () => {
	saveDraft();
	toggleCommitButton();
});

commitDescriptionInput.addEventListener("input", () => {
	saveDraft();
});

commitBtn.addEventListener("click", () => {
	// Pull
	if (isPullRequired) {
		commitBtn.disabled = true;
		commitBtn.classList.add("disabled");
		commitBtn.textContent = "Pulling...";

		sendIpcMessage(IpcActions.REPO_PULL, {
			repoPath: currentRepoPath
		});
		return;
	}

	// Commit
	const message = commitMessageInput.value.trim();
	const description = commitDescriptionInput.value.trim();

	if (message === "" || currentRepoPath === "" || currentChangesCount === 0) { return; }

	const selectedFiles = [];
	const allFileCheckboxes = document.querySelectorAll(".changes-item-checkbox:checked");

	allFileCheckboxes.forEach((checkbox) => {
		const itemRow = checkbox.closest(".change-item");
		const filePath = itemRow.dataset.path;
		selectedFiles.push(filePath);
	});

	if (selectedFiles.length === 0) { return; }

	commitBtn.disabled = true;
	commitBtn.classList.add("disabled");

	sendIpcMessage(IpcActions.REPO_COMMIT, {
		repoPath: currentRepoPath,
		message: message,
		description: description,
		files: selectedFiles
	});
});

// Changes section (right-sidebar)
changesItemMenuDiscard.addEventListener("click", (event) => {
	event.stopPropagation();
	const filePath = changesItemContextMenu.dataset.targetPath;
	if (filePath) {
		sendIpcMessage(IpcActions.CHANGE_DISCARD, {
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
		sendIpcMessage(IpcActions.CHANGE_IGNORE, {
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

		const isWindows = currentRepoPath.includes("\\");

		if (isWindows) { absPath = absPath.replace(/\//g, "\\").replace(/\\\\/g, "\\"); }
		else { absPath = absPath.replace(/\\/g, "/").replace(/\/\//g, "/"); }

		navigator.clipboard.writeText(absPath).then(() => console.log(`Copied '${absPath}'`));
	}
	closeDropdowns();
});

changesItemMenuCopyRelPath.addEventListener("click", (event) => {
	event.stopPropagation();
	const filePath = changesItemContextMenu.dataset.targetPath;
	if (filePath) { copyToClipboard(filePath); }
	closeDropdowns();
});

changesItemMenuExplorer.addEventListener("click", (event) => {
	event.stopPropagation();
	const filePath = changesItemContextMenu.dataset.targetPath;
	if (filePath) {
		let rawPath = `${currentRepoPath}/${filePath}`;
		let lastSlashIndex = rawPath.lastIndexOf("/");
		let dirPath = rawPath.substring(0, lastSlashIndex);

		sendIpcMessage(IpcActions.EXPLORER_OPEN, {
			path: dirPath
		});
	}
	closeDropdowns();
});

// ======================== HISTORY LISTENERS ========================
// Click/Input listeners scoped to commit history and history item context menu.

historyList.addEventListener("scroll", () => {
	updateCustomScrollbar(historyList, historyScrollbar);

	if (isFetchingHistory || hasReachedEndOfHistory || !currentBranch) { return; }

	const threshold = 30; // 30px
	const isNearBottom = historyList.scrollTop + historyList.clientHeight >= historyList.scrollHeight - threshold;

	if (isNearBottom) {
		isFetchingHistory = true;
		currentHistorySkip += historyTake;

		sendIpcMessage(IpcActions.GET_BRANCH_HISTORY, {
			repoPath: currentRepoPath,
			branchName: currentBranch,
			skip: currentHistorySkip,
			take: historyTake
		});
	}
});

historyItemMenuCheckout.addEventListener("click", (event) => {
	event.stopPropagation();
	const hash = historyItemContextMenu.dataset.targetHash;
	if (hash) {
		sendIpcMessage(IpcActions.HISTORY_CHECKOUT, {
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
		sendIpcMessage(IpcActions.HISTORY_REVERT, {
			repoPath: currentRepoPath,
			commitHash: hash
		});
	}
	closeDropdowns();
});

historyItemMenuCreateBranch.addEventListener("click", (event) => {
	event.stopPropagation();
	const hash = historyItemContextMenu.dataset.targetHash;
	if (hash) {
		branchHistoryNewModalName.textContent = hash.substring(0, 7); // hash shortened to 7 chars
		branchHistoryNewModal.dataset.targetHash = hash;
		openModalWithFocus(branchHistoryNewModal, validateBranchHistoryNewModal, branchHistoryNewModalInputName);
	}
	closeDropdowns();
});

historyItemMenuCopySHA.addEventListener("click", (event) => {
	event.stopPropagation();
	const hash = historyItemContextMenu.dataset.targetHash;
	if (hash) { copyToClipboard(hash); }
	closeDropdowns();
});

// ======================== TODO LISTENERS ========================
// Click listeners scoped to opening and saving the TodoModal.

todoBtn.addEventListener("click", () => {
	if (!currentRepoPath) { return; }

	draftTodos = JSON.parse(JSON.stringify(activeTodos));

	renderTodoList();
	todoModal.classList.add("show");
});

todoModalConfirmBtn.addEventListener("click", () => {
	if (!currentRepoPath) { return; }

	activeTodos = JSON.parse(JSON.stringify(draftTodos));

	sendIpcMessage(IpcActions.TODO_SAVE, {
		repoPath: currentRepoPath,
		todos: activeTodos
	});

	closeAndClearModal(todoModal);
});

// ======================== CONFIG & SETTINGS LISTENERS ========================
// Click/Input listeners scoped to local/global config and AppSettings modals.

// LeftSidebar binding (left-sidebar)
configBtn.addEventListener("click", () => {
	if (!currentRepoPath) { return; }

	sendIpcMessage(IpcActions.CONFIG_LOCAL_GET, { repoPath: currentRepoPath });
});

settingsBtn.addEventListener("click", () => {
	settingsModalThemeSelect.value = currentTheme;
	settingsModal.classList.add("show");
});

accountBtn.addEventListener("click", () =>
	sendIpcMessage(IpcActions.CONFIG_GLOBAL_GET, {})
);

// Settings modal (modals)
settingsModalLogsView.addEventListener("click", () =>
	sendIpcMessage(IpcActions.EXPLORER_OPEN, {
		path: "%APP_DATA%"
	})
);

settingsModalLogsClear.addEventListener("click", () =>
	sendIpcMessage(IpcActions.LOGS_CLEAR, {})
);

settingsModalConfirmBtn.addEventListener("click", () => {
	const updatedSettings = {
		Id: 1,
		Theme: settingsModalThemeSelect.value
	};

	sendIpcMessage(IpcActions.SETTINGS_SAVE, updatedSettings);

	currentTheme = updatedSettings.Theme;

	applyTheme(updatedSettings.Theme);
	closeAndClearModal(settingsModal);
});

// Config modal (modals)
configModalConfirmBtn.addEventListener("click", () => {
	if (!currentRepoPath) { return; }

	sendIpcMessage(IpcActions.CONFIG_LOCAL_SAVE, {
		repoPath: currentRepoPath,
		name: configModalName.value.trim(),
		email: configModalEmail.value.trim()
	});

	closeAndClearModal(configModal);
});

// Account modal (modals)
accountModalConfirmBtn.addEventListener("click", () => {
	sendIpcMessage(IpcActions.CONFIG_GLOBAL_SAVE, {
		name: accountModalInputName.value.trim(),
		email: accountModalInputEmail.value.trim()
	});

	closeAndClearModal(accountModal);
});

// ======================== FETCH LISTENERS ========================
// Click listener that triggers a remote fetch.

fetchBtn.addEventListener("click", () => {
	if (!currentRepoPath || fetchBtn.classList.contains("fetching")) { return; }

	fetchBtn.classList.add("fetching");

	sendIpcMessage(IpcActions.REPO_FETCH, {
		repoPath: currentRepoPath,
	});
});

// ======================== APP INIT ========================
// Initial app code execution (the only part of the code that directly executed functions).
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
branchesBtn.classList.add("disabled"); // FINISH
analyticsBtn.classList.add("disabled"); // FINISH
setRepoToolsEnabled(false);

switchToChangesTab();
toggleCommitButton();
resetDetailsViewer();

window.addEventListener("DOMContentLoaded", () => {
	sendIpcMessage(IpcActions.APP_READY);
});
