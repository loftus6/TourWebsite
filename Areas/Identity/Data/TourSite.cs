using System.ComponentModel.DataAnnotations.Schema;

namespace TourWebsite.Models
{
    public class TourSite
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(25, 8)")]
        public double Longitude { get; set; }
        [Column(TypeName = "decimal(25, 8)")]
        public double Lattitude { get; set; }

    }
}
