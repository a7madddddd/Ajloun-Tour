document.addEventListener("DOMContentLoaded", async function () {
    const toursPackagesApi = "https://localhost:44357/api/ToursPackages";
    const reviewsApi = "https://localhost:44357/api/Reviews";

    try {
        // Fetch data in parallel
        const [toursPackagesResponse, reviewsResponse] = await Promise.all([
            fetch(toursPackagesApi),
            fetch(reviewsApi)
        ]);

        const toursPackagesData = await toursPackagesResponse.json();
        const reviewsData = await reviewsResponse.json();

        // Extract data arrays
        const toursPackages = toursPackagesData.$values || [];
        const reviews = reviewsData.$values || [];

        // Create reviews map
        const reviewsMap = {};
        reviews.forEach(review => {
            if (!reviewsMap[review.tourId]) {
                reviewsMap[review.tourId] = { sum: 0, count: 0 };
            }
            reviewsMap[review.tourId].sum += review.rating;
            reviewsMap[review.tourId].count += 1;
        });

        // Process and combine data
        const combinedData = toursPackages
            .filter(pkg => pkg.isActive)  // Only active packages
            .map(item => {
                const tourReviews = reviewsMap[item.tourId] || { sum: 0, count: 0 };
                const avgRating = tourReviews.count > 0 ?
                    tourReviews.sum / tourReviews.count : 0;

                return {
                    tourId: item.tourId,
                    packageId: item.packageId,
                    packageName: item.packageName,
                    details: item.details,
                    price: item.price,
                    tourDays: item.tourDays,
                    tourNights: item.tourNights,
                    numberOfPeople: item.numberOfPeople,
                    image: item.image,
                    avgRating: avgRating,
                    reviewCount: tourReviews.count,
                    location: item.location
                };
            });

        renderPackages(combinedData);

    } catch (error) {
        console.error("Error fetching data:", error);
        document.querySelector(".package-inner .row").innerHTML = `
            <div class="col-12 text-center">
                <p>Failed to load tour packages. Please try again later.</p>
            </div>
        `;
    }
});

function renderPackages(data) {
    const packageContainer = document.querySelector(".package-inner .row");
    packageContainer.innerHTML = "";

    data.forEach(item => {
        const starsHtml = generateStars(Math.round(item.avgRating));
        const shortDetails = item.details.length > 100 ?
            item.details.substring(0, 100) + '...' :
            item.details;

        const packageHtml = `
            <div class="col-lg-4 col-md-6">
                <div class="package-wrap">
                    <figure class="feature-image">
                        <a href="package-detail.html?tourId=${item.tourId}&packageId=${item.packageId}">
                            <img src="https://localhost:44357/PakagesImages/${item.image}" 
                                 alt="${item.packageName}" 
                                 style="height: 40vh; object-fit: cover;">
                        </a>
                    </figure>
                    <div class="package-price">
                        <h6>
                            <span>$${item.price}</span> / per person
                        </h6>
                    </div>
                    <div class="package-content-wrap">
                        <div class="package-meta text-center">
                            <ul>
                                <li>
                                    <i class="far fa-clock"></i> 
                                    ${item.tourDays}D/${item.tourNights}N
                                </li>
                                <li>
                                    <i class="fas fa-user-friends"></i> 
                                    People: ${item.numberOfPeople}
                                </li>
                                <li>
                                    <i class="fas fa-map-marker-alt"></i> 
                                    Tour ID: ${item.location}
                                </li>
                            </ul>
                        </div>
                        <div class="package-content">
                            <h3>
                                <a href="package-detail.html?tourId=${item.tourId}&packageId=${item.packageId}">
                                    ${item.packageName}
                                </a>
                            </h3>
                            <div class="review-area">
                                <span class="review-text">
                                    (${item.reviewCount} reviews)
                                </span>
                                <div class="rating-stars">
                                    ${starsHtml}
                                </div>
                            </div>
                            <p>${shortDetails}</p>
                            <div class="btn-wrap">
                                <a href="package-detail.html?tourId=${item.tourId}&packageId=${item.packageId}" 
                                   class="button-text width-6">
                                    Book Now<i class="fas fa-arrow-right"></i>
                                </a>
                                <a href="#" class="button-text width-6">
                                    Wish List<i class="far fa-heart"></i>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
        packageContainer.innerHTML += packageHtml;
    });
}

function generateStars(rating) {
    let starsHtml = '';
    for (let i = 1; i <= 5; i++) {
        if (i <= rating) {
            starsHtml += '<i class="fas fa-star" style="color: #F56960;"></i>';
        } else {
            starsHtml += '<i class="far fa-star" style="color: #F56960;"></i>';
        }
    }
    return starsHtml;
}