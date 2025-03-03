using Ajloun_Tour.DTOs2.ToursProgramDTOs;

namespace Ajloun_Tour.DTOs2.PackagesProgramDTOs
{
    public class PackageWithProgramsDTO
    {
        public int PackageId { get; set; }
        public string Name { get; set; } = null!;
        public List<PackagesProgramDTO> Programs { get; set; } = new List<PackagesProgramDTO>();
    }
}
