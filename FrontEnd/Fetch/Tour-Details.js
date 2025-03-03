// استخراج tourId من رابط الـ URL
const urlParams = new URLSearchParams(window.location.search);
const tourId = urlParams.get('tourId');

if (tourId) {
    fetch(`https://localhost:44357/api/Tours/id?id=${tourId}`, {
        method: 'GET',
        headers: {
            'Accept': 'text/plain'
        }
    })
        .then(response => response.json())
        .then(data => {
            // تحديث بيانات الصفحة بالعناصر المسترجعة من API
            document.getElementById('tour-title').textContent = data.tourName;
            document.getElementById('tour-description').textContent = data.description;
            document.getElementById('tour-price').textContent = `$${data.price}`;
            document.getElementById('tour-duration').textContent = `${data.duration} Hours`;
            document.getElementById('tour-location').textContent = data.location.toUpperCase();
            document.getElementById('tour-image').src = `https://localhost:44357/ToursImages/${data.tourImage}`;
            document.getElementById('tour-map').src = data.map;

        })
        .catch(error => console.error('Error fetching tour data:', error));
} else {
    console.error('No tourId found in URL');
}



// programs
// programs
const tourIdProgram = new URLSearchParams(window.location.search).get('tourId');

if (tourIdProgram) {
    fetch(`https://localhost:44357/api/ToursPrograms/tour/${tourIdProgram}`, {
        method: 'GET',
        headers: {
            'Accept': 'text/plain'
        }
    })
        .then(response => response.json())
        .then(data => {
            const itineraryWrap = document.querySelector(".itinerary-timeline-wrap ul");
            itineraryWrap.innerHTML = ""; // Clear old content before adding new

            // Handle the new response structure where programs are in $values array
            let programs = [];
            if (data && data.$values && Array.isArray(data.$values)) {
                programs = data.$values;
            } else if (Array.isArray(data)) {
                programs = data;
            } else if (data) {
                programs = [data];
            }

            if (programs.length > 0) {
                programs.forEach(program => {
                    const programHTML = `
                        <li>
                            <div class="timeline-content">
                                <div class="day-count">Day <span>${program.dayNumber}</span></div>
                                <h4>${program.programTitle || program.customTitle}</h4>
                                <p>${program.customDescription || "No description available."}</p>
                            </div>
                        </li>
                    `;
                    itineraryWrap.innerHTML += programHTML;
                });

                // Update title with number of days
                document.querySelector(".itinerary-content h3").innerHTML = `Program <span>(${programs.length} days)</span>`;
            } else {
                itineraryWrap.innerHTML = "<li><p>No program available for this tour.</p></li>";
            }
        })
        .catch(error => console.error("Error fetching tour program:", error));
} else {
    console.error("No tourId found in URL");
}



//reviews
// Get tourId from URL
function getTourIdFromUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get('tourId');
}

// Fetch reviews from API
async function fetchReviews() {
    const tourId = getTourIdFromUrl();

    if (!tourId) {
        console.error('No tourId found in URL');
        return;
    }

    try {
        const response = await fetch(`https://localhost:44357/api/Reviews/getReviewByTourId?tourId=${tourId}`);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        // Filter active reviews if isActive field is present
        const activeReviews = data.$values.filter(review =>
            review.isActive === true || review.isActive === null // Include if isActive is true or not specified
        );

        // Get user details for each review
        const reviewsWithUserData = await Promise.all(
            activeReviews.map(async (review) => {
                const userData = await fetchUserData(review.userId);
                return { ...review, userData };
            })
        );

        displayReviews(reviewsWithUserData);
        updateReviewSummary(reviewsWithUserData);
    } catch (error) {
        console.error('Error fetching reviews:', error);
    }
}

// Fetch user data from Users API
async function fetchUserData(userId) {
    try {
        const response = await fetch(`https://localhost:44357/api/Users/id?id=${userId}`);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
            return null;
        }

        return await response.json();
    } catch (error) {
        console.error(`Error fetching user data for user ID ${userId}:`, error);
        return null;
    }
}

