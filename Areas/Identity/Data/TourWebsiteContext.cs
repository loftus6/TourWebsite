using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TourWebsite.Areas.Identity.Data;

namespace TourWebsite.Data;

public class TourWebsiteContext : IdentityDbContext<TourWebsiteUser>
{
    public TourWebsiteContext(DbContextOptions<TourWebsiteContext> options)
        : base(options)
    {
    }

    public DbSet<TourWebsite.Models.TourSite> TourSites { get; set; } = default!;

    public DbSet<UploadedFile> UploadedFiles { get; set; } = default!;

    public async Task<NonTourPage> GetMain()
    {
        return await GetByName("Main");

    }

    public async Task<NonTourPage> GetByName(string name)
    {
        var pages = from m in NonTourPage
                    select m;

        var mainList = pages.Where(s => s.Title == name);

        var listAsync = await mainList.ToListAsync();



        return listAsync[0]; //May be a better approach


    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<TourWebsite.Areas.Identity.Data.TourWebsiteRole> TourWebsiteRole { get; set; } = default!;

    public DbSet<TourWebsite.Areas.Identity.Data.NonTourPage> NonTourPage { get; set; } = default!;
}
