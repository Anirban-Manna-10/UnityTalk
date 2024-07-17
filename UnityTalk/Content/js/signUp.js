const inputs = document.querySelectorAll(".contact-input");

inputs.forEach(ipt => {
    ipt.addEventListener("focus", () => {
        ipt.parentNode.classList.add("focus");
        ipt.parentNode.classList.add("not-empty");
    });

    ipt.addEventListener("blur", () => {
        if (ipt.value == "") {
            ipt.parentNode.classList.remove("not-empty");
        }
        ipt.parentNode.classList.remove("focus");
    });
});


toast = document.querySelector(".Successtoast")
closeIcon = document.querySelector(".toastclose"),
    progress = document.querySelector(".progress");

let timer1, timer2;

document.addEventListener("DOMContentLoaded", function () {
    toast.classList.add("toastactive");

    progress.classList.add("toastactive");
    console.log("yo on au");

    timer1 = setTimeout(() => {
        toast.classList.remove("toastactive");
    }, 5000);

    timer2 = setTimeout(() => {
        progress.classList.remove("toastactive");
        console.log("yo off au");
    }, 5300);
});

closeIcon.addEventListener("click", () => {
    toast.classList.remove("toastactive");

    setTimeout(() => {
        progress.classList.remove("toastactive");
        console.log("yo off");
    }, 300);

    clearTimeout(timer1);
    clearTimeout(timer2);
});