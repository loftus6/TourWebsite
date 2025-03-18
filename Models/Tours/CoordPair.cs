using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TourWebsite.Models.Tours
{
    public class CoordPair
    {
        public double Longitude { get; set; }

        [Required]
        [Range(-90, 90)]
        [Column(TypeName = "decimal(25, 8)")]
        public double Lattitude { get; set; }
    }
}
