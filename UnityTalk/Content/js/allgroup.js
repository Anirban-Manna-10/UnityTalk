console.log("hello");
let drpdownBtn = document.querySelector(".GrpdropdownText");
let list = document.querySelector(".Grpdropdown-list");
let icon = document.querySelector(".dropdownicon");
let span = document.querySelector("#dropdown-text-span")
let input = document.querySelector("#Grpsearch-input")
let listItems = document.querySelectorAll(".grp-drop-down-list-item")
let searchByinp = document.querySelector("#GrpsearchBy-input");
drpdownBtn.addEventListener("click", function () {
    if (list.classList.contains("show")) {
        icon.style.rotate = "0deg";
    } else {
        icon.style.rotate = "-180deg";
    }
    list.classList.toggle("show");
});

window.onclick = function (e) {
    if (e.target.id !== "GrpdropdownText" &&
        e.target.id !== "dropdown-text-span" &&
        e.target.id !== "dropdownicon"
    ) {
        list.classList.remove("show");
        icon.style.rotate = "0deg";
    }
}


for (item of listItems) {
    item.onclick = function (e) {
        span.innerText = e.target.innerText;
        searchByinp.value = e.target.getAttribute('data-searchby');
        input.placeholder = "Search in " + e.target.innerText + "...";
    }
}