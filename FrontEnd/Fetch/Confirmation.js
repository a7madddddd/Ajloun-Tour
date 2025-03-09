// confirmation.js
const API_BASE_URL = 'https://localhost:44357';

document.addEventListener('DOMContentLoaded', async function () {
    try {
        const urlParams = new URLSearchParams(window.location.search);
        const encryptedData = urlParams.get('data');

        if (!encryptedData) {
            showError('No confirmation data found');
            return;
        }

        // Decrypt and parse the data
        const confirmationData = JSON.parse(atob(encryptedData));
        console.log('Confirmation data:', confirmationData);

        // Update the booking details
        updateBookingDetails(confirmationData, confirmationData.userDetails);

        // Update the summary with options
        await updateSummary(confirmationData);
        updatePaymentDetails(confirmationData);
        updateViewBookingDetails(confirmationData);


        // Show success message
        showSuccessMessage(confirmationData.userDetails.email);

    } catch (error) {
        console.error('Error loading confirmation page:', error);
        showError('Failed to load confirmation details');
    }
});

function showSuccessMessage(email) {
    const successNotify = document.querySelector('.success-notify');
    if (successNotify) {
        successNotify.innerHTML = `
            <div class="success-icon">
                <i class="fas fa-check"></i>
            </div>
            <div class="success-content">
                <h3>PAYMENT CONFIRMED</h3>
                <p>Thank you, your payment has been successful and your booking is now confirmed. 
                   A confirmation email has been sent to ${email}.</p>
            </div>
        `;
    }
}

async function updateSummary(confirmationData) {
    const summaryTable = document.querySelector('.widget-table-summary tbody');
    if (!summaryTable) return;

    try {
        console.log('Full confirmation data:', confirmationData);

        // Handle both single booking and multiple bookings
        const bookings = confirmationData.booking.bookings ||
            [confirmationData.booking.bookingDetails];

        let summaryHTML = '';
        let totalAmount = 0;

        // Process each booking
        for (const booking of bookings) {
            if (!booking) continue;

            const basePrice = booking.totalPrice || 0;

            // Add booking row
            summaryHTML += `
                <tr>
                    <td><strong>Booking #${booking.bookingId}</strong></td>
                    <td class="text-right">$${basePrice.toFixed(2)}</td>
                </tr>`;

            // Fetch and add selected options
            try {
                const optionsResponse = await fetch(`${API_BASE_URL}/api/BookingOptionsSelections/byBooking/${booking.bookingId}`);
                if (optionsResponse.ok) {
                    const optionsData = await optionsResponse.json();
                    const selectedOptions = optionsData.$values || [];
                    let optionsTotal = 0;

                    if (selectedOptions.length > 0) {
                        summaryHTML += `
                            <tr>
                                <td>
                                    <strong>Selected Options for Booking #${booking.bookingId}:</strong>
                                    <ul style="list-style: none; margin: 5px 0; padding-left: 15px;">
                                        ${selectedOptions.map(option => {
                            optionsTotal += parseFloat(option.optionPrice);
                            return `<li>• ${option.optionName} - $${parseFloat(option.optionPrice).toFixed(2)}</li>`;
                        }).join('')}
                                    </ul>
                                </td>
                                <td class="text-right">$${optionsTotal.toFixed(2)}</td>
                            </tr>`;
                    }

                    totalAmount += basePrice + optionsTotal;
                } else {
                    totalAmount += basePrice;
                }
            } catch (error) {
                console.error(`Error fetching options for booking ${booking.bookingId}:`, error);
                totalAmount += basePrice;
            }
        }

        // Calculate tax and total
        const tax = totalAmount * 0.01; // 1% tax
        const total = totalAmount + tax;

        // Add tax and total rows
        summaryHTML += `
            <tr>
                <td><strong>Tax (1%)</strong></td>
                <td class="text-right">$${tax.toFixed(2)}</td>
            </tr>
            <tr class="total">
                <td><strong>Total cost</strong></td>
                <td class="text-right"><strong>$${total.toFixed(2)}</strong></td>
            </tr>`;

        summaryTable.innerHTML = summaryHTML;

    } catch (error) {
        console.error('Error updating summary:', error);
        console.error('Error details:', error.message);
        summaryTable.innerHTML = `
            <tr class="total">
                <td><strong>Error loading summary</strong></td>
                <td class="text-right"><strong>--</strong></td>
            </tr>`;
    }
}
function updateSuccessNotification(email) {
    const successContent = document.querySelector('.success-content');
    if (successContent) {
        successContent.querySelector('p').innerHTML =
            `Thank you, your payment has been successful and your booking is now confirmed. 
             A confirmation email has been sent to ${email}.`;
    }
}

