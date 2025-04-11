document.addEventListener('DOMContentLoaded', function () {
    // Initial load
    initializeFilters();
    loadCategories();
    state.currentPage = 1;
    loadProducts();
    loadRecentProducts();
    loadProductGallery();

    // Handle sorting
    const orderBySelect = document.querySelector('select[name="orderby"]');
    orderBySelect.addEventListener('change', loadProducts);

    const resetButton = document.createElement('button');
    resetButton.className = 'reset-filter button-primary ';
    resetButton.textContent = 'Show All';
    resetButton.onclick = function () {
        state.selectedCategory = null;
        state.currentPage = 1;
        document.querySelectorAll('.widget_category_product_thumb li').forEach(li => {
            li.classList.remove('active');
        });
        loadProducts();
    };
    document.querySelector('.widget_category_product_thumb').prepend(resetButton);
});

// Global state for filters
const state = {
    searchTerm: '',
    priceRange: { min: 0, max: 500 },
    selectedCategory: null,
    currentPage: 1,
    itemsPerPage: 6,
    totalPages: 1
};

async function initializeFilters() {
    // Initialize price range slider
    const slider = document.getElementById('slider-range');
    const maxAmountInput = document.getElementById('maxAmount');

    // Initialize jQuery UI slider if you're using it
    $(slider).slider({
        range: true,
        min: 0,
        max: 500,
        values: [0, 500],
        slide: function (event, ui) {
            state.priceRange.min = ui.values[0];
            state.priceRange.max = ui.values[1];
            maxAmountInput.value = `${ui.values[0]} - ${ui.values[1]}`;
        }
    });

    // Initialize search
    const searchInput = document.querySelector('input[name="search"]');
    searchInput.addEventListener('input', (e) => {
        state.searchTerm = e.target.value;
        debounce(loadProducts, 300)();
    });

    // Initialize price filter button
    const filterButton = document.querySelector('.price-handel button');
    filterButton.addEventListener('click', (e) => {
        e.preventDefault();
        loadProducts();
    });
}
async function loadProductGallery() {
    try {
        const response = await fetch('https://localhost:44357/api/Products?includeInactive=true');
        const data = await response.json();
        const products = data.$values || [];

        // Collect all product images
        let allImages = [];
        products.forEach(product => {
            if (product.images?.$values) {
                allImages = allImages.concat(product.images.$values);
            }
        });

        // Take first 6 images or fill with default if less than 6
        const galleryImages = allImages.slice(0, 6);
        while (galleryImages.length < 6) {
            galleryImages.push({ imageURL: 'assets/images/default-product.jpg' });
        }

        // Get the gallery container
        const galleryContainer = document.querySelector('.widget_gallery .gallery');

        // Update gallery HTML
        galleryContainer.innerHTML = galleryImages.map(image => `
            <figure class="gallery-item">
                <a href="#" class="gallery-link">
                    <img src="https://localhost:44357/${image.imageURL}" alt="Product Image" class="gallery-image">
                </a>
            </figure>
        `).join('');

    } catch (error) {
        console.error('Error loading gallery images:', error);
    }
}


async function loadCategories() {
    try {
        const response = await fetch('https://localhost:44357/api/Categories');
        const data = await response.json();
        const categories = data.$values || [];

        const categoryList = document.querySelector('.widget_category_product_thumb ul');
        categoryList.innerHTML = categories.map(category => `
            <li data-category-id="${category.categoryId}">
                <figure class="product-thumb">
                    <a href="#">
                    </a>
                </figure>
                <div class="product-content">
                    <h5>${category.name}</h5>
                    <span class="count">(0)</span>
                </div>
            </li>
        `).join('');

        // Add click handlers for categories
        categoryList.querySelectorAll('li').forEach(li => {
            li.addEventListener('click', (e) => {
                e.preventDefault();

                // Remove active class from all categories
                categoryList.querySelectorAll('li').forEach(item => {
                    item.classList.remove('active');
                });

                // Add active class to clicked category
                li.classList.add('active');

                // Update selected category
                state.selectedCategory = li.dataset.categoryId;
                state.currentPage = 1; // Reset to first page
                loadProducts();
            });
        });

        // Initial load of products to update category counts
        loadProducts();

    } catch (error) {
        console.error('Error loading categories:', error);
    }
}

