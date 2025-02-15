using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TourWebsite.Areas.Identity.Data;
using TourWebsite.Data;

namespace TourWebsite.Controllers
{
    public class RoleController : Controller
    {
        private readonly TourWebsiteContext _context;

        public RoleController(TourWebsiteContext context)
        {
            _context = context;
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
            return View(await _context.TourWebsiteRole.ToListAsync());
        }

        // GET: Role/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourWebsiteRole = await _context.TourWebsiteRole
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tourWebsiteRole == null)
            {
                return NotFound();
            }

            return View(tourWebsiteRole);
        }

        // GET: Role/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Id,Name,NormalizedName,ConcurrencyStamp")] TourWebsiteRole tourWebsiteRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tourWebsiteRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tourWebsiteRole);
        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourWebsiteRole = await _context.TourWebsiteRole.FindAsync(id);
            if (tourWebsiteRole == null)
            {
                return NotFound();
            }
            return View(tourWebsiteRole);
        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,NormalizedName,ConcurrencyStamp")] TourWebsiteRole tourWebsiteRole)
        {
            if (id != tourWebsiteRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tourWebsiteRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourWebsiteRoleExists(tourWebsiteRole.Id))
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
            return View(tourWebsiteRole);
        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourWebsiteRole = await _context.TourWebsiteRole
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tourWebsiteRole == null)
            {
                return NotFound();
            }

            return View(tourWebsiteRole);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tourWebsiteRole = await _context.TourWebsiteRole.FindAsync(id);
            if (tourWebsiteRole != null)
            {
                _context.TourWebsiteRole.Remove(tourWebsiteRole);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TourWebsiteRoleExists(string id)
        {
            return _context.TourWebsiteRole.Any(e => e.Id == id);
        }
    }
}
