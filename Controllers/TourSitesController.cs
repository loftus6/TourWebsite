using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TourWebsite.Areas.Identity.Data;
using TourWebsite.Data;
using TourWebsite.Models;
using TourWebsite.Models.Tours;

namespace TourWebsite.Controllers
{
    public class TourSitesController : Controller
    {
        private readonly TourWebsiteContext _context;
        private IAuthorizationService authService;
        private UserManager<TourWebsiteUser> userManager;

        public TourSitesController(TourWebsiteContext context, IAuthorizationService auth, UserManager<TourWebsiteUser> userManager)
        {
            _context = context;
            authService = auth;
            this.userManager = userManager;
        }

        // GET: TourSites
        public async Task<IActionResult> Index()
        {


            return View(await _context.TourSites.ToListAsync());
        }

        // GET: TourSites/Details/5
        public async Task<IActionResult> Details(string? id)
        {
          

            if (id == null)
            {
                return NotFound();
            }



            var tourSite = await _context.TourSites
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tourSite == null)
            {
                return NotFound();
            }
            var allowedUsers = tourSite.ApprovedUsers;

            AuthorizationResult authorized = await authService.AuthorizeAsync(User, allowedUsers, "TourAccess");


            if (authorized.Succeeded)
                return View(tourSite);
            else
                return new ChallengeResult();

        }

        // GET: TourSites/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TourSites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Longitude,Lattitude")] TourSite tourSite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tourSite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tourSite);
        }

        // GET: TourSites/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourSite = await _context.TourSites.FindAsync(id);
            if (tourSite == null)
            {
                return NotFound();
            }


            TourEdit edit = new TourEdit();

            List<TourWebsiteUser> members = new List<TourWebsiteUser>();
            List<TourWebsiteUser> nonMembers = new List<TourWebsiteUser>();

            var ids = tourSite.ApprovedEditUsers;

            if (ids != null)
            {
                foreach (var _id in ids)
                {
                    members.Add(await userManager.FindByIdAsync(_id));
                }
            }

            edit.TourSite = tourSite;
            edit.Members = members;
            return View(edit);
        }

        // POST: TourSites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(TourModification tourModification)
        {



            if (ModelState.IsValid)
            {
                var tourSite = await _context.TourSites.FindAsync(tourModification.TourID);

                if (tourSite is null)
                {
                    return NotFound();
                }
                tourSite.Title = tourModification.TourName;
                tourSite.Description = tourModification.TourDescription;
                tourSite.Longitude = tourModification.Longitude;
                tourSite.Lattitude = tourModification.Longitude;

                if (tourModification.AddEmail != null)
                {
                    TourWebsiteUser user1 = await userManager.FindByEmailAsync(tourModification.AddEmail);
                    if (user1 != null)
                    {
                        if (tourSite.ApprovedEditUsers == null)
                        {
                            tourSite.ApprovedEditUsers = new string[] { user1.Id };
                        }
                        else
                        {
                            tourSite.ApprovedEditUsers.Append(user1.Id);
                        }
                    }
                }

                if (tourModification.DeleteIds is not null && tourSite.ApprovedEditUsers is not null) {
                    var list = tourSite.ApprovedEditUsers.OfType<string>().ToList();
            
                    foreach (string userId in tourModification.DeleteIds)
                    {
                        list.Remove(userId);
                    }

                    tourSite.ApprovedEditUsers = list.ToArray();
                }


                try
                {
                    _context.Update(tourSite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourSiteExists(tourSite.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return await Edit(tourModification.TourID);
        }

        // GET: TourSites/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourSite = await _context.TourSites
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tourSite == null)
            {
                return NotFound();
            }

            return View(tourSite);
        }

        // POST: TourSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tourSite = await _context.TourSites.FindAsync(id);
            if (tourSite != null)
            {
                _context.TourSites.Remove(tourSite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TourSiteExists(string id)
        {
            return _context.TourSites.Any(e => e.Id == id);
        }
    }
}
