namespace Ajloun_Tour.DTOs2.ToursProgramDTOs
{
    public class CreateToursProgram
    {
        public int TourId { get; set; }
        public int DayNumber { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? ProgramDate { get; set; }
        public int? PackageId { get; set; }

    }
}
