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
    // SIDEBAR TOGGLE
    // ===============================
    const menuBtn = document.querySelector("#menu-btn");
    const closeBtn = document.querySelector("#close-btn");
    const sidebar = document.querySelector("aside");

    if (menuBtn) {
        menuBtn.addEventListener("click", () => {
            sidebar.classList.add("active");
        });
    }

    if (closeBtn) {
        closeBtn.addEventListener("click", () => {
            sidebar.classList.remove("active");
        });
    }

    // ===============================
    // CLOSE SIDEBAR WHEN CLICKING OUTSIDE (MOBILE)
    // ===============================
    document.addEventListener('click', (e) => {
        if (window.innerWidth <= 768) {
            if (sidebar.classList.contains('active') &&
                !sidebar.contains(e.target) &&
                e.target !== menuBtn &&
                !menuBtn.contains(e.target)) {
                sidebar.classList.remove('active');
            }
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