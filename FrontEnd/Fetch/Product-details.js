class CartManager {
    constructor() {
        this.token = sessionStorage.getItem('token');
        this.userId = this.getUserIdFromToken();
    }

    // Extract user ID from JWT token
    getUserIdFromToken() {
        if (!this.token) return null;

        try {
            const payload = JSON.parse(atob(this.token.split('.')[1]));
            return payload.nameid;
        } catch (e) {
            console.error('Error parsing token:', e);
            return null;
        }
    }

    // Get cart ID for the user
    async getCartId() {
        try {
            const response = await fetch(`https://localhost:44357/api/ToursCarts/user/${this.userId}`, {
                headers: {
                    'Authorization': `Bearer ${this.token}`,
                    'accept': 'text/plain'
                }
            });

            if (!response.ok) throw new Error('Failed to get cart ID');
            return await response.json();
        } catch (error) {
            console.error('Error getting cart ID:', error);
            throw error;
        }
    }

    // Add product to cart
    async addToCart(productId, quantity, price) {
        try {
            const cartId = await this.getCartId();

            const cartItem = {
                cartId: cartId,
                quantity: quantity,
                price: price,
                selectedDate: new Date().toISOString(),
                productId: productId
            };

            const response = await fetch('https://localhost:44357/api/TourCartItems/AddByProduct', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${this.token}`,
                    'Content-Type': 'application/json',
                    'accept': 'text/plain'
                },
                body: JSON.stringify(cartItem)
            });

            if (!response.ok) throw new Error('Failed to add item to cart');
            return await response.json();
        } catch (error) {
            console.error('Error adding to cart:', error);
            throw error;
        }
    }
}


class ProductDetail {
    constructor() {
        this.cartManager = new CartManager();
        this.init();
        this.setupEventListeners();
    }

    init() {
        // Get product ID from URL
        const urlParams = new URLSearchParams(window.location.search);
        const productId = urlParams.get('id');
        if (productId) {
            this.fetchProductData(productId);
        } else {
            console.error('No product ID found in URL');
        }
    }

    setupEventListeners() {
        const addToCartForm = document.querySelector('.cart-item');
        if (addToCartForm) {
            addToCartForm.addEventListener('submit', async (e) => {
                e.preventDefault();
                await this.handleAddToCart();
            });
        }

        // Add click handlers for thumbnails
        document.querySelector('.product-thumb-nav')?.addEventListener('click', (e) => {
            const thumbnail = e.target.closest('.single-product-item');
            if (thumbnail) {
                const index = Array.from(thumbnail.parentElement.children).indexOf(thumbnail);
                this.updateMainImage(index);
            }
        });
    }

    updateMainImage(index) {
        if (this.currentProduct?.images?.$values[index]) {
            const image = this.currentProduct.images.$values[index];
            const mainImage = document.querySelector('.product-thumbnails .feature-image img');
            const lightboxLink = document.querySelector('.product-thumbnails .image-search-icon a');

            if (mainImage) {
                mainImage.src = `https://localhost:44357/${image.imageURL}`;
            }
            if (lightboxLink) {
                lightboxLink.href = `https://localhost:44357/${image.imageURL}`;
            }
        }
    }

    fetchProductData(productId) {
        const apiUrl = `https://localhost:44357/api/Products/${productId}`;
        console.log('Fetching from:', apiUrl);

        fetch(apiUrl)
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                if (!data) {
                    throw new Error('No data received');
                }
                console.log('Received data:', data);
                this.updateUI(data);
            })
            .catch(error => {
                console.error('Error fetching product:', error);
                alert('Failed to load product details. Please try again.');
            });
    }

    async handleAddToCart() {
        try {

            const token = sessionStorage.getItem('token');
            if (!token) {
                window.location.href = 'login.html'; 
                return;
            }

            if (!this.currentProduct) {
                throw new Error('No product data available');
            }

            const quantityInput = document.querySelector('.cart-item input[name="quantity"]');
            const quantity = parseInt(quantityInput.value) || 1;

            if (quantity < 1) {
                alert('Please enter a valid quantity');
                return;
            }
            const productId = this.currentProduct.productId;

            // Determine the correct price to use
            let priceToUse;
            const originalPrice = parseFloat(this.currentProduct.price) || 0;
            const discountPrice = parseFloat(this.currentProduct.discountPrice) || 0;

            // Use discount price if it exists and is less than original price
            if (discountPrice && discountPrice < originalPrice) {
                priceToUse = discountPrice;
            } else {
                priceToUse = originalPrice;
            }

            if (this.currentProduct.limit && quantity > this.currentProduct.limit) {
                alert(`Sorry, only ${this.currentProduct.limit} items available`);
                return;
            }

            const addToCartButton = document.querySelector('.cart-item .button-primary');
            const originalButtonText = addToCartButton.textContent;
            addToCartButton.textContent = 'Adding...';
            addToCartButton.disabled = true;

            await this.cartManager.addToCart(productId, quantity, priceToUse);

            addToCartButton.textContent = 'Added!';
            setTimeout(() => {
                addToCartButton.textContent = originalButtonText;
                addToCartButton.disabled = false;
            }, 2000);

        } catch (error) {
            console.error('Error adding to cart:', error);
            alert('Failed to add item to cart. Please try again.');

            // Reset button state
            const addToCartButton = document.querySelector('.cart-item .button-primary');
            if (addToCartButton) {
                addToCartButton.textContent = 'Add to cart';
                addToCartButton.disabled = false;
            }
        }
    }

    updateUI(product) {
        console.log('Updating UI with product:', product);
        this.currentProduct = product; // Store the current product data

        try {
            // Update product thumbnails
            const productThumbnails = document.querySelector('.product-thumbnails');
            if (productThumbnails && product.images.$values.length > 0) {
                // Only show the first image in the main view
                const mainImage = product.images.$values[0];
                productThumbnails.innerHTML = `
        <div class="single-product-item">
            <figure class="feature-image">
                <img src="https://localhost:44357/${mainImage.imageURL}" alt="${product.name}">
            </figure>
            <div class="image-search-icon">
                <a href="https://localhost:44357/${mainImage.imageURL}" data-lightbox="lightbox-set">
                    <i class="fas fa-search"></i>
                </a>
            </div>
        </div>
    `;
            }

            // Update product thumb nav
            const productThumbNav = document.querySelector('.product-thumb-nav');
            if (productThumbNav && product.images.$values.length > 0) {
                let thumbNavHTML = '';
                // Create thumbnails for all images
                product.images.$values.forEach(image => {
                    thumbNavHTML += `
            <div class="single-product-item">
                                    <figure class="feature-image">
                    <img src="https://localhost:44357/${image.imageURL}" alt="${product.name}" style="width: 100% ; height: 10vh;">
                                    </figure>
                                 </div>
        `;
                });
                productThumbNav.innerHTML = thumbNavHTML;
            }
            const overviewContent = document.querySelector('.overview-content');
            if (overviewContent && product.description) {
                overviewContent.innerHTML = `<p>${product.description}</p>`;
            }

            // Update additional information tab content
            const additionalInfoTable = document.querySelector('#add-info table');
            if (additionalInfoTable) {
                additionalInfoTable.innerHTML = `
        <tr>
            <th>Weight</th>
            <td>${product.weight} kg</td>
        </tr>
        <tr>
            <th>Dimensions</th>
            <td>${product.dimensions}</td>
        </tr>
    `;
            }

            // Update breadcrumb
            const breadcrumbActive = document.querySelector('.breadcrumb-item.active a');
            if (breadcrumbActive) {
                breadcrumbActive.textContent = product.categoryName;
            }

            // Update product title
            const productTitle = document.querySelector('.product-summary h2');
            if (productTitle) {
                productTitle.textContent = product.name;
            }

            // Update prices
            // More robust version with error checking
            const productPrice = document.querySelector('.product-price');
            if (productPrice) {
                const originalPrice = parseFloat(product.price) || 0;
                const discountPrice = parseFloat(product.discountPrice) || 0;

                if (originalPrice > discountPrice && discountPrice !== 0) {
                    productPrice.innerHTML = `
            <del>$${originalPrice.toFixed(2)}</del>
            <ins>$${discountPrice.toFixed(2)}</ins>
        `;
                } else {
                    productPrice.innerHTML = `<ins>$${originalPrice.toFixed(2)}</ins>`;
                }
            }

            // Update categories
            const catDetail = document.querySelector('.cat-detail');
            if (catDetail) {
                let categoryHTML = '<strong>Categories:</strong>';
                if (product.categoryName) {
                    categoryHTML += `<a href="#">${product.categoryName}</a>`;
                }
                catDetail.innerHTML = categoryHTML;
            }

            // Update tags
            const tagDetail = document.querySelector('.tag-detail');
            if (tagDetail && product.tag) {
                let tagHTML = '<strong>Tags:</strong>';
                const tags = product.tag.split(',');
                tags.forEach(tag => {
                    tagHTML += `<a href="#">${tag.trim()}</a>`;
                });
                tagDetail.innerHTML = tagHTML;
            }

            // Update description
            const productDesc = document.querySelector('.product-desc');
            if (productDesc) {
                productDesc.innerHTML = `<p>${product.description}</p>`;
            }

        } catch (error) {
            console.error('Error updating UI:', error);
        }
    }
}
// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    console.log('DOM loaded');
    new ProductDetail();
});