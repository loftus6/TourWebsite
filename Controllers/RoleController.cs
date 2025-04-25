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
using TourWebsite.Models.Roles;

namespace TourWebsite.Controllers
{
    public class RoleController : Controller
    {
        private readonly TourWebsiteContext _context;

 

        private RoleManager<TourWebsiteRole> roleManager;
        private UserManager<TourWebsiteUser> userManager;

        public RoleController(TourWebsiteContext context, RoleManager<TourWebsiteRole> roleMgr, UserManager<TourWebsiteUser> userMrg)

        {
            _context = context;
            roleManager = roleMgr;
            userManager = userMrg;
        }

        // GET: Role
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View((await _context.TourWebsiteRole.ToListAsync(), await _context.NonTourPage.ToListAsync()));
        }

        // GET: Role/Details/5
        [Authorize(Roles = "Admin")]
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

        public async Task<ActionResult> UserList(string Target, string searchString = "")
        {

            var users = from m in _context.Users
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Email!.ToUpper().Contains(searchString.ToUpper()));
            }
            return PartialView((await users.ToListAsync(), Target));
        }

        private bool TourWebsiteRoleExists(string id)
        {
            return _context.TourWebsiteRole.Any(e => e.Id == id);
        }

        //based on https://www.yogihosting.com/aspnet-core-identity-roles/
        public async Task<IActionResult> Update(string id)
        {
            TourWebsiteRole role = await roleManager.FindByIdAsync(id);
            List<TourWebsiteUser> members = new List<TourWebsiteUser>();
            List<TourWebsiteUser> nonMembers = new List<TourWebsiteUser>();
            foreach (TourWebsiteUser user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEdit
            {
                Role = role,
                Members = members,
                Email = ""
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoleModification model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {

                if (model.AddEmail != null)
                {
                    TourWebsiteUser user1 = await userManager.FindByEmailAsync(model.AddEmail);
                    if (user1 != null)
                    {
                        result = await userManager.AddToRoleAsync(user1, model.RoleName);
                        if (!result.Succeeded)
                            return NotFound();
                    }
                }
                foreach (string userId in model.DeleteIds ?? new string[] { })
                {
                    TourWebsiteUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            return NotFound();
                    }
                }
                
            }



            return await Update(model.RoleId);
        }
    }
}
