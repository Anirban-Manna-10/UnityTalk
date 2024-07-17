
    var div = document.getElementById('AnnouncementInput');
    var button = document.getElementById('AnnouncementAdd');
    var display = 0;

    function hideshow() {
        if (display == 1) {
            div.style.height = '0px';
            div.style.padding = '0px';
            div.style.marginBottom = '0px';
            display = 0;
        }
        else {
            div.style.height = '100px';
            div.style.padding = '10px';
            div.style.marginBottom = '10px';
            display = 1;
        }
    }

    let cpyBtn = document.getElementById("grpCpyBtn");

    cpyBtn.addEventListener("click", function () {
        let codeElement = document.getElementById("codeTocpy");

        let textarea = document.createElement("textarea");
        textarea.value = codeElement.textContent;

        document.body.appendChild(textarea);
        textarea.select();

        document.execCommand("copy");

        document.body.removeChild(textarea);

    });
