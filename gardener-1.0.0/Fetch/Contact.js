
document.addEventListener('DOMContentLoaded', function () {
    const contactForm = document.querySelector('form');

    contactForm.addEventListener('submit', function (e) {
        e.preventDefault();

        // Check form validity
        if (!this.checkValidity()) {
            e.stopPropagation();
            this.classList.add('was-validated');
            return;
        }

        // Get form values
        const formData = new FormData();
        formData.append('Name', document.getElementById('name').value);
        formData.append('Email', document.getElementById('email').value);
        formData.append('Subject', document.getElementById('subject').value);
        formData.append('Message', document.getElementById('message').value);
        formData.append('SubmittedAt', new Date().toISOString());
        formData.append('IsRead', 'false');

        // Disable submit button
        const submitButton = this.querySelector('button[type="submit"]');
        submitButton.disabled = true;
        submitButton.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Sending...';

        // Send the form data
        fetch('https://localhost:44357/api/Contacts', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                // Show success message
                Swal.fire({
                    icon: 'success',
                    title: 'Message Sent!',
                    text: 'Thank you for contacting us. We will get back to you soon.',
                    confirmButtonColor: '#0F4229'
                });

                // Reset form and validation states
                contactForm.reset();
                contactForm.classList.remove('was-validated');

                // Reset all input borders
                const inputs = contactForm.querySelectorAll('input, textarea');
                inputs.forEach(input => {
                    input.classList.remove('is-valid', 'is-invalid');
                    input.style.borderColor = '';
                });
            })
            .catch(error => {
                console.error('Error:', error);
                // Show error message
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Something went wrong! Please try again later.',
                    confirmButtonColor: '#dc3545'
                });
            })
            .finally(() => {
                // Re-enable submit button
                submitButton.disabled = false;
                submitButton.innerHTML = 'Send Message';
            });
    });
});





