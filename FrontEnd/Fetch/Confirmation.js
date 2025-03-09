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

        // Get user details from token
        const userDetails = getClaimsFromToken();

        // Update success notification
        updateSuccessNotification(userDetails.email);

        // Update booking details
        updateBookingDetails(confirmationData, userDetails);

        // Update payment details
        updatePaymentDetails(confirmationData.payment);

        // Update summary
        updateSummary(confirmationData.booking.bookingDetails);

    } catch (error) {
        console.error('Error loading confirmation page:', error);
        showError('Failed to load confirmation details');
    }
});

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
    if (bookingTableBody) {
        const bookingId = generateFormattedBookingId(confirmationData.booking.bookingDetails.bookingId);

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
                <td>${confirmationData.billingDetails.country}</td>
            </tr>
            <tr>
                <td>Zip Code:</td>
                <td>${confirmationData.billingDetails.zipCode}</td>
            </tr>
            <tr>
                <td>Address:</td>
                <td>${confirmationData.billingDetails.address}</td>
            </tr>
        `;
    }
}

function updatePaymentDetails(payment) {
    const paymentDesc = document.querySelector('.payment-details .details-desc');
    if (paymentDesc) {
        paymentDesc.innerHTML = `
            <p>Payment is successful via ${payment.paymentMethod}</p>
        `;
    }
}

function updateSummary(bookingDetails) {
    const summaryTable = document.querySelector('.widget-table-summary tbody');
    if (summaryTable && bookingDetails) {
        // Get the base booking price
        const bookingPrice = bookingDetails.totalPrice || 0;

        // Get selected options
        const selectedOptions = bookingDetails.selectedOptions?.$values || [];
        const optionsTotal = selectedOptions.reduce((sum, option) => sum + option.optionPrice, 0);

        // Calculate tax (1%)
        const subtotal = bookingPrice + optionsTotal;
        const taxRate = 0.01; // 1%
        const tax = subtotal * taxRate;
        const total = subtotal + tax;

        let summaryHTML = `
            <tr>
                <td><strong>Booking #${bookingDetails.bookingId}</strong></td>
                <td class="text-right">$${bookingPrice.toFixed(2)}</td>
            </tr>`;

        // Add selected options if any
        if (selectedOptions.length > 0) {
            summaryHTML += `
                <tr>
                    <td>
                        <strong>Selected Options:</strong>
                        <ul style="list-style: none; padding-left: 15px;">
                            ${selectedOptions.map(option =>
                `<li>${option.optionName} - $${option.optionPrice.toFixed(2)}</li>`
            ).join('')}
                        </ul>
                    </td>
                    <td class="text-right">$${optionsTotal.toFixed(2)}</td>
                </tr>`;
        }

        // Add tax and total
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
    }
}

function generateFormattedBookingId(id) {
    if (!id) return '';
    return `999-QSDE-${id.toString().padStart(2, '0')}`;
}

function formatPhoneNumber(phone) {
    if (!phone) return '';
    const cleaned = phone.replace(/\D/g, '');
    return cleaned.replace(/(\d{3})(\d{3})(\d{3})/, '$1 - $2 - $3');
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