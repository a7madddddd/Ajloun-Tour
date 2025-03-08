// Constants
const API_BASE_URL = "https://localhost:44357";
const DEFAULT_IMAGE = `${API_BASE_URL}/assets/images/default.jpg`;

// Get token from session storage
const token = sessionStorage.getItem("token");

// Parse the token to get user ID
const parseJwt = (token) => {
  try {
    return JSON.parse(atob(token.split(".")[1]));
  } catch (e) {
    return null;
  }
};

const tokenPayload = parseJwt(token);
const userId = tokenPayload?.nameid;

// Cache for item details
const itemDetailsCache = new Map();

// Helper Functions
function showLoading() {
  document.querySelector("table tbody").innerHTML = `
        <tr>
            <td colspan="5" class="text-center">
                <div class="loading-spinner">Loading...</div>
            </td>
        </tr>
    `;
}

function showError(message) {
  const errorDiv = document.createElement('div');
  errorDiv.className = 'alert alert-danger';
  errorDiv.textContent = message;

  // Remove any existing error messages
  const existingErrors = document.querySelectorAll('.alert-danger');
  existingErrors.forEach(error => error.remove());

  // Add the new error message
  const container = document.querySelector('.cart-list-inner');
  container.insertBefore(errorDiv, container.firstChild);

  // Remove the error message after 3 seconds
  setTimeout(() => {
    errorDiv.remove();
  }, 3000);
}

// API Functions
async function getCartByUserId() {
  try {
    const response = await fetch(
      `${API_BASE_URL}/api/ToursCarts/user/${userId}`,
      {
        method: "GET",
        headers: {
          accept: "text/plain",
        },
      }
    );

    if (!response.ok) {
      throw new Error("Failed to fetch cart");
    }

    const cartId = await response.json();
    return cartId;
  } catch (error) {
    console.error("Error fetching cart:", error);
    return null;
  }
}

async function getItemDetails(type, id) {
  const cacheKey = `${type}-${id}`;

  if (itemDetailsCache.has(cacheKey)) {
    return itemDetailsCache.get(cacheKey);
  }

  try {
    const response = await fetch(`${API_BASE_URL}/api/${type}/id?id=${id}`);
    if (!response.ok) {
      throw new Error(`Failed to fetch ${type} details`);
    }
    const data = await response.json();
    itemDetailsCache.set(cacheKey, data);
    return data;
  } catch (error) {
    console.error(`Error fetching ${type} details:`, error);
    return null;
  }
}

async function fetchCartItems(cartId) {
  showLoading();
  try {
    const response = await fetch(
      `${API_BASE_URL}/api/TourCartItems/cart/${cartId}`,
      {
        method: "GET",
        headers: {
          accept: "text/plain",
        },
      }
    );

    if (!response.ok) {
      throw new Error("Failed to fetch cart items");
    }

    const cartItemsData = await response.json();
    if (!cartItemsData.$values || cartItemsData.$values.length === 0) {
      document.querySelector("table tbody").innerHTML =
        '<tr><td colspan="5" class="text-center">Your cart is empty</td></tr>';
      return;
    }

    await displayCartItems(cartItemsData.$values);
    updateTotals(cartItemsData.$values);
  } catch (error) {
    console.error("Error fetching cart items:", error);
    showError("Failed to load cart items");
  }
}

async function getImageUrl(item) {
  let imageDetails;

  try {
    if (item.tourId) {
      imageDetails = await getItemDetails("Tours", item.tourId);
      return imageDetails?.tourImage
        ? `${API_BASE_URL}/ToursImages/${imageDetails.tourImage}`
        : DEFAULT_IMAGE;
    } else if (item.packageId) {
      imageDetails = await getItemDetails("Packages", item.packageId);
      return imageDetails?.image
        ? `${API_BASE_URL}/PakagesImages/${imageDetails.image}`
        : DEFAULT_IMAGE;
    } else if (item.offerId) {
      imageDetails = await getItemDetails("Offers", item.offerId);
      return imageDetails?.image
        ? `${API_BASE_URL}/OffersImages/${imageDetails.image}`
        : DEFAULT_IMAGE;
    }
    return DEFAULT_IMAGE;
  } catch (error) {
    console.error("Error getting image URL:", error);
    return DEFAULT_IMAGE;
  }
}

