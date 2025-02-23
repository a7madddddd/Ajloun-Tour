using System;
using System.Collections.Generic;

namespace Ajloun_Tour.Models
{
    public partial class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int TourId { get; set; }
        public int? Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime SelectedDate { get; set; }
        public int NumberOfPeople { get; set; }
        public bool? HasTourGuide { get; set; }
        public bool? HasInsurance { get; set; }
        public bool? HasDinner { get; set; }
        public bool? HasBikeRent { get; set; }
        public decimal? TourGuidePrice { get; set; }
        public decimal? InsurancePrice { get; set; }
        public decimal? DinnerPrice { get; set; }
        public decimal? BikeRentPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Cart Cart { get; set; } = null!;
        public virtual Tour Tour { get; set; } = null!;
    }
}
