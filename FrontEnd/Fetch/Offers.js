document.addEventListener('DOMContentLoaded', async function () {
    try {
        // جلب بيانات العروض
        const offersResponse = await fetch('https://localhost:44357/api/Offers');
        if (!offersResponse.ok) throw new Error('Failed to fetch offers data');
        const offersData = await offersResponse.json();

        // جلب بيانات الجولات
        const toursResponse = await fetch('https://localhost:44357/api/Tours');
        if (!toursResponse.ok) throw new Error('Failed to fetch tours data');
        const toursData = await toursResponse.json();

        // استخراج البيانات من الاستجابة
        const offers = offersData.$values || [];
        const tours = toursData.$values || [];

        // تصفية العروض النشطة فقط
        const activeOffers = offers.filter(offer => offer.isActive === true);

        // إنشاء خريطة للبحث السريع عن بيانات الجولات حسب الـ ID
        const tourMap = {};
        tours.forEach(tour => {
            tourMap[tour.tourId] = tour;
        });

        // تحديد مكان إدراج العروض
        const offerContainer = document.getElementById('offerContainer');
        offerContainer.innerHTML = ''; // تفريغ المحتوى الحالي

        if (activeOffers.length === 0) {
            offerContainer.innerHTML = '<div class="col-12 text-center"><p>No active offers available at the moment.</p></div>';
            return;
        }

        // إنشاء العناصر وإضافتها إلى الصفحة
        activeOffers.forEach(offer => {
            const tour = tourMap[offer.id]; // البحث عن الجولة المرتبطة بالعرض
            if (tour) {
                const originalPrice = offer.price;
                const discountedPrice = originalPrice * (1 - (offer.discountPercentage / 100));

                // إنشاء كود HTML للعرض
                const offerHTML = `
                    <div class="col-md-6 col-lg-4">

                        <div class="special-item">
                            <figure class="special-img">
                                <img src="https://localhost:44357/OffersImages/${offer.image}" alt="${offer.title}" style="height: 60vh;">
                            </figure>
                            <div class="badge-dis">
                                <span>
                                    <strong>${offer.discountPercentage}%</strong>
                                    off
                                </span>
                            </div>
                            <div class="special-content">
                                <div class="meta-cat">
           
                                    <a href="Offers-detail.html?offerId=${offer.id}&tourId=${tour.tourId}">${tour.tourName || 'Ajloun'}</a>
                                    <div class="badge-dis">
                                        <span>
                                            <strong><i class="fas fa-user-friends">   ${offer.peapole}   </i></strong>
                                        </span>
                                    </div>
                                </div>
                                <h3>
                                    <a href="Offers-detail.html?offerId=${offer.id}&tourId=${tour.tourId}">${offer.title}</a>
                                </h3>
                                <div class="package-price">
                                    Price:
                                    <del>$${originalPrice.toFixed(2)}</del>
                                    <ins>$${discountedPrice.toFixed(2)}</ins>
                                </div>
                            </div>
                        </div>
                    </div>
                `;
                offerContainer.innerHTML += offerHTML;
            }
        });

    } catch (error) {
        console.error('Error:', error);
        document.getElementById('offerContainer').innerHTML = `
            <div class="col-12 text-center">
                <p>Failed to load offers. Please try again later.</p>
            </div>
        `;
    }
});
