using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TourWebsite.Areas;
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
        private IConfiguration config;
        private UserManager<TourWebsiteUser> userManager;

        public TourSitesController(TourWebsiteContext context, IAuthorizationService auth, UserManager<TourWebsiteUser> userManager, IConfiguration config)
        {
            _context = context;
            authService = auth;
            this.userManager = userManager;
            this.config = config;
        }

        // GET: TourSites
        public async Task<IActionResult> Index()
        {


            return View(new CoordPair());
        }

        public async Task<IActionResult> ListView()
        {


            return View((await _context.TourSites.ToListAsync(), authService));
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


            if (!(tourSite.Visibility == VisibilityType.Public)) //if public, we don't need a visbility check
            {

                var allowedUsers = tourSite.ApprovedUsers; //restricted allows all viewers and editors, even without privledge
                allowedUsers = allowedUsers.Concat(tourSite.ApprovedEditUsers).ToList();
                if (tourSite.Visibility == VisibilityType.Private) //if private, we don't have approved users active
                    allowedUsers = tourSite.ApprovedEditUsers;

                AuthorizationResult authorized = await authService.AuthorizeAsync(User, allowedUsers, "TourAccess"); //check the rule, allows admins, editors, and anyone in the list


                if (!authorized.Succeeded)
                    return Redirect(Globals.AccessDeniedPath);
            }

            return View(tourSite);
        }

        // GET: TourSites/Create

        [Authorize]
        public IActionResult PassToCreate(CoordPair pair)
        {

            TourEdit edit = new TourEdit() {
                Longitude = pair.Longitude,
                Lattitude = pair.Lattitude
            };

            return RedirectToAction(nameof(Create), edit);
        }
        [Authorize]
        public IActionResult Create(TourEdit edit)
        {
            return View(edit);
        }


        // POST: TourSites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(TourModification tourModification)
        {

            var tourSite = new TourSite() { Title = tourModification.Title };


            AuthorizationResult authorized = await authService.AuthorizeAsync(User, null, "TourAccess");

            if (!authorized.Succeeded)
            {
                return Redirect(Globals.AccessDeniedPath);
            }

            tourSite.Description = tourModification.TourDescription;
            tourSite.Longitude = tourModification.Longitude;
            tourSite.Lattitude = tourModification.Lattitude;
            tourSite.Visibility = tourModification.Visibility;

            List<string> newApprovedEditors = new List<string>();
            List<string> newApprovedViewers = new List<string>();



            if (tourModification.Email != null) //Adds editor if email exists
            {
                TourWebsiteUser user1 = await userManager.FindByEmailAsync(tourModification.Email);
                if (user1 != null)
                {

                    newApprovedEditors.Add(user1.Email);

   
                }
            }

            tourSite.ApprovedEditUsers = newApprovedEditors;
            tourSite.ApprovedUsers = newApprovedViewers; //this is set to an empty list


            var thumb = tourModification.Thumbnail;


            if (thumb != null && thumb.Length > 0) //if file is existing
            {
                using (var memoryStream = new MemoryStream())
                {

                    await thumb.CopyToAsync(memoryStream);

                    if (memoryStream.Length < (15 * 1024 * 1024) + 1)
                    { //max of 15 megabytes

                        var newThumbnail = new UploadedFile()
                        {
                            Bytes = memoryStream.ToArray(),
                            FileName = thumb.FileName,
                            FileType = Path.GetExtension(thumb.FileName)
                        };

                        _context.Add(newThumbnail);


                        tourSite.ThumbnailID = newThumbnail.Id; //Add reference to id
                    }
                    else
                    {
                        ModelState.AddModelError("File", "File is to large, must be less than 15 mb");
                    }
                }
            }


            _context.Add(tourSite);
            await _context.SaveChangesAsync();
  
            return RedirectToAction(nameof(Index));

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
                return Redirect(Globals.AccessDeniedPath);
            }

            TourEdit edit = new TourEdit();

            List<TourWebsiteUser> members = new List<TourWebsiteUser>();
            List<TourWebsiteUser> viewers = new List<TourWebsiteUser>();


            if (allowedUsers != null)
            {
                foreach (var email in allowedUsers)
                {
                    members.Add(await userManager.FindByEmailAsync(email));
                }
            }

            var allowedViewers = tourSite.ApprovedUsers;

            if (allowedViewers != null)
            {
                foreach (var email in allowedViewers)
                {
                    viewers.Add(await userManager.FindByEmailAsync(email));
                }
            }





            edit.TourSite = tourSite;
            edit.Members = members;
            edit.Viewers = viewers;

            return View(edit); //pass auth service to this view
        }

        // POST: TourSites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(TourModification tourModification)
        {



            if (ModelState.IsValid || true) //This model is invalid for some reason, but it seems to work
            {
                var tourSite = await _context.TourSites.FindAsync(tourModification.TourID);

                if (tourSite is null)
                {
                    return NotFound();
                }

                var allowedUsers = tourSite.ApprovedEditUsers;
                var allowedViewers = tourSite.ApprovedUsers;
                AuthorizationResult authorized = await authService.AuthorizeAsync(User, allowedUsers, "TourAccess");

                if (!authorized.Succeeded)
                {
                    return Redirect(Globals.AccessDeniedPath);
                }

                tourSite.Title = tourModification.Title;
                tourSite.Description = tourModification.TourDescription;
                tourSite.Longitude = tourModification.Longitude;
                tourSite.Lattitude = tourModification.Lattitude;
                tourSite.Visibility = tourModification.Visibility;

                List<string> newApprovedUsers = new List<string>();
                List<string> newApprovedViewers = new List<string>();

                if (allowedUsers != null) //adds the editors
                {
                    foreach (string user in allowedUsers)
                    {
                        newApprovedUsers.Add(user);
                    }
                }


                if (tourModification.Email != null)
                {
                    TourWebsiteUser user1 = await userManager.FindByEmailAsync(tourModification.Email);
                    if (user1 != null)
                    {

                        newApprovedUsers.Add(user1.Email);

                    }
                }

                if (tourModification.DeleteIds != null) { //removes users

                    foreach (string userId in tourModification.DeleteIds)
                    {
                        newApprovedUsers.Remove(userId);
                    }

                }

                tourSite.ApprovedEditUsers = newApprovedUsers;




                if (allowedViewers != null) //adds the viewers
                {
                    foreach (string user in allowedViewers)
                    {
                        newApprovedViewers.Add(user);
                    }
                }


                if (tourModification.EmailViewer != null)
                {
                    TourWebsiteUser user1 = await userManager.FindByEmailAsync(tourModification.EmailViewer);
                    if (user1 != null)
                    {

                        newApprovedViewers.Add(user1.Email);

                    }
                }

                if (tourModification.DeleteIdsViewer != null) //removes viewers
                {

                    foreach (string userId in tourModification.DeleteIdsViewer)
                    {
                        newApprovedViewers.Remove(userId);
                    }

                }

                tourSite.ApprovedUsers = newApprovedViewers;


                var thumb = tourModification.Thumbnail; //adds thumbnail image


                if (thumb != null && thumb.Length > 0) //if file is existing
                {
                    using (var memoryStream = new MemoryStream())
                    {

                        await thumb.CopyToAsync(memoryStream);

                        if (memoryStream.Length < (15 * 1024 * 1024) + 1)
                        { //max of 15 megabytes

                            var newThumbnail = new UploadedFile()
                            {
                                Bytes = memoryStream.ToArray(),
                                FileName = thumb.FileName,
                                FileType = Path.GetExtension(thumb.FileName)
                            };

                            _context.Add(newThumbnail);


                            tourSite.ThumbnailID = newThumbnail.Id; //Add reference to id
                        }
                        else
                        {
                            ModelState.AddModelError("File", "File is to large, must be less than 15 mb");
                        }
                    }
                }


                if (tourModification.RemoveThumbnail) {

                    var thumbId = tourSite.ThumbnailID;

                    if (thumbId != null)
                    {
                        var toRemove = _context.UploadedFiles.Find(thumbId);

                        if (toRemove != null)
                        {
                            _context.Remove(toRemove);

                            tourSite.ThumbnailID = null;
                        }
                    }
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
            }
            //return RedirectToAction(nameof(Index));
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
            var allowedUsers = tourSite.ApprovedEditUsers;
            AuthorizationResult authorized = await authService.AuthorizeAsync(User, allowedUsers, "TourAccess");
            if (!authorized.Succeeded)
            {
                return Redirect(Globals.AccessDeniedPath);
            }

            return View(tourSite);
        }

        // POST: TourSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tourSite = await _context.TourSites.FindAsync(id);


            if (tourSite != null)
            {

                var allowedUsers = tourSite.ApprovedEditUsers;
                AuthorizationResult authorized = await authService.AuthorizeAsync(User, allowedUsers, "TourAccess");
                if (!authorized.Succeeded)
                {
                    return Redirect(Globals.AccessDeniedPath);
                }
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
