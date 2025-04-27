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
                mainPage.MainColor = "#e0fff3";
                mainPage.AccentColor = "#becfc8";

                mainPage.ColorName1 = "Background Color";
                mainPage.ColorName2 = "Toolbar Color";

                context.NonTourPage.Add(mainPage);

                var privacyPage = new NonTourPage();
                privacyPage.Title = "Privacy";
                

                context.NonTourPage.Add(privacyPage);


                var mapPage = new NonTourPage();
                mapPage.Title = "Tour Map";
                mapPage.ColorName1 = "Button Color";

                context.NonTourPage.Add(mapPage);


                context.SaveChanges();


            }
        }
    }
}
