using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TourWebsite.Areas.Identity.Data;

namespace TourWebsite.Models.Roles
{
    public class RoleEdit
    {
        public TourWebsiteRole Role { get; set; }
        public IEnumerable<TourWebsiteUser> Members { get; set; }


        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        [StringLength(30)]
        public string Email { get; set; }
    }
}
