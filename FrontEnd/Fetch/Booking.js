document.addEventListener('DOMContentLoaded', function () {
    // Get tour ID from URL parameters
    const urlParams = new URLSearchParams(window.location.search);
    const tourId = urlParams.get('tourId');

    // Display error if tour ID is missing
    if (!tourId) {
        alert('Error: Tour ID is missing from the URL. Cannot proceed with booking.');
        // Disable the form submission
        const submitBtn = document.querySelector('input[type="submit"]');
        if (submitBtn) {
            submitBtn.disabled = true;
        }
        return;
    }

    // Function to check if user is logged in and get user ID from token
    function getUserIdFromToken() {
        const token = sessionStorage.getItem('token');
        if (!token) return null;

        try {
            // Parse JWT payload (second part of the token)
            const payload = JSON.parse(atob(token.split('.')[1]));
            return payload.nameid; // The user ID is stored in the 'nameid' claim
        } catch (error) {
            console.error('Error parsing token:', error);
            return null;
        }
    }

    // Check login status immediately
    const userId = getUserIdFromToken();
    if (!userId) {
        // User is not logged in - display a message and disable the form or redirect
        alert('You must be logged in to book a tour. Please login first.');

        // Disable the form submission
        const submitBtn = document.querySelector('input[type="submit"]');
        if (submitBtn) {
            submitBtn.disabled = true;
        }
    }

    // Initialize date picker
    const datePicker = document.querySelector('.input-date-picker');
    if (datePicker) {
        // You would need to include a date picker library like flatpickr
        // Using native date input as fallback
        datePicker.type = 'date';
        datePicker.min = new Date().toISOString().split('T')[0]; // Set min date to today
    }

    // Handle form submission
    const bookingForm = document.querySelector('.booking-form');
    bookingForm.addEventListener('submit', async function (event) {
        event.preventDefault();

        // Check login status again when submitting (in case token expired)
        const currentUserId = getUserIdFromToken();
        if (!currentUserId) {
            alert('Your session has expired or you are not logged in. Please login to continue.');
            return;
        }

        const userName = document.querySelector('input[name="name_booking"]').value;
        const userEmail = document.querySelector('input[name="email_booking"]').value;
        const numberOfPeopleInput = document.querySelector('input[name="phone_booking"]').value;
        const bookingDateInput = document.querySelector('.input-date-picker').value;

        // Validate required fields
        if (!userName || !userEmail || !numberOfPeopleInput || !bookingDateInput) {
            alert('Please fill in all required fields.');
            return;
        }

        const numberOfPeople = parseInt(numberOfPeopleInput);

        // Validate number of people
        if (isNaN(numberOfPeople) || numberOfPeople <= 0) {
            alert('Please enter a valid number of people.');
            return;
        }

        // Format booking date correctly as ISO string (YYYY-MM-DDT00:00:00)
        let bookingDate;
        try {
            // Ensure the date is in the correct format
            const dateObj = new Date(bookingDateInput);
            bookingDate = dateObj.toISOString();
        } catch (error) {
            alert('Please enter a valid date.');
            return;
        }

        // Get selected options
        const selectedOptionIds = [];
        const checkboxes = document.querySelectorAll('input[type="checkbox"]:checked');
        checkboxes.forEach(checkbox => {
            // Get option ID from the checkbox value or data attribute
            // For this example, we'll use a mapping function based on the label text
            const optionLabel = checkbox.closest('.checkbox-list').textContent.trim();
            const optionId = getOptionIdByName(optionLabel);
            if (optionId) {
                selectedOptionIds.push(optionId);
            }
        });

        try {
            // Step 1: Create the booking using FormData (for multipart/form-data)
            const formData = new FormData();
            formData.append('TourId', tourId);
            formData.append('UserId', currentUserId);
            formData.append('BookingDate', bookingDate);
            formData.append('NumberOfPeople', numberOfPeople);
            formData.append('TotalPrice', calculateTotalPrice(tourId, numberOfPeople, selectedOptionIds));
            formData.append('Status', 'Pending');
            formData.append('CreatedAt', new Date().toISOString());

            console.log('Sending booking data with FormData');

            const bookingResponse = await fetch('https://localhost:44357/api/Bookings', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${sessionStorage.getItem('token')}`
                    // No Content-Type header - browser sets it automatically with boundary for multipart/form-data
                },
                body: formData
            });

            if (!bookingResponse.ok) {
                const errorText = await bookingResponse.text();
                throw new Error(`Failed to create booking: ${errorText}`);
            }

            const bookingResult = await bookingResponse.json();
            const bookingId = bookingResult.bookingId;

            console.log('Booking created with ID:', bookingId);

            // Step 2: Add options to the booking
            let allOptionsAdded = true;

            for (const optionId of selectedOptionIds) {
                const optionFormData = new FormData();
                optionFormData.append('BookingId', bookingId);
                optionFormData.append('OptionId', optionId);

                const optionResponse = await fetch('https://localhost:44357/api/BookingOptionsSelections', {
                    method: 'POST',
                    headers: {
                        'Authorization': `Bearer ${sessionStorage.getItem('token')}`
                    },
                    body: optionFormData
                });

                if (!optionResponse.ok) {
                    console.error('Failed to add option:', optionId);
                    allOptionsAdded = false;
                }
            }

            if (!allOptionsAdded) {
                alert('Booking was created, but some options could not be added.');
            } else {
                alert('Booking successful!');
            }

            // Redirect to confirmation page or booking history
            // window.location.href = '/booking-confirmation.html?id=' + bookingId;

        } catch (error) {
            console.error('Error during booking:', error);
            alert('There was an error processing your booking: ' + error.message);
        }
    });

    // Function to map option names to IDs - update this with your actual option IDs
    function getOptionIdByName(name) {
        const trimmedName = name.trim();
        const optionMap = {
            'Tour guide': 1,
            'meal': 2
            // Add more options as needed
        };

        // Try to find the option by exact match
        if (optionMap[trimmedName]) {
            return optionMap[trimmedName];
        }

        // If no exact match, check if the name contains any of the keys
        for (const key in optionMap) {
            if (trimmedName.includes(key)) {
                return optionMap[key];
            }
        }

        console.warn('Could not find option ID for:', trimmedName);
        return null;
    }

    // Function to calculate total price based on tour, people count, and selected options
    function calculateTotalPrice(tourId, peopleCount, selectedOptions) {
        // You should fetch the actual tour price from your API
        // For now, using a placeholder calculation:
        const baseTourPrice = 100; // Example base price per person

        let optionPrice = 0;
        if (selectedOptions.includes(1)) { // Tour guide
            optionPrice += 50; // Fixed price for tour guide
        }
        if (selectedOptions.includes(2)) { // Meal
            optionPrice += 25 * peopleCount; // Price per person for meal
        }

        return (baseTourPrice * peopleCount) + optionPrice;
    }
});