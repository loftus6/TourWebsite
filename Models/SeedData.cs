using Microsoft.EntityFrameworkCore;
using TourWebsite.Data;

namespace TourWebsite.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TourWebsiteContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<TourWebsiteContext>>()))
            {
                if (context == null || context.TourSites == null)
                {
                    throw new ArgumentNullException("Null RazorPagesMovieContext");
                }

                // Look for any movies.
                if (context.TourSites.Any())
                {
                    return;   // DB has been seeded
                }

                //context.TourSites.AddRange(
                //    new TourSite
                //    {
                //        Title = "When Harry Met Sally",
                //        Description = "asdad",
                //        Longitude = 10,
                //        Lattitude = 10,
                //        ApprovedEditUsers = new List<string>(),
                //        ApprovedUsers = new List<string>(),

                //    });

                context.SaveChanges();
            }
        }
    }
}
