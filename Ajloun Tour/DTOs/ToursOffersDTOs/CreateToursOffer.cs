﻿namespace Ajloun_Tour.DTOs.ToursOffersDTOs
{
    public class CreateToursOffer
    {
        public int TourId { get; set; }
        public int OfferId { get; set; }
        public string? TourName { get; set; }
        public string? OfferTitle { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }


    }
}
