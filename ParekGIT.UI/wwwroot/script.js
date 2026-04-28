const topbar = document.getElementById("topbar");
const branchesDropdown = document.getElementById("branches-dropdown");

const leftSidebar = document.getElementById("left-sidebar");

const mainContent = document.getElementById("main-content");

const rightSidebar = document.getElementById("right-sidebar");
const resizer = document.getElementById("resizer");

let minRightWidth, maxRightWidth;
let isResizing = false;

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
        branchesDropdown.style.width = `${newWidth}px`;
    }
}

function stopResize() {
    isResizing = false;
    document.removeEventListener("mousemove", resize);
    document.removeEventListener("mouseup", stopResize);
    document.body.style.userSelect = "";
}