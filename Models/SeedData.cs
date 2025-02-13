using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TourWebsite.Models;

namespace TourWebsite.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new TourContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<TourContext>>()))
        {
            // Look for any tours.
            if (context.TourSites.Any())
            {
                return;   // DB has been seeded
            }
            context.TourSites.AddRange(
                new TourSite
                {
                    Title = "Place1",
                    Description = "Test Descrip 1",
                    Coordinates = [0,0,0]
                },
                new TourSite
                {
                    Title = "Place2",
                    Description = "Test Descrip 2",
                    Coordinates = [0, 0, 0.5431]
                }

            );
            context.SaveChanges();
        }
    }
}