// Generate star HTML based on rating
function generateStarRating(rating) {
    let starsHtml = '';

    // Generate filled and empty stars
    for (let i = 1; i <= 5; i++) {
        if (i <= rating) {
            // Full star
            starsHtml += '<i class="fas fa-star"></i>';
        } else if (i - 0.5 <= rating) {
            // Half star
            starsHtml += '<i class="fas fa-star-half-alt"></i>';
        } else {
            // Empty star
            starsHtml += '<i class="far fa-star"></i>';
        }
    }

    return starsHtml;
}

// Helper function to get user's initials as a fallback for missing profile picture
function getUserInitials(fullName) {
    if (!fullName) return 'U';

    const names = fullName.split(' ');
    if (names.length === 1) return names[0].charAt(0).toUpperCase();
    return (names[0].charAt(0) + names[names.length - 1].charAt(0)).toUpperCase();
}

// Display reviews in the comment area
// Display reviews in the comment area
function displayReviews(reviews) {
    const commentAreaInner = document.querySelector('.comment-area-inner ol');

    // Clear existing reviews
    commentAreaInner.innerHTML = '';

    // Update review count
    document.querySelector('.comment-title').textContent = `${reviews.length} Reviews`;

    // Add each review
    reviews.forEach(review => {
        const reviewDate = new Date(review.createdAt).toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });

        // Create star rating HTML
        const starsHtml = generateStarRating(review.rating);

        // Get user data
        const userData = review.userData;
        let userName = userData ? userData.fullName : `User ID: ${review.userId}`;

        // Create user avatar (either image, initials, or default placeholder)
        let userAvatarHtml;
        if (userData) {
            if (userData.userImage) {
                // Use the correct API endpoint to retrieve the user image
                // Make sure this URL matches exactly how your server serves these images
                const imageUrl = `https://localhost:44357/UsersImages/${userData.userImage}`;

                userAvatarHtml = `<img src="${imageUrl}" alt="${userData.fullName}" onerror="this.onerror=null; this.src='';">`;

                // Optional: Log the image URL for debugging
                console.log("Attempting to load user image from:", imageUrl);
            } else {
                // Generate initials fallback for user
                const initials = getUserInitials(userData.fullName);

                // Initials placeholder
                userAvatarHtml = `
                    <div class="user-avatar">
                        <div class="avatar-initials">${initials}</div>
                    </div>
                `;
            }
        } else {
            // Default placeholder
            userAvatarHtml = `<img src="" alt="Default avatar">`;
        }

        // Create review HTML
        const reviewHtml = `
            <li>
                <figure class="comment-thumb">
                    ${userAvatarHtml}
                </figure>
                <div class="comment-content">
                    <div class="comment-header">
                        <h5 class="author-name">${userName}</h5>
                        <span class="post-on">${reviewDate}</span>
                        <div class="rating-stars">
                            ${starsHtml}
                        </div>
                    </div>
                    <p>${review.subject ? `<strong>${review.subject}</strong><br>` : ''}${review.comment}</p>
                     <!--<a href="#" class="reply"><i class="fas fa-reply"></i>Reply</a>-->
                </div>
            </li>
        `;

        commentAreaInner.innerHTML += reviewHtml;
    });
}

// Helper function to get user initials
function getUserInitials(fullName) {
    if (!fullName) return "?";
    return fullName
        .split(' ')
        .map(name => name.charAt(0))
        .join('')
        .toUpperCase();
}

