using Ajloun_Tour.DTOs.ToursDTOs;

namespace Ajloun_Tour.DTOs2.TourCartItemsDTOs
{
    public class CreateCartItemDTO
    {
        public int CartID { get; set; }
        public int TourID { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; }
        public DateTime SelectedDate { get; set; }
        public int NumberOfPeople { get; set; }
        public bool HasTourGuide { get; set; } = false;
        public bool HasInsurance { get; set; } = false;
        public bool HasDinner { get; set; } = false;
        public bool HasBikeRent { get; set; } = false;
        public decimal TourGuidePrice { get; set; } = 0;
        public decimal InsurancePrice { get; set; } = 0;
        public decimal DinnerPrice { get; set; } = 0;
        public decimal BikeRentPrice { get; set; } = 0;
    }
}
