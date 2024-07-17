const dropArea = document.querySelector('.UserUploadPartDiv')
const selectFile = document.querySelector('.FileBrowseBtn')
const selectFileArea = document.querySelector('.filechoseArea')

selectFile.onclick = () => selectFileArea.click()
dropArea.onchange = () => {
    [...dropArea.files].forEach((file) => {
        if (typeValidation(file.type)) {
            console.log(file);
        }
    })
}

dropArea.ondragover = (e) => {
    e.preventDefault();
    [...e.dataTransfer.items].forEach((item) => {
        console.log("Item type:", item.type);
        if (typeValidation(item.type)) {
            dropArea.classList.add('drag-over-effect')
        }
    })
}

dropArea.ondragleave = (e) => {
    dropArea.classList.remove('drag-over-effect')
}



dropArea.ondrop = (e) => {
    e.preventDefault();
    dropArea.classList.remove('drag-over-effect')
    if (e.dataTransfer.items) {
        [...e.dataTransfer.items].forEach((item) => {
            if (item.type === 'file') {
                const file = item.geAsFile();
                if (typeValidation(file.type)) {
                    uploadFile(file)
                }
            }
        })
    } else {
        [...e.dataTransfer.files].forEach((file) => {
            if (typeValidation(file.type)) {
                uploadFile(file)
            }
        })
    }
}

function typeValidation(type) {
    var splitType = type.split('/')[0]

    if (splitType == 'application/pdf' || splitType == 'image' || splitType == 'video') {
        dropArea.classList.add('drag-over-effect')
        return true;
    }
    if (splitType == 'application/zip') {
        dropArea.classList.add('drag-over-effect')
        return true;
    }

    else {
        dropArea.classList.remove('drag-over-effect')
        return false;

    }
}

document.addEventListener("DOMContentLoaded", () => {
    const searchInput = document.getElementById("FileSearchInput");
    const cardList = document.getElementById("groupFileshareCont");
    const cardItems = cardList.getElementsByClassName("groupFileshareBox");

    searchInput.addEventListener("input", function () {
        const searchTerm = this.value.toLowerCase();
        Array.from(cardItems).forEach((cardItem) => {
            const cardName = cardItem.querySelector(".GroupFileName p").innerText.toLowerCase();
            if (cardName.includes(searchTerm)) {
                cardItem.style.display = "block";
            } else {
                cardItem.style.display = "none";
            }
        });
    });
});
