document.addEventListener('DOMContentLoaded', () => {
    const urlParams = new URLSearchParams(window.location.search);
    const encryptedData = urlParams.get('data');

    if (!encryptedData) {
        showError('No checkout data found');
        return;
    }

    try {
        const data = JSON.parse(atob(encryptedData));

        if (data.type && data.ids) {
            // Single item checkout
            console.log(`Processing ${data.type} checkout for ID: ${data.ids[0]}`);
            processSingleCheckout(data.type, data.ids[0]);
        } else {
            // Multiple items checkout
            console.log('Processing multiple items:', data);
            processMultipleCheckout(data.cart, data.bookings);
        }
    } catch (error) {
        console.error('Error processing checkout data:', error);
        showError('Invalid checkout data');
    }
});

function showError(message) {
    const contentDiv = document.getElementById('checkout-content') || document.body;
    contentDiv.innerHTML = `<div class="error-message">${message}</div>`;
}

async function processSingleCheckout(type, id) {
    // Add your checkout logic here
    console.log(`Processing single ${type} checkout for ID: ${id}`);
}

async function processMultipleCheckout(cartIds, bookingIds) {
    // Add your checkout logic here
    console.log('Processing cart IDs:', cartIds);
    console.log('Processing booking IDs:', bookingIds);
}