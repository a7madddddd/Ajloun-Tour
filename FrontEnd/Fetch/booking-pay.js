// Constants
const API_BASE_URL = 'https://localhost:44357';

// Get checkout data from URL
document.addEventListener('DOMContentLoaded', async () => {
    const urlParams = new URLSearchParams(window.location.search);
    const encryptedData = urlParams.get('data');

    if (!encryptedData) {
        showError('No checkout data found');
        return;
    }

    try {
        const data = JSON.parse(atob(encryptedData));
        console.log('Decoded data:', data);

        if (data.type === 'booking' && data.ids) {
            // Single booking checkout
            console.log('Processing single booking with ID:', data.ids[0]);
            await processSingleCheckout(data);
        } else if (data.bookings || data.cart) {
            // Multiple items checkout
            console.log('Processing multiple items:', data);
            await processMultipleCheckout(data);
        } else {
            throw new Error('Invalid data format');
        }

        
    } catch (error) {
        console.error('Error processing checkout data:', error);
        showError('Invalid checkout data');
    }
});
async function processSingleCheckout(data) {
    try {
        const booking = data.bookingDetails;
        if (!booking) {
            throw new Error('No booking details found');
        }

        // Fetch selected options if any
        try {
            const optionsResponse = await fetch(`${API_BASE_URL}/api/BookingOptionsSelections/byBooking/${booking.bookingId}`);
            if (optionsResponse.ok) {
                const optionsData = await optionsResponse.json();
                booking.selectedOptions = optionsData;
            }
        } catch (error) {
            console.log(`No options found for booking ${booking.bookingId}`);
            booking.selectedOptions = null;
        }

        // Update summary section
        updateSummaryWithBooking(booking);

    } catch (error) {
        console.error('Error processing single checkout:', error);
        showError('Failed to process checkout');
    }
}
async function processMultipleCheckout(data) {
    try {
        const { bookings, cart } = data;
        let total = 0;

        // Update summary section
        const summaryTable = document.querySelector('.widget-table-summary tbody');
        summaryTable.innerHTML = '';

        // Process bookings
        if (bookings && bookings.length > 0) {
            // Fetch options for all bookings
            const bookingsWithOptions = await Promise.all(bookings.map(async (booking) => {
                try {
                    const optionsResponse = await fetch(`${API_BASE_URL}/api/BookingOptionsSelections/byBooking/${booking.bookingId}`);
                    if (optionsResponse.ok) {
                        const optionsData = await optionsResponse.json();
                        return { ...booking, selectedOptions: optionsData };
                    }
                } catch (error) {
                    console.log(`No options found for booking ${booking.bookingId}`);
                }
                return { ...booking, selectedOptions: null };
            }));

            // Display bookings with their options
            bookingsWithOptions.forEach(booking => {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td><strong>Booking #${booking.bookingId}</strong></td>
                    <td class="text-right">$${booking.totalPrice.toFixed(2)}</td>
                `;
                summaryTable.appendChild(row);
                total += booking.totalPrice;

                // Add options if any
                if (booking.selectedOptions && booking.selectedOptions.$values && booking.selectedOptions.$values.length > 0) {
                    const optionsTotal = booking.selectedOptions.$values.reduce((sum, option) =>
                        sum + option.optionPrice, 0);
                    if (optionsTotal > 0) {
                        const optionsRow = document.createElement('tr');
                        optionsRow.innerHTML = `
                            <td class="option-row">
                                Selected Options for Booking #${booking.bookingId}:
                                <ul class="options-list">
                                    ${booking.selectedOptions.$values.map(option => `
                                        <li>${option.optionName} - $${option.optionPrice.toFixed(2)}</li>
                                    `).join('')}
                                </ul>
                            </td>
                            <td class="text-right">$${optionsTotal.toFixed(2)}</td>
                        `;
                        summaryTable.appendChild(optionsRow);
                        total += optionsTotal;
                    }
                }
            });
        }

        // Calculate and add tax (1%)
        const tax = total * 0.01;
        const totalWithTax = total + tax;

        // Add tax and total rows
        summaryTable.innerHTML += `
            <tr>
                <td><strong>Tax (1%)</strong></td>
                <td class="text-right">$${tax.toFixed(2)}</td>
            </tr>
            <tr class="total">
                <td><strong>Total cost</strong></td>
                <td class="text-right"><strong>$${totalWithTax.toFixed(2)}</strong></td>
            </tr>
        `;

    } catch (error) {
        console.error('Error processing multiple checkout:', error);
        showError('Failed to process checkout');
    }
}



async function fetchBookingDetails(bookingId) {
    try {
        const response = await fetch(`${API_BASE_URL}/api/Bookings/id?id=${bookingId}`);
        if (!response.ok) throw new Error('Failed to fetch booking details');
        const data = await response.json();
        return data;
    } catch (error) {
        console.error(`Error fetching booking ${bookingId}:`, error);
        return null;
    }
}
async function fetchBookingOptions(bookingId) {
    try {
        // Add the API endpoint for fetching booking options
        const response = await fetch(`${API_BASE_URL}/api/BookingOptions/booking/${bookingId}`);
        if (!response.ok) throw new Error('Failed to fetch booking options');
        const data = await response.json();
        return data;
    } catch (error) {
        console.error(`Error fetching options for booking ${bookingId}:`, error);
        return null;
    }
}
async function fetchCartDetails(cartId) {
    try {
        const response = await fetch(`${API_BASE_URL}/api/CartItems/cart/${cartId}`);
        if (!response.ok) throw new Error('Failed to fetch cart details');
        return await response.json();
    } catch (error) {
        console.error(`Error fetching cart ${cartId}:`, error);
        return null;
    }
}

function updateSummaryWithBooking(booking) {
    const summaryTable = document.querySelector('.widget-table-summary tbody');
    let total = booking.totalPrice;

    // Clear existing rows
    summaryTable.innerHTML = '';

    // Add booking row
    summaryTable.innerHTML = `
        <tr>
            <td><strong>Booking #${booking.bookingId}</strong></td>
            <td class="text-right">$${booking.totalPrice.toFixed(2)}</td>
        </tr>
    `;

    // Add options if any
    if (booking.selectedOptions && booking.selectedOptions.$values && booking.selectedOptions.$values.length > 0) {
        const options = booking.selectedOptions.$values;
        const optionsTotal = options.reduce((sum, option) => sum + option.optionPrice, 0);

        if (optionsTotal > 0) {
            summaryTable.innerHTML += `
                <tr class="option-row">
                    <td>
                        Selected Options:
                        <ul class="options-list">
                            ${options.map(option => `
                                <li>${option.optionName} - $${option.optionPrice.toFixed(2)}</li>
                            `).join('')}
                        </ul>
                    </td>
                    <td class="text-right">$${optionsTotal.toFixed(2)}</td>
                </tr>
            `;
            total += optionsTotal;
        }
    }

    // Calculate tax (1%)
    const tax = total * 0.01;
    const totalWithTax = total + tax;

    // Add tax and total
    summaryTable.innerHTML += `
        <tr>
            <td><strong>Tax (1%)</strong></td>
            <td class="text-right">$${tax.toFixed(2)}</td>
        </tr>
        <tr class="total">
            <td><strong>Total cost</strong></td>
            <td class="text-right"><strong>$${totalWithTax.toFixed(2)}</strong></td>
        </tr>
    `;
}






























// booking button
// booking-pay.js

// Add this function to get claims from token
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

// Function to get user ID from token
function getUserIdFromToken() {
    try {
        const token = sessionStorage.getItem('token');
        if (!token) {
            throw new Error('No token found');
        }

        const tokenParts = token.split('.');
        if (tokenParts.length !== 3) {
            throw new Error('Invalid token format');
        }

        const payload = JSON.parse(atob(tokenParts[1]));
        const userId = payload.nameid;

        if (!userId) {
            throw new Error('No user ID in token');
        }

        console.log('User ID from token:', userId); // Debug log
        return parseInt(userId);
    } catch (error) {
        console.error('Error getting user ID from token:', error);
        throw new Error('Failed to get user ID from token');
    }
}

async function getCartIdByUserId(userId) {
    try {
        const response = await fetch(`${API_BASE_URL}/api/ToursCarts/user/${userId}`, {
            method: 'GET',
            headers: {
                'accept': 'text/plain'
            }
        });

        if (!response.ok) {
            throw new Error('Failed to fetch cart ID');
        }

        const cartId = await response.json();
        console.log('Retrieved cart ID:', cartId);
        return cartId;
    } catch (error) {
        console.error('Error fetching cart ID:', error);
        return null;
    }
}

// Function to populate form fields
function populateFormFields() {
    const claims = getClaimsFromToken();
    if (!claims) {
        console.error('No claims found in token');
        return;
    }

    const nameParts = claims.fullName.split(' ');
    const firstName = nameParts[0];
    const lastName = nameParts.slice(1).join(' ');

    // Populate fields and set them as readonly
    const fields = {
        'firstname_booking': firstName,
        'lastname_booking': lastName,
        'email_booking': claims.email,
        'phone_booking': claims.phone,
    };

    for (const [fieldName, value] of Object.entries(fields)) {
        const inputs = document.querySelectorAll(`input[name="${fieldName}"]`);
        inputs.forEach(input => {
            input.value = value;
            input.readOnly = true;
            input.classList.remove('is-invalid');
        });

        // Special handling for phone field
        if (fieldName === 'phone_booking') {
            const phoneInput = document.getElementById('phone-pay');
            if (phoneInput) {
                phoneInput.value = value;
                phoneInput.readOnly = true;
                phoneInput.classList.remove('is-invalid');
            }
        }
    }
}

async function processPayment() {
    try {
        if (!validateForm()) {
            return;
        }

        showLoading();

        // Get form data with correct card holder name
        const formData = {
            cardHolderName: document.querySelector('#card_holder_name').value.trim(),
            cardNumber: document.querySelector('#card_number').value.replace(/\s/g, ''),
            expiryMonth: document.querySelector('#expire_month').value.trim(),
            expiryYear: document.querySelector('#expire_year').value.trim(),
            cvv: document.querySelector('#ccv').value.trim(),
            billingAddress: document.querySelector('input[name="street_1"]').value.trim(),
            billingCity: document.querySelector('input[name="city_booking"]').value.trim(),
            billingCountry: document.querySelector('#country').value.trim(),
            billingZipCode: document.querySelector('input[name="postal_code"]').value.trim()
        };

        // Validate card holder name specifically
        if (!formData.cardHolderName) {
            throw new Error('Card holder name is required');
        }

        // Get booking data from URL
        const urlParams = new URLSearchParams(window.location.search);
        const encryptedData = urlParams.get('data');
        const bookingData = JSON.parse(atob(encryptedData));

        // Create payment record
        const payment = await createPaymentRecord(bookingData, formData);
        if (!payment) {
            throw new Error('Failed to create payment record');
        }

        console.log('Payment created:', payment);

        // Add payment details with correct case for properties
        const paymentDetails = await addPaymentDetails(payment.paymentId, {
            ...formData,
            CardHolderName: formData.cardHolderName // Ensure correct case for API
        });

        if (!paymentDetails) {
            throw new Error('Failed to add payment details');
        }

        console.log('Payment details added:', paymentDetails);

        // Add payment history
        try {
            await addPaymentHistory(payment.paymentId, 'Processing', 'Payment is being processed');
            console.log('Payment history added successfully');
        } catch (historyError) {
            console.error('Failed to add payment history:', historyError);
        }

        // Update payment status
        await updatePaymentStatus(payment.paymentId, 'Completed');

        // Create confirmation data
        const confirmationData = {
            payment: {
                paymentId: payment.paymentId,
                paymentMethod: 'card',
                paymentStatus: 'Completed',
                cardNumber: formData.cardNumber
            },
            booking: bookingData,
            userDetails: getClaimsFromToken(),
            billingDetails: {
                country: formData.billingCountry,
                state: document.querySelector('input[name="state_booking"]').value.trim(),
                city: formData.billingCity,
                zipCode: formData.billingZipCode,
                address: formData.billingAddress
            }
        };

        // Redirect to confirmation page
        const encryptedConfirmationData = btoa(JSON.stringify(confirmationData));
        window.location.href = `confirmation.html?data=${encryptedConfirmationData}`;

    } catch (error) {
        console.error('Payment processing error:', error);
        showError('Payment processing failed: ' + error.message);
    } finally {
        hideLoading();
    }
}


function validateForm() {
    // Remove any existing error messages first
    const existingErrors = document.querySelectorAll('.alert-danger');
    existingErrors.forEach(error => error.remove());

    // Fields that should be skipped in validation (pre-filled from token)
    const skipFields = ['firstname_booking', 'lastname_booking', 'email_booking', 'phone_booking'];

    const requiredFields = {
        'card_number': 'Card Number',
        'expire_month': 'Expiry Month',
        'expire_year': 'Expiry Year',
        'ccv': 'CVV',
        'street_1': 'Billing Address',
        'city_booking': 'City',
        'state_booking': 'State',
        'country': 'Country',
        'postal_code': 'Postal Code'
    };

    let isValid = true;
    let errorMessages = new Set();

    // Check required fields
    for (const [fieldId, fieldName] of Object.entries(requiredFields)) {
        // Skip pre-filled fields
        if (skipFields.includes(fieldId)) continue;

        let field;
        if (fieldId === 'country') {
            field = document.querySelector('#country');
        } else {
            field = document.querySelector(`[name="${fieldId}"]`) || document.getElementById(fieldId);
        }

        if (!field || !field.value.trim()) {
            errorMessages.add(`${fieldName} is required`);
            isValid = false;
            if (field) {
                field.classList.add('is-invalid');
            }
        } else {
            if (field) {
                field.classList.remove('is-invalid');
            }
        }
    }

    // Check terms acceptance
    const termsAccepted = document.querySelector('input[type="checkbox"]').checked;
    if (!termsAccepted) {
        errorMessages.add('Please accept the terms and conditions');
        isValid = false;
    }

    if (!isValid) {
        showError(Array.from(errorMessages).join('<br>'));
    }

    return isValid;
}

function showError(message) {
    // Remove any existing error messages first
    const existingErrors = document.querySelectorAll('.alert-danger');
    existingErrors.forEach(error => error.remove());

    const errorDiv = document.createElement('div');
    errorDiv.className = 'alert alert-danger';
    errorDiv.innerHTML = message;

    const form = document.querySelector('.booking-form-wrap');
    form.insertBefore(errorDiv, form.firstChild);

    // Scroll to error message
    errorDiv.scrollIntoView({ behavior: 'smooth', block: 'start' });
}


async function createPaymentRecord(bookingData, formData) {
    try {
        const amount = calculateTotalAmount();
        const userId = getUserIdFromToken();

        // Generate a transaction ID for card payments
        const transactionId = `CARD_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;

        const cartId = await getCartIdByUserId(userId);

        let bookingId = null;
        if (bookingData.bookingDetails) {
            bookingId = bookingData.bookingDetails.bookingId;
        } else if (bookingData.bookings && bookingData.bookings.length > 0) {
            bookingId = bookingData.bookings[0].bookingId;
        }

        const paymentData = {
            bookingID: bookingId,
            userID: userId,
            gatewayID: 2, // Use a different gateway ID for card payments
            cartId: cartId,
            amount: parseFloat(amount.toFixed(2)),
            paymentMethod: "card",
            paymentStatus: "Pending",
            transactionId: transactionId // Add the generated transaction ID
        };

        console.log('Payment data to be sent:', paymentData);

        const response = await fetch(`${API_BASE_URL}/api/Payments`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'accept': 'text/plain'
            },
            body: JSON.stringify(paymentData)
        });

        if (!response.ok) {
            const errorData = await response.json();
            console.error('Payment API Error:', errorData);
            throw new Error(errorData.message || 'Failed to create payment record');
        }

        const responseData = await response.json();
        console.log('Payment Created Successfully:', responseData);
        return responseData;

    } catch (error) {
        console.error('Create Payment Error:', error);
        throw error;
    }
}


async function addPaymentDetails(paymentId, formData) {
    try {
        console.log('Adding payment details for payment ID:', paymentId);
        console.log('Form data:', formData);

        // Format expiry date to MM/YY format
        const expiryMonth = formData.expiryMonth.padStart(2, '0');
        const expiryYear = formData.expiryYear.length === 4 ?
            formData.expiryYear.slice(-2) :
            formData.expiryYear.padStart(2, '0');
        const additionalNotes = document.getElementById('note').value.trim();

        const expiryDate = `${expiryMonth}/${expiryYear}`;

        const paymentDetailsData = {
            PaymentID: parseInt(paymentId),
            CardHolderName: formData.cardHolderName, // Make sure this is present
            CardNumber: formData.cardNumber.replace(/\s+/g, ''),
            ExpiryDate: expiryDate,
            CVV: formData.cvv,
            BillingAddress: formData.billingAddress,
            BillingCity: formData.billingCity,
            BillingCountry: formData.billingCountry,
            BillingZipCode: formData.billingZipCode,
            AdditionalNotes: additionalNotes 

        };

        console.log('Payment details data to be sent:', paymentDetailsData);

        const response = await fetch(`${API_BASE_URL}/api/PaymentDetails`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'accept': 'text/plain'
            },
            body: JSON.stringify(paymentDetailsData)
        });

        if (!response.ok) {
            const errorData = await response.json();
            console.error('Payment Details API Error:', errorData);
            if (errorData.errors) {
                console.error('Validation errors:', errorData.errors);
                const errorMessages = Object.values(errorData.errors).flat().join(', ');
                throw new Error(`Validation failed: ${errorMessages}`);
            }
            throw new Error(errorData.message || 'Failed to add payment details');
        }

        const responseData = await response.json();
        console.log('Payment details added successfully:', responseData);
        return responseData;

    } catch (error) {
        console.error('Add Payment Details Error:', error);
        throw error;
    }
}

