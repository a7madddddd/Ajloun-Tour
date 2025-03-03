namespace Ajloun_Tour.DTOs2.ToursProgramDTOs
{
    public class UpdateTourProgramDTO
    {
        public int DayNumber { get; set; }
        public DateTime? ProgramDate { get; set; }
        public string? CustomTitle { get; set; }
        public string? CustomDescription { get; set; }
    }
}
