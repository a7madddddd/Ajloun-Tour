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








// packages
document.addEventListener("DOMContentLoaded", async function () {
    const toursApi = "https://localhost:44357/api/Tours";
    const toursPackagesApi = "https://localhost:44357/api/ToursPackages";
    const reviewsApi = "https://localhost:44357/api/Reviews";

    try {
        // Fetch all APIs in parallel
        const [toursResponse, toursPackagesResponse, reviewsResponse] = await Promise.all([
            fetch(toursApi),
            fetch(toursPackagesApi),
            fetch(reviewsApi)
        ]);

        // Convert responses to JSON
        const toursData = await toursResponse.json();
        const toursPackagesData = await toursPackagesResponse.json();
        const reviewsData = await reviewsResponse.json();

        // Extract relevant values
        const tours = toursData.$values;
        const toursPackages = toursPackagesData.$values.filter(pkg => pkg.isActive); // Filter active packages
        const reviews = reviewsData.$values;

        // Merge data based on tourId
        const combinedData = toursPackages.map(package => {
            const tour = tours.find(t => t.tourId === package.tourId);
            const review = reviews.find(r => r.tourId === package.tourId);

            return {
                tourId: package.tourId,
                packageId: package.packageId,
                packageName: package.packageName,
                details: package.details,
                price: package.price,
                tourDays: package.tourDays,
                tourNights: package.tourNights,
                numberOfPeople: package.numberOfPeople,
                tourImage: tour ? tour.tourImage : "default.jpg",
                location: tour ? tour.location || " Location" : " Location",
                rating: review ? review.rating : 0 // Default rating 0 if no review exists
            };
        });

        // Get the last 3 active packages
        const lastThreePackages = combinedData.slice(-3);

        // Render HTML
        renderPackages(lastThreePackages);

    } catch (error) {
        console.error("Error fetching data:", error);
    }
});

