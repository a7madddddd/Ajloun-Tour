using Ajloun_Tour.DTOs2.ToursProgramDTOs;

namespace Ajloun_Tour.DTOs2.OffersProgramDTOs
{
    public class OfferWithProgramsDTO
    {
        public int OfferId { get; set; }
        public string Title { get; set; } = null!;
        public List<OffersProgramDTO> Programs { get; set; } = new List<OffersProgramDTO>();
    }
}
