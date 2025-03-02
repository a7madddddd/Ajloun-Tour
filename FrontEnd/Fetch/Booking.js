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

    const userInfo = getUserInfoFromToken();
    if (!userInfo) {
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
    if (nameInput) nameInput.value = userInfo.fullName;
    if (emailInput) emailInput.value = userInfo.email;

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

    bookingForm.addEventListener('submit', async function (event) {
        event.preventDefault();

        const currentUserInfo = getUserInfoFromToken();
        if (!currentUserInfo) {
            Swal.fire({
                title: 'Session Expired',
                text: 'Your session has expired or you are not logged in. Please login to continue.',
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
            const formData = new FormData();
            formData.append('TourId', tourId);
            formData.append('UserId', currentUserInfo.userId);
            formData.append('BookingDate', new Date(bookingDate).toISOString());
            formData.append('NumberOfPeople', numberOfPeople);
            formData.append('TotalPrice', calculateTotalPrice(tourId, numberOfPeople, selectedOptionIds));
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

            Swal.fire({ title: 'Success!', text: 'Your booking has been created!', icon: 'success', confirmButtonText: 'View My Bookings' })
                .then((result) => { if (result.isConfirmed) window.location.href = `/confirmation.html?id=${bookingId}`; });

        } catch (error) {
            console.error('Error during booking:', error);
            Swal.fire({ title: 'Error', text: `There was an error processing your booking: ${error.message}`, icon: 'error', confirmButtonText: 'OK' });
        }
    });

    function calculateTotalPrice(tourId, peopleCount, selectedOptions) {
        return (100 * peopleCount) + (selectedOptions.includes(1) ? 50 : 0) + (selectedOptions.includes(2) ? 25 * peopleCount : 0);
    }
});