// Update review summary section
function updateReviewSummary(reviews) {
    if (reviews.length === 0) return;

    // Calculate average rating
    const totalRating = reviews.reduce((sum, review) => sum + review.rating, 0);
    const averageRating = (totalRating / reviews.length).toFixed(1);

    // Update summary section
    document.querySelector('.review-score span').textContent = averageRating;

    // Get rating text based on average rating
    let ratingText = "Excellent";
    if (averageRating < 2) ratingText = "Poor";
    else if (averageRating < 3) ratingText = "Fair";
    else if (averageRating < 4) ratingText = "Good";
    else if (averageRating < 4.5) ratingText = "Very Good";

    // Update review count and add star rating to the summary
    const summaryElement = document.querySelector('.review-score-content h3');
    summaryElement.innerHTML = `
        ${ratingText}
        <span>( Based on ${reviews.length} reviews )</span>
    `;

    // Add star rating to summary section
    const existingSummaryStars = document.querySelector('.summary-stars');
    if (existingSummaryStars) {
        existingSummaryStars.innerHTML = generateStarRating(parseFloat(averageRating));
    } else {
        const summaryStarsDiv = document.createElement('div');
        summaryStarsDiv.className = 'summary-stars';
        summaryStarsDiv.innerHTML = generateStarRating(parseFloat(averageRating));
        summaryElement.after(summaryStarsDiv);
    }
}

// Add enhanced CSS for star ratings and user avatars
function addStyles() {
    const style = document.createElement('style');
    style.textContent = `
        .rating-stars {
            display: inline-block;
            margin-left: 10px;
        }
        
        .rating-stars i.fas.fa-star, 
        .rating-stars i.fas.fa-star-half-alt {
            color: #F56960;
        }
        
        .rating-stars i.far.fa-star {
            color: #e4e5e9;
        }
        
        .summary-stars {
            margin-top: 5px;
            font-size: 18px;
        }
        
        .summary-stars i.fas.fa-star,
        .summary-stars i.fas.fa-star-half-alt {
            color: #F56960;
        }
        
        .summary-stars i.far.fa-star {
            color: #e4e5e9;
        }
        
        .user-avatar {
            width: 60px;
            height: 60px;
            border-radius: 50%;
            background-color: #e1e1e1;
            display: flex;
            justify-content: center;
            align-items: center;
            overflow: hidden;
        }
        
        .avatar-initials {
            font-size: 24px;
            font-weight: bold;
            color: #555;
        }
        
        .comment-thumb img {
            width: 60px;
            height: 60px;
            border-radius: 50%;
            object-fit: cover;
        }
    `;
    document.head.appendChild(style);
}

// Run when page loads
document.addEventListener('DOMContentLoaded', () => {
    addStyles();
    fetchReviews();
});





