using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TourWebsite.Areas;
using TourWebsite.Areas.Identity.Data;
using TourWebsite.Data;
using TourWebsite.Models;

namespace TourWebsite.Controllers
{
    public class NonTourPagesController : Controller
    {
        private readonly TourWebsiteContext _context;
        private IAuthorizationService authService;

        public NonTourPagesController(TourWebsiteContext context, IAuthorizationService authService)
        {
            _context = context;
            this.authService = authService;
        }

        // GET: NonTourPages
        public async Task<IActionResult> Index()
        {
            return View(await _context.NonTourPage.ToListAsync());
        }

        // GET: NonTourPages/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nonTourPage = await _context.NonTourPage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nonTourPage == null)
            {
                return NotFound();
            }

            return View(nonTourPage);
        }

        // GET: NonTourPages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NonTourPages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,ThumbnailID,Title")] NonTourPage nonTourPage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nonTourPage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), controllerName:"Home");
            }
            return View(nonTourPage);
        }

        // GET: NonTourPages/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nonTourPage = await _context.NonTourPage.FindAsync(id);
            if (nonTourPage == null)
            {
                return NotFound();
            }

            return View(nonTourPage);
        }

        // POST: NonTourPages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, NonTourPage pageEdit)
        {
            if (ModelState.IsValid)
            {
                var page = await _context.NonTourPage.FindAsync(id);

                if (page is null)
                {
                    return NotFound();
                }

                AuthorizationResult authorized = await authService.AuthorizeAsync(User, null, "TourAccess");

                if (!authorized.Succeeded)
                {
                    return Redirect(Globals.AccessDeniedPath);
                }

                page.Title = pageEdit.Title;
                page.Description = pageEdit.Description;


                page.MainColor = pageEdit.MainColor;
                page.AccentColor = pageEdit.AccentColor;



                try
                {
                    _context.Update(page);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NonTourPageExists(page.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), controllerName:"Home");
            }
            return await Edit(pageEdit.Id);
        }


        private bool NonTourPageExists(string id)
        {
            return _context.NonTourPage.Any(e => e.Id == id);
        }
    }
}
