namespace Ajloun_Tour.DTOs2.OffersProgramDTOs
{
    public class OffersProgramDTO
    {
        public int OfferProgramId { get; set; }
        public int OfferId { get; set; }
        public int ProgramId { get; set; }
        public string ProgramTitle { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int DayNumber { get; set; }
        public DateTime? ProgramDate { get; set; }
        public string? CustomTitle { get; set; }
        public string? CustomDescription { get; set; }

    }
}