// add review
// Star Rating Functionality with Multiple API Approaches
document.addEventListener('DOMContentLoaded', function () {
    const stars = document.querySelectorAll('.fas');
    let selectedRating = 0;

    // API URLs
    const API_BASE_URL = 'https://localhost:44357/api';
    const REVIEWS_ENDPOINT = `${API_BASE_URL}/Reviews`;

    // Parse JWT token from sessionStorage
    const token = sessionStorage.getItem('token');
    let userInfo = null;

    if (token) {
        try {
            const base64Url = token.split('.')[1];
            const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            userInfo = JSON.parse(window.atob(base64));
            console.log('Parsed user info:', userInfo);

            document.querySelector('input[name="FullName"]').value = userInfo.unique_name || '';
            document.querySelector('input[name="email"]').value = userInfo.email || '';
        } catch (error) {
            console.error('Error parsing token:', error);
        }
    }

    // Star rating functionality
    stars.forEach(star => {
        star.style.color = '#ccc';
        star.style.cursor = 'pointer';
        star.style.fontSize = '20px'; // Making the stars bigger

        star.addEventListener('mouseover', function () {
            const rating = parseInt(this.getAttribute('data-rating'));
            highlightStars(rating);
        });

        star.addEventListener('mouseleave', function () {
            highlightStars(selectedRating);
        });

        star.addEventListener('click', function () {
            selectedRating = parseInt(this.getAttribute('data-rating'));
            highlightStars(selectedRating);
            console.log('Rating selected:', selectedRating);
        });
    });

    function highlightStars(rating) {
        stars.forEach(star => {
            const starRating = parseInt(star.getAttribute('data-rating'));
            if (starRating <= rating) {
                star.style.color = '#F56960';
            } else {
                star.style.color = '#ccc';
            }
        });
    }

    // Form submission
    const form = document.querySelector('.comment-form');
    form.addEventListener('submit', function (event) {
        event.preventDefault();

        // Check if user is logged in (token exists in sessionStorage)
        if (!sessionStorage.getItem('token')) {
            // If not logged in, ask user to log in
            if (typeof Swal !== 'undefined') {
                Swal.fire({
                    title: 'Please Log In',
                    text: 'You need to log in to submit a review.',
                    icon: 'warning',
                    confirmButtonText: 'Log In',
                    preConfirm: () => {
                        // Redirect to login page
                        window.location.href = 'login.html';  // Adjust the login page URL as needed
                    }
                });
            } else {
                alert('You need to log in to submit a review.');
            }
            return;
        }

        // Proceed if a rating is selected
        if (!selectedRating) {
            // Using SweetAlert if available, otherwise falling back to regular alert
            if (typeof Swal !== 'undefined') {
                Swal.fire({
                    title: 'Error!',
                    text: 'Please select a rating before submitting.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            } else {
                alert('Please select a rating before submitting.');
            }
            return;
        }

        const subject = document.querySelector('input[name="subject"]').value;
        const commentText = document.querySelector('textarea').value;

        if (!subject || !commentText) {
            // Using SweetAlert if available, otherwise falling back to regular alert
            if (typeof Swal !== 'undefined') {
                Swal.fire({
                    title: 'Error!',
                    text: 'Please fill in all fields.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            } else {
                alert('Please fill in all fields.');
            }
            return;
        }

        // Create FormData object - this is crucial for multipart/form-data
        const formData = new FormData();
        formData.append('UserId', userInfo.nameid );
        formData.append('TourId', urlParams.get('tourId')); // Set your default tourId
        formData.append('Rating', selectedRating.toString());
        formData.append('Subject', subject);
        formData.append('Comment', commentText);



        // Show loading message if SweetAlert is available
        if (typeof Swal !== 'undefined') {
            Swal.fire({
                title: 'Submitting...',
                text: 'Please wait while we submit your review',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });
        }

        // Make the API request
        const authToken = sessionStorage.getItem('token');
        const headers = {
            'Accept': '*/*'
        };

        if (authToken) {
            headers['Authorization'] = `Bearer ${authToken}`;
        }

        fetch(REVIEWS_ENDPOINT, {
            method: 'POST',
            headers: headers,
            body: formData
        })
            .then(response => {
                console.log('Response status:', response.status);

                if (!response.ok) {
                    return response.text().then(text => {
                        throw new Error(`API error (${response.status}): ${text}`);
                    });
                }

                return response.json();
            })
            .then(responseData => {
                console.log('API response data:', responseData);

                // Success message
                if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        title: 'Success!',
                        text: 'Your review has been submitted successfully!',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    });
                } else {
                    alert('Your review has been submitted successfully!');
                }

                // Reset form
                form.reset();
                selectedRating = 0;
                highlightStars(0);

                // Get latest reviews
                setTimeout(() => {
                    fetch(REVIEWS_ENDPOINT, {
                        method: 'GET',
                        headers: { 'Accept': 'application/json' }
                    })
                        .then(response => response.json())
                        .then(data => {
                            console.log('Current reviews from server:', data);
                        })
                        .catch(error => {
                            console.error('GET Reviews error:', error);
                        });
                }, 1000);
            })
            .catch(error => {
                console.error('Error submitting review:', error);

                if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        title: 'Error!',
                        text: 'Could not submit your review. Please try again later.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                } else {
                    alert('Could not submit your review. Please try again later.');
                }
            });
    });
});