async function loadProducts() {
    setLoadingState(true);

    try {
        const response = await fetch('https://localhost:44357/api/Products?includeInactive=true');
        const data = await response.json();
        let products = data.$values || [];

        // Apply filters
        products = products.filter(product => {
            const matchesSearch = !state.searchTerm ||
                product.name.toLowerCase().includes(state.searchTerm.toLowerCase());
            const matchesCategory = !state.selectedCategory ||
                product.categoryId === parseInt(state.selectedCategory);
            const matchesPrice = (!state.priceRange.min || product.price >= state.priceRange.min) &&
                (!state.priceRange.max || product.price <= state.priceRange.max);

            return matchesSearch && matchesCategory && matchesPrice;
        });

        // Update category counts
        updateCategoryCounts(products);

        // Calculate pagination
        state.totalPages = Math.ceil(products.length / state.itemsPerPage);

        // Get products for current page
        const startIndex = (state.currentPage - 1) * state.itemsPerPage;
        const paginatedProducts = products.slice(startIndex, startIndex + state.itemsPerPage);

        // Update product display
        const productContainer = document.querySelector('.product-item-wrapper .row');
        productContainer.innerHTML = paginatedProducts.map(product => createProductHTML(product)).join('');

        // Update pagination
        updatePagination();

        // Update result count
        document.querySelector('.product-result-count').textContent =
            `Showing ${startIndex + 1}-${Math.min(startIndex + state.itemsPerPage, products.length)} of ${products.length} results`;

    } catch (error) {
        console.error('Error loading products:', error);
    } finally {
        setLoadingState(false);
    }
}

// Function to update pagination UI
function updatePagination() {
    const paginationContainer = document.querySelector('.post-navigation-wrap nav ul.pagination');
    let paginationHTML = '';

    // Previous arrow
    paginationHTML += `
        <li>
            <a href="#" onclick="changePage(${state.currentPage - 1}); return false;" 
               ${state.currentPage === 1 ? 'style="opacity: 0.5; pointer-events: none;"' : ''}>
                <i class="fas fa-arrow-left"></i>
            </a>
        </li>
    `;

    // Page numbers
    for (let i = 1; i <= state.totalPages; i++) {
        paginationHTML += `
            <li ${state.currentPage === i ? 'class="active"' : ''}>
                <a href="#" onclick="changePage(${i}); return false;">${i}</a>
            </li>
        `;
    }

    // Next arrow
    paginationHTML += `
        <li>
            <a href="#" onclick="changePage(${state.currentPage + 1}); return false;"
               ${state.currentPage === state.totalPages ? 'style="opacity: 0.5; pointer-events: none;"' : ''}>
                <i class="fas fa-arrow-right"></i>
            </a>
        </li>
    `;

    paginationContainer.innerHTML = paginationHTML;
}

function changePage(newPage) {
    if (newPage < 1 || newPage > state.totalPages || newPage === state.currentPage) {
        return;
    }

    state.currentPage = newPage;

    // Smooth scroll to top of products
    document.querySelector('.product-item-wrapper').scrollIntoView({
        behavior: 'smooth',
        block: 'start'
    });

    loadProducts();
}


function createProductHTML(product) {
    // Get the first image URL from the nested $values array
    const imageUrl = product.images?.$values?.[0]?.imageURL || 'assets/images/default-product.jpg';

    // Check if there's a discount
    const hasDiscount = product.discountPrice < product.price;

    return `
        <div class="col-sm-6">
            <div class="product-item text-center">
                <figure class="product-image">
                    <a href="#">
                        <img src="https://localhost:44357/${imageUrl}" alt="${product.name || 'Product'}">
                    </a>
                    ${hasDiscount ? '<span class="sale">Sale</span>' : ''}
                </figure>
                <div class="product-content">
                    <h3 class="product-title">
                        <a href="product-detail.html?id=${product.productId}">${product.name}</a>
                    </h3>
                    <div class="product-price">
                        ${hasDiscount
            ? `<del>$${product.price.toFixed(2)}</del> <span class="current-price">$${product.discountPrice.toFixed(2)}</span>`
            : `<span class="current-price">$${product.price.toFixed(2)}</span>`
        }
                    </div>
                      <button
                    onclick="addToCart(${product.productId}, ${product.price})" 
                    class="button-primary add-to-cart-btn">
                    Add to cart
                </button>
                </div>
            </div>
        </div>
    `;
}
async function loadRecentProducts() {
    try {
        const response = await fetch('https://localhost:44357/api/Products?includeInactive=true');
        const data = await response.json();
        const products = (data.$values || [])
            .sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))
            .slice(0, 3); // Get latest 3 products

        const recentProductsList = document.querySelector('.widget_product_post ul');
        recentProductsList.innerHTML = products.map(product => `
            <li>
                <figure class="product-thumb">
                    <a href="#">
                        <img src="https://localhost:44357${product.images?.$values?.[0]?.imageURL || 'assets/images/product8.jpg'}"
                             alt="${product.name}">
                    </a>
                </figure>
                <div class="product-content">
                    <h5>
                        <a href="#">${product.name}</a>
                    </h5>
                    <div class="entry-meta">
                        <span class="byline">
                            <a href="#">${product.categoryName || 'Uncategorized'}</a>
                        </span>
                        <span class="posted-on">
                            <a href="#">${new Date(product.createdAt).toLocaleDateString()}</a>
                        </span>
                    </div>
                </div>
            </li>
        `).join('');

    } catch (error) {
        console.error('Error loading recent products:', error);
    }
}

