const topbar = document.getElementById("topbar");
const branchDropdown = document.getElementById("branches-dropdown");

const leftSidebar = document.getElementById("left-sidebar");

const mainContent = document.getElementById("main-content");

const rightSidebar = document.getElementById("right-sidebar");
const resizer = document.getElementById("resizer");

let minRightWidth, maxRightWidth;
let isResizing = false;

// Topbar Dropdown Panel
document.addEventListener('DOMContentLoaded', () => {
    const repoBtn = document.querySelector("#repositories-dropdown .topbar-btn");
    const repoContainer = document.getElementById("repositories-dropdown");
    const repoPanel = document.querySelector("#repositories-dropdown .dropdown-panel");

    const branchBtn = document.querySelector("#branches-dropdown .topbar-btn");
    const branchPanel = document.querySelector("#branches-dropdown .dropdown-panel");

    const rightSidebar = document.getElementById("right-sidebar");

    const backdrop = document.getElementById("dropdown-backdrop");

    // Close everything
    const closeDropdowns = () => {
        repoPanel.classList.remove('show');
        branchPanel.classList.remove('show');
        backdrop.classList.remove('show');
    }

    // Keep widths in sync
    const updatePanelWidths = () => {
        repoPanel.style.width = `${repoContainer.offsetWidth}px`;
        repoPanel.style.left = `${repoContainer.offsetLeft}px`;
        branchPanel.style.width = window.getComputedStyle(rightSidebar).width;
    }

    // Initial sync
    updatePanelWidths();
    window.addEventListener('resize', updatePanelWidths);
    
    // Toggle repos
    repoBtn.addEventListener('click', (event) => {
        event.stopPropagation();
        updatePanelWidths();
        branchPanel.classList.remove('show');

        const isOpening = repoPanel.classList.toggle('show');
        if (isOpening) backdrop.classList.add('show');
        else backdrop.classList.remove('show');
    });

    // Toggle branches
    branchBtn.addEventListener('click', (event) => {
        event.stopPropagation();
        updatePanelWidths();
        repoPanel.classList.remove('show');

        const isOpening = branchPanel.classList.toggle('show');
        if (isOpening) backdrop.classList.add('show');
        else backdrop.classList.remove('show');
    });

    // Close when clicking outside
    backdrop.addEventListener('click', closeDropdowns);
    window.addEventListener('click', closeDropdowns);

    // Prevent closing when insidie
    document.querySelectorAll('.dropdown-panel').forEach(panel => {
        panel.addEventListener('click', (event) => event.stopPropagation());
    });
});

// Split Container
resizer.addEventListener("mousedown", (event) => {
    isResizing = true;

    let rightCSS = window.getComputedStyle(rightSidebar);
    let mainCSS = window.getComputedStyle(mainContent);

    minRightWidth = parseInt(rightCSS.minWidth) || 192;
    let minMainWidth = parseInt(mainCSS.minWidth) || 360;

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
        branchDropdown.style.width = `${newWidth}px`;

        const branchPanel = document.querySelector("#branches-dropdown .dropdown-panel");
        const repoPanel = document.querySelector("#repositories-dropdown .dropdown-panel");
        const repoContainer = document.getElementById("repositories-dropdown");

        if (branchPanel) branchPanel.style.width = `${newWidth}px`;
        if (repoPanel) {
            repoPanel.style.width = `${repoContainer.offsetWidth}px`;
            repoPanel.style.left = `${repoContainer.offsetLeft}px`;
        }
    }
}

function stopResize() {
    isResizing = false;
    document.removeEventListener("mousemove", resize);
    document.removeEventListener("mouseup", stopResize);
    document.body.style.userSelect = "";
}