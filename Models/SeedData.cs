using Microsoft.EntityFrameworkCore;
using TourWebsite.Areas.Identity.Data;
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
                if (context.NonTourPage.Any())
                {
                    return;   // DB has been seeded
                }

                var mainPage = new NonTourPage();
                mainPage.Title = "Main";
                mainPage.BackgroundColor = "#e0fff3";
                mainPage.AccentColor1 = "#becfc8";

                context.NonTourPage.Add(mainPage);

                context.SaveChanges();


            }
        }
    }
}
