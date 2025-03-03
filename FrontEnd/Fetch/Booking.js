document.addEventListener('DOMContentLoaded', function () {
    const urlParams = new URLSearchParams(window.location.search);
    const tourId = urlParams.get('tourId');

    if (!tourId) {
        Swal.fire({
            title: 'Error!',
            text: 'Tour ID is missing from the URL. Cannot proceed with booking.',
            icon: 'error',
            confirmButtonText: 'OK'
        });
        return;
    }

    function getUserInfoFromToken() {
        const token = sessionStorage.getItem('token');
        if (!token) return null;
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            return {
                userId: payload.nameid,
                fullName: payload.unique_name || '',
                email: payload.email || ''
            };
        } catch (error) {
            console.error('Error parsing token:', error);
            return null;
        }
    }

    // Get user info but don't check it yet
    const userInfo = getUserInfoFromToken();

    // Still populate fields if user is logged in
    if (userInfo) {
        const nameInput = document.querySelector('input[name="name_booking"]');
        const emailInput = document.querySelector('input[name="email_booking"]');
        if (nameInput) nameInput.value = userInfo.fullName;
        if (emailInput) emailInput.value = userInfo.email;
    }

    const datePicker = document.querySelector('.input-date-picker');
    if (datePicker) {
        datePicker.type = 'date';
        datePicker.min = new Date().toISOString().split('T')[0];
    }

    const bookingForm = document.getElementById("bookingForm");
    if (!bookingForm) {
        console.error("Error: bookingForm not found in the document.");
        return;
    }

    function getSelectedOptions() {
        const selectedOptionIds = [];
        document.querySelectorAll('input[type="checkbox"]:checked').forEach(checkbox => {
            const optionId = parseInt(checkbox.value, 10);
            if (!isNaN(optionId)) {
                selectedOptionIds.push(optionId);
            }
        });
        return selectedOptionIds;
    }

    async function getTourPrice(tourId) {
        try {
            const response = await fetch(`https://localhost:44357/api/Tours/id?id=${tourId}`, {
                method: 'GET',
                headers: { 'accept': 'text/plain' }
            });

            if (!response.ok) throw new Error('Failed to fetch tour details.');

            const tourData = await response.json();
            return tourData.price || 0; // Return the real price from the tour data or 0 if not available
        } catch (error) {
            console.error('Error fetching tour price:', error);
            return 0; // Return 0 if there's an error
        }
    }

    async function getOptionPrice(optionId) {
        try {
            const response = await fetch(`https://localhost:44357/api/BookingOptions/${optionId}`, {
                method: 'GET',
                headers: { 'accept': 'text/plain' }
            });

            if (!response.ok) throw new Error('Failed to fetch option price.');

            const optionData = await response.json();
            return optionData.optionPrice || 0; // Return the option price or 0 if not available
        } catch (error) {
            console.error('Error fetching option price:', error);
            return 0; // Return 0 if there's an error
        }
    }

    async function calculateTotalPrice(tourId, peopleCount, selectedOptions) {
        const tourPrice = await getTourPrice(tourId); // Fetch the real tour price
        let totalPrice = tourPrice * peopleCount; // Start with the tour price multiplied by the number of people

        // Fetch the price for each selected option
        for (const optionId of selectedOptions) {
            const optionPrice = await getOptionPrice(optionId);
            totalPrice += optionPrice * peopleCount; // Add the option price, considering the number of people
        }

        return totalPrice;
    }

    bookingForm.addEventListener('submit', async function (event) {
        event.preventDefault();

        // Check login status when form is submitted
        const currentUserInfo = getUserInfoFromToken();
        if (!currentUserInfo) {
            Swal.fire({
                title: 'Authentication Required',
                text: 'You must be logged in to book a tour. Please login first.',
                icon: 'warning',
                confirmButtonText: 'Go to Login',
                showCancelButton: true,
                cancelButtonText: 'Cancel'
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = '/login.html?returnUrl=' + encodeURIComponent(window.location.href);
                }
            });
            return;
        }

        const nameInput = document.querySelector('input[name="name_booking"]');
        const emailInput = document.querySelector('input[name="email_booking"]');
        const userName = nameInput.value;
        const userEmail = emailInput.value;
        const numberOfPeople = parseInt(document.querySelector('input[name="phone_booking"]').value, 10);
        const bookingDate = document.querySelector('.input-date-picker').value;

        if (!userName || !userEmail || isNaN(numberOfPeople) || numberOfPeople <= 0 || !bookingDate) {
            Swal.fire({
                title: 'Missing Information',
                text: 'Please fill in all required fields.',
                icon: 'warning',
                confirmButtonText: 'OK'
            });
            return;
        }

        const selectedOptionIds = getSelectedOptions();
        console.log("Selected options:", selectedOptionIds);

        Swal.fire({ title: 'Processing', text: 'Creating your booking...', allowOutsideClick: false, didOpen: () => Swal.showLoading() });

        try {
            // Calculate the total price dynamically before submitting the form
            const totalPrice = await calculateTotalPrice(tourId, numberOfPeople, selectedOptionIds);

            const formData = new FormData();
            formData.append('TourId', tourId);
            formData.append('UserId', currentUserInfo.userId);
            formData.append('BookingDate', new Date(bookingDate).toISOString());
            formData.append('NumberOfPeople', numberOfPeople);
            formData.append('TotalPrice', totalPrice); // Use the dynamically calculated price
            formData.append('Status', 'Pending');
            formData.append('CreatedAt', new Date().toISOString());

            const bookingResponse = await fetch('https://localhost:44357/api/Bookings', {
                method: 'POST',
                headers: { 'Authorization': `Bearer ${sessionStorage.getItem('token')}` },
                body: formData
            });

            if (!bookingResponse.ok) throw new Error(`Failed to create booking: ${await bookingResponse.text()}`);

            const bookingResult = await bookingResponse.json();
            const bookingId = bookingResult.bookingId;
            console.log('Booking created with ID:', bookingId);

            for (const optionId of selectedOptionIds) {
                await fetch('https://localhost:44357/api/BookingOptionsSelections', {
                    method: 'POST',
                    headers: {
                        'Authorization': `Bearer ${sessionStorage.getItem('token')}`,
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ BookingId: bookingId, OptionId: optionId })
                });
            }

            Swal.fire({ title: 'Success!', text: `Your booking has been created! with Total Price: ${totalPrice}`, icon: 'success', confirmButtonText: 'View My Bookings' })
                .then((result) => { if (result.isConfirmed) window.location.href = `/confirmation.html?id=${bookingId}`; });

        } catch (error) {
            console.error('Error during booking:', error);
            Swal.fire({ title: 'Error', text: `There was an error processing your booking: ${error.message}`, icon: 'error', confirmButtonText: 'OK' });
        }
    });
});