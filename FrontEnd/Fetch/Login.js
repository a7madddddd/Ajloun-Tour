// 🚀 Signup Function with SweetAlert2
document.getElementById("signupForm").addEventListener("submit", async function (event) {
    event.preventDefault();

    // Get Form Data
    const fullName = document.getElementById("signupFullName").value;
    const email = document.getElementById("signupEmail").value;
    const phone = document.getElementById("signupPhone").value;
    const password = document.getElementById("signupPassword").value;
    const userImage = document.getElementById("signupUserImage").files[0];

    const formData = new FormData();
    formData.append("FullName", fullName);
    formData.append("Phone", phone);
    formData.append("Email", email);
    formData.append("Password", password);
    if (userImage) {
        formData.append("UserImage", userImage);
    }

    // Show loading alert
    Swal.fire({
        title: "Creating Account...",
        text: "Please wait",
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading(10000);
        }
    });

    try {
        const response = await fetch("https://localhost:44357/api/Users", {
            method: "POST",
            body: formData
        });

        const data = await response.json();

        if (response.ok) {
            Swal.fire({
                icon: "success",
                title: "Signup Successful!",
                text: "Your account has been created successfully.",
                confirmButtonText: "OK"
            });
        } else {
            Swal.fire({
                icon: "error",
                title: "Signup Failed",
                text: data.errors ? JSON.stringify(data.errors) : "Something went wrong",
                confirmButtonText: "Try Again"
            });
        }
    } catch (error) {
        Swal.fire({
            icon: "error",
            title: "Error",
            text: "An error occurred while signing up.",
            confirmButtonText: "OK"
        });
    }
});

// 🚀 Login Function with SweetAlert2
document.getElementById("loginForm").addEventListener("submit", async function (event) {
    event.preventDefault();

    // Get Form Data
    const email = document.getElementById("loginEmail").value;
    const password = document.getElementById("loginPassword").value;

    // Show loading alert
    Swal.fire({
        title: "Logging In...",
        text: "Please wait",
        timer: 2000,
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });

    try {
        const response = await fetch("https://localhost:44357/api/Users/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password })
        });

        const data = await response.json();
        
        if (response.ok) {
            sessionStorage.setItem("token", data.token); // Save JWT Token

            Swal.fire({
                icon: "success",
                title: "Login Successful!",
                text: "Redirecting...",
                timer: 2000,
                showConfirmButton: false
            }).then(() => {
                window.location.href = document.referrer ? document.referrer : "index.html";
            });

        } else {
            Swal.fire({
                icon: "error",
                title: "Login Failed",
                text: data.message || "Invalid email or password",
                confirmButtonText: "Try Again"
            });
        }

    } catch (error) {
        Swal.fire({
            icon: "error",
            title: "Error",
            text: "An error occurred while logging in.",
            confirmButtonText: "OK"
        });
    }
});
