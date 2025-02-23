using System.ComponentModel.DataAnnotations.Schema;

namespace Ajloun_Tour.Models
{
    [Table("TourPackages")]
    public class TourPackage
    {
        public int TourId { get; set; }
        public int PackageId { get; set; }

        public Tour? Tour { get; set; }
        public Package? Package { get; set; }
    }
}
