
   document.addEventListener('DOMContentLoaded', async function() {
      try {
         // Fetch tours data
         const toursResponse = await fetch('https://localhost:44357/api/Tours');
         if (!toursResponse.ok) {
            throw new Error('Failed to fetch tours data');
         }
         const toursData = await toursResponse.json();
         
         // Fetch reviews data
         const reviewsResponse = await fetch('https://localhost:44357/api/Reviews');
         if (!reviewsResponse.ok) {
            throw new Error('Failed to fetch reviews data');
         }
         const reviewsData = await reviewsResponse.json();
         
         // Get tours array
         const tours = toursData.$values || [];
         
         // Get reviews array
         const reviews = reviewsData.$values || [];
         
         // Calculate average ratings for each tour
         const tourRatings = calculateTourRatings(reviews);
         
         // Filter active tours
         const activeTours = tours.filter(tour => tour.isActive === true);
         
         // Create HTML for each row
         const firstRowHTML = generateRowHTML(activeTours.slice(0, 4), tourRatings, true);
         const secondRowHTML = generateRowHTML(activeTours.slice(4, 8), tourRatings, false);
         
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

   /**
    * Calculates average ratings for each tour from reviews
    * @param {Array} reviews - The reviews data
        * @returns {Object} Object with tourId as keys and rating info as values
        */
        function calculateTourRatings(reviews) {
      const tourRatings = { };

      // Group reviews by tourId
      reviews.forEach(review => {
         if (!review.tourId) return;

        if (!tourRatings[review.tourId]) {
            tourRatings[review.tourId] = {
                totalRating: 0,
                count: 0
            };
         }

        tourRatings[review.tourId].totalRating += review.rating;
        tourRatings[review.tourId].count += 1;
      });

      // Calculate average rating for each tour
      Object.keys(tourRatings).forEach(tourId => {
         const ratingInfo = tourRatings[tourId];
        ratingInfo.averageRating = ratingInfo.totalRating / ratingInfo.count;
      });

        return tourRatings;
   }

   /**
    * Generates HTML for a row of tours
    * @param {Array} tours - The tours to display
        * @param {Object} tourRatings - The ratings for each tour
        * @param {boolean} isFirstRow - Whether this is the first row (affects layout)
        * @returns {string} HTML string
        */
        function generateRowHTML(tours, tourRatings, isFirstRow) {
      if (!tours || tours.length === 0) {
         return '';
      }

        let html = '';

        if (isFirstRow) {
            // First row layout: 7-5 column split
            html += '<div class="col-lg-7"><div class="row">';

        // Add first two tours in 6-6 columns within the left 7-column section
        for (let i = 0; i < Math.min(2, tours.length); i++) {
            html += generateTourItemHTML(tours[i], tourRatings, 'col-sm-6');
         }

        html += '</div></div > ';

// Right 5-column section
html += '<div class="col-lg-5"><div class="row">';

// Add next two tours
for (let i = 2; i < Math.min(4, tours.length); i++) {
    html += generateTourItemHTML(tours[i], tourRatings, 'col-md-6 col-xl-12');
}

html += '</div></div>';
      } else {
    // Second row layout: 5-7 column split (reverse of first row)
    html += '<div class="col-lg-5"><div class="row">';

    // Add first two tours in the left 5-column section
    for (let i = 0; i < Math.min(2, tours.length); i++) {
        html += generateTourItemHTML(tours[i], tourRatings, 'col-md-6 col-xl-12');
    }

    html += '</div></div>';

    // Right 7-column section
    html += '<div class="col-lg-7"><div class="row">';

    // Add next two tours in 6-6 columns
    for (let i = 2; i < Math.min(4, tours.length); i++) {
        html += generateTourItemHTML(tours[i], tourRatings, 'col-sm-6');
    }

    html += '</div></div>';
}

return html;
   }

/**
 * Generates HTML for a single tour item
 * @param {Object} tour - The tour data
 * @param {Object} tourRatings - The ratings for each tour
 * @param {string} columnClass - The CSS class for the column
 * @returns {string} HTML string
 */
function generateTourItemHTML(tour, tourRatings, columnClass) {
    if (!tour) return '';

    // Get tour rating information
    const ratingInfo = tourRatings[tour.tourId] || { averageRating: 0, count: 0 };
    const rating = ratingInfo.averageRating || 0;
    const reviewCount = ratingInfo.count || 0;

    // Calculate rating width for the stars display
    const ratingPercentage = (rating / 5) * 100;

    return `
         <div class="${columnClass}">
            <div class="desti-item overlay-desti-item">
               <figure class="desti-image">
                  <img src="https://localhost:44357/ToursImages/${tour.tourImage}" alt="${tour.tourName}" style="height: 60vh;">
               </figure>
               <div class="meta-cat bg-meta-cat">
                  <a href="#">${tour.location ? tour.location.toUpperCase() : 'JORDAN'}</a>
               </div>
               <div class="desti-content">
                  <h3>
                     <a href="#">${tour.tourName}</a>
                  </h3>
                  <div class="review-area">
                     <span class="review-text">(${reviewCount} reviews)</span>
                     <div class="rating-start" title="Rated ${rating.toFixed(1)} out of 5">
                        <span style="width: ${ratingPercentage}%"></span>
                     </div>
                  </div>
               </div>
            </div>
         </div>
      `;
}
