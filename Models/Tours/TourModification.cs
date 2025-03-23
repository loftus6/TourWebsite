using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourWebsite.Models.Tours
{
    public class TourModification
    {

        public string TourID { get; set; }
        public string Title { get; set; }
        public string TourDescription { get; set; }
        [Range(-180, 180)]
        [Column(TypeName = "decimal(25, 8)")]
        public double Longitude { get; set; }
        [Range(-90, 90)]
        [Column(TypeName = "decimal(25, 8)")]
        public double Lattitude { get; set; }

        public VisibilityType Visibility { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email.")]
        public string? Email { get; set; }

        public string[]? DeleteIds { get; set; } //This ID is actually in the form of an email for compatibility with Identity Library


        [EmailAddress(ErrorMessage = "Invalid Email.")]
        public string? EmailViewer { get; set; }

        public string[]? DeleteIdsViewer { get; set; } //Likewise

        public string Thumbnail { get; set; }

        public bool RemoveThumbnail { get; set; }
    }
}
