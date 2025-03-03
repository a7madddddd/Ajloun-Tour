namespace Ajloun_Tour.DTOs2.ToursProgramDTOs
{
    public class ToursProgramDTO
    {
        public int TourProgramId { get; set; }
        public int TourId { get; set; }
        public string? TourName { get; set; }
        public int ProgramId { get; set; }
        public string? ProgramTitle { get; set; }
        public int DayNumber { get; set; }
        public DateTime? ProgramDate { get; set; }
        public string? CustomTitle { get; set; }
        public string? CustomDescription { get; set; }

    }
}
