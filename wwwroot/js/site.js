// Basic JavaScript to toggle the sidebar on and off.  This improves
// usability on small screens by allowing the user to hide the sidebar.
document.addEventListener('DOMContentLoaded', function () {
    const wrapper = document.getElementById('wrapper');
    const toggleButton = document.getElementById('sidebarToggle');
    if (toggleButton) {
        toggleButton.addEventListener('click', function () {
            wrapper.classList.toggle('toggled');
        });
    }
});