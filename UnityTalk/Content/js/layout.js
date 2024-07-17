
    const body = document.querySelector("body");
    const sidebar = body.querySelector("nav.sidebar");
    const toggle = body.querySelector(".toggle");
    const modeSwitch = body.querySelector(".toggle-switch");
    let hamburgerBtn = document.querySelector(".hamburger input");

    /*
        SideBar
    */
// Initialize tooltips when the page loads if navbar is initially closed
window.onload = function () {
    if (sidebar.classList.contains("close")) {
        initializeTooltips();
    }
};

// Function to save sidebar state to local storage
function saveSidebarState(isClosed) {
    localStorage.setItem('sidebarState', isClosed ? 'closed' : 'open');
}

// Function to retrieve sidebar state from local storage
function getSidebarState() {
    return localStorage.getItem('sidebarState');
}

// Initialize sidebar state based on local storage
const savedSidebarState = getSidebarState();
if (savedSidebarState === 'closed') {
    sidebar.classList.add('close');
} else {
    sidebar.classList.remove('close');
}

// Add event listener for toggle button
toggle.addEventListener('click', () => {
    sidebar.classList.toggle('close');

    // Save sidebar state to local storage
    saveSidebarState(sidebar.classList.contains('close'));

    if (sidebar.classList.contains('close')) {
        initializeTooltips();
        hamburgerBtn.checked = false;
        sidebar.classList.remove('smallopen');
        hamburgerBtn.parentElement.classList.remove('opened');
    } else {
        destroyTooltips();
    }
});

function initializeTooltips() {
    var tooltipTriggerList = [].slice.call(
        document.querySelectorAll('[data-bs-toggle="tooltip"]')
    );
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

function destroyTooltips() {
    var tooltipTriggerList = [].slice.call(
        document.querySelectorAll('[data-bs-toggle="tooltip"]')
    );
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        var tooltipInstance = bootstrap.Tooltip.getInstance(tooltipTriggerEl);
        if (tooltipInstance) {
            tooltipInstance.dispose();
        }
    });
}


hamburgerBtn.addEventListener("click", function () {
    const sidebar = document.querySelector("nav.sidebar");
    const bodyk = document.querySelector("body");
    const home = document.querySelector(".home");

    if (hamburgerBtn.checked) {
        sidebar.classList.remove("close");
        sidebar.classList.toggle("smallopen");
        hamburgerBtn.parentElement.classList.toggle("opened");
        destroyTooltips()
    } else {
        sidebar.classList.remove("close");
        sidebar.classList.toggle("smallopen");
        hamburgerBtn.parentElement.classList.toggle("opened");
        destroyTooltips()
    }
});
/*
    DArk Mode or Theme Change 
*/

    modeSwitch.addEventListener("click", () => {
        body.classList.toggle("dark");
        localStorage.setItem("modeTheme", body.classList.contains("dark"));
    });

    body.classList.toggle("dark", localStorage.getItem("modeTheme") === 'true');



    const joinDropdown = document.querySelector(".joinDropDown");
    const joinContent = document.querySelector(".joinBtn");

    joinDropdown.addEventListener("click", () => {
        joinContent.classList.toggle("expanded");
    });

    const currentDate = new Date();
    const months = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    const currentMonth = months[currentDate.getMonth()];
    const daysOfWeek = [
        "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ];
    const currentDayOfWeek = daysOfWeek[currentDate.getDay()];
    const dayOfMonth = currentDate.getDate();
    const currentYear = currentDate.getFullYear();

    const dateContent = document.querySelector(".date");
    dateContent.innerHTML = `${currentDayOfWeek}, <span>${currentMonth} ${dayOfMonth}, ${currentYear}`;

    const navLinksContainer = document.querySelector('.menu-links');

    navLinksContainer.addEventListener("click", function (event) {
        // Check if the clicked element or its ancestor has the .nav-link class
        const navLink = event.target.closest('.nav-link');

        if (navLink) {
            // Remove 'active' class from all nav links
            navLinksContainer.querySelectorAll('.nav-link').forEach(link => link.classList.remove('active'));
            // Add 'active' class to the clicked nav link
            navLink.classList.add("active");
        }
    });


toast = document.querySelector(".Successtoast")
closeIcon = document.querySelector(".toastclose"),
    progress = document.querySelector(".progress");

let timer1, timer2;

document.addEventListener("DOMContentLoaded", function () {
    toast.classList.add("toastactive");

    progress.classList.add("toastactive");

    timer1 = setTimeout(() => {
        toast.classList.remove("toastactive");
    }, 5000); //1s = 1000 milliseconds

    timer2 = setTimeout(() => {
        progress.classList.remove("toastactive");
    }, 5100);

    timer3 = setTimeout(() => {
        toast.style.transform = "scale(0)";
    }, 5100);
});

closeIcon.addEventListener("click", () => {
    toast.classList.remove("toastactive");

    setTimeout(() => {
        progress.classList.remove("toastactive");
        
    }, 300);

    clearTimeout(timer1);
    clearTimeout(timer2);
});