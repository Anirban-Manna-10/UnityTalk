
var swiper = new Swiper(".ebook-swip", {
    loop: false,
    spaceBetween: 20,
    grabCursor: true,
    slidesPerView: 'auto',
    centeredSlides: 'auto',
    pagination: {
        el: ".swiper-pagination",
        dynamicBullets: true,
        clickable: true,
    },
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    breakpoints: {
        474: {
            slidesPerView: 2,
            centeredSlides: false,
        },
        641: {
            slidesPerView: 2,
            centeredSlides: false,
        },
        650: {
            slidesPerView: 2,
            centeredSlides: false,
        },

        749: {
            slidesPerView: 2,
            centeredSlides: false,
        },
        803: {
            slidesPerView: 2,
            centeredSlides: false,
        },
        1003: {
            slidesPerView: 3,
            centeredSlides: false,
        },
        1093: {
            slidesPerView: 3,
            centeredSlides: false,
        }

    },
});