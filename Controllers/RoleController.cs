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

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace TourWebsite.Controllers
{
    public class RoleController : Controller
    {
        private readonly TourWebsiteContext _context;

 

        private RoleManager<TourWebsiteRole> roleManager;
        private UserManager<TourWebsiteUser> userManager;
        private IConfiguration config;
        private readonly IUserStore<TourWebsiteUser> userStore;
        private readonly IUserEmailStore<TourWebsiteUser> emailStore;
        
        
        private List<string> colorsList = new List<string>()
        {
            "Red",
            "Blue",
            "Green",
            "Purple",
            "Indigo",
            "Orange",
            "Magenta",
        };
        
        private List<string> animalList = new List<string>()
        {
            "Labrador",
            "Leopard",
            "Horse",
            "Cheetah",
            "Falcon",
            "Dolphin",
            "Salamander",
        };
        
        private List<string> characterList = new List<string>()
        {
            "!",
            "#",
            "$",
            "*",
            "%",
            "^",
            "~",
        };

        public string PasswordGen()
        {
            Random rnd = new Random();
            var pass = "";
            pass += colorsList[rnd.Next(0, colorsList.Count)];
            pass += animalList[rnd.Next(0, animalList.Count)];
            pass += rnd.Next(0, 1000);
            pass += characterList[rnd.Next(0, characterList.Count)];

            return pass;
        }

        public RoleController(TourWebsiteContext context, RoleManager<TourWebsiteRole> roleMgr, 
            UserManager<TourWebsiteUser> userMrg, IConfiguration config, IUserStore<TourWebsiteUser> userStore)

        {
            _context = context;
            roleManager = roleMgr;
            userManager = userMrg;
            this.config = config;
            this.userStore = userStore;
            this.emailStore = (IUserEmailStore<TourWebsiteUser>)userStore;
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
            if (ModelState.IsValid || true)
            {

                if (model.AddEmail != null) //Adds editor if email exists
                {

                    var emails = model.AddEmail.Split("\n");

                    foreach (string email in emails)
                    {
                        var fixed_email = email.Trim();
                        TourWebsiteUser user1 = await userManager.FindByEmailAsync(fixed_email);

                        if (user1 != null)
                        {

                            result = await userManager.AddToRoleAsync(user1, model.RoleName);
                            if (!result.Succeeded)
                                return NotFound();

                        }

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


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InviteUser(string email)
        {

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Fairfield History WV", config["SMTPSender"]));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Welcome to Fairfield History WV";
            
            var user = Activator.CreateInstance<TourWebsiteUser>();
            
            var password = PasswordGen();
            
            await userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await userManager.CreateAsync(user, password);


            if (result.Succeeded)
            {
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var result2 = await userManager.ConfirmEmailAsync(user, code);
                var EmailConfirmationUrl = Url.Page(
                    "/Account/Login",
                    pageHandler: null,
                    values: new { area = "Identity", DefaultPassword=true },
                    protocol: Request.Scheme);

                var bodyText =
                    "Hello, \n Please click the link below to join fairfield history WV.\n" +
                    "Login with the following credentials:\n" +
                    "Username: " +email +"\n" +
                    "Password: " +password +"\n"+
                    EmailConfirmationUrl + "\n this was an automatically sent message, please do not reply"; 
                

                message.Body = new TextPart("plain")
                {
                    Text = bodyText
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("mail.smtp2go.com", 2525, false);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(config["SMTPUser"], config["SMTPPassword"]);

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            
            
            return RedirectToAction(nameof(Index));
        }
    }
}
