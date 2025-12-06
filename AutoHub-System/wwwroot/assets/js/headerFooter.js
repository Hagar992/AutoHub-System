const menuIcon = document.getElementById("menu-icon");
const sidebar = document.getElementById("sidebar");
const overlay = document.getElementById("overlay");
const closeSidebar = document.getElementById("close-sidebar");
const navLinks = document.querySelectorAll("#nav a, #nav button");
const sidebarData = document.getElementById("sidebar-data");


sidebarData.innerHTML = "";
navLinks.forEach(link => {
    const cloned = link.cloneNode(true);
    cloned.addEventListener("click", () => {
        sidebar.classList.remove("open");
        overlay.classList.remove("show");
        document.body.classList.remove("sidebar-open");
    });
    sidebarData.appendChild(cloned);
});


menuIcon.addEventListener("click", () => {
    sidebar.classList.add("open");
    overlay.classList.add("show");
    document.body.classList.add("sidebar-open");
});


function closeAll() {
    sidebar.classList.remove("open");
    overlay.classList.remove("show");
    document.body.classList.remove("sidebar-open");
}

closeSidebar.addEventListener("click", closeAll);
overlay.addEventListener("click", closeAll);


document.addEventListener("DOMContentLoaded", function () {

    const profileBtn = document.getElementById("profileBtn");
    const dropdownMenu = document.getElementById("dropdownMenu");

    if (profileBtn) {
        profileBtn.addEventListener("click", function (e) {
            e.stopPropagation();
            dropdownMenu.classList.toggle("show");
            profileBtn.classList.toggle("active");
        });
    }

    // غلق القائمة عند الضغط خارجها
    document.addEventListener("click", function (e) {
        if (
            dropdownMenu &&
            profileBtn &&
            !dropdownMenu.contains(e.target) &&
            !profileBtn.contains(e.target)
        ) {
            dropdownMenu.classList.remove("show");
            profileBtn.classList.remove("active");
        }
    });
});
