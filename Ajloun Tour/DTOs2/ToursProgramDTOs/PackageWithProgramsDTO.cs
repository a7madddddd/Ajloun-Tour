namespace Ajloun_Tour.DTOs2.ToursProgramDTOs
{
    public class PackageWithProgramsDTO
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; } = null!;
        public int ProgramId { get; set; }
        public int DayNumber { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? ProgramDate { get; set; }
        public List<ToursProgramDTO> TourPrograms { get; set; } = new List<ToursProgramDTO>();


    }
}
