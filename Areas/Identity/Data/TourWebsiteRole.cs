using Microsoft.AspNetCore.Identity;

namespace TourWebsite.Areas.Identity.Data
{
    public class TourWebsiteRole : IdentityRole
    {

        public TourWebsiteRole(string name)
        {
            Name = name;
        }
    }
}