async function addPaymentHistory(paymentId, status, message) {
    try {
        // Log the input data
        console.log('Adding payment history:', { paymentId, status, message });

        const paymentHistoryData = {
            PaymentID: paymentId,
            Status: status,
            Message: message,
            CreatedAt: new Date().toISOString()
        };

        console.log('Payment history data to be sent:', paymentHistoryData);

        // Updated endpoint to match the API
        const response = await fetch(`${API_BASE_URL}/api/PaymentHistories`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'accept': 'text/plain'
            },
            body: JSON.stringify(paymentHistoryData)
        });

        if (!response.ok) {
            const errorData = await response.json();
            console.error('Payment History API Error:', errorData);
            throw new Error(errorData.message || 'Failed to add payment history');
        }

        const responseData = await response.json();
        console.log('Payment history added successfully:', responseData);
        return responseData;

    } catch (error) {
        console.error('Add Payment History Error:', error);
        throw error;
    }
}


async function updatePaymentStatus(paymentId, status) {
    try {
        // First, get the current payment data
        const getResponse = await fetch(`${API_BASE_URL}/api/Payments/${paymentId}`);
        if (!getResponse.ok) {
            throw new Error('Failed to fetch current payment data');
        }
        const currentPayment = await getResponse.json();

        // Prepare the update data
        const updateData = {
            bookingID: currentPayment.bookingId,
            userID: currentPayment.userId,
            gatewayID: currentPayment.gatewayId,
            cartId: currentPayment.cartId,
            amount: currentPayment.amount,
            paymentMethod: currentPayment.paymentMethod,
            paymentStatus: status,
            transactionId: currentPayment.transactionId // Make sure to include the transaction ID
        };

        console.log('Updating payment with data:', updateData);

        const response = await fetch(`${API_BASE_URL}/api/Payments/${paymentId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'accept': 'text/plain'
            },
            body: JSON.stringify(updateData)
        });

        if (!response.ok) {
            const errorData = await response.json();
            console.error('Update payment error:', errorData);
            throw new Error(`Failed to update payment: ${errorData.title || response.statusText}`);
        }

        return await response.json();
    } catch (error) {
        console.error('Update payment status error:', error);
        throw error;
    }
}

function getBookingDataFromUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    const encryptedData = urlParams.get('data');
    if (encryptedData) {
        try {
            return JSON.parse(atob(encryptedData));
        } catch (error) {
            console.error('Error parsing booking data:', error);
            return null;
        }
    }
    return null;
}

// Update calculateTotalAmount function to handle decimal values properly
function calculateTotalAmount() {
    try {
        // Get all amount cells from the summary table
        const summaryTable = document.querySelector('.widget-table-summary tbody');
        if (!summaryTable) {
            console.error('Summary table not found');
            throw new Error('Could not find summary table');
        }

        // Find the final total row (last row with class 'total')
        const totalRow = summaryTable.querySelector('tr.total');
        if (totalRow) {
            const totalCell = totalRow.querySelector('td.text-right strong');
            if (totalCell) {
                // Extract the amount, removing currency symbol and any commas
                const amountStr = totalCell.textContent
                    .replace(/[^0-9.-]+/g, '')  // Remove all non-numeric characters except decimal point
                    .trim();
                const amount = parseFloat(amountStr);

                console.log('Calculated total amount:', amount);

                if (!isNaN(amount) && amount > 0) {
                    return amount;
                }
            }
        }

        // Fallback: Calculate total by summing all booking amounts
        let total = 0;
        const bookingRows = summaryTable.querySelectorAll('tr:not(.total)');

        bookingRows.forEach(row => {
            const amountCell = row.querySelector('td.text-right');
            if (amountCell) {
                const amountStr = amountCell.textContent
                    .replace(/[^0-9.-]+/g, '')
                    .trim();
                const amount = parseFloat(amountStr);
                if (!isNaN(amount)) {
                    total += amount;
                }
            }
        });

        console.log('Calculated total from individual rows:', total);

        if (total > 0) {
            return total;
        }

        // If still no valid amount, try getting from booking data
        const bookingData = getBookingDataFromUrl();
        if (bookingData) {
            if (bookingData.bookings && Array.isArray(bookingData.bookings)) {
                const bookingsTotal = bookingData.bookings.reduce((sum, booking) => {
                    return sum + (parseFloat(booking.totalPrice) || 0);
                }, 0);

                console.log('Calculated total from booking data:', bookingsTotal);

                if (bookingsTotal > 0) {
                    return bookingsTotal;
                }
            } else if (bookingData.bookingDetails && bookingData.bookingDetails.totalPrice) {
                const singleBookingTotal = parseFloat(bookingData.bookingDetails.totalPrice);

                console.log('Single booking total:', singleBookingTotal);

                if (singleBookingTotal > 0) {
                    return singleBookingTotal;
                }
            }
        }

        throw new Error('Could not determine payment amount');
    } catch (error) {
        console.error('Calculate Amount Error:', error);
        throw new Error('Invalid amount format');
    }
}



function showLoading() {
    const loadingDiv = document.createElement('div');
    loadingDiv.id = 'loading';
    loadingDiv.className = 'loading-overlay';
    loadingDiv.innerHTML = '<div class="loading-spinner">Processing payment...</div>';
    document.body.appendChild(loadingDiv);
}

function hideLoading() {
    const loadingDiv = document.getElementById('loading');
    if (loadingDiv) {
        loadingDiv.remove();
    }
}

// Initialize form handlers
// Initialize form on page load
document.addEventListener('DOMContentLoaded', async function () {
    // First populate form fields
    populateFormFields();

    initializePayPal();


    try {
        const amount = calculateTotalAmount();
        console.log('Initial amount calculation:', amount);
    } catch (error) {
        console.error('Initial amount calculation error:', error);
    }

    try {
        // Verify token and user ID
        const userId = getUserIdFromToken();
        console.log('Initial user ID verification:', userId);
    } catch (error) {
        console.error('Token verification error:', error);
        showError('Please log in again');
    }

    const expireMonth = document.getElementById('expire_month');
    if (expireMonth) {
        expireMonth.addEventListener('input', function (e) {
            let value = e.target.value;
            // Remove non-digits
            value = value.replace(/\D/g, '');
            // Ensure it's a valid month
            if (value.length === 2) {
                const month = parseInt(value);
                if (month < 1) value = '01';
                if (month > 12) value = '12';
            }
            e.target.value = value;
        });
    }

    // Format expiry year input
    const expireYear = document.getElementById('expire_year');
    if (expireYear) {
        expireYear.addEventListener('input', function (e) {
            let value = e.target.value;
            // Remove non-digits
            value = value.replace(/\D/g, '');
            // Keep only last two digits if more are entered
            if (value.length > 2) {
                value = value.slice(-2);
            }
            e.target.value = value;
        });
    }

    // Add validation before form submission
    function validateExpiryDate() {
        const month = expireMonth.value.padStart(2, '0');
        const year = expireYear.value.padStart(2, '0');

        // Check if date is in the past
        const today = new Date();
        const expiry = new Date(2000 + parseInt(year), parseInt(month) - 1);

        if (expiry < today) {
            showError('Card has expired');
            return false;
        }

        return true;
    }

    // Add this validation to your validateForm function
    const originalValidateForm = validateForm;
    validateForm = function () {
        return originalValidateForm() && validateExpiryDate();
    };
    // Remove any existing error messages
    const existingErrors = document.querySelectorAll('.alert-danger');
    existingErrors.forEach(error => error.remove());

    // Get and process checkout data
    const urlParams = new URLSearchParams(window.location.search);
    const encryptedData = urlParams.get('data');

    if (!encryptedData) {
        showError('No checkout data found');
        return;
    }

    try {
        const data = JSON.parse(atob(encryptedData));
        console.log('Decoded data:', data);

        if (data.type === 'booking' && data.ids) {
            // Single booking checkout
            console.log('Processing single booking with ID:', data.ids[0]);
            await processSingleCheckout(data);
        } else if (data.bookings || data.cart) {
            // Multiple items checkout
            console.log('Processing multiple items:', data);
            await processMultipleCheckout(data);
        } else {
            throw new Error('Invalid data format');
        }

        // Setup form handler
        const form = document.querySelector('.booking-form-wrap');
        if (form) {
            const bookNowButton = form.querySelector('.button-primary');
            if (bookNowButton) {
                bookNowButton.addEventListener('click', async function (e) {
                    e.preventDefault();
                    if (validateForm()) {
                        await processPayment();
                    }
                });
            }
        }

    } catch (error) {
        console.error('Error processing checkout data:', error);
        showError('Invalid checkout data');
    }

    async function getCartIdByUserId(userId) {
        try {
            const response = await fetch(`${API_BASE_URL}/api/ToursCarts/user/${userId}`, {
                method: 'GET',
                headers: {
                    'accept': 'text/plain'
                }
            });

            if (!response.ok) {
                throw new Error('Failed to fetch cart ID');
            }

            const cartId = await response.json();
            console.log('Retrieved cart ID:', cartId);
            return cartId;
        } catch (error) {
            console.error('Error fetching cart ID:', error);
            return null;
        }
    }

    // paypal
    // Add these new PayPal functions

    // Initialize PayPal
    function initializePayPal() {
        try {
            const amount = calculateTotalAmount();
            console.log('Initializing PayPal with amount:', amount);

            paypal.Buttons({
                createOrder: function (data, actions) {
                    return actions.order.create({
                        purchase_units: [{
                            amount: {
                                value: amount.toString()
                            }
                        }]
                    });
                },
                onApprove: function (data, actions) {
                    return actions.order.capture().then(function (details) {
                        processPayPalPayment(details);
                    });
                },
                onError: function (err) {
                    console.error('PayPal Error:', err);
                    showError('PayPal payment failed. Please try again.');
                }
            }).render('#paypal-button-container')
                .catch(err => {
                    console.error('PayPal render error:', err);
                });
        } catch (error) {
            console.error('PayPal initialization error:', error);
        }
    }

    // Add this new validation function specifically for billing details
    function validateBillingDetails() {
        const requiredFields = {
            'street_1': 'Billing Address',
            'city_booking': 'City',
            'state_booking': 'State',
            'country': 'Country',
            'postal_code': 'Postal Code'
        };

        let isValid = true;
        let errorMessages = new Set();

        for (const [fieldId, fieldName] of Object.entries(requiredFields)) {
            let field;
            if (fieldId === 'country') {
                field = document.querySelector('#country');
            } else {
                field = document.querySelector(`[name="${fieldId}"]`);
            }

            if (!field || !field.value.trim()) {
                errorMessages.add(`${fieldName} is required`);
                isValid = false;
                if (field) {
                    field.classList.add('is-invalid');
                }
            } else {
                if (field) {
                    field.classList.remove('is-invalid');
                }
            }
        }

        if (!isValid) {
            showError(Array.from(errorMessages).join('<br>'));
        }

        return isValid;
    }


    // Process PayPal Payment
    async function processPayPalPayment(details) {
        try {
            if (!validateBillingDetails()) {
                return;
            }

            showLoading();

            // Get booking data from URL
            const urlParams = new URLSearchParams(window.location.search);
            const encryptedData = urlParams.get('data');
            const bookingData = JSON.parse(atob(encryptedData));
            
            // Handle multiple bookings
            const bookings = bookingData.bookings || [bookingData.bookingDetails];

            // Fetch selected options for all bookings
            const bookingsWithOptions = await Promise.all(bookings.map(async (booking) => {
                try {
                    const optionsResponse = await fetch(`${API_BASE_URL}/api/BookingOptionsSelections/byBooking/${booking.bookingId}`);
                    if (optionsResponse.ok) {
                        const options = await optionsResponse.json();
                        return {
                            ...booking,
                            selectedOptions: options
                        };
                    }
                    return booking;
                } catch (error) {
                    console.error(`Error fetching options for booking ${booking.bookingId}:`, error);
                    return booking;
                }
            }));

            // Get billing details
            const billingDetails = {
                street1: document.querySelector('input[name="street_1"]').value.trim(),
                street2: document.querySelector('input[name="street_2"]')?.value.trim(),
                city: document.querySelector('input[name="city_booking"]').value.trim(),
                state: document.querySelector('input[name="state_booking"]').value.trim(),
                country: document.querySelector('#country').value.trim(),
                zipCode: document.querySelector('input[name="postal_code"]').value.trim(),
                additionalNotes: document.getElementById('note').value.trim()

            };

            // Create payment record
            const payment = await createPayPalPaymentRecord(bookingData, details);
            if (!payment) {
                throw new Error('Failed to create PayPal payment record');
            }

            // Create confirmation data
            const confirmationData = {
                payment: {
                    paymentId: payment.paymentId,
                    paymentMethod: 'PayPal',
                    paymentStatus: 'Completed',
                    transactionId: details.id
                },
                booking: {
                    bookings: bookingsWithOptions // Include all bookings with their options
                },
                userDetails: getClaimsFromToken(),
                billingDetails: {
                    country: billingDetails.country,
                    state: billingDetails.state,
                    city: billingDetails.city,
                    zipCode: billingDetails.zipCode,

                    address: [
                        billingDetails.street1,
                        billingDetails.street2,
                        billingDetails.city,
                        billingDetails.state,
                        billingDetails.country
                    ].filter(Boolean).join(', ')
                }
            };

            const encryptedConfirmationData = btoa(JSON.stringify(confirmationData));
            sessionStorage.setItem('confirmationData', encryptedConfirmationData);
            window.location.href = `confirmation.html?data=${encryptedConfirmationData}`;

        } catch (error) {
            console.error('PayPal payment processing error:', error);
            showError('PayPal payment processing failed: ' + error.message);
        } finally {
            hideLoading();
        }
    }



    // Create PayPal Payment Record
    async function createPayPalPaymentRecord(bookingData, paypalDetails) {
        try {
            const amount = calculateTotalAmount();
            const userId = getUserIdFromToken();

            // Get cart ID
            const cartId = await getCartIdByUserId(userId);
            console.log('Cart ID for payment:', cartId);

            // Get booking ID
            let bookingId = null;
            if (bookingData.bookingDetails) {
                bookingId = bookingData.bookingDetails.bookingId;
            } else if (bookingData.bookings && bookingData.bookings.length > 0) {
                bookingId = bookingData.bookings[0].bookingId;
            }

            // Extract PayPal transaction ID
            const transactionId = paypalDetails.id || // For newer PayPal SDK
                paypalDetails.orderID || // Alternative property
                paypalDetails.purchase_units?.[0]?.payments?.captures?.[0]?.id; // Deeper check

            if (!transactionId) {
                console.error('PayPal details:', paypalDetails);
                throw new Error('No transaction ID found in PayPal response');
            }

            const paymentData = {
                bookingID: bookingId,
                userID: userId,
                gatewayID: 1,
                cartId: cartId,
                amount: parseFloat(amount.toFixed(2)),
                paymentMethod: "PayPal",
                paymentStatus: "Completed",
                transactionId: transactionId // Set the transaction ID here
            };

            console.log('PayPal payment data to be sent:', paymentData);

            const response = await fetch(`${API_BASE_URL}/api/Payments`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'accept': 'text/plain'
                },
                body: JSON.stringify(paymentData)
            });

            if (!response.ok) {
                const errorData = await response.json();
                console.error('PayPal Payment API Error:', errorData);
                throw new Error(errorData.message || 'Failed to create PayPal payment record');
            }

            return await response.json();

        } catch (error) {
            console.error('Create PayPal Payment Error:', error);
            throw error;
        }
    }


    function showSuccess(message) {
        // Remove any existing success or error messages
        const existingMessages = document.querySelectorAll('.alert');
        existingMessages.forEach(msg => msg.remove());

        // Create new success message element
        const successDiv = document.createElement('div');
        successDiv.className = 'alert alert-success';
        successDiv.style.cssText = `
        padding: 15px;
        margin-bottom: 20px;
        border: 1px solid #d6e9c6;
        border-radius: 4px;
        color: #3c763d;
        background-color: #dff0d8;
    `;
        successDiv.innerHTML = message;

        // Insert the success message at the top of the form
        const formWrap = document.querySelector('.booking-form-wrap');
        if (formWrap) {
            formWrap.insertBefore(successDiv, formWrap.firstChild);

            // Scroll to the success message
            successDiv.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }

        // Optionally, remove the success message after a few seconds
        setTimeout(() => {
            if (successDiv && successDiv.parentNode) {
                successDiv.remove();
            }
        }, 5000); // Remove after 5 seconds
    }

    // Initialize PayPal when the page loads
    document.addEventListener('DOMContentLoaded', function () {
        // Your existing DOMContentLoaded code remains here...

        // Initialize PayPal
        initializePayPal();
    });



});


function debugToken() {
    const token = sessionStorage.getItem('token');
    if (token) {
        const tokenParts = token.split('.');
        const payload = JSON.parse(atob(tokenParts[1]));
        console.log('Token payload:', payload);
    }
}

// Call debug function on load
debugToken();












function isTokenValid() {
    try {
        const token = sessionStorage.getItem('token');
        if (!token) return false;

        const payload = JSON.parse(atob(token.split('.')[1]));
        const expiry = payload.exp * 1000; // Convert to milliseconds
        return Date.now() < expiry;
    } catch (error) {
        console.error('Token validation error:', error);
        return false;
    }
}

// Helper function to handle token expiry
function handleTokenExpiry() {
    if (!isTokenValid()) {
        sessionStorage.removeItem('token');
        window.location.href = 'login.html';
        return false;
    }
    return true;
}
