namespace Ajloun_Tour.Models
{
    public class TourPackage
    {
        public int TourId { get; set; }
        public int PackageId { get; set; }

        public Tour? Tour { get; set; }
        public Package? Package { get; set; }
    }
}
