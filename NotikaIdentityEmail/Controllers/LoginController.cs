using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.IdentityModels;
using NotikaIdentityEmail.Models.JwtModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NotikaIdentityEmail.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSettingsModel _jwtSettingsModel;

        // ✅ Artık IOptions<JwtSettingsModel> üzerinden alıyoruz
        public LoginController(SignInManager<AppUser> signInManager, EmailContext context, UserManager<AppUser> userManager, IOptions<JwtSettingsModel> jwtSettings)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
            _jwtSettingsModel = jwtSettings.Value;
        }

        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(UserLoginViewModel model)
        {
            var value = _context.Users.FirstOrDefault(x => x.UserName == model.Username);

            if (value == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return View(model);
            }

            if (!value.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Your email address has not been confirmed yet.");
                return View(model);
            }

            if (!value.IsActive)
            {
                ModelState.AddModelError(string.Empty, "User status is passive.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, true);
            if (result.Succeeded)
            {
                var simpleUserViewModel = new SimpleUserViewModel
                {
                    Email = value.Email,
                    Name = value.Name,
                    Surname = value.Surname,
                    City = value.City,
                    Username = value.UserName,
                    Id = value.Id
                };

                var token = GenerateJwtToken(simpleUserViewModel);
                
                Response.Cookies.Append("jwtToken", token, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtSettingsModel.ExpireMinutes)
                });

                return RedirectToAction("EditProfile", "Profile");
            }

            ModelState.AddModelError(string.Empty, "Incorrect username or password.");
            return View(model);
        }

        // JWT token generation
        public string GenerateJwtToken(SimpleUserViewModel simpleUserViewModel)
        {
            var claims = new[]
            {
                new Claim("Name", simpleUserViewModel.Name),
                new Claim("Surname", simpleUserViewModel.Surname),
                new Claim("City", simpleUserViewModel.City),
                new Claim("Username", simpleUserViewModel.Username),
                new Claim(ClaimTypes.NameIdentifier, simpleUserViewModel.Id),
                new Claim(ClaimTypes.Email, simpleUserViewModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettingsModel.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettingsModel.Issuer,
                audience: _jwtSettingsModel.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettingsModel.ExpireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        public IActionResult LoginWithGoogle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Login", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpPost]
        public async Task<IActionResult> ExternalLoginCallBack(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");
            if (remoteError != null)
            {
                ModelState.AddModelError("", $"External Provider Error: {remoteError}");
                return RedirectToAction("Login");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("UserLogin");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Inbox", "Message");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = new AppUser
            {
                UserName = email,
                Email = email,
                Name = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "Google",
                Surname = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "User"
            };

            var identityResult = await _userManager.CreateAsync(user);
            if (identityResult.Succeeded)
            {
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Inbox", "Message");
            }

            return RedirectToAction("UserLogin");
        }
    }
}
