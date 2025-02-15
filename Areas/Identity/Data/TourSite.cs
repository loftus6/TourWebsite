using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TourWebsite.Models
{
    public class TourSite
    {
        public TourSite()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Range(-180, 180)]
        [Column(TypeName = "decimal(25, 8)")]
        public double Longitude { get; set; }
        [Range(-90, 90)]
        [Column(TypeName = "decimal(25, 8)")]
        public double Lattitude { get; set; }

        public List<string> ApprovedEditUsers { get; set; }
        public List<string> ApprovedUsers { get; set; }

    }
}
