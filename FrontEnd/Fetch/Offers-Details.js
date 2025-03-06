// Add this JavaScript code to your page
document.addEventListener('DOMContentLoaded', function () {
    // Get URL parameters
    const urlParams = new URLSearchParams(window.location.search);
    const offerId = urlParams.get('offerId');
    const tourId = urlParams.get('tourId');


});

// program
// Add this to your existing fetch code or create a new function
function fetchProgramData(offerId) {
    fetch(`https://localhost:44357/api/OffersPrograms/offer/${offerId}/Withprograms`, {
        method: 'GET',
        headers: {
            'accept': 'text/plain'
        }
    })
        .then(response => response.json())
        .then(data => {
            // Update the program section
            const programTab = document.getElementById('program');

            // Update the program title and days count
            const programTitle = programTab.querySelector('h3');
            const daysCount = data.programs.$values.length;
            programTitle.innerHTML = `${data.title} <span>( ${daysCount} days )</span>`;

            // Create the timeline items
            const timelineList = programTab.querySelector('.itinerary-timeline-wrap ul');
            timelineList.innerHTML = ''; // Clear existing content

            // Sort programs by dayNumber if needed
            const sortedPrograms = data.programs.$values.sort((a, b) => a.dayNumber - b.dayNumber);

            // Add each program to the timeline
            sortedPrograms.forEach(program => {
                const programDate = new Date(program.programDate);
                const formattedDate = programDate.toLocaleDateString();

                const timelineItem = `
                <li>
                    <div class="timeline-content">
                        <div class="day-count">Day <span>${program.dayNumber}</span></div>
                        <h4>${program.customTitle}</h4>
                        <p>${program.customDescription}</p>
                        <small>Date: ${formattedDate}</small>
                    </div>
                </li>
            `;
                timelineList.innerHTML += timelineItem;
            });
        })
        .catch(error => {
            console.error('Error fetching program details:', error);
            const programTab = document.getElementById('program');
            programTab.innerHTML = '<div class="error">Failed to load program details</div>';
        });
}

// Modify your existing DOMContentLoaded event listener
document.addEventListener('DOMContentLoaded', function () {
    // Get the URL parameters
    const urlParams = new URLSearchParams(window.location.search);
    const offerId = urlParams.get('offerId');
    const tourId = urlParams.get('tourId');
    // Fetch the tour offer details
    fetch(`https://localhost:44357/api/ToursOffers/id?tourId=${tourId}&offerId=${offerId}`, {
        method: 'GET',
        headers: {
            'accept': 'text/plain'
        }
    })
        .then(response => response.json())
        .then(data => {
            // Update the HTML with the fetched data
            const tourInner = document.querySelector('.single-tour-inner');

            // Update the title
            tourInner.querySelector('h2').textContent = data.tourName;

            // Update the image
            tourInner.querySelector('.feature-image img').src = `https://localhost:44357/OffersImages/${data.image}`;
            // Note: Adjust the image path according to your server setup

            // Update the package meta information
            const metaList = tourInner.querySelector('.package-meta ul');
            metaList.innerHTML = `
            <li>
                <i class="far fa-clock"></i>
            ${calculateTimeDifference(data.startDate, data.endDate)}
            </li>
            <li>
                <i class="fas fa-user-friends"></i>
                People: ${data.peapole}
            </li>
            <li>
                <i class="fas fa-tag"></i>
                Price: $${data.price}
            </li>
            <li>
                <i class="fas fa-percentage"></i>
                Discount: ${data.discountPercentage}%
            </li>
        `;

            let price3 = document.getElementById('offer-price');
            price3.innerHTML = `$${data.price}`;
            
            let price2 = document.getElementById('.tour-price');
            price2.value = `$${data.price} for this Offer`;
            let Map = document.getElementById('tour-map');
            Map.src = data.map;

            // Add additional details if needed
            if (data.description) {
                let description = document.getElementById('tour-description');

                // If the element doesn't exist, create it
                if (!description) {
                    description = document.createElement('p');
                    description.id = 'tour-description';
                    tourInner.appendChild(description);
                }

                description.className = 'tour-description';
                description.textContent = data.description;
            }

        })
        .catch(error => {
            console.error('Error fetching tour offer details:', error);
            // Handle error - display message to user
            const tourInner = document.querySelector('.single-tour-inner');
            tourInner.innerHTML = '<p>Error loading tour details. Please try again later.</p>';
        });

    function calculateTimeDifference(startDate, endDate) {
        const start = new Date(startDate);
        const end = new Date(endDate);

        if (isNaN(start) || isNaN(end)) {
            return "(Invalid date)";
        }

        // Calculate the difference in milliseconds
        const diffTime = Math.abs(end - start);

        // Convert milliseconds to hours
        const totalHours = Math.floor(diffTime / (1000 * 60 * 60));
        const days = Math.floor(totalHours / 24);
        const hours = totalHours % 24;

        if (days >= 1) {
            return `(${days} day${days > 1 ? 's' : ''} ${hours} hours)`;
        } else {
            return `(${hours} hours)`;
        }
    }

    // Fetch program details
    fetchProgramData(offerId);
});










