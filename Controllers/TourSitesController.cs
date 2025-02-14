using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TourWebsite.Data;
using TourWebsite.Models;

namespace TourWebsite.Controllers
{
    public class TourSitesController : Controller
    {
        private readonly TourWebsiteContext _context;

        public TourSitesController(TourWebsiteContext context)
        {
            _context = context;
        }

        // GET: TourSites
        public async Task<IActionResult> Index()
        {
            return View(await _context.TourSites.ToListAsync());
        }

        // GET: TourSites/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: TourSites/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TourSites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Edit(int? id)
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
            return View(tourSite);
        }

        // POST: TourSites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Longitude,Lattitude")] TourSite tourSite)
        {
            if (id != tourSite.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
            return View(tourSite);
        }

        // GET: TourSites/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        private bool TourSiteExists(int id)
        {
            return _context.TourSites.Any(e => e.Id == id);
        }
    }
}
