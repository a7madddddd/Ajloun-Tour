namespace Ajloun_Tour.DTOs2.ToursProgramDTOs
{
    public class ProgramDTO
    {
        public int ProgramId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? DefaultDayNumber { get; set; }
    }
}
