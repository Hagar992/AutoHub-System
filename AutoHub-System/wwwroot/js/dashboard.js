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
            darkIcon?.classList.add("active");
            lightIcon?.classList.remove("active");
        } else {
            document.body.classList.remove("dark-theme");
            lightIcon?.classList.add("active"); 
            darkIcon?.classList.remove("active");
        }

        themeToggler.addEventListener("click", () => {
            document.body.classList.toggle("dark-theme");

            const isDark = document.body.classList.contains("dark-theme");
            lightIcon?.classList.toggle("active", !isDark);
            darkIcon?.classList.toggle("active", isDark);

            localStorage.setItem("theme", isDark ? "dark" : "light");
        });
    }



    // ===============================
    // SIDEBAR TOGGLE
    // ===============================
    const menuBtn = document.querySelector("#menu-btn");
    const closeBtn = document.querySelector("#close-btn");
    const sidebar = document.querySelector("aside");

    menuBtn?.addEventListener("click", () => sidebar?.classList.add("active"));
    closeBtn?.addEventListener("click", () => sidebar?.classList.remove("active"));



    // ===============================
    // DATE PICKER DEFAULT VALUE
    // ===============================
    const datePicker = document.querySelector("#date-picker");
    if (datePicker) {
        datePicker.valueAsDate = new Date();
    }



    // ===============================
    // JQUERY FORM VALIDATION
    // ===============================
    if (window.jQuery && $.fn.validate) {
        $("form").each(function () {
            $(this).validate({
                errorClass: "text-danger",
                errorPlacement: function (error, element) {
                    error.insertAfter(element);
                }
            });
        });
    }

});
