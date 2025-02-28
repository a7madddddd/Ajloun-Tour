// Get the container element
const toursContainer = document.getElementById('toursContainer');

// Function to calculate rating percentage
function calculateRatingPercentage(rating) {
    return (rating / 5) * 100;
}

// Function to fetch tours and reviews
async function fetchToursAndReviews() {
    try {
        console.log('Fetching tours and reviews...');

        // Fetch tours
        const toursResponse = await fetch('https://localhost:44357/api/Tours/LastFourToursWithRatings', {
            method: 'GET',
            headers: {
                'Accept': 'text/plain',
                'Content-Type': 'application/json'
            }
        });
        const toursData = await toursResponse.json();
        console.log('Tours data:', toursData);

        // Fetch reviews
        const reviewsResponse = await fetch('https://localhost:44357/api/Reviews', {
            method: 'GET',
            headers: {
                'Accept': 'text/plain',
                'Content-Type': 'application/json'
            }
        });
        const reviewsData = await reviewsResponse.json();
        console.log('Reviews data:', reviewsData);

        // Create a map of tourId to average rating
        const reviewsMap = {};
        if (reviewsData.$values && Array.isArray(reviewsData.$values)) {
            reviewsData.$values.forEach(review => {
                if (!reviewsMap[review.tourId]) {
                    reviewsMap[review.tourId] = { sum: 0, count: 0 };
                }
                reviewsMap[review.tourId].sum += review.rating;
                reviewsMap[review.tourId].count += 1;
            });
        }
        console.log('Reviews map:', reviewsMap);

        // Get all destination items
        const destinationItems = toursContainer.getElementsByClassName('desti-item');
        console.log(`Found ${destinationItems.length} destination items`);

        // Update each destination item
        Array.from(destinationItems).forEach((item, index) => {
            if (!toursData.$values || !Array.isArray(toursData.$values)) {
                console.error('Tours data is not in expected format');
                return;
            }

            const tour = toursData.$values[index];
            console.log(`Processing item ${index}:`, tour);

            if (tour) {
                // Find the desti-content element in this item
                const destiContent = item.querySelector('.desti-content');
                if (destiContent) {
                    // Find and update the title element inside desti-content
                    const titleElement = destiContent.querySelector('.tour-name');
                    if (titleElement) {
                        titleElement.textContent = tour.tourName; // Update the second <a> tag
                        console.log(`Updated title to: ${tour.tourName}`);
                    } else {
                        console.warn(`No title element found in desti-content for item ${index}`);
                    }
                } else {
                    console.warn(`No desti-content found for item ${index}`);
                }

                // Update destination location
                const locationElement = item.querySelector('.meta-cat a');
                if (locationElement && tour.destination) {
                    locationElement.textContent = tour.destination.toUpperCase(); // Update the first <a> tag
                    console.log(`Updated location to: ${tour.destination}`);
                }

                // Update the first <a> tag with the tour name as well
                if (locationElement) {
                    locationElement.textContent = tour.tourName; // This will set the first <a> tag to the tour name
                }

                // Update image
                const imageElement = item.querySelector('img');
                if (imageElement && tour.tourImage) {
                    imageElement.src = `https://localhost:44357/ToursImages/${tour.tourImage}`;
                    console.log(`Updated image to: ${tour.tourImage}`);
                }

                // Update rating
                const ratingElement = item.querySelector('.rating-start');
                if (ratingElement) {
                    let ratingPercentage = 0;
                    let ratingTitle = 'No ratings yet';

                    if (reviewsMap[tour.tourId] && reviewsMap[tour.tourId].count > 0) {
                        const averageRating = reviewsMap[tour.tourId].sum / reviewsMap[tour.tourId].count;
                        ratingPercentage = calculateRatingPercentage(averageRating);
                        ratingTitle = `Rated ${averageRating.toFixed(1)} out of 5`;
                    } else {
                        // Default rating if no reviews (for testing purposes)
                        ratingPercentage = 100; // 100% = 5 stars
                    }

                    console.log(`Setting rating for tour ${tour.tourId} to ${ratingPercentage}%`);

                    // Update stars based on rating
                    const stars = ratingElement.querySelectorAll('.star');
                    const filledStars = Math.round(ratingPercentage / 20); // Assuming 5 stars, each star represents 20%

                    stars.forEach((star, index) => {
                        if (index < filledStars) {
                            star.classList.add('filled');
                        } else {
                            star.classList.remove('filled');
                        }
                    });

                    // Update the parent title attribute
                    const ratingParent = ratingElement.parentElement;
                    if (ratingParent) {
                        ratingParent.setAttribute('title', ratingTitle);
                    }
                } else {
                    console.warn(`No rating element found for item ${index}`);
                }
            } else {
                console.warn(`No tour data for item at index ${index}`);
            }
        });

        // Add CSS for rating stars if it doesn't exist
        if (!document.getElementById('rating-styles')) {
            const styleElement = document.createElement('style');
            styleElement.id = 'rating-styles';
            styleElement.textContent = `
                .rating-start {
                    display: flex;
                }
                .star {
                    width: 20px; /* Adjust size as needed */
                    height: 20px; /* Adjust size as needed */
                    background-color: transparent; /* No background for unfilled stars */
                    border: 2px solid #F56960; /* Outline color for unfilled stars */
                    clip-path: polygon(50% 0%, 61% 35%, 98% 35%, 68% 57%, 79% 91%, 50% 70%, 21% 91%, 32% 57%, 2% 35%, 39% 35%); /* Star shape */
                    margin-right: 2px; /* Space between stars */
                }
                .star.filled {
                    background-color: #F56960; /* Fill color for filled stars */
                    border: 2px solid #F56960; /* Same color for border */
                }
            `;
            document.head.appendChild(styleElement);
        }

    } catch (error) {
        console.error('Error in fetchToursAndReviews:', error);
    }
}

// Add event listener to button
const moreButton = document.querySelector('.btn-wrap .button-primary');
if (moreButton) {
    moreButton.addEventListener('click', function (e) {
        e.preventDefault();
        window.location.href = "tours.html";
    });
}

// Run when the DOM is ready
document.addEventListener('DOMContentLoaded', function () {
    // Ensure we wait for everything to load
    setTimeout(fetchToursAndReviews, 500);
});