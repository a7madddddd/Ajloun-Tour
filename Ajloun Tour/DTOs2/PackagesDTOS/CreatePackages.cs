﻿namespace Ajloun_Tour.DTOs2.PackagesDTOS
{
    public class CreatePackages
    {
        public string Name { get; set; } = null!;
        public string? Details { get; set; }
        public decimal? Price { get; set; }
        public int? TourDays { get; set; }
        public IFormFile? Image { get; set; }
        public int? TourNights { get; set; }
        public int? NumberOfPeople { get; set; }
        public string? Location { get; set; }
        public string? Map { get; set; }
        public bool? IsActive { get; set; }

    }
}
