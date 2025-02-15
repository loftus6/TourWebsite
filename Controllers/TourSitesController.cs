using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TourWebsite.Areas.Identity.Data;
using TourWebsite.Areas.Identity.Pages.Account;
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
                return NotFound();
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

            var allowedUsers = tourSite.ApprovedEditUsers;

            AuthorizationResult authorized = await authService.AuthorizeAsync(User, allowedUsers, "TourAccess");

            if (!authorized.Succeeded)
            {
                return RedirectToAction(nameof(new AccessDeniedModel));
            }

                TourEdit edit = new TourEdit();

            List<TourWebsiteUser> members = new List<TourWebsiteUser>();
            List<TourWebsiteUser> nonMembers = new List<TourWebsiteUser>();


            if (allowedUsers != null)
            {
                foreach (var _id in allowedUsers)
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

                var allowedUsers = tourSite.ApprovedEditUsers;
                AuthorizationResult authorized = await authService.AuthorizeAsync(User, allowedUsers, "TourAccess");

                if (!authorized.Succeeded)
                {
                    return new ChallengeResult();
                }

                tourSite.Title = tourModification.TourName;
                tourSite.Description = tourModification.TourDescription;
                tourSite.Longitude = tourModification.Longitude;
                tourSite.Lattitude = tourModification.Longitude;

                List<string> newApprovedUsers = new List<string>();

                if (allowedUsers != null)
                {
                    foreach (string user in allowedUsers)
                    {
                        newApprovedUsers.Add(user);
                    }
                }


                if (tourModification.AddEmail != null)
                {
                    TourWebsiteUser user1 = await userManager.FindByEmailAsync(tourModification.AddEmail);
                    if (user1 != null)
                    {
    
                        newApprovedUsers.Add(user1.Id);
  
                    }
                }

                if (tourModification.DeleteIds != null) {
            
                    foreach (string userId in tourModification.DeleteIds)
                    {
                        newApprovedUsers.Remove(userId);
                    }

                }

                tourSite.ApprovedEditUsers = newApprovedUsers;


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
                //return RedirectToAction(nameof(Index));
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
