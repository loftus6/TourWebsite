using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourWebsite.Areas;
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
        public IActionResult Upload(FileSetup setup)
        {

            var fileEdit = new FileEdit();

            fileEdit.FileType = setup.fileType;

            fileEdit.EmbedOrUpload = setup.attachType == AttachType.Linked;

            return View(fileEdit);
        }

        [Authorize]
        public IActionResult UploadSetup()
        {
            return View(new FileSetup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult UploadSetup(FileSetup setup)
        {

            return RedirectToAction(nameof(Upload), setup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Upload(FileEdit fileEdit)
        {
            var newFile = fileEdit.NewFile;


            string newName = fileEdit.FileName;

  

            if (fileEdit.EmbedOrUpload && fileEdit.EmbedUrl != null)
            {

                newName = newName ?? "Embedded File";
                var embedFile = new UploadedFile()
                {
                    FileName = newName.Trim(),
                    Embed = true,
                    EmbedUrl = fileEdit.EmbedUrl,
                    FileType=fileEdit.FileType,
                    Bytes = [],
                    FileExtension = "Embed",
                };


                if (!String.IsNullOrWhiteSpace(fileEdit.TagToAdd))
                {
                    var split = fileEdit.TagToAdd.Split("\n");

                    foreach (string tag in split)
                    {
                        if (!String.IsNullOrWhiteSpace(tag) && !(embedFile.Tags.Contains(tag.Trim())))
                            embedFile.Tags.Add(tag.Trim());
                    }
                }


                _context.Add(embedFile);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (newFile != null && newFile.Length > 0) //if file is existing
                {
                    using (var memoryStream = new MemoryStream())
                    {

                        newName = newName ?? Path.GetFileNameWithoutExtension(newFile.FileName);
                        await newFile.CopyToAsync(memoryStream);

                        if (memoryStream.Length < (15 * 1024 * 1024) + 1)
                        { //max of 15 megabytes

                            var addedFile = new UploadedFile()
                            {
                                Bytes = memoryStream.ToArray(),
                                FileName = newName.Trim(),
                                FileExtension = Path.GetExtension(newFile.FileName),
                                FileType=fileEdit.FileType,
                                Embed = false,
                                EmbedUrl = ""
                            };


                            if (!String.IsNullOrWhiteSpace(fileEdit.TagToAdd))
                            {
                                var split = fileEdit.TagToAdd.Split("\n");

                                foreach (string tag in split)
                                {

                                    if (!String.IsNullOrWhiteSpace(tag) && !(addedFile.Tags.Contains(tag.Trim())))
                                        addedFile.Tags.Add(tag.Trim());
                                }
                            }
                            _context.Add(addedFile);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            ModelState.AddModelError("File", "File is to large, must be less than 15 mb");
                        }
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.UploadedFiles
                .FirstOrDefaultAsync(m => m.Id == id);



            if (file == null)
            {
                return NotFound();
            }

            AuthorizationResult authorized = await authService.AuthorizeAsync(User, null, "TourAccess");
            if (!authorized.Succeeded)
            {
                return Redirect(Globals.AccessDeniedPath);
            }

            return View(file);
        }

        // POST: TourSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var file = await _context.UploadedFiles.FindAsync(id);


            if (file != null)
            {


                AuthorizationResult authorized = await authService.AuthorizeAsync(User, null, "TourAccess");
                if (!authorized.Succeeded)
                {
                    return Redirect(Globals.AccessDeniedPath);
                }
                _context.UploadedFiles.Remove(file);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //Loading the view
        [Authorize]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.UploadedFiles.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }

            AuthorizationResult authorized = await authService.AuthorizeAsync(User, null, "TourAccess"); //Maybe make a new access rule 

            if (!authorized.Succeeded)
            {
                return Redirect(Globals.AccessDeniedPath);
            }

            FileEdit edit = new FileEdit();







            edit.FileToEdit = file;

            return View(edit); //pass auth service to this view
        }

        //Submitting the edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(FileEdit fileEdit)
        {

            var file = await _context.UploadedFiles.FindAsync(fileEdit.FileToEditId);

            if (file is null)
            {
                return NotFound();
            }

            AuthorizationResult authorized = await authService.AuthorizeAsync(User, null, "TourAccess");

            if (!authorized.Succeeded)
            {
                return Redirect(Globals.AccessDeniedPath);
            }


            //Updates
            file.FileName = fileEdit.FileName;

            var tags = file.Tags;


            if (!String.IsNullOrWhiteSpace(fileEdit.TagToAdd))
            {
                var split = fileEdit.TagToAdd.Split("\n");

                foreach (string tag in split)
                {
                    if (!String.IsNullOrWhiteSpace(tag) && !(tags.Contains(tag.Trim())))
                        tags.Add(tag.Trim());
                }
            }

            if (fileEdit.RemoveTags != null)
            {
                foreach (string remove in fileEdit.RemoveTags)
                {
                    tags.Remove(remove);
                }
            }
            file.Tags = tags;

            //push to db
            try
            {
                _context.Update(file);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileExists(file.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await Edit(fileEdit.FileToEditId);


        }


        public async Task<IActionResult> Details(string? id)
        {


            if (id == null)
            {
                return NotFound();
            }



            var file = await _context.UploadedFiles
                .FirstOrDefaultAsync(m => m.Id == id);

            if (file == null)
            {
                return NotFound();
            }


            AuthorizationResult authorized = await authService.AuthorizeAsync(User, null, "TourAccess"); //maybe make custom rule


            if (!authorized.Succeeded)
                return Redirect(Globals.AccessDeniedPath);

            return View(file);
        }

        public async Task<ActionResult> AttachmentList(string Target, string Target2, FileType fileType, string searchString = "", string searchType="")
        {





            var files = from m in _context.UploadedFiles
                        select m;

            var lst = new List<UploadedFile>();


            files = files.Where(s => s.FileType == fileType);

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchType == "Title")
                    files = files.Where(s => s.FileName!.ToUpper().Contains(searchString.ToUpper()));



                if (searchType == "Tag")
                {
                    var lst1 = await files.ToListAsync();

                    lst = lst1.Where(s => s.Tags!.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();

                }
            }

            if (searchType != "Tag" || String.IsNullOrEmpty(searchString))
                lst = await files.ToListAsync();

            return PartialView((lst, fileType, Target, Target2));
        }

        private bool FileExists(string id)
        {
            return _context.UploadedFiles.Any(e => e.Id == id);
        }

    }




}
