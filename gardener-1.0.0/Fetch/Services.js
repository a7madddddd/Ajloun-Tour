document.addEventListener("DOMContentLoaded", function () {
    fetchTours();
});

function fetchTours() {
    fetch("https://localhost:44357/api/Tours")
        .then(response => response.json())
        .then(data => {
            if (data.$values) {
                displayTours(data.$values);
            } else {
                console.error("Invalid response format", data);
            }
        })
        .catch(error => console.error("Error fetching tours:", error));
}

function displayTours(tours) {
    const toursContainer = document.getElementById("tours-container");
    toursContainer.innerHTML = ""; // Clear existing content

    tours
        .filter(tour => tour.isActive)
        .forEach(tour => {
            // Create a data attribute string with escaped values
            const tourData = {
                tourId: tour.tourId,
                tourName: tour.tourName,
                description: tour.description,
                price: tour.price,
                duration: tour.duration,
                tourImage: tour.tourImage
            };

            const tourHTML = `
                <div class="col-lg-4 col-md-6 wow fadeInUp" data-wow-delay="0.1s">
                    <div class="service-item rounded d-flex h-100">
                        <div class="service-img rounded">
                            <img class="img-fluid" src="https://localhost:44357/ToursImages/${tour.tourImage}" alt="${tour.tourName}">
                        </div>
                        <div class="service-text rounded p-5" style="width: 100%">
                            <div class="btn-square rounded-circle mx-auto mb-3">
                                <img class="img-fluid" src="img/icon/icon-3.png" alt="Icon">
                            </div>
                            <h4 class="mb-3">${tour.tourName}</h4>
                            <p class="mb-4">${tour.description}</p>
                            <button class="btn btn-sm" 
                                onclick="showTourDetails(${tour.tourId}, '${tour.tourName}', '${tour.tourImage}', '${tour.description}', ${tour.price}, '${tour.duration}')">
                                <i class="fa fa-plus text-primary me-2"></i>Details
                            </button>
                            <button class="btn btn-sm">
                                <i class="fa fa-clock text-primary me-2"></i>${tour.duration} Hours
                            </button>
                        </div>
                    </div>
                </div>
            `;
            toursContainer.innerHTML += tourHTML;
        });
}

function showTourDetails(tourId, tourName, tourImage, description, price, duration) {
    console.log('Starting showTourDetails...');

    // Get and validate token
    const token = sessionStorage.getItem('token');
    console.log('Token from session:', token ? 'exists' : 'not found');

    // Debug token parsing
    if (token) {
        const decodedToken = parseJwt(token);
        console.log('Decoded token:', decodedToken);
        console.log('User ID in token:', decodedToken?.nameid);
    }

    // Show tour details regardless of login status
    Swal.fire({
        title: tourName,
        html: `
            <div class="tour-details-container">
                <img src="https://localhost:44357/ToursImages/${tourImage}" 
                     alt="${tourName}" 
                     class="img-fluid rounded mb-3" 
                     style="max-height: 300px; width: auto;">
                <div class="tour-info">
                    <p class="description">${description}</p>
                    <div class="details-grid">
                        <div class="detail-item">
                            <i class="fa fa-clock text-primary"></i>
                            <span>${duration} Hours</span>
                        </div>
                        <div class="detail-item">
                            <i class="fa fa-dollar-sign text-primary"></i>
                            <span>$${price}</span>
                        </div>
                    </div>
                </div>
            </div>
        `,
        showCloseButton: true,
        showConfirmButton: true,
        confirmButtonText: 'Book Now',
        confirmButtonColor: '#0F4229',
        width: '600px',
        customClass: {
            container: 'tour-details-swal'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            // Check authentication only when trying to book
            if (!token) {
                Swal.fire({
                    title: 'Login Required',
                    text: 'Please login to book this tour',
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Login',
                    cancelButtonText: 'Cancel'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = 'login.html';
                    }
                });
                return;
            }

            const decodedToken = parseJwt(token);
            const userId = decodedToken?.nameid;

            if (!userId) {
                Swal.fire({
                    icon: 'error',
                    title: 'Authentication Error',
                    text: 'Please login again',
                    confirmButtonText: 'OK'
                }).then(() => {
                    sessionStorage.removeItem('token');
                    window.location.href = 'login.html';
                });
                return;
            }

            handleBooking({
                tourId: tourId,
                tourName: tourName,
                price: price,
                duration: duration,
                userId: userId
            });
        }
    });
}

function parseJwt(token) {
    if (!token) return null;

    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        return JSON.parse(jsonPayload);
    } catch (error) {
        console.error('Error parsing token:', error);
        return null;
    }
}           

function getUserIdFromToken() {
    const token = sessionStorage.getItem('token');
    if (!token) {
        console.log('No token found');
        return null;
    }

    const decodedToken = parseJwt(token);
    if (!decodedToken) {
        console.log('Could not decode token');
        return null;
    }

    const userId = decodedToken.nameid;
    console.log('Found userId:', userId);

    return userId || null;
}