function updateCategoryCounts(products) {
    // Calculate counts for each category
    const categoryCounts = products.reduce((acc, product) => {
        acc[product.categoryId] = (acc[product.categoryId] || 0) + 1;
        return acc;
    }, {});

    // Update the count display for each category
    document.querySelectorAll('.widget_category_product_thumb li').forEach(li => {
        const categoryId = parseInt(li.dataset.categoryId);
        const countSpan = li.querySelector('.count');
        if (countSpan) {
            countSpan.textContent = `(${categoryCounts[categoryId] || 0})`;
        }
    });
}

function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

function sortProducts(products, orderBy) {
    if (!Array.isArray(products)) {
        console.error('Products is not an array:', products);
        return [];
    }

    const productsArray = [...products];

    switch (orderBy) {
        case 'popularity':
            return productsArray;

        case 'rating':
            return productsArray;

        case 'date':
            return productsArray.sort((a, b) =>
                new Date(b.createdAt) - new Date(a.createdAt));

        case 'price':
            return productsArray.sort((a, b) =>
                parseFloat(a.price) - parseFloat(b.price));

        case 'price-desc':
            return productsArray.sort((a, b) =>
                parseFloat(b.price) - parseFloat(a.price));

        default: // menu_order
            return productsArray;
    }
}

function setLoadingState(isLoading) {
    const wrapper = document.querySelector('.product-item-wrapper');
    if (isLoading) {
        wrapper.classList.add('loading');
    } else {
        wrapper.classList.remove('loading');
    }
}

// Updated CSS to match the Sleeping bag style
const styles = `
    <style>
     
  .widget_category_product_thumb li {
            cursor: pointer;
            transition: opacity 0.3s;
        }
        .widget_category_product_thumb li:hover {
            opacity: 0.8;
        }

        .product-item:hover {
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
        }

        .product-image {
            position: relative;
        }

        .product-image img {
            width: 100%;
            height: 250px;
            object-fit: cover;
            display: block;
        }

        .sale {
            position: absolute;
            top: 10px;
            right: 10px;
            background-color: #0099cc;
            color: white;
            padding: 5px 10px;
            font-size: 12px;
            text-transform: uppercase;
        }

        .product-title {
            font-size: 18px;
        }

        .product-title a {
            color: #333;
            text-decoration: none;
        }

        .product-title a:hover {
            color: #0099cc;
        }

        .product-price {
            font-size: 16px;
        }

        .product-price del {
            color: #999;
            margin-right: 5px;
            font-size: 14px;
        }

        .current-price {
            color: #0099cc;
            font-weight: bold;
        }

        .add-to-cart-button {
            background: #ff6b6b;
            color: white;
            border: none;
            padding: 10px 20px;
            border-radius: 3px;
            cursor: pointer;
            transition: background 0.3s ease;
            text-transform: uppercase;
            font-size: 14px;
            margin-top: 10px;
        }

        .add-to-cart-button:hover {
            background: #ff5252;
        }

        /* Responsive adjustments */
        @media (max-width: 768px) {
            .col-sm-6 {
                width: 100%;
            }
            
            .product-image img {
                height: 200px;
            }
        }
                .alert {
            position: fixed;
            top: 20px;
            right: 20px;
            padding: 15px;
            border-radius: 4px;
            z-index: 1000;
            animation: slideIn 0.3s ease-out;
        }

        .alert-success {
            background-color: #d4edda;
            border-color: #c3e6cb;
            color: #155724;
        }

        .alert-danger {
            background-color: #f8d7da;
            border-color: #f5c6cb;
            color: #721c24;
        }

        .close-btn {
            background: none;
            border: none;
            float: right;
            font-size: 1.5rem;
            font-weight: bold;
            line-height: 1;
            color: inherit;
            opacity: 0.5;
            cursor: pointer;
        }

        .close-btn:hover {
            opacity: 1;
        }

        @keyframes slideIn {
            from {
                transform: translateX(100%);
                opacity: 0;
            }
            to {
                transform: translateX(0);
                opacity: 1;
            }
        }

        .add-to-cart-btn {
            position: relative;
        }

        .add-to-cart-btn.loading {
            opacity: 0.7;
            pointer-events: none;
        }

        .add-to-cart-btn.loading::after {
            content: '';
            position: absolute;
            width: 16px;
            height: 16px;
            top: 50%;
            left: 50%;
            margin: -8px 0 0 -8px;
            border: 2px solid transparent;
            border-top-color: #ffffff;
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }

        @keyframes spin {
            to { transform: rotate(360deg); }
        }
    </style>
`;