// Function to fetch user data
async function fetchUserData(userId) {
    try {
        const response = await fetch(`https://localhost:44357/api/Users/id?id=${userId}`, {
            method: 'GET',
            headers: {
                'accept': 'text/plain'
            }
        });
        return await response.json();
    } catch (error) {
        console.error('Error fetching user data:', error);
        return null;
    }
}

// Function to generate stars
function generateStars(rating) {
    let starsHtml = '';
    for (let i = 1; i <= 5; i++) {
        const starClass = i <= rating ? 'star filled' : 'star';
        starsHtml += `<span class="${starClass}">★</span>`;
    }
    return starsHtml;
}

// Function to calculate average rating
function calculateAverageRating(reviews) {
    if (!reviews || reviews.length === 0) return 0;
    const sum = reviews.reduce((acc, review) => acc + review.rating, 0);
    return (sum / reviews.length).toFixed(1);
}

// Function to get rating text based on average
function getRatingText(average) {
    if (average >= 4.5) return 'Excellent';
    if (average >= 4.0) return 'Very Good';
    if (average >= 3.0) return 'Good';
    if (average >= 2.0) return 'Fair';
    return 'Poor';
}
async function fetchReviews(offerId) {
    try {
        const response = await fetch(`https://localhost:44357/api/Reviews/getReviewByOfferId?offerId=${offerId}`, {
            method: 'GET',
            headers: {
                'accept': 'text/plain'
            }
        });

        const data = await response.json();
        const reviews = data.$values;
        const averageRating = calculateAverageRating(reviews);

        // Update only the reviews section
        const summaryReview = document.querySelector('.summary-review');
        const summaryReview2 = document.getElementById('.rating-star');
        summaryReview2.innerHTML = `<span style="width: 60%;" id="stars2">${generateStars(averageRating)}</span>`;
        summaryReview.innerHTML = `
            <div class="review-score">
                <span>${averageRating}</span>
            </div>
            <div class="rating-details">
                <div class="rating-stars">
                    ${generateStars(averageRating)}
                </div>
                <h3>Excellent <span>( Based on ${reviews.length} reviews )</span></h3>
                <p>Our customers have shared their experiences, providing valuable insights into our service.</p>
            </div>
        `;

        // Update the reviews list
        const reviewsList = document.querySelector('.comment-area-inner ol');
        reviewsList.innerHTML = ''; // Clear existing reviews

        // Update review count
        document.querySelector('.comment-title').textContent = `${reviews.length} Reviews`;

        // Add each review
        for (const review of reviews) {
            const userData = await fetchUserData(review.userId);
            const reviewDate = new Date(review.createdAt).toLocaleDateString('en-US', {
                month: 'numeric',
                day: 'numeric',
                year: 'numeric'
            });

            const reviewItem = document.createElement('li');
            reviewItem.innerHTML = `
                <figure class="comment-thumb">
                    <img src="https://localhost:44357/UsersImages/${userData.userImage}" 
                         alt="${userData.fullName}"
                         onerror="this.src='assets/images/default-user.jpg'">
                </figure>
                <div class="comment-content">
                    <div class="comment-header">
                        <h5 class="author-name">${userData.fullName}</h5>
                        <span class="post-on">${reviewDate}</span>
                        <div class="rating-wrap">
                            <div class="stars">
                                ${generateStars(review.rating)}
                            </div>
                        </div>
                    </div>
                    <p>${review.comment}</p>
                </div>
            `;
            reviewsList.appendChild(reviewItem);
        }

    } catch (error) {
        console.error('Error:', error);
        const commentArea = document.querySelector('.comment-area-inner');
        commentArea.innerHTML = '<div class="error">Failed to load reviews</div>';
    }
}

