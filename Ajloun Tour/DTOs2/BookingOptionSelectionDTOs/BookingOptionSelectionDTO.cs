namespace Ajloun_Tour.DTOs2.BookingOptionSelectionDTOs
{
    public class BookingOptionSelectionDTO
    {
        public int SelectionId { get; set; }
        public int BookingId { get; set; }
        public int OptionId { get; set; }
        public string? OptionName { get; set; }
        public decimal? OptionPrice { get; set; }

    }
}
