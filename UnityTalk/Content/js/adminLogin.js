document.addEventListener("DOMContentLoaded", function () {
    const inputs = document.querySelectorAll(".inp input");

    inputs.forEach(input => {
        input.addEventListener("input", function () {
            const label = this.nextElementSibling;
            label.classList.toggle("up", this.value.trim() !== "");
        });
    });
});

function focusinp(inp) {
    if (inp == 'usr') {
        document.getElementById("username").focus();
    } else if (inp == 'pass') {
        document.getElementById("password").focus();
    } else {
        document.getElementById("username").focus();
    }
}