namespace Ajloun_Tour.DTOs2.ToursProgramDTOs
{
    public class ToursProgramDTO
    {
        public int ProgramId { get; set; }
        public int TourId { get; set; }
        public string TourName { get; set; } = null!;  
        public int DayNumber { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? ProgramDate { get; set; }
    }
}
