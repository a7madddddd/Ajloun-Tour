document.addEventListener('DOMContentLoaded', () => {
    // Get jobId from URL parameters
    const urlParams = new URLSearchParams(window.location.search);
    const jobId = urlParams.get('jobId');

    if (!jobId) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No job ID provided'
        });
        return;
    }

    async function fetchJobDetails() {
        try {
            const response = await fetch(`https://localhost:44357/api/Jobs/${jobId}`, {
                method: 'GET',
                headers: {
                    'Accept': 'text/plain'
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const jobDetails = await response.json();
            updateJobDetailsPage(jobDetails);

        } catch (error) {
            console.error('Error fetching job details:', error);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Unable to load job details. Please try again later.'
            });
        }
    }

    function updateJobDetailsPage(job) {
        // Update job description section
        const jobDescription = document.querySelector('.job-description ul');
        jobDescription.innerHTML = `
            <li>
                <span>Post :</span>
                <h4>${job.title}</h4>
            </li>
            <li>
                <span>Time :</span>
                <h4>${job.jobType}</h4>
            </li>
            <li>
                <span>Salary :</span>
                <h4>${job.salary ? `$${job.salary}` : 'Negotiable'}</h4>
            </li>
            <li>
                <span>No. of Vacancy :</span>
                <h4>${job.vacancies}</h4>
            </li>
        `;

        // Update tab contents
        // Job Description tab
        const overviewContent = document.querySelector('#overview .overview-content');
        overviewContent.innerHTML = `
            <p>${job.description || 'No description available.'}</p>
        `;

        // Experience & Overview tab
        const experienceContent = document.querySelector('#experience .experience-content');
        experienceContent.innerHTML = `
            <h5>#OVERVIEW</h5>
            <p>${job.overview || 'No overview available.'}</p>
            <p>&nbsp;</p>
            <h5>#EXPERIENCE</h5>
            <p>${job.experinces || 'No experience requirements specified.'}</p>
        `;

        // Requirements tab
        const requirementContent = document.querySelector('#requirement .requirement-content');
        requirementContent.innerHTML = `
            <p>${job.requirements || 'No specific requirements listed.'}</p>
        `;

        // Update page header info
        const postInfo = document.querySelector('.post-detail-wrap');
        if (postInfo) {
            postInfo.innerHTML = `
                <div class="post-categories">
                    <span>Post :</span>
                    <a href="#">${job.title}</a>
                </div>
                <div class="post-time">
                    <span>Time :</span>
                    <a href="#">${job.jobType}</a>
                </div>
                <div class="post-salary">
                    <span>Salary :</span>
                    <a href="#">${job.salary ? `$${job.salary}` : 'Negotiable'}</a>
                </div>
                <div class="post-vacancy">
                    <span>No. of Vacancy :</span>
                    <a href="#">${job.vacancies}</a>
                </div>
            `;
        }

        // Update page title if needed
        document.title = `${job.title} - Career Details`;

        // If there are images, update them
        const jobImage = document.querySelector('.job-imgage img');
        if (jobImage && job.mainImage) {
            jobImage.src = `https://localhost:44357/JobsImages/${job.mainImage}`;
            jobImage.alt = job.title;
        
        }



        // Update the "How To Apply?" section if needed
        const howToApply = document.querySelector('.how-to-apply');
        if (howToApply) {
            howToApply.innerHTML = `
                <h3>How To Apply?</h3>
                <p>Please review the job details and requirements carefully before applying.</p>
                <ul>
                    <li>Review the job requirements</li>
                    <li>Prepare your CV and credentials</li>
                    <li>Fill out the application form completely</li>
                    <li>Submit your application</li>
                </ul>
            `;
        }
    }

    // Fetch job details when page loads
    fetchJobDetails();
});