using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;

namespace NotikaIdentityEmail.Controllers
{
    public class ActivationController : Controller
    {
        private readonly EmailContext _context;

        public ActivationController(EmailContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult UserActivation()
        {
            var email = TempData["EmailMove"];
            TempData["Test1"] = email;
            return View();
        }

        [HttpPost]
        public IActionResult UserActivation(int userCodeParameter)
        {
            string email = TempData["Test1"].ToString();
            var code = _context.Users.Where(x => x.Email == email).Select(y => y.ActivationCode).FirstOrDefault();

            if (userCodeParameter == code)
            {
                var value = _context.Users.Where(x => x.Email == email).FirstOrDefault();
                value.EmailConfirmed = true;
                _context.SaveChanges();
                return RedirectToAction("UserLogin", "Login");
            }
            return View();
        }
    }
}