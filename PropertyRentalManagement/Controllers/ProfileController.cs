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
            ViewData["Email"] = HttpContext.User?.Identity?.Name;
            ViewData["FirstName"] = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type=="FirstName")?.Value;
            ViewData["LastName"] = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "LastName")?.Value;
            ViewData["RoleName"] = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
            ViewData["RoleId"] = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleId")?.Value;
            return View();
        }
    }
}
