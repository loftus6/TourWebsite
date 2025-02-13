using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TourWebsite.Models;

namespace TourWebsite.Data
{
    public class TourWebsiteContext : DbContext
    {
        public TourWebsiteContext (DbContextOptions<TourWebsiteContext> options)
            : base(options)
        {
        }

        public DbSet<TourWebsite.Models.TourSite> TourSites { get; set; } = default!;
    }
}
