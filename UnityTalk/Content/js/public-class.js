var Newswiper = new Swiper('.card-slider', {
    loop: false,
    slidesPerView: 1,
    grabCursor: true,
    spaceBetween: 20,
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
        240: {
            slidesPerView: 1,
            spaceBetween: 20,
        },
        476: {
            slidesPerView: 2,
            spaceBetween: 10,
        },
        582: {
            slidesPerView: 2,
            spaceBetween: 20,
        },
        641: {
            slidesPerView: 2,
            spaceBetween: 10,
        },
        756: {
            slidesPerView: 2,
            spaceBetween: 20,
        },

        778: {
            slidesPerView: 2,
            spaceBetween: 30,
        },
        907: {
            slidesPerView: 2,
            spaceBetween: 10,
        },
        967: {
            slidesPerView: 2,
            spaceBetween: 20,
        },
        1000: {
            slidesPerView: 3,
            spaceBetween: 20,
        },
        1205: {
            slidesPerView: 3,
            spaceBetween: 30,
        },
        1225: {
            slidesPerView: 3,
            spaceBetween: 30,
        },
        1425: {
            slidesPerView: 3,
            spaceBetween: 30,
        },
        1640: {
            slidesPerView: 3,
            spaceBetween: 30,
        }
    },
});