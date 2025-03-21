using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourWebsite.Areas.Identity.Data;
using TourWebsite.Data;
using TourWebsite.Models;
using TourWebsite.Models.Files;
using TourWebsite.Models.Tours;

namespace TourWebsite.Controllers
{
    public class FileController : Controller
    {

        private readonly TourWebsiteContext _context;
        private IAuthorizationService authService;
        private UserManager<TourWebsiteUser> userManager;

        public FileController(TourWebsiteContext context, IAuthorizationService auth, UserManager<TourWebsiteUser> userManager, IConfiguration config)
        {
            _context = context;
            authService = auth;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.UploadedFiles.ToListAsync());
        }

        [Authorize]
        public IActionResult Upload()
        {
            return View(new FileEdit());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Upload(FileEdit fileEdit)
        {
            var newFile = fileEdit.NewFile;

            var one = 1 + 1;
            if (newFile != null && newFile.Length > 0) //if file is existing
            {
                using (var memoryStream = new MemoryStream())
                {

                    await newFile.CopyToAsync(memoryStream);

                    if (memoryStream.Length < (15 * 1024 * 1024) + 1)
                    { //max of 15 megabytes

                        var addedFile = new UploadedFile()
                        {
                            Bytes = memoryStream.ToArray(),
                            FileName = Path.GetFileNameWithoutExtension(newFile.FileName),
                            FileType = Path.GetExtension(newFile.FileName)
                        };

                        _context.Add(addedFile);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("File", "File is to large, must be less than 15 mb");
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
