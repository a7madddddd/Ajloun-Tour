document.addEventListener("DOMContentLoaded", async function () {
    const toursApi = "https://localhost:44357/api/Tours";
    const packagesApi = "https://localhost:44357/api/Packages";
    const reviewsApi = "https://localhost:44357/api/Reviews";

    try {
        // جلب البيانات بالتوازي
        const [toursResponse, packagesResponse, reviewsResponse] = await Promise.all([
            fetch(toursApi),
            fetch(packagesApi),
            fetch(reviewsApi)
        ]);

        // تحويل الاستجابات إلى JSON
        const toursData = await toursResponse.json();
        const packagesData = await packagesResponse.json();
        const reviewsData = await reviewsResponse.json();

        // استخراج البيانات ذات الصلة
        const tours = toursData.$values || [];
        const packages = packagesData.$values.filter(pkg => pkg.isActive); // تصفية الحزم الفعالة
        const reviews = reviewsData.$values || [];

        // إنشاء خريطة التقييمات لكل جولة
        const reviewsMap = {};
        reviews.forEach(review => {
            if (!reviewsMap[review.tourId]) {
                reviewsMap[review.tourId] = { sum: 0, count: 0 };
            }
            reviewsMap[review.tourId].sum += review.rating;
            reviewsMap[review.tourId].count += 1;
        });

        // دمج البيانات وربط كل حزمة بجولتها وتقييماتها
        const combinedData = packages.map(pkg => {
            const tour = tours.find(t => t.tourId === pkg.id); // افتراض أن package.id هو tourId

            // حساب متوسط التقييم
            const tourReviews = reviewsMap[pkg.id] || { sum: 0, count: 0 };
            const avgRating = tourReviews.count > 0 ? tourReviews.sum / tourReviews.count : 0;
            const reviewCount = tourReviews.count || 0;

            return {
                packageId: pkg.id,
                packageName: pkg.name,
                details: pkg.details,
                price: pkg.price,
                tourDays: pkg.tourDays,
                tourNights: pkg.tourNights,
                numberOfPeople: pkg.numberOfPeople,
                tourImage: tour ? tour.tourImage : "default.jpg",
                location: tour ? tour.location || " Location" : " Location",
                avgRating: avgRating,
                reviewCount: reviewCount
            };
        });

        // عرض جميع الحزم الفعالة
        renderPackages(combinedData);

    } catch (error) {
        console.error("Error fetching data:", error);
        document.querySelector(".package-inner .row").innerHTML = `
            <div class="col-12 text-center">
                <p>Failed to load tour packages. Please try again later.</p>
            </div>
        `;
    }
});

// وظيفة لعرض الحزم في HTML
function renderPackages(data) {
    const packageContainer = document.querySelector(".package-inner .row");
    packageContainer.innerHTML = ""; // تنظيف المحتوى السابق

    data.forEach(item => {
        // توليد نجوم التقييم
        const starsHtml = generateStars(Math.round(item.avgRating));

        // تقصير النصوص الطويلة
        const shortDetails = item.details.length > 100 ? item.details.substring(0, 100) + '...' : item.details;

        const packageHtml = `
            <div class="col-lg-4 col-md-6">
                <div class="package-wrap">
                    <figure class="feature-image">
                        <a href="#">
                            <img src="https://localhost:44357/ToursImages/${item.tourImage}" alt="${item.packageName}" style="height: 40vh; object-fit: cover;">
                        </a>
                    </figure>
                    <div class="package-price">
                        <h6><span>$${item.price}</span> / per person</h6>
                    </div>
                    <div class="package-content-wrap">
                        <div class="package-meta text-center">
                            <ul>
                                <li><i class="far fa-clock"></i> ${item.tourDays}D/${item.tourNights}N</li>
                                <li><i class="fas fa-user-friends"></i> People: ${item.numberOfPeople}</li>
                                <li><i class="fas fa-map-marker-alt"></i> ${item.location}</li>
                            </ul>
                        </div>
                        <div class="package-content">
                            <h3><a href="#">${item.packageName}</a></h3>
                            <div class="review-area">
                                <span class="review-text">(${item.reviewCount} reviews)</span>
                                <div class="rating-stars">
                                    ${starsHtml}
                                </div>
                            </div>
                            <p>${shortDetails}</p>
                            <div class="btn-wrap">
                                <a href="#" class="button-text width-6">Book Now<i class="fas fa-arrow-right"></i></a>
                                <a href="#" class="button-text width-6">Wish List<i class="far fa-heart"></i></a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
        packageContainer.innerHTML += packageHtml;
    });
}

// وظيفة لإنشاء نجوم التقييم
function generateStars(rating) {
    let starsHtml = '';

    for (let i = 1; i <= 5; i++) {
        if (i <= rating) {
            starsHtml += '<i class="fas fa-star" style="color: #F56960;"></i>'; // نجمة ممتلئة
        } else {
            starsHtml += '<i class="far fa-star" style="color: #F56960;"></i>'; // نجمة فارغة
        }
    }

    return starsHtml;
}
