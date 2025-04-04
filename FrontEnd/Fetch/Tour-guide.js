document.addEventListener('DOMContentLoaded', () => {
    const employeesContainer = document.querySelector('.guide-page-section .row');

    async function fetchEmployees() {
        try {
            const response = await fetch('https://localhost:44357/api/Employees', {
                method: 'GET',
                headers: {
                    'Accept': 'text/plain'
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            const employees = data.$values || [];

            // Clear existing content
            employeesContainer.innerHTML = '';

            // Add employees to the container
            employees.forEach(employee => {
                const employeeElement = document.createElement('div');
                employeeElement.className = 'col-lg-4 col-md-6';
                employeeElement.innerHTML = `
                    <div class="guide-content-wrap text-center">
                        <div class="guide-content">
                            <h3>${employee.fullName}</h3>
                            <h5>${employee.jobTitle}</h5>
                            <div class="guide-social social-links">
                                <ul>
                                    <li><a href="mailto:${employee.email}" title="${employee.email}">
                                        <i class="fas fa-envelope" aria-hidden="true"></i>
                                    </a></li>
                                    <li><a href="tel:${employee.phone}" title="${employee.phone}">
                                        <i class="fas fa-phone" aria-hidden="true"></i>
                                    </a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                `;
                employeesContainer.appendChild(employeeElement);
            });

        } catch (error) {
            console.error('Error fetching employees:', error);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Unable to load employees. Please try again later.'
            });
        }
    }

    // Initial load
    fetchEmployees();
});