using Microsoft.EntityFrameworkCore;

namespace TourWebsite.Models
{
    public class TourContext : DbContext
    {
        public DbSet<TourSite> TourSites { get; set; }

        public TourContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
