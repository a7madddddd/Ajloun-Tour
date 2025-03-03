namespace Ajloun_Tour.DTOs2.ToursProgramDTOs
{
    public class CreateToursProgram
    {
        public int TourId { get; set; }
        public int ProgramId { get; set; }
        public int DayNumber { get; set; }
        public DateTime? ProgramDate { get; set; }
        public string? CustomTitle { get; set; }
        public string? CustomDescription { get; set; }

    }
}
