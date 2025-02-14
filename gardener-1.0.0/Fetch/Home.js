// Fetch tours data from the API
fetch('https://localhost:44357/api/Tours')
    .then(response => response.json())
    .then(data => {
        const tours = data.$values;

        // Filter active tours and limit to the last 6
        const activeTours = tours.filter(tour => tour.isActive).slice(-6);

        // Get the container to inject tour items
        const container = document.getElementById('tours-container');
        container.innerHTML = ''; // Clear any previous content

        activeTours.forEach(tour => {
            // Create a new HTML structure for each tour
            const tourCard = document.createElement('div');
            tourCard.classList.add('col-lg-4', 'col-md-6', 'wow', 'fadeInUp');
            tourCard.setAttribute('data-wow-delay', '0.1s');
            tourCard.innerHTML = `
                    <div class="service-item rounded d-flex h-100" data-wow-delay="0.1s">
                        <div class="service-img rounded">
                            <img class="img-fluid" src="https://localhost:44357/ToursImages/${tour.tourImage}" alt="${tour.tourName}">
                        </div>
                        <div class="service-text rounded p-5" style="width: 100%">
                            <div class="btn-square rounded-circle mx-auto mb-3">
                                <img class="img-fluid" src="img/icon/icon-3.png" alt="Icon">
                            </div>
                            <h4 class="mb-3">${tour.tourName}</h4>
                            <p class="mb-4">${tour.description}</p>
                            <a class="btn btn-sm" href=""><i class="fa fa-clock text-primary me-2"></i>${tour.duration} hours</a>
                            <a class="btn btn-sm" href="service.html"><i class="fa fa-plus text-primary me-2"></i>Read More</a>
                        </div>
                    </div>
                `;
            container.appendChild(tourCard);
        });
    })
    .catch(error => console.error('Error fetching tours:', error));








document.addEventListener('DOMContentLoaded', function() {
    // Get DOM elements
    const portfolioItems = document.getElementById('portfolio-items');
    const filterButtons = document.querySelectorAll('#portfolio-flters li');

    // Fetch projects from API
    async function fetchProjects() {
        try {
            const response = await fetch('https://localhost:44357/api/Projects');
            const data = await response.json();
            console.log('Fetched data:', data); // Debug log
            // Extract the $values array from the response
            return data.$values || [];
        } catch (error) {
            console.error('Error fetching projects:', error);
            return [];
        }
    }

    // Create project HTML with null checks
    function createProjectHTML(project) {
        if (!project) {
            console.error('Invalid project data');
            return '';
        }

        // Safely get status and assign the correct class
        const status = (project.status || 'unknown').toLowerCase();
        const statusClass = status === 'completed' ? 'first' : (status === 'ongoing' ? 'second' : '');

        return `
        <div class="col-lg-4 col-md-6 portfolio-item ${statusClass} wow fadeInUp show" data-wow-delay="0.1s">
            <div class="portfolio-inner rounded">
                <img class="img-fluid" src="https://localhost:44357/ProjectsImages/${project.projectImage}" alt="${project.projectName}">
                <div class="portfolio-text">
                    <h4 class="text-white mb-4">${project.projectName}</h4>
                    <div class="d-flex">
                        <a class="btn btn-lg-square rounded-circle mx-2" href="https://localhost:44357/ProjectsImages/${project.projectImage}" data-lightbox="portfolio">
                            <i class="fa fa-eye"></i>
                        </a>
                        <a class="btn btn-lg-square rounded-circle mx-2" href="#">
                            <i class="fa fa-link"></i>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    `;
    }


    // Filter projects
    function filterProjects(filter) {
        const items = document.querySelectorAll('.portfolio-item');
        items.forEach(item => {
            if (filter === '*' || item.classList.contains(filter)) {
                item.classList.add('show');
            } else {
                item.classList.remove('show');
            }
        });
    }

    // Handle filter clicks
    filterButtons.forEach(button => {
        button.addEventListener('click', () => {
            filterButtons.forEach(btn => btn.classList.remove('active'));
            button.classList.add('active');
            const filter = button.getAttribute('data-filter');
            filterProjects(filter);
        });
    });

    // Initialize projects with error handling
    async function initializeProjects() {
        try {
            const projects = await fetchProjects();
            if (!projects || projects.length === 0) {
                portfolioItems.innerHTML = '<div class="col-12 text-center">No projects found</div>';
                return;
            }

            const validProjects = projects.filter(project => project && typeof project === 'object');
            portfolioItems.innerHTML = validProjects.map(project => createProjectHTML(project)).join('');
        } catch (error) {
            console.error('Error initializing projects:', error);
            portfolioItems.innerHTML = '<div class="col-12 text-center">Error loading projects</div>';
        }
    }

    // Initial load
    initializeProjects();
});