// Function to render data in HTML
function renderPackages(data) {
    const packageContainer = document.querySelector(".package-inner .row");
    packageContainer.innerHTML = ""; // Clear existing content

    data.forEach(item => {
        // Create the stars for rating
        const stars = generateStars(item.rating);

        const packageHtml = `
            <div class="col-lg-4 col-md-6">
                <div class="package-wrap">
                    <figure class="feature-image">
                        <a href="#">
                            <img src="https://localhost:44357/ToursImages/${item.tourImage}" alt="${item.packageName}">
                        </a>
                    </figure>
                    <div class="package-price">
                        <h6><span>$${item.price}</span> / per person</h6>
                    </div>
                    <div class="package-content-wrap">
                        <div class="package-meta text-center">
                            <ul>
                                <li><i class="far fa-clock"></i> ${item.tourDays}D/${item.tourNights}N</li>
                                <li><i class="fas fa-user-friends"></i> People: ${item.numberOfPeople}</li>
                                <li><i class="fas fa-map-marker-alt"></i> ${item.location}</li>
                            </ul>
                        </div>
                        <div class="package-content">
                            <h3><a href="#">${item.packageName}</a></h3>
                            <div class="review-area">
                                <span class="review-text">(${item.rating} reviews)</span>
                                <div class="rating-start" title="Rated ${item.rating} out of 5">
                                    ${stars}  <!-- Render stars dynamically -->
                                </div>
                            </div>
                            <p>${item.details}</p>
                            <div class="btn-wrap">
                                <a href="#" class="button-text width-6">Book Now<i class="fas fa-arrow-right"></i></a>
                                <a href="#" class="button-text width-6">Wish List<i class="far fa-heart"></i></a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
        packageContainer.innerHTML += packageHtml;
    });
}

// Function to generate stars based on rating
function generateStars(rating) {
    let starsHtml = '';

    for (let i = 1; i <= 5; i++) {
        if (i <= rating) {
            starsHtml += '<i class="fas fa-star" style="color: #F56960;"></i>'; 
        } else {
            starsHtml += '<i class="far fa-star" style="color: #F56960;"></i>'; // Empty star
        }
    }

    return starsHtml;
}



// offers
document.addEventListener('DOMContentLoaded', async function () {
    try {
        // Fetch offers data
        const offersResponse = await fetch('https://localhost:44357/api/Offers');
        if (!offersResponse.ok) {
            throw new Error('Failed to fetch offers data');
        }
        const offersData = await offersResponse.json();

        // Fetch tours data
        const toursResponse = await fetch('https://localhost:44357/api/Tours');
        if (!toursResponse.ok) {
            throw new Error('Failed to fetch tours data');
        }
        const toursData = await toursResponse.json();

        // Process the data
        const offers = offersData.$values || [];
        const tours = toursData.$values || [];

        // Filter active offers
        const activeOffers = offers.filter(offer => offer.isActive === true);

        // Get the last 3 active offers
        const lastThreeActiveOffers = activeOffers.slice(-3);

        // Create a map of tour id to tour data for easier lookup
        const tourMap = {};
        tours.forEach(tour => {
            tourMap[tour.tourId] = tour;
        });

        // Generate HTML for each offer
        const offerContainer = document.getElementById('offerContainer');

        lastThreeActiveOffers.forEach(offer => {
            // Find associated tour (for this example, assuming tour ID matches offer ID)
            // In a real application, you might have a tourId in the offer object
            const tour = tourMap[offer.id];

            if (tour) {
                // Calculate original and discounted prices
                const originalPrice = tour.price;
                const discountedPrice = originalPrice * (1 - (offer.discountPercentage / 100));

                // Create offer HTML
                const offerHTML = `
                            <div class="col-md-6 col-lg-4">
                                <div class="special-item">
                                    <figure class="special-img">
                                    <img src="https://localhost:44357/ToursImages/${tour.tourImage}" alt="${tour.packageName}" style="height: 60vh;">
                                    </figure>
                                    <div class="badge-dis">
                                        <span>
                                            <strong>${offer.discountPercentage}%</strong>
                                            off
                                        </span>
                                    </div>
                                    <div class="special-content">
                                        <div class="meta-cat">
                                            <a href="#">${tour.tourName || 'Ajloun'}</a>
                                        </div>
                                        <h3>
                                            <a href="#">${offer.title}</a>
                                        </h3>
                                        <div class="package-price">
                                            Price:
                                            <del>$${originalPrice.toFixed(2)}</del>
                                            <ins>$${discountedPrice.toFixed(2)}</ins>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        `;

                offerContainer.innerHTML += offerHTML;
            }
        });

        // If no active offers were found
        if (lastThreeActiveOffers.length === 0) {
            offerContainer.innerHTML = '<div class="col-12 text-center"><p>No active offers available at the moment.</p></div>';
        }

    } catch (error) {
        console.error('Error:', error);
        document.getElementById('offerContainer').innerHTML = `
                    <div class="col-12 text-center">
                        <p>Failed to load offers. Please try again later.</p>
                    </div>
                `;
    }
});





// Add an event listener to the form submission
// Add an event listener to the form submission
document.getElementById('subscribe-form').addEventListener('submit', function (event) {
    event.preventDefault(); // Prevent the default form submission

    // Get the email value
    const email = document.getElementById('email').value;

    // Prepare the data to send
    const formData = new FormData();
    formData.append('Email', email);
    formData.append('SubscribedAt', new Date().toISOString()); // Automatically set to the current date and time
    formData.append('IsActive', 'true'); // Assuming you want to set IsActive to true

    // Send the POST request using Fetch API
    fetch('https://localhost:44357/api/NewsLatters', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
        },
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            // Handle success using SweetAlert
            Swal.fire({
                title: 'Success!',
                text: 'You have successfully subscribed!',
                icon: 'success',
                confirmButtonText: 'Great!'
            }).then(() => {
                // Clear the form fields after success
                document.getElementById('subscribe-form').reset();
            });

            console.log(data); // Log the response from the server
        })
        .catch(error => {
            // Handle error using SweetAlert
            Swal.fire({
                title: 'Oops!',
                text: 'Something went wrong. Please try again.',
                icon: 'error',
                confirmButtonText: 'Try Again'
            });
            console.error(error);
        });
});