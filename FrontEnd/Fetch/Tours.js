document.addEventListener('DOMContentLoaded', async function () {
    try {
        console.log('Fetching tours and reviews...');

        // Fetch tours data
        const toursResponse = await fetch('https://localhost:44357/api/Tours');
        if (!toursResponse.ok) {
            throw new Error('Failed to fetch tours data');
        }
        const toursData = await toursResponse.json();
        console.log('Tours data:', toursData);

        // Fetch reviews data
        const reviewsResponse = await fetch('https://localhost:44357/api/Reviews');
        if (!reviewsResponse.ok) {
            throw new Error('Failed to fetch reviews data');
        }
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

        // Get tours array
        const tours = toursData.$values || [];

        // Filter active tours
        const activeTours = tours.filter(tour => tour.isActive === true);

        // Generate HTML for the first and second rows
        const firstRowHTML = generateRowHTML(activeTours.slice(0, 4), reviewsMap, true);
        const secondRowHTML = generateRowHTML(activeTours.slice(4, 8), reviewsMap, false);

        // Insert HTML into the page
        document.getElementById('tours-row-1').innerHTML = firstRowHTML;
        document.getElementById('tours-row-2').innerHTML = secondRowHTML;

    } catch (error) {
        console.error('Error:', error);
        document.getElementById('tours-row-1').innerHTML = `
            <div class="col-12 text-center">
                <p>Failed to load tours. Please try again later.</p>
            </div>
        `;
    }
});

function generateRowHTML(tours, reviewsMap, isFirstRow) {
    if (!tours || tours.length === 0) {
        return '';
    }

    let html = '';

    if (isFirstRow) {
        html += '<div class="col-lg-7"><div class="row">';

        // Add first two tours in 6-6 columns within the left 7-column section
        // These will have taller images
        for (let i = 0; i < Math.min(2, tours.length); i++) {
            html += generateTourItemHTML(tours[i], reviewsMap, 'col-sm-6', '84vh');
        }

        html += '</div></div>';

        // Right 5-column section
        html += '<div class="col-lg-5"><div class="row">';

        // Add next two tours with standard height
        for (let i = 2; i < Math.min(4, tours.length); i++) {
            html += generateTourItemHTML(tours[i], reviewsMap, 'col-md-6 col-xl-12', '40vh');
        }

        html += '</div></div>';
    } else {
        html += '<div class="col-lg-5"><div class="row">';

        // Add first two tours in the left 5-column section with standard height
        for (let i = 0; i < Math.min(2, tours.length); i++) {
            html += generateTourItemHTML(tours[i], reviewsMap, 'col-md-6 col-xl-12', '40vh');
        }

        html += '</div></div>';

        // Right 7-column section
        html += '<div class="col-lg-7"><div class="row">';

        // Add next two tours in 6-6 columns with taller images
        for (let i = 2; i < Math.min(4, tours.length); i++) {
            html += generateTourItemHTML(tours[i], reviewsMap, 'col-sm-6', '84vh');
        }

        html += '</div></div>';
    }

    return html;
}

function generateTourItemHTML(tour, reviewsMap, columnClass, imageHeight) {
    if (!tour) return '';

    // Get tour rating information from the reviewsMap
    const ratingInfo = reviewsMap[tour.tourId] || { sum: 0, count: 0 };
    const rating = ratingInfo.count > 0 ? ratingInfo.sum / ratingInfo.count : 0;
    const reviewCount = ratingInfo.count || 0;

    // Generate filled stars based on rating
    const filledStars = Math.round(rating);

    return `
         <div class="${columnClass}">
            <div class="desti-item overlay-desti-item">
                <figure class="desti-image">
                        <img src="https://localhost:44357/ToursImages/${tour.tourImage}" 
                            alt="${tour.tourName}" 
                            style="height: ${imageHeight}; width: 100%; object-fit: cover;">
                    </figure>
               <div class="meta-cat bg-meta-cat">
                  <a href="details.html?tourId=${tour.tourId}">${tour.location ? tour.location.toUpperCase() : 'JORDAN'}</a>
               </div>
               <div class="desti-content">
                  <h3>
                     <a href="details.html?tourId=${tour.tourId}">${tour.tourName}</a>
                  </h3>
                  <div class="review-stars">
                     ${generateStarsHTML(filledStars)}
                  </div>
               </div>
            </div>
         </div>
      `;
}

function generateStarsHTML(filledStars) {
    let starsHTML = '';

    for (let i = 0; i < 5; i++) {
        if (i < filledStars) {
            starsHTML += '<i class="fas fa-star" style="color: #F56960;"></i>';
        } else {
            starsHTML += '<i class="far fa-star" style="color: #F56960;"></i>';
        }
    }

    return starsHTML;
}