async function removeBooking(bookingId) {
  if (!confirmDelete("Are you sure you want to cancel this booking?")) {
    return;
  }

  try {
    const response = await fetch(`${API_BASE_URL}/api/Bookings/${bookingId}`, {
      method: "DELETE",
      headers: {
        accept: "text/plain",
      },
    });

    if (!response.ok) {
      throw new Error("Failed to remove booking");
    }

    const cartId = await getCartByUserId();
    if (cartId) {
      await fetchCartItems(cartId);
    }
  } catch (error) {
    console.error("Error:", error);
    showError("Failed to remove booking");
  }
}

// Display Functions
async function displayCartItems(cartItems) {
  const tbody = document.querySelector("table tbody");
  tbody.innerHTML = ""; // Clear existing items

  try {
    const rows = await Promise.all(
      cartItems.map(async (item) => {
        if (item.isFromBooking) {
          return await createBookingRow(item);
        } else {
          return await createCartItemRow(item);
        }
      })
    );

    tbody.innerHTML = rows.join("");
  } catch (error) {
    console.error("Error displaying cart items:", error);
    showError("Error loading cart items");
  }
}

async function createCartItemRow(item) {
  const imageUrl = await getImageUrl(item);
  let itemName = "";

  if (item.tourId) {
    const tourDetails = await getItemDetails("Tours", item.tourId);
    itemName = tourDetails?.tourName || `Tour ID: ${item.tourId}`;
  } else if (item.packageId) {
    const packageDetails = await getItemDetails("Packages", item.packageId);
    itemName = packageDetails?.name || `Package ID: ${item.packageId}`;
  } else if (item.offerId) {
    const offerDetails = await getItemDetails("Offers", item.offerId);
    itemName = offerDetails?.title || `Offer ID: ${item.offerId}`;
  }

  return `
        <tr class="cart-item" data-id="${item.cartItemId}">
            <td class="">
                <button class="close" onclick="removeCartItem(${
                  item.cartItemId
                })" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">×</span>
                </button>
                <span class="cartImage">
                    <img src="${imageUrl}" alt="${itemName}" onerror="this.src='${DEFAULT_IMAGE}'">
                </span>
            </td>
            <td data-column="Product Name">
                <h6>${itemName}</h6>
            </td>
            <td data-column="Price">$ ${item.price.toFixed(2)}</td>
            <td data-column="Quantity" class="count-input">
                <div>
                    <a class="minus-btn" onclick="updateQuantity(${
                      item.cartItemId
                    }, -1)">
                        <i class="fa fa-minus"></i>
                    </a>
                    <input class="quantity" type="text" value="${
                      item.quantity
                    }" id="qty-${item.cartItemId}">
                    <a class="plus-btn" onclick="updateQuantity(${
                      item.cartItemId
                    }, 1)">
                        <i class="fa fa-plus"></i>
                    </a>
                </div>
            </td>
            <td data-column="Sub Total" class="total-column">
                <div class="price-info">
                    <div class="main-price">$ ${(item.price * item.quantity).toFixed(2)}</div>
                </div>
                <a class="checkout-btn" data-type="cart" data-id="${item.cartItemId}">
                    Checkout
                </a>
            </td>
        </tr>
    `;
}

