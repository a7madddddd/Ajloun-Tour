$(document).ready(function () {
    // Auth Service for token and user data management
    const AuthService = {
        setToken(token) {
            sessionStorage.setItem('token', token);
        },
        getToken() {
            return sessionStorage.getItem('token');
        },
        setUserData(userData) {
            // sessionStorage.setItem('userData', JSON.stringify(userData));
        },
        clearAuth() {
            sessionStorage.removeItem('token');
            sessionStorage.removeItem('userData');
        }
    };

    // Registration Form Submit Handler
    $('form.signup').on('submit', function (e) {
        e.preventDefault();

        const form = $(this);
        const submitButton = form.find('input[type="submit"]');

        // Create FormData object
        const formData = new FormData();
        formData.append('FullName', form.find('input[name="FullName"]').val().trim());
        formData.append('Email', form.find('input[name="Email"]').val().trim());
        formData.append('Password', form.find('input[name="Password"]').val());
        formData.append('Phone', form.find('input[name="Phone"]').val().trim());
        formData.append('CreatedAt', new Date().toISOString());

        // Log the FormData entries
        for (let pair of formData.entries()) {
            console.log(pair[0] + ': ' + pair[1]);
        }

        submitButton.prop('disabled', true);

        $.ajax({
            url: 'https://localhost:44357/api/Users',
            method: 'POST',
            processData: false, // Important for FormData
            contentType: false, // Important for FormData
            data: formData,
            success: function (response) {
                console.log('Success Response:', response);
                Swal.fire({
                    icon: 'success',
                    title: 'Registration Successful!',
                    text: 'Please login with your credentials',
                    timer: 2000,
                    showConfirmButton: false
                }).then(() => {
                    $('#login').prop('checked', true);
                    form[0].reset();
                });
            },
            error: function (xhr) {
                console.log('Error Response:', xhr.responseJSON);
                let errorMessage = 'Registration failed:\n';

                if (xhr.responseJSON && xhr.responseJSON.errors) {
                    const errors = xhr.responseJSON.errors;
                    Object.keys(errors).forEach(key => {
                        errorMessage += `${key}: ${errors[key].join(', ')}\n`;
                    });
                } else {
                    errorMessage = 'An error occurred during registration. Please try again.';
                }

                Swal.fire({
                    icon: 'error',
                    title: 'Registration Failed',
                    text: errorMessage
                });
            },
            complete: function () {
                submitButton.prop('disabled', false);
            }
        });
    });

    // Login Form Submit Handler
    $('form.login').on('submit', function (e) {
        e.preventDefault();

        const form = $(this);
        const submitButton = form.find('input[type="submit"]');
        submitButton.prop('disabled', true); // Disable button during submission

        const loginData = {
            email: form.find('input[placeholder="Email Address"]').val().trim(),
            password: form.find('input[placeholder="Password"]').val()
        };

        // Basic validation
        if (!loginData.email || !loginData.password) {
            alert('Please enter both email and password');
            submitButton.prop('disabled', false);
            return;
        }

        $.ajax({
            url: 'https://localhost:44357/api/Users/login',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(loginData),
            success: function (response) {
                // Store auth data
                AuthService.setToken(response.token);
                console.log('Token stored:', response.token);

                

                Swal.fire({
                    icon: 'success',
                    title: 'Login Successful!',
                    text: 'Welcome back!',
                    timer: 1500,
                    showConfirmButton: false
                }).then(() => {
                    // Retrieve stored referrer or use document.referrer as a fallback
                    let referrer = localStorage.getItem("referrer") || document.referrer || "/home.html";

                    // Redirect the user
                    window.location.href = referrer;

                    // Clear stored referrer to prevent unwanted redirections in the future
                    localStorage.removeItem("referrer");
                });

            },
            error: function (xhr) {
                let errorMessage = 'Login failed';

                if (xhr.status === 401) {
                    errorMessage = 'Invalid email or password';
                } else if (xhr.responseJSON && xhr.responseJSON.message) {
                    errorMessage = xhr.responseJSON.message;
                }

                Swal.fire({
                    icon: 'error',
                    title: 'Login Failed',
                    text: errorMessage
                });
            },
            complete: function () {
                submitButton.prop('disabled', false); // Re-enable button
            }
        });
    });

    // Add this if you want to check auth status on page load
    function checkAuthStatus() {
        const token = AuthService.getToken();
        if (token) {
            // Handle authenticated state (e.g., show/hide elements)
            $('.authenticated-content').show();
            $('.unauthenticated-content').hide();
        } else {
            $('.authenticated-content').hide();
            $('.unauthenticated-content').show();
        }
    }

    // Optional: Add logout handler
    $('.logout-button').on('click', function (e) {
        e.preventDefault();
        AuthService.clearAuth();
        window.location.href = 'login.html';
    });

    // Check auth status on page load
    checkAuthStatus();
});