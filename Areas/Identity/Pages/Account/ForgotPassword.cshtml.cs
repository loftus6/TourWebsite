﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using TourWebsite.Areas.Identity.Data;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace TourWebsite.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<TourWebsiteUser> _userManager;
        private readonly IEmailSender _emailSender;
        private IConfiguration config;

        public ForgotPasswordModel(UserManager<TourWebsiteUser> userManager, IEmailSender emailSender, IConfiguration config)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            this.config = config;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Fairfield History WV", config["SMTPSender"]));
                message.To.Add(new MailboxAddress("", Input.Email));
                message.Subject = "Password Reset";
                
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);
                
                var bodyText =
                    "Hello, \n Please click the link below to reset your password\n" +
                    callbackUrl + "\n this was an automatically sent message, please do not reply"; 
                

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
                
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
