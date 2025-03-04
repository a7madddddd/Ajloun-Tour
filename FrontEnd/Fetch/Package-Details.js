document.addEventListener("DOMContentLoaded", function () {
    // Get TourId and PackageId from URL
    const urlParams = new URLSearchParams(window.location.search);
    const tourId = urlParams.get('tourId');
    const packageId = urlParams.get('packageId');

    if (tourId && packageId) {
        fetchTourPackage(tourId, packageId);
    } else {
        console.error("TourId or PackageId is missing in the URL.");
    }
});

async function fetchTourPackage(tourId, packageId) {
    const apiUrl = `https://localhost:44357/api/ToursPackages/id?TourId=${tourId}&PackageId=${packageId}`;

    try {
        const response = await fetch(apiUrl, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        // Update HTML content
        document.getElementById("tour-title").innerText = data.packageName;
        document.getElementById("tour-location").innerText = data.location;
        document.getElementById("program-description").innerText = data.details;
        document.getElementById("program-map").src = data.map;
        document.getElementById(".tour-price").innerText = `$${data.price}`;
        document.getElementById("tour-price").innerText = `$${data.price}`;
        document.getElementById("..tour-price").value = `$${data.price} For ${data.numberOfPeople} People`;
        document.getElementById("tour-days").innerText = `${data.tourDays} Days / ${data.tourNights} Nights`;
        document.getElementById("tour-people").innerText = `For ${data.numberOfPeople} People`;
        document.getElementById("tour-image").src = `https://localhost:44357/PakagesImages/${data.image}`;
    } catch (error) {
        console.error("Error fetching data:", error);
    }
}











// Assuming the packageId is part of the URL, you can fetch it like this:

// Improved API fetching and rendering function
class ProgramManager {
    constructor(apiBaseUrl) {
        this.apiBaseUrl = apiBaseUrl;
    }

    // Helper method to safely get URL parameter
    getUrlParameter(name) {
        const urlParams = new URLSearchParams(window.location.search);
        return urlParams.get(name);
    }

    // Fetch and render program data
    async fetchProgramData() {
        try {
            // Get package ID from URL, with fallback
            const packId = this.getUrlParameter('packageId') || '1';

            // Construct full API URL
            const url = `${this.apiBaseUrl}/pack/${packId}/Withprograms`;

            // Fetch with error handling and flexible response parsing
            const response = await fetch(url, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();

            // Safely extract programs, handle empty data
            const programs = data['$values'] || [];

            if (programs.length === 0) {
                this.showNoDataMessage();
                return;
            }

            // Render the first program (or modify to render all)
            this.renderProgram(programs[0]);
        } catch (error) {
            console.error('Error fetching program data:', error);
            this.showErrorMessage(error);
        }
    }

    // Render program details
    renderProgram(program) {
        // Update program title and duration
        document.getElementById('program-duration').textContent =
            `(${program.dayNumber || 'N/A'} days)`;

        document.getElementById('program-duration-info').textContent =
            program.customDescription || program.programTitle || 'No description available.';

        // Render program details in timeline
        const programList = document.getElementById('program-list');

        programList.innerHTML = ''; // Clear existing items

        // Create timeline item
        const listItem = document.createElement('li');
        listItem.innerHTML = `
                <div class="timeline-content">
                    <div class="day-count">Day <span>${program.dayNumber || 'N/A'}</span></div>
                    <h4>${program.programTitle || 'Unnamed Program'}</h4>
                    <p>${program.customDescription || 'No detailed description provided.'}</p>
                    <small>Created: ${new Date(program.createdAt).toLocaleString()}</small>
                </div>
            `;
        programList.appendChild(listItem);
    }

    // Handle no data scenario
    showNoDataMessage() {
        const programList = document.getElementById('program-list');
        programList.innerHTML = `
                <li>
                    <div class="timeline-content">
                        <p>No program details available for this package.</p>
                    </div>
                </li>
            `;
    }

    // Handle error scenario
    showErrorMessage(error) {
        const programList = document.getElementById('program-list');
        programList.innerHTML = `
                <li>
                    <div class="timeline-content error">
                        <p>Error loading program: ${error.message}</p>
                    </div>
                </li>
            `;
    }
}

// Initialize and fetch program data on page load
document.addEventListener('DOMContentLoaded', () => {
    const programManager = new ProgramManager('https://localhost:44357/api/PackagesPrograms');
    programManager.fetchProgramData();
});






// reviews
/////////////
class ReviewManager {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
        this.reviewsList = document.getElementById('reviews-list');
        this.reviewCountElement = document.getElementById('review-count');
        this.overallRatingElement = document.getElementById('overall-rating');
        this.reviewQualityElement = document.getElementById('review-quality');
        this.reviewCountTextElement = document.getElementById('review-count-text');
        this.reviewSummaryStarsElement = document.getElementById('review-summary-stars');
        this.reviewSummaryStarsElement2 = document.getElementById('.review-summary-stars');
        this.reviewSummaryTextElement = document.getElementById('review-summary-text');
    }

    // Get URL parameter
    getUrlParameter(name) {
        const urlParams = new URLSearchParams(window.location.search);
        return urlParams.get(name);
    }

    // Render star rating
    renderStarRating(rating, isFullSize = false) {
        const totalStars = 5;
        let starsHTML = '';
        for (let i = 1; i <= totalStars; i++) {
            if (i <= rating) {
                starsHTML += `<i class="fas fa-star" style="font-size: ${isFullSize ? '24px' : '18px'}"></i>`;
            } else {
                starsHTML += `<i class="far fa-star" style="font-size: ${isFullSize ? '24px' : '18px'}"></i>`;
            }
        }
        return starsHTML;
    }

    // Get review quality description
    getReviewQualityDescription(averageRating) {
        if (averageRating >= 4.5) return "Outstanding";
        if (averageRating >= 4.0) return "Excellent";
        if (averageRating >= 3.0) return "Good";
        if (averageRating >= 2.0) return "Average";
        return "Needs Improvement";
    }

    // Fetch user details
    async fetchUserDetails(userId) {
        try {
            const response = await fetch(`${this.baseUrl}/Users/id?id=${userId}`);
            if (!response.ok) throw new Error('User details fetch failed');
            return await response.json();
        } catch (error) {
            console.error('Error fetching user details:', error);
            return null;
        }
    }

    // Fetch reviews
    async fetchReviews() {
        try {
            // Get package ID from URL or use default
            const packId = this.getUrlParameter('packageId') ;

            // Show loading state
            this.reviewsList.innerHTML = '<li class="loading">Loading reviews...</li>';

            // Fetch reviews
            const reviewsResponse = await fetch(`${this.baseUrl}/Reviews/getReviewByPackId?packId=${packId}`);
            if (!reviewsResponse.ok) throw new Error('Reviews fetch failed');

            const reviewsData = await reviewsResponse.json();
            const reviews = reviewsData['$values'] || [];

            // Calculate review statistics
            const totalRatings = reviews.reduce((sum, review) => sum + review.rating, 0);
            const averageRating = reviews.length > 0 ? (totalRatings / reviews.length) : 0;

            // Update review summary
            this.overallRatingElement.textContent = averageRating.toFixed(1);
            this.reviewSummaryStarsElement.innerHTML = this.renderStarRating(Math.round(averageRating), true);
            this.reviewSummaryStarsElement2.innerHTML = this.renderStarRating(Math.round(averageRating), true);
            this.reviewQualityElement.innerHTML = `
                    ${this.getReviewQualityDescription(averageRating)}
                    <span>( Based on ${reviews.length} review${reviews.length !== 1 ? 's' : ''} )</span>
                `;
            this.reviewSummaryTextElement.textContent = this.generateReviewSummary(averageRating, reviews.length);

            // Update review count
            this.reviewCountElement.textContent = `${reviews.length} Review${reviews.length !== 1 ? 's' : ''}`;

            // Clear loading state
            this.reviewsList.innerHTML = '';

            // No reviews handling
            if (reviews.length === 0) {
                this.reviewsList.innerHTML = '<li class="no-reviews">No reviews yet.</li>';
                return;
            }

            // Process each review
            for (const review of reviews) {
                // Fetch user details for each review
                const userDetails = await this.fetchUserDetails(review.userId);

                // Create review element
                const reviewElement = document.createElement('li');
                reviewElement.classList.add('comment-list-item');

                // User image fallback
                const userImageUrl = userDetails?.userImage
                    ? `https://localhost:44357/UsersImages/${userDetails.userImage}`
                    : 'https://via.placeholder.com/80';

                reviewElement.innerHTML = `
                        <figure class="comment-thumb">
                            <img src="${userImageUrl}" alt="${userDetails?.fullName || 'Anonymous User'}">
                        </figure>
                        <div class="comment-content">
                            <div class="comment-header">
                                <h5 class="author-name">${userDetails?.fullName || 'Anonymous'}</h5>
                                <span class="post-on">${new Date(review.createdAt).toLocaleDateString()}</span>
                                <div class="star-rating">${this.renderStarRating(review.rating)}</div>
                            </div>
                            <p>${review.comment || 'No comment provided'}</p>
                            <!--<a href="#" class="reply"><i class="fas fa-reply"></i> Reply</a>-->
                        </div>
                    `;

                this.reviewsList.appendChild(reviewElement);
            }
        } catch (error) {
            console.error('Error in review fetching:', error);
            this.reviewsList.innerHTML = `
                    <li class="no-reviews">
                        Failed to load reviews. ${error.message}
                    </li>
                `;
        }
    }

    // Generate review summary text
    generateReviewSummary(averageRating, reviewCount) {
        const descriptions = [
            "Our customers have shared their experiences, providing valuable insights into our service.",
            "We take pride in the feedback from our customers, continuously striving to improve.",
            "Your reviews help us understand our strengths and areas for improvement.",
            "Thank you to all our customers who took the time to share their thoughts."
        ];

        // Randomly select a description
        const description = descriptions[Math.floor(Math.random() * descriptions.length)];

        return description;
    }

    // Initialize review fetching
    init() {
        this.fetchReviews();
    }
}

// Initialize on DOM load
document.addEventListener('DOMContentLoaded', () => {
    const reviewManager = new ReviewManager('https://localhost:44357/api');
    reviewManager.init();
});






// add review
class ReviewSubmissionManager {
    constructor(apiBaseUrl) {
        this.apiBaseUrl = apiBaseUrl;
        this.form = document.getElementById('review-form');
        this.starRating = document.getElementById('star-rating');
        this.fullNameInput = document.getElementById('full-name');
        this.emailInput = document.getElementById('email');
        this.errorContainer = document.getElementById('error-container');
        this.successContainer = document.getElementById('success-container');
        this.currentRating = 0;

        this.initializeForm();
        this.initializeEventListeners();
    }

    initializeForm() {
        // Check and populate user details from token
        const token = sessionStorage.getItem('token') || sessionStorage.getItem('authToken');

        console.log('Retrieved token:', token);

        if (!token) {
            this.showError('Please log in to submit a review');
            this.disableForm();
            return;
        }

        try {
            const decodedToken = this.decodeToken(token);

            console.log('Decoded token:', decodedToken);

            this.fullNameInput.value = decodedToken.unique_name || 'Unknown User';
            this.emailInput.value = decodedToken.email || 'No email';
            this.emailInput.readOnly = true; // Prevent editing
        } catch (error) {
            console.error('Token decoding error:', error);
            this.showError('Invalid user token: ' + error.message);
            this.disableForm();
        }
    }

    disableForm() {
        this.form.querySelectorAll('input, textarea').forEach(el => {
            el.disabled = true;
        });
        this.form.querySelector('.submit-btn').disabled = true;
    }

    initializeEventListeners() {
        // Interactive star rating
        this.starRating.addEventListener('mouseover', this.handleStarHover.bind(this));
        this.starRating.addEventListener('mouseout', this.resetStars.bind(this));
        this.starRating.addEventListener('click', this.setRating.bind(this));

        // Form submission
        this.form.addEventListener('submit', this.handleSubmit.bind(this));
    }

    handleStarHover(event) {
        if (event.target.matches('[data-rating]')) {
            const rating = parseInt(event.target.dataset.rating);
            this.highlightStars(rating);
        }
    }

    resetStars() {
        this.highlightStars(this.currentRating);
    }

    setRating(event) {
        if (event.target.matches('[data-rating]')) {
            this.currentRating = parseInt(event.target.dataset.rating);
            this.highlightStars(this.currentRating);
        }
    }

    highlightStars(rating) {
        const stars = this.starRating.querySelectorAll('i');
        stars.forEach(star => {
            const starRating = parseInt(star.dataset.rating);
            if (starRating <= rating) {
                star.classList.remove('far');
                star.classList.add('fas');
            } else {
                star.classList.remove('fas');
                star.classList.add('far');
            }
        });
    }

    async handleSubmit(event) {
        event.preventDefault();

        // Validate booking date
        if (!this.dateInput.value) {
            Swal.fire({
                icon: 'error',
                title: 'Invalid Date',
                text: 'Please select a booking date',
                confirmButtonText: 'OK'
            });
            return;
        }

        // Get price and number of people from elements
        const priceText = this.priceElement ? this.priceElement.innerText : '';
        const peopleText = this.peopleElement ? this.peopleElement.innerText : '';

        // Extract price and number of people
        const price = parseFloat(priceText.replace('$', '')) || 0;
        const numberOfPeople = parseInt(peopleText.replace('For ', '').replace(' People', '')) || 1;

        // Check if user is logged in
        const token = sessionStorage.getItem('token');
        if (!token) {
            Swal.fire({
                icon: 'error',
                title: 'Login Required',
                text: 'Please log in to submit a booking',
                confirmButtonText: 'OK'
            }).then(() => {
                // Redirect the user to the login page
                window.location.href = '/login.html';  // Replace '/login.html' with your actual login page URL
            });
            return;
        }


        try {
            // Get package ID from URL
            const urlParams = new URLSearchParams(window.location.search);
            const packageId = urlParams.get('packageId');  // Ensure this gets 'packageId'

            console.log('Package ID from URL:', packageId); // Debugging line

            if (!packageId) {
                throw new Error('Package ID is missing');
            }

            // Decode token to get user ID
            const decodedToken = this.decodeToken(token);
            const userId = decodedToken.nameid;

            // Prepare booking data
            const bookingData = new FormData();
            bookingData.append('PackageId', packageId); // Ensure packageId is correctly added here
            bookingData.append('UserId', userId);

            // Use selected date, converted to ISO string
            const selectedDate = new Date(this.dateInput.value);
            bookingData.append('BookingDate', selectedDate.toISOString());

            // Add additional required fields
            bookingData.append('NumberOfPeople', numberOfPeople);
            bookingData.append('Status', 'Pending');
            bookingData.append('CreatedAt', new Date().toISOString());

            // Optional fields (can be empty)
            bookingData.append('TourId', '');
            bookingData.append('OfferId', '');

            // Calculate total price
            const totalPrice = price;
            bookingData.append('TotalPrice', totalPrice);

            // Debug: Log the data being sent
            console.log('Booking Data to Send:', {
                PackageId: packageId,
                UserId: userId,
                BookingDate: selectedDate.toISOString(),
                NumberOfPeople: numberOfPeople,
                TotalPrice: totalPrice
            });

            // Submit booking
            const response = await fetch(`${this.apiBaseUrl}/Bookings/By PackId`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`
                },
                body: bookingData
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`Booking failed: ${errorText}`);
            }

            const bookingResult = await response.json();

            // Show success alert
            Swal.fire({
                icon: 'success',
                title: 'Booking Successful!',
                text: `Your booking is confirmed. Total Price: ${bookingResult.totalPrice}`,
                confirmButtonText: 'OK'
            });

            // Reset form
            this.form.reset();
        } catch (error) {
            console.error('Booking submission error:', error);

            // Show error alert
            Swal.fire({
                icon: 'error',
                title: 'Booking Error',
                text: error.message,
                confirmButtonText: 'OK'
            });
        }
    }


    decodeToken(token) {
        try {
            // Detailed logging for token parts
            console.log('Full token:', token);

            // Split the token into parts
            const parts = token.split('.');
            console.log('Token parts:', parts);

            if (parts.length !== 3) {
                throw new Error('Invalid token format. Expected 3 parts.');
            }

            // Base64Url decode the payload
            const base64Url = parts[1];
            console.log('Base64Url payload:', base64Url);

            const base64 = base64Url
                .replace(/-/g, '+')   // Replace URL-safe characters
                .replace(/_/g, '/')   // Replace URL-safe characters
                // Add padding if needed
                .padEnd(base64Url.length + (4 - (base64Url.length % 4)) % 4, '=');

            console.log('Padded Base64 payload:', base64);

            // Decode the payload
            const decodedPayload = decodeURIComponent(
                atob(base64)
                    .split('')
                    .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
                    .join('')
            );

            console.log('Decoded payload:', decodedPayload);

            // Parse the JSON payload
            return JSON.parse(decodedPayload);
        } catch (error) {
            console.error('Detailed token decoding error:', error);
            throw error;
        }
    }
    getPackageId() {
        // Get package ID from URL or default
        const urlParams = new URLSearchParams(window.location.search);
        return urlParams.get('packageId') ;
    }

    showError(message) {
        this.errorContainer.textContent = message;
    }

    showSuccess(message) {
        this.successContainer.textContent = message;
    }

    clearMessages() {
        this.errorContainer.textContent = '';
        this.successContainer.textContent = '';
    }
}

// Initialize on DOM load
document.addEventListener('DOMContentLoaded', () => {
    const reviewSubmissionManager = new ReviewSubmissionManager('https://localhost:44357/api');
});








//booking
class BookingSubmissionManager {
    constructor(apiBaseUrl) {
        this.apiBaseUrl = apiBaseUrl;

        // Validate packageId presence in URL immediately
        try {
            this.packageId = this.getPackageIdFromUrl();
        } catch (error) {
            console.error('Package ID validation error:', error);
            Swal.fire({
                icon: 'error',
                title: 'Invalid URL',
                text: 'Missing or invalid package ID in URL',
                confirmButtonText: 'OK'
            });
        }

        this.form = document.querySelector('.booking-form');
        this.nameInput = this.form.querySelector('input[name="name_booking"]');
        this.emailInput = this.form.querySelector('input[name="email_booking"]');
        this.dateInput = this.form.querySelector('.input-date-picker');

        // Get price and number of people from existing elements
        this.priceElement = document.getElementById("tour-price");
        this.peopleElement = document.getElementById("tour-people");

                this.initializeDatePicker();  // Add this line

        this.initializeForm();
        this.initializeEventListeners();
    }

    getPackageIdFromUrl() {
        const urlParams = new URLSearchParams(window.location.search);
        const packageId = urlParams.get('packageId');

        // Check if packageId exists and is valid
        if (!packageId) {
            throw new Error('Package ID is missing from URL');
        }

        return packageId;
    }

    initializeDatePicker() {
        if (this.dateInput) {
            flatpickr(this.dateInput, {
                dateFormat: "Y-m-d",
                minDate: "today",
                enableTime: false,
                altInput: true,
                altFormat: "F j, Y",
                placeholder: "Select Date",
                disableMobile: false,
                onChange: (selectedDates, dateStr) => {
                    // Update the input value when date is selected
                    this.dateInput.value = dateStr;
                }
            });
        }
    }

    initializeForm() {
        // Try to prefill form if token exists
        const token = sessionStorage.getItem('token');

        if (token) {
            try {
                const decodedToken = this.decodeToken(token);

                // Prefill name and email
                this.nameInput.value = decodedToken.unique_name || 'Unknown User';
                this.nameInput.readOnly = true;
                this.emailInput.value = decodedToken.email || 'No email';
                this.emailInput.readOnly = true;

                // Enable date picker
                this.dateInput.removeAttribute('readonly');
            } catch (error) {
                console.error('Token decoding error:', error);
            }
        }
    }

    initializeEventListeners() {
        this.form.addEventListener('submit', this.handleSubmit.bind(this));
    }

    async handleSubmit(event) {
        event.preventDefault();

        try {
            // Validate booking date
            if (!this.dateInput.value) {
                Swal.fire({
                    icon: 'error',
                    title: 'Invalid Date',
                    text: 'Please select a booking date',
                    confirmButtonText: 'OK'
                });
                return;
            }

            // Check if user is logged in
            const token = sessionStorage.getItem('token');
            if (!token) {
                Swal.fire({
                    icon: 'error',
                    title: 'Login Required',
                    text: 'Please log in to make booking',
                    confirmButtonText: 'OK'
                }).then(() => {
                    // Redirect the user to the login page
                    window.location.href = '/login.html';  // Replace '/login.html' with your actual login page URL
                });
                return;
            }

            // Get price and number of people from elements
            const priceText = this.priceElement ? this.priceElement.innerText : '';
            const peopleText = this.peopleElement ? this.peopleElement.innerText : '';

            // Extract price and number of people
            const price = parseFloat(priceText.replace('$', '')) || 0;
            const numberOfPeople = parseInt(peopleText.replace('For ', '').replace(' People', '')) || 1;

            // Decode token to get user ID
            const decodedToken = this.decodeToken(token);
            const userId = decodedToken.nameid;

            // Prepare booking data
            const bookingData = new FormData();
            bookingData.append('PackageId', this.packageId);
            bookingData.append('UserId', userId);

            // Use selected date, converted to ISO string
            const selectedDate = new Date(this.dateInput.value);
            bookingData.append('BookingDate', selectedDate.toISOString());

            // Add additional required fields
            bookingData.append('NumberOfPeople', numberOfPeople);
            bookingData.append('Status', 'Pending');
            bookingData.append('CreatedAt', new Date().toISOString());

            // Optional fields
            bookingData.append('TourId', '');
            bookingData.append('OfferId', '');

            // Calculate total price
            const totalPrice = price;
            bookingData.append('TotalPrice', totalPrice);

            // Submit booking
            const response = await fetch(`${this.apiBaseUrl}/Bookings/By PackId`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`
                },
                body: bookingData
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`Booking failed: ${errorText}`);
            }

            const bookingResult = await response.json();

            // Show success alert
            Swal.fire({
                icon: 'success',
                title: 'Booking Successful!',
                text: `Your booking is confirmed. Total Price: ${bookingResult.totalPrice}`,
                confirmButtonText: 'OK'
            });

            // Reset form
            this.form.reset();

        } catch (error) {
            console.error('Booking submission error:', error);

            // Show error alert
            Swal.fire({
                icon: 'error',
                title: 'Booking Error',
                text: error.message,
                confirmButtonText: 'OK'
            });
        }
    }

    decodeToken(token) {
        try {
            // Split the token into parts
            const parts = token.split('.');
            if (parts.length !== 3) {
                throw new Error('Invalid token format');
            }

            // Base64Url decode the payload
            const base64Url = parts[1];
            const base64 = base64Url
                .replace(/-/g, '+')
                .replace(/_/g, '/')
                .padEnd(base64Url.length + (4 - (base64Url.length % 4)) % 4, '=');

            // Decode the payload
            const decodedPayload = decodeURIComponent(
                atob(base64)
                    .split('')
                    .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
                    .join('')
            );

            // Parse the JSON payload
            return JSON.parse(decodedPayload);
        } catch (error) {
            console.error('Token decoding error:', error);
            throw new Error('Invalid token');
        }
    }
}

// Initialize on DOM load
document.addEventListener('DOMContentLoaded', () => {
    // Include Sweet Alert library
    const sweetAlertScript = document.createElement('script');
    sweetAlertScript.src = 'https://cdn.jsdelivr.net/npm/sweetalert2@11';
    document.head.appendChild(sweetAlertScript);

    sweetAlertScript.onload = () => {
        const bookingSubmissionManager = new BookingSubmissionManager('https://localhost:44357/api');
    };
});