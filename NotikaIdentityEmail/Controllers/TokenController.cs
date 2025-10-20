using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NotikaIdentityEmail.Models.JwtModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotikaIdentityEmail.Controllers
{
    public class TokenController : Controller
    {
        private readonly JwtSettingsModel _jwtSettingsModel;

        public TokenController(IOptions<JwtSettingsModel> jwtSettingsModel)
        {
            _jwtSettingsModel = jwtSettingsModel.Value;
        }

        [HttpGet]
        public IActionResult Generate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Generate(SimpleUserViewModel simpleUserViewModel)
        {
            var claim = new[]
            {
                new Claim("Name", simpleUserViewModel.Name),
                new Claim("Surname", simpleUserViewModel.Surname),
                new Claim("City", simpleUserViewModel.City),
                new Claim("Username", simpleUserViewModel.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettingsModel.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtSettingsModel.Issuer,
                audience: _jwtSettingsModel.Audience,
                claims: claim,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettingsModel.ExpireMinutes),
                signingCredentials: creds
                );

            simpleUserViewModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
            return View(simpleUserViewModel);
        }
    }
}
