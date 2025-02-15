using System.ComponentModel.DataAnnotations;
using TourWebsite.Areas.Identity.Data;

namespace TourWebsite.Models.Tours
{
    public class TourEdit
    {

        public TourSite TourSite { get; set; }

        public IEnumerable<TourWebsiteUser> Members { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        [StringLength(30)]
        public string Email { get; set; }
    }
}
