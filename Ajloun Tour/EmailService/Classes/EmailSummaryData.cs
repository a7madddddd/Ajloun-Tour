namespace Ajloun_Tour.EmailService.Classes
{
    public class EmailSummaryData
    {
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public List<BookingOptionSummary> SelectedOptions { get; set; }
    }
}
