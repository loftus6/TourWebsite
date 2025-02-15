using Microsoft.AspNetCore.Identity;
using TourWebsite.Areas.Identity.Data;

namespace TourWebsite.Models.Roles
{
    public class RoleEdit
    {
        public TourWebsiteRole Role { get; set; }
        public IEnumerable<TourWebsiteUser> Members { get; set; }
        public IEnumerable<TourWebsiteUser> NonMembers { get; set; }
    }
}