async function createBookingRow(item) {
    const booking = item.bookingDetails;
    const selectedOptions = booking.selectedOptions.$values || [];
    const imageUrl = await getImageUrl(booking);

    let itemName = '';
    if (booking.tourId) {
        const tourDetails = await getItemDetails('Tours', booking.tourId);
        itemName = tourDetails?.tourName || `Tour ID: ${booking.tourId}`;
    } else if (booking.packageId) {
        const packageDetails = await getItemDetails('Packages', booking.packageId);
        itemName = packageDetails?.name || `Package ID: ${booking.packageId}`;
    } else if (booking.offerId) {
        const offerDetails = await getItemDetails('Offers', booking.offerId);
        itemName = offerDetails?.title || `Offer ID: ${booking.offerId}`;
    }

    // Calculate total options price
    const optionsTotal = selectedOptions.reduce((sum, option) => sum + option.optionPrice, 0);

    return `
        <tr class="booking-item" data-id="${booking.bookingId}">
            <td class="">
                <button class="close" onclick="removeBooking(${booking.bookingId})" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">×</span>
                </button>
                <span class="bookingImage">
                    <img src="${imageUrl}" alt="${itemName}" onerror="this.src='${DEFAULT_IMAGE}'">
                </span>
            </td>
            <td data-column="Booking Details">
                <div class="booking-info">
                    <h5>${itemName}</h5>
                    <p>Booking #${booking.bookingId}</p>
                    <p>Status: <span class="badge ${booking.status.toLowerCase()}">${booking.status}</span></p>
                    <p>Date: ${new Date(booking.bookingDate).toLocaleDateString()}</p>
                    <p>People: ${booking.numberOfPeople}</p>
                    ${selectedOptions.length > 0 ? `
                        <div class="selected-options">
                            <p><strong>Selected Options:</strong></p>
                            <ul>
                                ${selectedOptions.map(option => `
                                    <li>${option.optionName} - $${option.optionPrice.toFixed(2)}</li>
                                `).join('')}
                            </ul>
                        </div>
                    ` : ''}
                </div>
            </td>
            <td data-column="Price">$ ${booking.totalPrice.toFixed(2)}</td>
            <td data-column="People">${booking.numberOfPeople}</td>
              <td data-column="Total" class="total-column">
                <div class="price-info">
                    <div class="main-price">$ ${booking.totalPrice.toFixed(2)}</div>
                    ${optionsTotal > 0 ? `
                        <div class="options-price">
                            <strong>Options Total:</strong> $${optionsTotal.toFixed(2)}
                        </div>
                    ` : ''}
                    <div class="total-with-options">
                        <strong>Total:</strong> $${(booking.totalPrice + optionsTotal).toFixed(2)}
                    </div>
                </div>
                <a class="checkout-btn" data-type="booking" data-id="${booking.bookingId}">
                    Checkout
                </a>
            </td>
        </tr>
    `;
}

// Function to encrypt data (if needed)
function encryptIds(ids) {
  return btoa(JSON.stringify(ids));
}

// Single unified checkout function
async function checkoutItem(type, id) {
  try {
    let data;

    if (type === 'booking') {
      // Fetch booking details
      const bookingResponse = await fetch(`${API_BASE_URL}/api/Bookings/id?id=${id}`);
      if (!bookingResponse.ok) throw new Error('Failed to fetch booking');
      const bookingData = await bookingResponse.json();

      data = {
        type: type,
        ids: [id],
        bookingDetails: bookingData
      };
    } else if (type === 'cart') {
      data = {
        type: type,
        ids: [id]
      };
    }

    // Encrypt and redirect
    const encryptedData = btoa(JSON.stringify(data));
    window.location.href = `booking.html?data=${encryptedData}`;

  } catch (error) {
    console.error("Error during checkout:", error);
    showError("Failed to process checkout");
  }
}

// Function for checking out all items
// Function for checking out all items
async function checkoutAll() {
  try {
    const cartItems = document.querySelectorAll('.cart-item');
    const bookingItems = document.querySelectorAll('.booking-item');

    if (cartItems.length === 0 && bookingItems.length === 0) {
      showError('No items to checkout');
      return;
    }

    // Get IDs from data attributes
    const cartIds = Array.from(cartItems)
      .map(item => item.getAttribute('data-id'))
      .filter(id => id);

    const bookingIds = Array.from(bookingItems)
      .map(item => item.getAttribute('data-id'))
      .filter(id => id);

    // Fetch details for all bookings
    const bookingsData = await Promise.all(
      bookingIds.map(async (id) => {
        try {
          // Fetch booking details
          const bookingResponse = await fetch(`${API_BASE_URL}/api/Bookings/id?id=${id}`);
          if (!bookingResponse.ok) throw new Error(`Failed to fetch booking ${id}`);
          const bookingData = await bookingResponse.json();

          // The options are already included in the booking data
          // No need to fetch them separately
          return bookingData;
        } catch (error) {
          console.error(`Error fetching booking ${id}:`, error);
          return null;
        }
      })
    );

    // Filter out any null values from failed fetches
    const validBookings = bookingsData.filter(booking => booking !== null);

    const data = {
      cart: cartIds,
      bookings: validBookings
    };

    // Encrypt and redirect
    const encryptedData = btoa(JSON.stringify(data));
    window.location.href = `booking.html?data=${encryptedData}`;

  } catch (error) {
    console.error("Error during checkout:", error);
    showError("Failed to process checkout");
  }
}

