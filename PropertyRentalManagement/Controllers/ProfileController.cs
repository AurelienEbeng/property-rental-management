using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PropertyRentalManagement.Controllers
{
    public class ProfileController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            ViewData["currentUser"] = HttpContext.User?.Identity?.Name;
            ViewData["currentClaim"] = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ;
            return View();
        }
    }
}
