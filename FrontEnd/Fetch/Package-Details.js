document.addEventListener("DOMContentLoaded", function () {
    // Get TourId and PackageId from URL
    const urlParams = new URLSearchParams(window.location.search);
    const tourId = urlParams.get('tourId');
    const packageId = urlParams.get('packageId');

    if (tourId && packageId) {
        fetchTourPackage(tourId, packageId);
    } else {
        console.error("TourId or PackageId is missing in the URL.");
    }
});

async function fetchTourPackage(tourId, packageId) {
    const apiUrl = `https://localhost:44357/api/ToursPackages/id?TourId=${tourId}&PackageId=${packageId}`;

    try {
        const response = await fetch(apiUrl, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        // Update HTML content
        document.getElementById("tour-title").innerText = data.packageName;
        document.getElementById("tour-location").innerText = data.location;
        document.getElementById("tour-price").innerText = `$${data.price}`;
        document.getElementById("tour-days").innerText = `${data.tourDays} Days / ${data.tourNights} Nights`;
        document.getElementById("tour-people").innerText = `For ${data.numberOfPeople} People`;
        document.getElementById("tour-image").src = `https://localhost:44357/PakagesImages/${data.image}`;
    } catch (error) {
        console.error("Error fetching data:", error);
    }
}











// Assuming the packageId is part of the URL, you can fetch it like this:
const urlParams = new URLSearchParams(window.location.search);
const packageId = urlParams.get('packageId'); // Make sure the URL includes ?packageId=<id>

// API endpoint
const apiUrl = `https://localhost:44357/api/ToursPrograms/${packageId}/programs`;

// Fetch the program data
fetch(apiUrl)
    .then(response => response.json())
    .then(data => {
        // Check if data is available
        if (data && data.packageId && data.tourPrograms && data.tourPrograms.length > 0) {
            const programList = document.getElementById('program-list');
            const programDuration = document.getElementById('program-duration');
            const programDescription = document.getElementById('program-description');

            // Set package name and description if available
            programDuration.innerHTML = `(${data.tourPrograms.length} days)`;
            programDescription.innerHTML = data.packageName || "No description available.";

            // Loop through the tour programs and create list items
            data.tourPrograms.forEach(program => {
                const li = document.createElement('li');
                li.innerHTML = `
        <div class="timeline-content">
            <div class="day-count">Day <span>${program.dayNumber}</span></div>
            <h4>${program.title}</h4>
            <p>${program.description || 'No description available.'}</p>
        </div>
        `;
                programList.appendChild(li);
            });
        } else {
            // Handle case where there are no programs
            const programList = document.getElementById('program-list');
            programList.innerHTML = '<li>No programs available.</li>';
        }
    })
    .catch(error => {
        console.error('Error fetching program data:', error);
        const programList = document.getElementById('program-list');
        programList.innerHTML = '<li>Failed to load program data.</li>';
    });
