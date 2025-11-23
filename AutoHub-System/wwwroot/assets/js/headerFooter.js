 const menuIcon = document.getElementById("menu-icon");
  const sidebar = document.getElementById("sidebar");
  const overlay = document.getElementById("overlay");
  const closeSidebar = document.getElementById("close-sidebar");
  const navLinks = document.querySelectorAll("#nav a, #nav button");
  const sidebarData = document.getElementById("sidebar-data");

  // انسخ اللينكات اللي في الـ nav للـ sidebar
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

  // Open sidebar
  menuIcon.addEventListener("click", () => {
    sidebar.classList.add("open");
    overlay.classList.add("show");
    document.body.classList.add("sidebar-open");
  });

  // Close sidebar
  function closeAll() {
    sidebar.classList.remove("open");
    overlay.classList.remove("show");
    document.body.classList.remove("sidebar-open");
  }

  closeSidebar.addEventListener("click", closeAll);
  overlay.addEventListener("click", closeAll);