// Update the styles to match your design
const styles = `
    .summary-review {
        background: #f8f9fa;
        padding: 30px;
        border-radius: 8px;
        display: flex;
        align-items: flex-start;
        margin-bottom: 30px;
    }

    .review-score {
        font-size: 48px;
        font-weight: bold;
        color: black;
        margin-right: 20px;
        display: flex;
        align-items: center;
    }

    .review-score span {
        background-color: #fff;
        font-family: "Raleway", sans-serif;
        border-radius: 50%;
        width: 90px;
        height: 90px;
        margin: auto;
        font-weight: 700;
        font-size: 40px;
        line-height: 88px;
        text-align: center;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .rating-details {
        margin-left: 30px;
    }

    .rating-stars {
        margin-bottom: 10px;
    }

    .rating-stars .star {
        color: #ff6b6b;
        font-size: 24px;
        margin-right: 2px;
    }
        .stars .star {

        color: #ff6b6b;
        font-size: 24px;
        margin-right: 2px;
        }

    .star.empty {
        color: #ddd;
    }

    .comment-area {
        margin-top: 30px;
    }

    .comment-thumb img {
        width: 60px;
        height: 60px;
        border-radius: 50%;
        object-fit: cover;
    }

    .comment-content {
        margin-left: 20px;
    }

    .comment-header {
        display: flex;
        align-items: center;
        gap: 15px;
        margin-bottom: 10px;
    }

    .author-name {
        margin: 0;
        color: #333;
    }

    .post-on {
        color: #666;
    }

    .comment-area-inner ol {
        list-style: none;
        padding: 0;
    }

    .comment-area-inner li {
        display: flex;
        padding: 20px 0;
        border-bottom: 1px solid #eee;
    }

    .error {
        text-align: center;
        padding: 20px;
        color: #dc3545;
        background: #f8d7da;
        border-radius: 4px;
    }
`;

// Add styles to document
const styleSheet = document.createElement("style");
styleSheet.innerText = styles;
document.head.appendChild(styleSheet);
// Initialize when document is ready
document.addEventListener('DOMContentLoaded', function () {
    const urlParams = new URLSearchParams(window.location.search);
    const offerId = urlParams.get('offerId');

    if (offerId) {
        fetchReviews(offerId);
    }
});






























// Star rating functionality
let selectedRating = 0;
const stars = document.querySelectorAll('.star-rating i');

// Function to get user info from token
function getUserInfoFromToken() {
    const token = sessionStorage.getItem('token');
    if (!token) return null;

    try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        return {
            id: payload.nameid,
            fullName: payload.unique_name,
            email: payload.email
        };
    } catch (error) {
        console.error('Error parsing token:', error);
        return null;
    }
}

// Star rating event listeners
stars.forEach((star, index) => {
    star.addEventListener('click', () => {
        selectedRating = index + 1;
        updateStars();
    });

    star.addEventListener('mouseover', () => {
        highlightStars(index + 1);
    });

    star.addEventListener('mouseout', () => {
        highlightStars(selectedRating);
    });
});

function updateStars() {
    highlightStars(selectedRating);
}

function highlightStars(count) {
    stars.forEach((star, index) => {
        star.classList.toggle('active', index < count);
    });
}

// Initialize form when page loads
document.addEventListener('DOMContentLoaded', function () {
    const userInfo = getUserInfoFromToken();

    if (userInfo) {
        document.querySelector('input[name="fullName"]').value = userInfo.fullName;
        document.querySelector('input[name="email"]').value = userInfo.email;
    }

    // Handle form submission
    document.querySelector('.comment-form').addEventListener('submit', handleReviewSubmission);
});