function handleBooking(bookingData) {
    const { tourId, tourName, price, duration, userId } = bookingData;

    Swal.fire({
        title: 'Confirm Booking',
        html: `
            <div class="booking-confirmation">
                <h4>${tourName}</h4>
                <p>Duration: ${duration} Hours</p>
                <p>Price: $${price} per person</p>
                <div class="form-group">
                    <label>Select Date:</label>
                    <input type="date" id="booking-date" class="form-control" 
                           min="${new Date().toISOString().split('T')[0]}">
                </div>
                <div class="form-group mt-3">
                    <label>Number of People:</label>
                    <input type="number" id="number-of-people" class="form-control" 
                           min="1" value="1">
                </div>
                <div class="mt-3">
                    <strong>Total Price: $<span id="total-price">${price}</span></strong>
                </div>
            </div>
        `,
        showCancelButton: true,
        confirmButtonText: 'Confirm Booking',
        confirmButtonColor: '#28a745',
        cancelButtonText: 'Cancel',
        didOpen: () => {
            const numberOfPeopleInput = document.getElementById('number-of-people');
            const totalPriceSpan = document.getElementById('total-price');

            numberOfPeopleInput.addEventListener('input', () => {
                const numberOfPeople = parseInt(numberOfPeopleInput.value) || 1;
                const totalPrice = (price * numberOfPeople).toFixed(2);
                totalPriceSpan.textContent = totalPrice;
            });
        },
        preConfirm: () => {
            const bookingDate = document.getElementById('booking-date').value;
            const numberOfPeople = document.getElementById('number-of-people').value;

            if (!bookingDate) {
                Swal.showValidationMessage('Please select a date');
                return false;
            }
            if (!numberOfPeople || numberOfPeople < 1) {
                Swal.showValidationMessage('Please enter a valid number of people');
                return false;
            }

            return {
                bookingDate,
                numberOfPeople: parseInt(numberOfPeople)
            };
        }
    }).then((result) => {
        if (result.isConfirmed) {
            const formData = new FormData();
            formData.append('TourId', tourId.toString());
            formData.append('UserId', userId.toString());
            formData.append('BookingDate', result.value.bookingDate);
            formData.append('NumberOfPeople', result.value.numberOfPeople.toString());
            formData.append('TotalPrice', (price * result.value.numberOfPeople).toFixed(2));
            formData.append('Status', 'Pending');
            formData.append('CreatedAt', new Date().toISOString());

            const token = sessionStorage.getItem('token');

            fetch('https://localhost:44357/api/Bookings', {
                method: 'POST',
                body: formData,
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
                .then(response => {
                    if (!response.ok) throw new Error('Booking failed');
                    return response.json();
                })
                .then(data => {
                    Swal.fire({
                        icon: 'success',
                        title: 'Booked Successfully!',
                        text: 'Your booking has been confirmed!',
                        confirmButtonColor: '#28a745'
                    });
                })
                .catch(error => {
                    console.error('Booking error:', error);
                    Swal.fire({
                        icon: 'error',
                        title: 'Booking Failed',
                        text: 'There was an error processing your booking. Please try again.',
                        confirmButtonColor: '#dc3545'
                    });
                });
        }
    });
}

// Call this when page loads to verify token
document.addEventListener('DOMContentLoaded', function () {
    testTokenParsing();
});

// Test function to verify token parsing
function testTokenParsing() {
    const token = sessionStorage.getItem('token');
    console.log('Token exists:', !!token);

    const decodedToken = parseJwt(token);
    console.log('Full decoded token:', decodedToken);

    const userId = decodedToken.nameid;
    console.log('Extracted User ID:', userId);

    return userId;
}



// Add CSS
const style = document.createElement('style');
style.textContent = `

      .booking-confirmation .form-group {
        margin-bottom: 1rem;
        text-align: left;
    }

    .booking-confirmation label {
        display: block;
        margin-bottom: 0.5rem;
        font-weight: bold;
    }

    .booking-confirmation input {
        width: 100%;
        padding: 0.5rem;
        border: 1px solid #ced4da;
        border-radius: 0.25rem;
    }

    .booking-confirmation input[type="number"] {
        width: 100px;
    }

    .booking-confirmation .total-price {
        font-size: 1.2rem;
        font-weight: bold;
        color: #28a745;
    }
    .tour-details-swal .tour-details-container {
        text-align: center;
    }

    .tour-details-container .tour-info {
        margin-top: 1rem;
    }

    .tour-details-container .details-grid {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 1rem;
        margin-top: 1rem;
    }

    .tour-details-container .detail-item {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 0.5rem;
    }

    .booking-confirmation {
        text-align: left;
    }

    .booking-confirmation .form-group {
        margin-top: 1rem;
    }

    .booking-confirmation input[type="date"] {
        width: 100%;
        padding: 0.5rem;
        margin-top: 0.5rem;
    }
`;
document.head.appendChild(style);
// Helper function to get user ID from token
function getUserIdFromToken() {
    // Implement your token parsing logic here
    return sessionStorage.getItem('userId');
}