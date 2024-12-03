using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PropertyRentalManagement.Models;
using System.Security.Claims;
using PropertyRentalManagement.Context;
using Microsoft.EntityFrameworkCore;

namespace PropertyRentalManagement.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDBContext _context { get; }

        public AccountController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            var user = _context.Users.Where(u => u.Email == model.Email && u.Password == model.Password).First();
            if (user == null) { return View(); }

            var userRole = _context.UserRoleMappings.Where(u => u.UserId == user.Id).Include(r => r.Role).First();

            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("FirstName", user.FirstName),
                    new Claim(ClaimTypes.Role, userRole.Role.Name),
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authenticationProps = new AuthenticationProperties() { };
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authenticationProps).Wait();

           
            return RedirectToAction("Index", "Profile");

          
        }


        [HttpGet]
        public async Task <IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Unauthorized()
        {
            return View();
        }
    }
}