// Modify the bottom checkout button handler
document
  .querySelector(".checkBtnArea .button-primary")
  .addEventListener("click", async (e) => {
    e.preventDefault();
    checkoutAll();
  });

function confirmDelete(message) {
  return window.confirm(message);
}
function updateTotals(cartItems) {
  let cartTotal = 0;
  let bookingsTotal = 0;
  let optionsTotal = 0;

  cartItems.forEach((item) => {
    if (item.isFromBooking) {
      bookingsTotal += item.bookingDetails.totalPrice;
      const selectedOptions = item.bookingDetails.selectedOptions.$values || [];
      optionsTotal += selectedOptions.reduce(
        (sum, option) => sum + option.optionPrice,
        0
      );
    } else {
      cartTotal += item.price * item.quantity;
    }
  });

  const grandTotal = cartTotal + bookingsTotal + optionsTotal;

  document.querySelector(".totalAmountArea").innerHTML = `
        <ul class="list-unstyled">
            ${cartTotal > 0
      ? `<li><strong>Cart Items Total</strong> <span>$ ${cartTotal.toFixed(
        2
      )}</span></li>`
      : ""
    }
            ${bookingsTotal > 0
      ? `<li><strong>Bookings Total</strong> <span>$ ${bookingsTotal.toFixed(
        2
      )}</span></li>`
      : ""
    }
            ${optionsTotal > 0
      ? `<li><strong>Options Total</strong> <span>$ ${optionsTotal.toFixed(
        2
      )}</span></li>`
      : ""
    }
            <li><strong>Total cost</strong> <span class="grandTotal">$ ${grandTotal.toFixed(
      2
    )}</span></li>
        </ul>
    `;
}

// Cart Actions
async function updateQuantity(cartItemId, change) {
  const quantityInput = document.getElementById(`qty-${cartItemId}`);
  let newQuantity = parseInt(quantityInput.value) + change;

  if (newQuantity < 1) newQuantity = 1;

  try {
    const response = await fetch(
      `${API_BASE_URL}/api/TourCartItems/${cartItemId}`,
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          quantity: newQuantity,
        }),
      }
    );

    if (!response.ok) {
      throw new Error("Failed to update quantity");
    }

    const cartId = await getCartByUserId();
    if (cartId) {
      await fetchCartItems(cartId);
    }
  } catch (error) {
    console.error("Error:", error);
    showError("Failed to update quantity");
  }
}

async function removeCartItem(cartItemId) {
  if (
    !confirmDelete("Are you sure you want to remove this item from your cart?")
  ) {
    return;
  }

  try {
    const response = await fetch(
      `${API_BASE_URL}/api/TourCartItems/${cartItemId}`,
      {
        method: "DELETE",
        headers: {
          accept: "text/plain",
        },
      }
    );

    if (!response.ok) {
      throw new Error("Failed to remove item");
    }

    const cartId = await getCartByUserId();
    if (cartId) {
      await fetchCartItems(cartId);
    }
  } catch (error) {
    console.error("Error:", error);
    showError("Failed to remove item");
  }
}

// Initialize cart on page load
document.addEventListener("DOMContentLoaded", async () => {
  if (!token) {
    window.location.href = "login.html";
    return;
  }

  try {
    const cartId = await getCartByUserId();
    if (cartId) {
      await fetchCartItems(cartId);
      setupCheckoutButtons(); // Setup checkout buttons after content is loaded
    } else {
      showError("No cart found");
    }
  } catch (error) {
    console.error("Error initializing cart:", error);
    showError("Failed to load cart");
  }

  // Main checkout button
  const mainCheckoutBtn = document.querySelector('.checkBtnArea .button-primary');
  if (mainCheckoutBtn) {
    mainCheckoutBtn.addEventListener('click', async (e) => {
      e.preventDefault();
      await checkoutAll();
    });
  }
});

// Handle checkout button
// Update the click handlers
function setupCheckoutButtons() {
  // Individual checkout buttons
  document.querySelectorAll('.checkout-btn').forEach(button => {
    button.addEventListener('click', function (e) {
      e.preventDefault();
      const type = this.getAttribute('data-type');
      const id = this.getAttribute('data-id');
      if (type && id) {
        checkoutItem(type, id);
      }
    });
  });
}
