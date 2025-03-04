namespace Ajloun_Tour.DTOs2.OffersProgramDTOs
{
    public class CreateOffersProgram
    {
        public int OfferId { get; set; }
        public int ProgramId { get; set; }
        public int DayNumber { get; set; }
        public DateTime? ProgramDate { get; set; }
        public string? CustomTitle { get; set; }
        public string? CustomDescription { get; set; }

    }
}