function updateBookingDetails(confirmationData, userDetails) {
    const bookingTableBody = document.querySelector('.confirmation-details table tbody');
    if (!bookingTableBody) return;

    try {
        // Get the first booking ID for reference
        const firstBookingId = confirmationData.booking.bookings ?
            confirmationData.booking.bookings[0].bookingId :
            confirmationData.booking.bookingDetails?.bookingId;

        const bookingId = generateFormattedBookingId(firstBookingId);
        const billingDetails = confirmationData.billingDetails || {};

        bookingTableBody.innerHTML = `
            <tr>
                <td>Booking id:</td>
                <td>${bookingId}</td>
            </tr>
            <tr>
                <td>First Name:</td>
                <td>${userDetails.fullName.split(' ')[0]}</td>
            </tr>
            <tr>
                <td>Last Name:</td>
                <td>${userDetails.fullName.split(' ').slice(1).join(' ')}</td>
            </tr>
            <tr>
                <td>Email:</td>
                <td>${userDetails.email}</td>
            </tr>
            <tr>
                <td>Phone:</td>
                <td>${formatPhoneNumber(userDetails.phone)}</td>
            </tr>
            <tr>
                <td>Country:</td>
                <td>${billingDetails.country || 'Not provided'}</td>
            </tr>
            <tr>
                <td>State:</td>
                <td>${billingDetails.state || 'Not provided'}</td>
            </tr>
            <tr>
                <td>City:</td>
                <td>${billingDetails.city || 'Not provided'}</td>
            </tr>
            <tr>
                <td>Zip Code:</td>
                <td>${billingDetails.zipCode || 'Not provided'}</td>
            </tr>
            <tr>
                <td>Address:</td>
                <td>${billingDetails.address || 'Not provided'}</td>
            </tr>
        `;
    } catch (error) {
        console.error('Error updating booking details:', error);
        bookingTableBody.innerHTML = '<tr><td colspan="2">Error loading booking details</td></tr>';
    }
}





function updatePaymentDetails(confirmationData) {
    // Get the payment details section
    const paymentSection = document.querySelector('.payment-details .details-desc');
    if (paymentSection) {
        const payment = confirmationData.payment;
        const paymentMethod = payment.paymentMethod === 'PayPal' ? 'PayPal' : 'Master card';

        paymentSection.innerHTML = `<p>Payment is successful via ${paymentMethod}</p>`;
    }
}

function updateViewBookingDetails(confirmationData) {
    // Get the first booking ID (or the relevant one)
    const bookingId = confirmationData.booking.bookings?.[0]?.bookingId ||
        confirmationData.booking.bookingDetails?.bookingId;

    // Get the view booking details section
    const viewBookingSection = document.querySelector('.details:not(.payment-details) .details-desc');
    if (viewBookingSection) {
        viewBookingSection.innerHTML = `
            <p><a href="#">https://www.travele.com/sadsd-f646556</a></p>
        `;
    }
}


function generateFormattedBookingId(id) {
    if (!id) return 'Not provided';
    return `999-QSDE-${id.toString().padStart(2, '0')}`;
}

function formatPhoneNumber(phone) {
    if (!phone) return 'Not provided';
    const cleaned = phone.replace(/\D/g, '');
    return cleaned.replace(/(\d{3})(\d{3})(\d{4})/, '$1 - $2 - $3');
}

function showError(message) {
    const errorDiv = document.createElement('div');
    errorDiv.className = 'alert alert-danger';
    errorDiv.innerHTML = message;
    document.querySelector('.confirmation-inner').prepend(errorDiv);
}

function getClaimsFromToken() {
    const token = sessionStorage.getItem('token');
    if (!token) return null;

    try {
        const tokenData = JSON.parse(atob(token.split('.')[1]));
        return {
            userId: tokenData.nameid,
            email: tokenData.email,
            phone: tokenData['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone'],
            fullName: tokenData.unique_name
        };
    } catch (error) {
        console.error('Error parsing token:', error);
        return null;
    }
}