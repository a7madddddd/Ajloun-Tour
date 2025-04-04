document.addEventListener('DOMContentLoaded', () => {
    const jobsContainer = document.getElementById('jobs-container');
    const applicationForm = document.getElementById('application-form');
    let currentJobId = null;

    // Add a hidden input for jobId in the form
    const jobIdInput = document.createElement('input');
    jobIdInput.type = 'hidden';
    jobIdInput.name = 'jobId';
    applicationForm.appendChild(jobIdInput);

    // Add error message element
    const errorMessage = document.createElement('div');
    errorMessage.style.color = 'red';
    errorMessage.id = 'error-message';
    errorMessage.style.marginBottom = '10px';
    errorMessage.style.display = 'none';
    applicationForm.insertBefore(errorMessage, applicationForm.firstChild);

    async function fetchJobs() {
        try {
            const response = await fetch('https://localhost:44357/api/Jobs?includeInactive=true', {
                method: 'GET',
                headers: {
                    'Accept': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            const jobs = data.$values || [];

            jobsContainer.innerHTML = '';

            jobs.forEach(job => {
                const jobColumn = document.createElement('div');
                jobColumn.className = 'col-md-6';
                jobColumn.innerHTML = `
                    <div class="vacancy-content">
                        <h5>${job.jobType || 'Job Type Not Specified'}</h5>
                        <h3 class="job-title" data-job-id="${job.jobId}" style="cursor: pointer">${job.title || 'Untitled Job'}</h3>
                        <p>${job.description || 'No description available'}</p>
                        <a href="#application-form" class="button-primary apply-now" data-job-id="${job.jobId}" data-job-title="${job.title}">APPLY NOW</a>
                    </div>
                `;
                jobsContainer.appendChild(jobColumn);
            });

            // Add event listeners
            addJobEventListeners();

        } catch (error) {
            console.error('Error fetching jobs:', error);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Unable to load jobs. Please try again later.',
            });
        }
    }

    function addJobEventListeners() {
        // Event listener for job titles (redirect to details)
        document.querySelectorAll('.job-title').forEach(title => {
            title.addEventListener('click', (e) => {
                const jobId = e.target.dataset.jobId;
                window.location.href = `http://127.0.0.1:5500/career-detail.html?jobId=${jobId}`;
            });
        });

        // Event listener for apply buttons
        document.querySelectorAll('.apply-now').forEach(button => {
            button.addEventListener('click', (e) => {
                e.preventDefault();
                currentJobId = e.target.dataset.jobId;
                const jobTitle = e.target.dataset.jobTitle;
                jobIdInput.value = currentJobId;
                errorMessage.style.display = 'none';

                // Show confirmation with SweetAlert2
                Swal.fire({
                    icon: 'info',
                    title: 'Applying for ' + jobTitle,
                    text: 'Please fill out the application form below',
                    confirmButtonText: 'OK',
                    timer: 2000,
                    timerProgressBar: true
                }).then(() => {
                    document.querySelector('#application-form').scrollIntoView({
                        behavior: 'smooth'
                    });
                });
            });
        });
    }

    // Handle form submission
    applicationForm.addEventListener('submit', async (e) => {
        e.preventDefault();

        if (!validateForm()) {
            return;
        }

        if (!currentJobId) {
            Swal.fire({
                icon: 'warning',
                title: 'No Job Selected',
                text: 'Please select a job position first'
            });
            return;
        }

        // Show loading state
        Swal.fire({
            title: 'Submitting Application',
            text: 'Please wait...',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        const formData = new FormData();
        formData.append('JobId', currentJobId);
        formData.append('ApplicantName', document.querySelector('input[placeholder="Your Name"]').value);
        formData.append('Email', document.querySelector('input[placeholder="Your Email"]').value);
        formData.append('Phone', document.querySelector('input[placeholder="Your Phone"]').value);
        formData.append('Message', document.querySelector('textarea[placeholder="Enter your message"]').value);

        const cvInput = document.querySelector('input[type="file"]');
        if (cvInput && cvInput.files[0]) {
            formData.append('CV', cvInput.files[0]);
        }

        try {
            const response = await fetch('https://localhost:44357/api/JobApplications', {
                method: 'POST',
                body: formData
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();

            // Show success message
            Swal.fire({
                icon: 'success',
                title: 'Success!',
                text: 'Your application has been submitted successfully',
                confirmButtonText: 'OK',
                timer: 3000,
                timerProgressBar: true
            }).then(() => {
                // Reset form and currentJobId
                applicationForm.reset();
                currentJobId = null;
                jobIdInput.value = '';
            });

        } catch (error) {
            console.error('Error submitting application:', error);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'There was an error submitting your application. Please try again.',
                confirmButtonText: 'OK'
            });
        }
    });

    // Form validation function
    function validateForm() {
        const name = document.querySelector('input[placeholder="Your Name"]').value.trim();
        const email = document.querySelector('input[placeholder="Your Email"]').value.trim();
        const phone = document.querySelector('input[placeholder="Your Phone"]').value.trim();
        const message = document.querySelector('textarea[placeholder="Enter your message"]').value.trim();

        if (!name || !email || !phone || !message) {
            Swal.fire({
                icon: 'warning',
                title: 'Missing Information',
                text: 'Please fill in all required fields'
            });
            return false;
        }

        // Basic email validation
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            Swal.fire({
                icon: 'warning',
                title: 'Invalid Email',
                text: 'Please enter a valid email address'
            });
            return false;
        }

        // Basic phone validation (optional)
        const phoneRegex = /^\d{10}$/;  // Assumes 10-digit phone number
        if (!phoneRegex.test(phone.replace(/\D/g, ''))) {
            Swal.fire({
                icon: 'warning',
                title: 'Invalid Phone Number',
                text: 'Please enter a valid 10-digit phone number'
            });
            return false;
        }

        return true;
    }

    // Fetch jobs on page load
    fetchJobs();
});