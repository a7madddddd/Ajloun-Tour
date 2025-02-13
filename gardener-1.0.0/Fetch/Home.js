// Fetch tours data from the API
fetch('https://localhost:44357/api/Tours')
    .then(response => response.json())
    .then(data => {
        const tours = data.$values;

        // Filter active tours and limit to the last 6
        const activeTours = tours.filter(tour => tour.isActive).slice(-6);

        // Get the container to inject tour items
        const container = document.getElementById('tours-container');
        container.innerHTML = '';  // Clear any previous content

        activeTours.forEach(tour => {
            // Create a new HTML structure for each tour
            const tourCard = document.createElement('div');
            tourCard.classList.add('col-lg-4', 'col-md-6', 'wow', 'fadeInUp');
            tourCard.setAttribute('data-wow-delay', '0.1s');
            tourCard.innerHTML = `
                    <div class="service-item rounded d-flex h-100">
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
