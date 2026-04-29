// Topbar & it's children
const repoContainer = document.getElementById("repositories-container");
const repoBtn = document.querySelector("#repositories-container .topbar-btn");
const repoPanel = document.querySelector("#repositories-container .dropdown-panel");

const branchContainer = document.getElementById("branches-container");
const branchBtn = document.querySelector("#branches-container .topbar-btn");
const branchPanel = document.querySelector("#branches-container .dropdown-panel");

const backdrop = document.getElementById("dropdown-backdrop");

// The rest
const leftSidebar = document.getElementById("left-sidebar");

const mainContent = document.getElementById("main-content");

const rightSidebar = document.getElementById("right-sidebar");
const resizer = document.getElementById("resizer");

// ----- HELPERS -----
let minRightWidth = 0, maxRightWidth = 0;
let isResizing = false;

// Keep panel widths in sync (match their parents)
const updatePanelWidths = () => {
    repoPanel.style.width = `${repoContainer.offsetWidth}px`;
    repoPanel.style.left = `${repoContainer.offsetLeft}px`;
    branchPanel.style.width = window.getComputedStyle(rightSidebar).width;
}

// Close everything
const closeDropdowns = () => {
    repoPanel.classList.remove('show');
    branchPanel.classList.remove('show');
    backdrop.classList.remove('show');
}

const toggleDropdown = (toShow, toHide, event) => {
    event.stopPropagation();
    updatePanelWidths;
    toHide.classList.remove('show');

    const isOpening = toShow.classList.toggle('show');
    backdrop.classList.toggle('show', isOpening);
}

// ----- DROPDOWN EVENTS -----
// Initial setup
updatePanelWidths();

// Toggles
repoBtn.addEventListener('click', (event) => toggleDropdown(repoPanel, branchPanel, event));
branchBtn.addEventListener('click', (event) => toggleDropdown(branchPanel, repoPanel, event));

// Close triggers
backdrop.addEventListener('click', closeDropdowns);
window.addEventListener('click', closeDropdowns);
window.addEventListener('resize', updatePanelWidths); //???

// Prevent closing when inside
document.querySelectorAll('.dropdown-panel').forEach(panel => {
    panel.addEventListener('click', (event) => event.stopPropagation());
});

// ----- RESIZER LOGIC -----
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