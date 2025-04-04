document.getElementById("contactForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const form = e.target;
    const formData = new FormData();

    formData.append("Name", form.name.value);
    formData.append("Email", form.email.value);
    formData.append("Subject", form.subject.value);
    formData.append("Message", form.message.value);
    formData.append("SubmittedAt", new Date().toISOString());
    formData.append("IsRead", false);

    try {
        const response = await fetch("https://localhost:44357/api/Contacts", {
            method: "POST",
            body: formData
        });

        if (response.ok) {
            const result = await response.json();
            Swal.fire({
                icon: 'success',
                title: 'Message Sent!',
                text: 'Thanks, we received your message.',
                confirmButtonColor: '#3085d6'
            });
            form.reset();
        } else {
            const errorData = await response.text(); // fallback if not JSON
            Swal.fire({
                icon: 'error',
                title: 'Submission Failed',
                text: errorData || 'Something went wrong. Please try again.',
            });
        }
    } catch (error) {
        console.error("Error:", error);
        Swal.fire({
            icon: 'error',
            title: 'Network Error',
            text: 'Please check your internet connection and try again.'
        });
    }
});
