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
        document.getElementById("tour-details").innerText = data.details;
        document.getElementById("tour-price").innerText = `$${data.price}`;
        document.getElementById("tour-days").innerText = `${data.tourDays} Days / ${data.tourNights} Nights`;
        document.getElementById("tour-people").innerText = `For ${data.numberOfPeople} People`;
        document.getElementById("tour-image").src = `https://localhost:44357/PakagesImages/${data.image}`;
    } catch (error) {
        console.error("Error fetching data:", error);
    }
}