// Handle review submission
async function handleReviewSubmission(e) {
    e.preventDefault();

    if (!sessionStorage.getItem('token')) {
        Swal.fire({
            title: 'Please Log In',
            text: 'You need to be logged in to submit a review',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Log In',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = 'login.html';
            }
        });
        return;
    }

    if (!selectedRating) {
        Swal.fire({
            title: 'Rating Required',
            text: 'Please select a rating before submitting',
            icon: 'warning'
        });
        return;
    }

    const userInfo = getUserInfoFromToken();
    const urlParams = new URLSearchParams(window.location.search);
    const offerId = urlParams.get('offerId');

    const formData = new FormData();
    formData.append('UserId', userInfo.id);
    formData.append('OfferId', offerId);
    formData.append('Rating', selectedRating);
    formData.append('Subject', this.querySelector('input[name="subject"]').value);
    formData.append('Comment', this.querySelector('textarea[name="comment"]').value);
    formData.append('IsActive', 'false');
    formData.append('CreatedAt', new Date().toISOString());

    try {
        const response = await fetch('https://localhost:44357/api/Reviews', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${sessionStorage.getItem('token')}`
            },
            body: formData
        });

        if (response.ok) {
            Swal.fire({
                title: 'Success!',
                text: 'Your review has been submitted successfully',
                icon: 'success'
            }).then(() => {
                // Reset form
                this.querySelector('input[name="subject"]').value = '';
                this.querySelector('textarea[name="comment"]').value = '';
                selectedRating = 0;
                updateStars();
                // Refresh reviews if you have a function for that
                if (typeof fetchReviews === 'function') {
                    fetchReviews(offerId);
                }
            });
        } else {
            throw new Error('Failed to submit review');
        }
    } catch (error) {
        console.error('Error submitting review:', error);
        Swal.fire({
            title: 'Error',
            text: 'Failed to submit your review. Please try again.',
            icon: 'error'
        });
    }
}





// bookings 
document.addEventListener('DOMContentLoaded', function () {
    // Get user info from token and fill form
    const userInfo = getUserInfoFromToken();
    if (userInfo) {
        document.querySelector('input[name="name_booking"]').value = userInfo.fullName;
        document.querySelector('input[name="email_booking"]').value = userInfo.email;
    }

    // Add form submit event listener
    const bookingForm = document.querySelector('.booking-form');
    if (bookingForm) {
        bookingForm.addEventListener('submit', function (e) {
            e.preventDefault(); // Prevent form from submitting normally
            handleBookingSubmission(e);
        });
    }
});

function getUserInfoFromToken() {
    const token = sessionStorage.getItem('token');
    if (!token) return null;

    try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        return {
            id: payload.nameid,
            fullName: payload.unique_name,
            email: payload.email
        };
    } catch (error) {
        console.error('Error parsing token:', error);
        return null;
    }
}

async function handleBookingSubmission(e) {
    e.preventDefault();

    if (!sessionStorage.getItem('token')) {
        Swal.fire({
            title: 'Login Required',
            text: 'Please login to make a booking',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Login Now',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                sessionStorage.setItem('redirectAfterLogin', window.location.href);
                window.location.href = 'login.html';
            }
        });
        return;
    }

    const bookingDate = document.querySelector('.input-date-picker').value;
    if (!bookingDate) {
        Swal.fire({
            title: 'Date Required',
            text: 'Please select a booking date',
            icon: 'warning'
        });
        return;
    }

    const userInfo = getUserInfoFromToken();
    const urlParams = new URLSearchParams(window.location.search);
    const offerId = urlParams.get('offerId');

    // Get the offer details first to access numberOfPeople and price
    try {
        const offerResponse = await fetch(`https://localhost:44357/api/ToursOffers/id?tourId=${urlParams.get('tourId')}&offerId=${offerId}`, {
            method: 'GET',
            headers: {
                'accept': 'text/plain'
            }
        });

        if (!offerResponse.ok) {
            throw new Error('Failed to fetch offer details');
        }

        
        const offerData = await offerResponse.json();
        
        const formData = new FormData();
        formData.append('UserId', userInfo.id);
        formData.append('OfferId', offerId);
        formData.append('BookingDate', new Date(bookingDate).toISOString());
        formData.append('CreatedAt', new Date().toISOString());
        formData.append('NumberOfPeople', offerData.peapole.toString());
        formData.append('TotalPrice', offerData.price.toString());
        formData.append('Status', 'Pending');


        

        const response = await fetch('https://localhost:44357/api/Bookings/By Offerid', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${sessionStorage.getItem('token')}`
            },
            body: formData
        });

        if (response.ok) {
            const bookingData = await response.json();


            Swal.fire({
                title: 'Booking Successful!',
                html: `
                    Your booking has been confirmed.<br><br>
                    <b>Booking Details:</b><br>
                    Date: ${new Date(bookingData.bookingDate).toLocaleDateString()}<br>
                    Number of People: ${bookingData.numberOfPeople}<br>
                    Total Price: $${bookingData.totalPrice}<br>
                `,
                icon: 'success'
            }).then(() => {
                // Reset date picker
                document.querySelector('.input-date-picker').value = '';
            });
        } else {
            throw new Error('Failed to create booking');
        }
    } catch (error) {
        console.error('Error:', error);
        Swal.fire({
            title: 'Error',
            text: 'Failed to create your booking. Please try again.',
            icon: 'error'
        });
    }
}






















// <div class="col-sm-12">
//     <h4 class="">Add Options</h4>
// </div>
// <div class="col-sm-6">
//     <div class="form-group">
//         <label class="checkbox-list">
//             <input type="checkbox" name="s">
//             <span class="custom-checkbox"></span>
//             Tour guide
//         </label>
//     </div>
// </div>
// <div class="col-sm-6">
//     <div class="form-group">
//         <label class="checkbox-list">
//             <input type="checkbox" name="s">
//             <span class="custom-checkbox"></span>
//             Dinner
//         </label>
//     </div>
// </div>
