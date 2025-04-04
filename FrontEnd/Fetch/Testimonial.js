document.addEventListener('DOMContentLoaded', () => {
    const testimonialContainer = document.querySelector('.testimonials-container');
    const testimonialForm = document.getElementById('Testimonial-form');

    // Function to decode JWT token
    function parseJwt(token) {
        try {
            const base64Url = token.split('.')[1];
            const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join(''));
            return JSON.parse(jsonPayload);
        } catch (e) {
            console.error('Error parsing token:', e);
            return null;
        }
    }

    // Get user ID from token
    function getUserIdFromToken() {
        const token = sessionStorage.getItem('token');
        if (!token) {
            return null;
        }
        const decodedToken = parseJwt(token);
        return decodedToken?.nameid;
    }

    async function fetchUserDetails(userId) {
        try {
            const response = await fetch(`https://localhost:44357/api/Users/id?id=${userId}`, {
                method: 'GET',
                headers: {
                    'Accept': 'text/plain'
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error('Error fetching user details:', error);
            return null;
        }
    }

    async function fetchTestimonials() {
        try {
            const response = await fetch('https://localhost:44357/api/Testomonials', {
                method: 'GET',
                headers: {
                    'Accept': 'text/plain'
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            const testimonials = data.$values || [];

            // Clear existing content
            testimonialContainer.innerHTML = '';

            // Create row for testimonials grid
            const gridRow = document.createElement('div');
            gridRow.className = 'row';
            testimonialContainer.appendChild(gridRow);

            // Process only accepted testimonials
            for (const testimonial of testimonials) {
                if (testimonial.accepted) {
                    const userDetails = await fetchUserDetails(testimonial.userId);

                    if (userDetails) {
                        const testimonialElement = document.createElement('div');
                        testimonialElement.className = 'col-md-6';
                        testimonialElement.innerHTML = `
                            <div class="testimonial-item text-center">
                                <figure class="testimonial-img">
                                    <img src="https://localhost:44357/UsersImages/${userDetails.userImage}"
                                         alt="${userDetails.fullName}"
                                         onerror="this.src='assets/images/default-user.jpg'">
                                </figure>
                                <div class="testimonial-content">
                                    <p>${testimonial.message}</p>
                                    <h3>${userDetails.fullName}</h3>
                                </div>
                            </div>
                        `;
                        gridRow.appendChild(testimonialElement);
                    }
                }
            }

        } catch (error) {
            console.error('Error fetching testimonials:', error);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Unable to load testimonials. Please try again later.'
            });
        }
    }

    // Handle form submission
    testimonialForm.addEventListener('submit', async (e) => {
        e.preventDefault();

        const userId = getUserIdFromToken();
        if (!userId) {
            Swal.fire({
                icon: 'warning',
                title: 'Authentication Required',
                text: 'Please login to submit a testimonial'
            });
            return;
        }

        const message = testimonialForm.querySelector('textarea[name="message"]').value.trim();

        try {
            const response = await fetch('https://localhost:44357/api/Testomonials', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'text/plain',
                    'Authorization': `Bearer ${sessionStorage.getItem('token')}`
                },
                body: JSON.stringify({
                    message: message,
                    userId: parseInt(userId)
                })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            Swal.fire({
                icon: 'success',
                title: 'Thank You!',
                text: 'Your testimonial has been submitted for review.',
                timer: 3000,
                timerProgressBar: true
            });

            testimonialForm.reset();
            fetchTestimonials(); // Refresh testimonials after submission

        } catch (error) {
            console.error('Error submitting testimonial:', error);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Unable to submit your testimonial. Please try again later.'
            });
        }
    });

    // Add styles
    const style = document.createElement('style');
    style.textContent = `
        .testimonial-item {
            background: #fff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            margin-bottom: 30px;
        }

        .testimonial-img {
            width: 100px;
            height: 100px;
            margin: 0 auto 20px;
            overflow: hidden;
            border-radius: 50%;
        }

        .testimonial-img img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }

        .vacancy-form {
            background: #fff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            position: sticky;
            top: 20px;
        }

        .vacancy-form textarea {
            width: 100%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 4px;
            resize: vertical;
        }

        .button-primary {
            color: white;
            border: none;
            padding: 12px 25px;
            border-radius: 4px;
            cursor: pointer;
            width: 100%;
            transition: background 0.3s ease;
        }

        .button-primary:hover {
            background: #0056b3;
        }
    `;
    document.head.appendChild(style);

    // Initial load
    fetchTestimonials();
});