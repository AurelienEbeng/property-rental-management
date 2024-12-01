using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PropertyRentalManagement.Models;
using System.Security.Claims;

namespace PropertyRentalManagement.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (LoginUser(model.Email, model.Password))
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,"Rohit Dhand")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authenticationProps = new AuthenticationProperties() { };
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authenticationProps).Wait();

                //return Ok(new { success = true });
                return RedirectToAction("Index", "Profile");
            }
            /*
            else
            {
                return Ok(new { error = "Please check email or password" });
            }
            */

            return View();
        }

        private bool LoginUser(string email, string password)
        {
            if(email=="ebengaurelien@gmail.com" && password== "12345678")
            {
                return true;
            }

            return false;
        }

        [HttpGet]
        public async Task <IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Unauthorized()
        {
            return View();
        }
    }
}
