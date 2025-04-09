using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TourWebsite.Areas.Identity.Data;

namespace TourWebsite.Models.Tours
{
    public class TourEdit
    {

        public TourSite TourSite { get; set; }

        public IEnumerable<TourWebsiteUser> Members { get; set; }
        public IEnumerable<TourWebsiteUser> Viewers { get; set; }

        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")] //https://stackoverflow.com/questions/16712043/email-address-validation-using-asp-net-mvc-data-type-attributes
        //[StringLength(50)]
        //Add this handling in the controller to support lists
        public string Email { get; set; }

        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")] //https://stackoverflow.com/questions/16712043/email-address-validation-using-asp-net-mvc-data-type-attributes
        //[StringLength(50)]
        public string? EmailViewer { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Range(-180, 180)]
        [Column(TypeName = "decimal(25, 8)")] //These are just for input validation checking
        public double Longitude { get; set; }

        [Required]
        [Range(-90, 90)]
        [Column(TypeName = "decimal(25, 8)")]
        public double Lattitude { get; set; }

        public string Thumbnail { get; set; }

        public string AudioTrack { get; set; }

        public string IconColor { get; set; }
        public string IconBorderColor { get; set; }

        public string NextTourID { get; set; }


        //For editing
        public bool RemoveThumbnail { get; set; }
        public bool RemoveAudio { get; set; }
        public bool RemoveNextTour { get; set; }

        public string TourID { get; set; }
        public string TourDescription { get; set; }
        public VisibilityType Visibility { get; set; }

        public string[]? DeleteIds { get; set; } //This ID is actually in the form of an email for compatibility with Identity Library

        public string[]? DeleteIdsViewer { get; set; } //Likewise
    }
}