// Utility function to decode JWT token
function parseJwt(token) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        return JSON.parse(jsonPayload);
    } catch (e) {
        return null;
    }
}

// Function to check if user is logged in
function isUserLoggedIn() {
    const token = sessionStorage.getItem('token');
    if (!token) return false;

    // Check if token is expired
    const tokenData = parseJwt(token);
    if (!tokenData) return false;

    const expirationDate = new Date(tokenData.exp * 1000);
    return expirationDate > new Date();
}

// Function to get user ID from token
function getUserIdFromToken() {
    const token = sessionStorage.getItem('token');
    if (!token) return null;

    const tokenData = parseJwt(token);
    return tokenData?.nameid;
}

// Function to get cart ID for user
async function getUserCartId(userId) {
    try {
        const response = await fetch(`https://localhost:44357/api/ToursCarts/user/${userId}`, {
            headers: {
                'Authorization': `Bearer ${sessionStorage.getItem('token')}`
            }
        });

        if (!response.ok) throw new Error('Failed to get cart ID');

        const cartId = await response.json();
        return cartId;
    } catch (error) {
        console.error('Error getting cart ID:', error);
        throw error;
    }
}

// Function to add item to cart
async function addToCart(productId, price) {
    try {
        // Check if user is logged in
        if (!isUserLoggedIn()) {
            // Show login modal or redirect to login page
            showLoginModal();
            return;
        }

        // Get user ID and cart ID
        const userId = getUserIdFromToken();
        if (!userId) {
            throw new Error('User ID not found in token');
        }

        const cartId = await getUserCartId(userId);

        // Prepare cart item data
        const cartItem = {
            cartId: cartId,
            quantity: 1, // Default quantity, you might want to make this configurable
            price: price,
            selectedDate: new Date().toISOString(),
            productId: productId
        };

        // Add item to cart
        const response = await fetch('https://localhost:44357/api/TourCartItems/AddByProduct', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem('token')}`
            },
            body: JSON.stringify(cartItem)
        });

        if (!response.ok) throw new Error('Failed to add item to cart');

        // Show success message
        showSuccessMessage('Item added to cart successfully!');

    } catch (error) {
        console.error('Error adding to cart:', error);
        showErrorMessage('Failed to add item to cart. Please try again.');
    }
}

// UI Helper functions
function showLoginModal() {
    Swal.fire({
        title: 'Please log in',
        text: 'You need to log in to add items to the cart',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Login',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = 'Login.html';
        }
    });
}
function showSuccessMessage(message) {
    // Implement your success message UI
    const successDiv = document.createElement('div');
    successDiv.className = 'alert alert-success';
    successDiv.innerHTML = `
        <div class="success-message">
            ${message}
            <button class="close-btn">&times;</button>
        </div>
    `;
    document.body.appendChild(successDiv);

    // Remove after 3 seconds
    setTimeout(() => {
        successDiv.remove();
    }, 3000);
}

function showErrorMessage(message) {
    // Implement your error message UI
    const errorDiv = document.createElement('div');
    errorDiv.className = 'alert alert-danger';
    errorDiv.innerHTML = `
        <div class="error-message">
            ${message}
            <button class="close-btn">&times;</button>
        </div>
    `;
    document.body.appendChild(errorDiv);

    // Remove after 3 seconds
    setTimeout(() => {
        errorDiv.remove();
    }, 3000);
}



// Add styles to document head
document.head.insertAdjacentHTML('beforeend', styles);