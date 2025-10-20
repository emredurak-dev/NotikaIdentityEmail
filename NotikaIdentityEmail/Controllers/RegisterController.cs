using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.IdentityModels;
using System.Threading.Tasks;

namespace NotikaIdentityEmail.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterUserViewModel model)
        {
            Random rnd = new Random();
            int code = rnd.Next(100000, 1000000);
            AppUser appUser = new AppUser()
            {
                Name = model.Name,
                Surname = model.Surname,
                UserName = model.Username,
                Email = model.Email,
                ActivationCode = code
            };

            var result = await _userManager.CreateAsync(appUser, model.Password);

            if (result.Succeeded)
            {
                // Email configuration - Replace with your SMTP settings
                MimeMessage mimeMessage = new MimeMessage();

                MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin", "YOUR_EMAIL_ADDRESS_HERE");
                mimeMessage.From.Add(mailboxAddressFrom);

                MailboxAddress mailboxAddressTo = new MailboxAddress("User", model.Email);
                mimeMessage.To.Add(mailboxAddressTo);

                var budyBuilder = new BodyBuilder();
                budyBuilder.TextBody = $"Your registration has been completed successfully. Please use the code {code} to activate your account.";
                mimeMessage.Body = budyBuilder.ToMessageBody();

                mimeMessage.Subject = "Account Activation";

                SmtpClient client = new SmtpClient();
                client.Connect("YOUR_SMTP_SERVER_HERE", 587, false);
                client.Authenticate("YOUR_EMAIL_ADDRESS_HERE", "YOUR_EMAIL_PASSWORD_HERE");
                client.Send(mimeMessage);
                client.Disconnect(true);

                TempData["EmailMove"] = model.Email;

                return RedirectToAction("UserActivation", "Activation");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View();
        }
    }
}
