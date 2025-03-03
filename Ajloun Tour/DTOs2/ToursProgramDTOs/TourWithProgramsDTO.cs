namespace Ajloun_Tour.DTOs2.ToursProgramDTOs
{
    public class TourWithProgramsDTO
    {

        public int TourId { get; set; }
        public string TourName { get; set; } = null!;
        public List<ToursProgramDTO> Programs { get; set; } = new List<ToursProgramDTO>();
    }
}
