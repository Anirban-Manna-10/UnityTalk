const filterButtons = document.querySelectorAll(".filter-button");
const imageContainer = document.getElementById("image-container");

filterButtons.forEach((button) => {
    button.addEventListener("click", () => {
        const filterName = button.getAttribute("data-filter");
        const imageCards = imageContainer.getElementsByClassName("image-card");

        for (let card of imageCards) {
            const imageName = card.getAttribute("data-name");

            if (filterName === "All" || imageName === filterName) {
                card.style.display = "block";
            } else {
                card.style.display = "none";
            }
        }
    });
});

var div = document.getElementById('dropdown-content');
var display = 0;
function hideshow() {
    if (display == 1) {
        div.style.height = '0px';

        display = 0;
    }
    else {
        div.style.height = '300px';

        display = 1;
    }
}

const imageContainers = document.querySelectorAll("#dropdown-content .BannerImgThemeDiv");
const hidderBanner = document.querySelector("#BannerImgId");
const groupBannerImg = document.querySelector(".groupBannerImg");

imageContainers.forEach(function (div) {
    div.addEventListener("click", function () {
        const imageNo = div.dataset.imageNo;
        hidderBanner.value = imageNo;
        console.log(hidderBanner.value)
        const img = div.querySelector(".BannerImgDiv img");
        if (img) {
            groupBannerImg.style.backgroundImage = "url(" + img.src + ")";
            console.log("Selected image link:", img.src);
        }
        hideshow();
    });
});