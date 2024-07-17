//const allStar = document.querySelectorAll('.rating-input-div .star')
//const ratingValue = document.querySelector('.rating-input-div input')

//allStar.forEach((item, idx) => {
//	item.addEventListener('click', function () {
//		let click = 0
//		ratingValue.value = idx + 1

//		allStar.forEach(i => {
//			i.classList.replace('bxs-star', 'bx-star')
//			i.classList.remove('active')
//		})
//		for (let i = 0; i < allStar.length; i++) {
//			if (i <= idx) {
//				allStar[i].classList.replace('bx-star', 'bxs-star')
//				allStar[i].classList.add('active')
//			} else {
//				allStar[i].style.setProperty('--i', click)
//				click++
//			}
//		}
//	})
//})

// Get references to the star elements and the hidden input for the rating
const allStar = document.querySelectorAll('.rating-input-div .star');
const ratingValue = document.querySelector('.rating-input-div input');

// Function to set the stars based on the given rating
function setStars(rating) {
    let click = 0;

    // Reset all stars to the default state
    allStar.forEach(i => {
        i.classList.replace('bxs-star', 'bx-star');
        i.classList.remove('active');
    });

    // Set stars as active based on the rating value
    for (let i = 0; i < allStar.length; i++) {
        if (i < rating) {
            allStar[i].classList.replace('bx-star', 'bxs-star');
            allStar[i].classList.add('active');
        } else {
            allStar[i].style.setProperty('--i', click);
            click++;
        }
    }
}

// Set the stars based on the current input value when the page loads
document.addEventListener('DOMContentLoaded', () => {
    const currentRating = parseInt(ratingValue.value, 10) || 0; // Get the current rating from the hidden input
    setStars(currentRating);
});

// Add event listeners for star clicks to update the rating
allStar.forEach((item, idx) => {
    item.addEventListener('click', function () {
        const newRating = idx + 1; // Update the rating value
        ratingValue.value = newRating;

        setStars(newRating); // Update the star display
    });
});
