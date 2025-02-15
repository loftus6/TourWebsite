using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourWebsite.Models.Tours
{
    public class TourModification
    {

        public string TourID { get; set; }
        public string TourName { get; set; }
        public string TourDescription { get; set; }
        [Range(-180, 180)]
        [Column(TypeName = "decimal(25, 8)")]
        public double Longitude { get; set; }
        [Range(-90, 90)]
        [Column(TypeName = "decimal(25, 8)")]
        public double Lattitude { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email.")]
        public string? AddEmail { get; set; }

        public string[]? DeleteIds { get; set; }
    }
}
