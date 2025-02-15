using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TourWebsite.Areas.Identity.Data;

// Add profile data for application users by adding properties to the TourWebsiteUser class
public class TourWebsiteUser : IdentityUser
{
    public int[]? ApprovedSites { get; set; }
}

