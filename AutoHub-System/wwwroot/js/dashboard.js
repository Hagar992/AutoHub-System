// ===============================
// DASHBOARD MAIN JAVASCRIPT
// ===============================
document.addEventListener("DOMContentLoaded", () => {
    // ===============================
    // THEME TOGGLER
    // ===============================
    const themeToggler = document.querySelector('.theme-toggler');
    if (themeToggler) {
        const lightIcon = themeToggler.querySelector('span:nth-child(1)');
        const darkIcon = themeToggler.querySelector('span:nth-child(2)');
        const savedTheme = localStorage.getItem('theme');

        if (savedTheme === "dark") {
            document.body.classList.add("dark-theme");
            darkIcon.classList.add("active");
            lightIcon.classList.remove("active");
        } else {
            document.body.classList.remove("dark-theme");
            lightIcon.classList.add("active");
            darkIcon.classList.remove("active");
        }

        themeToggler.addEventListener("click", () => {
            document.body.classList.toggle("dark-theme");
            const isDark = document.body.classList.contains("dark-theme");
            lightIcon.classList.toggle("active", !isDark);
            darkIcon.classList.toggle("active", isDark);
            localStorage.setItem("theme", isDark ? "dark" : "light");
        });
    }

    // ===============================
    // SIDEBAR COLLAPSE TOGGLE (FOR LARGE SCREENS)
    // ===============================
    const sidebar = document.querySelector("aside");
    const body = document.body;

    // Function to toggle sidebar collapse
    function toggleSidebarCollapse() {
        if (window.innerWidth > 768) {
            // For desktop: toggle collapsed class
            sidebar.classList.toggle('collapsed');
            body.classList.toggle('sidebar-collapsed');

            // Save state to localStorage
            localStorage.setItem('sidebarCollapsed', sidebar.classList.contains('collapsed'));

            // Update button text/icon
            const collapseToggle = document.querySelector('.collapse-toggle');
            if (collapseToggle) {
                const icon = collapseToggle.querySelector('span');
                if (icon) {
                    icon.textContent = sidebar.classList.contains('collapsed') ? 'chevron_right' : 'chevron_left';
                }
            }
        } else {
            // For mobile: toggle active class
            sidebar.classList.toggle('active');

            // Show/hide overlay
            const overlay = document.querySelector('.sidebar-overlay');
            if (overlay) {
                overlay.classList.toggle('active');
            }
        }
    }

    // Function to load sidebar state
    function loadSidebarState() {
        if (window.innerWidth > 768) {
            const isCollapsed = localStorage.getItem('sidebarCollapsed') === 'true';

            if (isCollapsed) {
                sidebar.classList.add('collapsed');
                body.classList.add('sidebar-collapsed');

                const collapseToggle = document.querySelector('.collapse-toggle');
                if (collapseToggle) {
                    const icon = collapseToggle.querySelector('span');
                    if (icon) icon.textContent = 'chevron_right';
                }
            }
        }
    }

    // Initialize sidebar collapse
    if (sidebar) {
        loadSidebarState();

        // Add collapse toggle button to sidebar
        const collapseBtn = document.createElement('button');
        collapseBtn.className = 'collapse-toggle';
        collapseBtn.innerHTML = '<span class="material-symbols-sharp">chevron_left</span>';
        collapseBtn.title = 'Collapse/Expand Sidebar';

        const sidebarContainer = document.querySelector('.sidebar');
        if (sidebarContainer) {
            sidebarContainer.appendChild(collapseBtn);

            collapseBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                toggleSidebarCollapse();
            });
        }
    }

    // ===============================
    // MENU TOGGLE BUTTON
    // ===============================
    const menuBtn = document.querySelector("#menu-btn");
    if (menuBtn) {
        menuBtn.addEventListener("click", () => {
            if (window.innerWidth <= 768) {
                // Mobile: show sidebar with overlay
                sidebar.classList.add("active");
                const overlay = document.querySelector('.sidebar-overlay');
                if (overlay) overlay.classList.add('active');
            } else {
                // Desktop: toggle collapse
                toggleSidebarCollapse();
            }
        });
    }

    // ===============================
    // CLOSE BUTTON
    // ===============================
    const closeBtn = document.querySelector("#close-btn");
    if (closeBtn) {
        closeBtn.addEventListener("click", () => {
            sidebar.classList.remove("active");
            const overlay = document.querySelector('.sidebar-overlay');
            if (overlay) overlay.classList.remove('active');
        });
    }

    // ===============================
    // CLOSE SIDEBAR WHEN CLICKING OUTSIDE (MOBILE)
    // ===============================
    document.addEventListener('click', (e) => {
        const overlay = document.querySelector('.sidebar-overlay');
        if (overlay && overlay.classList.contains('active') && e.target === overlay) {
            sidebar.classList.remove('active');
            overlay.classList.remove('active');
        }
    });

    // ===============================
    // DATE PICKER DEFAULT VALUE
    // ===============================
    const datePicker = document.querySelector("#date-picker");
    if (datePicker) {
        datePicker.valueAsDate = new Date();
    }

    // ===============================
    // CARD HOVER EFFECTS
    // ===============================
    const cards = document.querySelectorAll('.card');
    cards.forEach(card => {
        card.addEventListener('mouseenter', () => {
            if (window.innerWidth > 768) {
                card.style.transform = 'translateY(-5px)';
            }
        });

        card.addEventListener('mouseleave', () => {
            card.style.transform = 'translateY(0)';
        });
    });

    // ===============================
    // ACTIVE LINK HIGHLIGHTING
    // ===============================
    function setActiveLink() {
        const currentPath = window.location.pathname;
        const links = document.querySelectorAll('.sidebar a');

        links.forEach(link => {
            if (link.getAttribute('href') === currentPath) {
                link.classList.add('active');
            } else {
                link.classList.remove('active');
            }
        });
    }

    // ===============================
    // PROFILE IMAGE FALLBACK
    // ===============================
    const profileImages = document.querySelectorAll('.profile-photo img');
    profileImages.forEach(img => {
        img.addEventListener('error', function () {
            this.src = '/images/default-profile.png';
            this.alt = 'Default Profile';
        });
    });

    // ===============================
    // RESIZE HANDLER
    // ===============================
    window.addEventListener('resize', () => {
        if (window.innerWidth > 768) {
            sidebar.classList.remove('active');
            const overlay = document.querySelector('.sidebar-overlay');
            if (overlay) overlay.classList.remove('active');

            // Load saved collapse state
            loadSidebarState();
        } else {
            // On mobile, remove collapsed state
            sidebar.classList.remove('collapsed');
            body.classList.remove('sidebar-collapsed');
        }
    });

    // ===============================
    // INITIALIZE
    // ===============================
    setActiveLink();

    console.log('Dashboard initialized successfully');
});

// ===============================
// UTILITY FUNCTIONS
// ===============================
function formatNumber(number) {
    return new Intl.NumberFormat().format(number);
}

function formatCurrency(amount, currency = 'USD') {
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: currency
    }).format(amount);
}

// ===============================
// NOTIFICATION SYSTEM
// ===============================
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `notification ${type}`;
    notification.innerHTML = `
        <span class="material-symbols-sharp">${getNotificationIcon(type)}</span>
        <p>${message}</p>
    `;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.remove();
    }, 5000);
}

function getNotificationIcon(type) {
    const icons = {
        'success': 'check_circle',
        'error': 'error',
        'warning': 'warning',
        'info': 'info'
    };
    return icons[type] || 'info';
}