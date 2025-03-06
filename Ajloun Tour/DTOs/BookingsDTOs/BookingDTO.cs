using Ajloun_Tour.DTOs2.BookingOptionsDTOs;
using Ajloun_Tour.DTOs2.ToursProgramDTOs;
using Ajloun_Tour.Models;

namespace Ajloun_Tour.DTOs.BookingsDTOs
{
    public class BookingDTO
    {
        public int BookingId { get; set; }
        public int? TourId { get; set; }
        public int? PackageId { get; set; }
        public int? OfferId { get; set; }
        public int? UserId { get; set; }
        public DateTime? BookingDate { get; set; }
        public int? NumberOfPeople { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }



    }
}
