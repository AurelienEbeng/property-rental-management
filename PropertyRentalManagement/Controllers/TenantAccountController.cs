using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyRentalManagement.Context;
using PropertyRentalManagement.Models;
using PropertyRentalManagement.Requests;

namespace PropertyRentalManagement.Controllers
{
    public class TenantAccountController : Controller
    {
        private readonly ApplicationDBContext _context;

        public TenantAccountController(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
            if (role != "Admin" && role != "Owner")
            {
                return RedirectToAction("Unauthorized", "Account");
            }
            var tenants = from ur in _context.UserRoleMappings
                          from r in _context.Roles
                          from u in _context.Users
                          where r.Name == "Tenant" && ur.RoleId == r.Id && ur.UserId == u.Id
                          select new
                          {
                              u.Id,
                              u.FirstName,
                              u.LastName,
                              u.Email,
                          };
            ViewBag.tenants = tenants;
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password,FirstName,LastName,ConfirmPassword")] CreateUser createUserRequest)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Where(u => createUserRequest.Email == u.Email).FirstOrDefault();
                if (user != null)
                {
                    ViewData["Error"] = "EmailTaken";
                    return View(createUserRequest);
                }

                _context.Add(new User()
                {
                    Email = createUserRequest.Email,
                    Password = createUserRequest.Password,
                    FirstName = createUserRequest.FirstName,
                    LastName = createUserRequest.LastName
                });
                await _context.SaveChangesAsync().ContinueWith(async task =>
                {
                    var createdUser = _context.Users.Where(u => u.FirstName == createUserRequest.FirstName && u.LastName == createUserRequest.LastName &&
                                        u.Email == createUserRequest.Email).First();
                    var role = _context.Roles.Where(p => p.Name == "Tenant").First();
                    _context.Add(new UserRoleMappings() { RoleId = role.Id, UserId = createdUser.Id });
                    await _context.SaveChangesAsync();
                });
                ViewData["Error"] = null;
                return RedirectToAction(nameof(Index));
            }
            return View(createUserRequest);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
            if (role != "Admin" && role != "Owner")
            {
                return RedirectToAction("Unauthorized", "Account");
            }
            if (id != null)
            {
                var editUser = from u in _context.Users
                               where u.Id == id
                               select new EditUser
                               {
                                   Id = u.Id,
                                   FirstName = u.FirstName,
                                   LastName = u.LastName,
                                   Email = u.Email,
                                   Password = u.Password,
                                   ConfirmPassword = u.Password
                               };
                return View(editUser.First());
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Email,Password,FirstName,LastName,ConfirmPassword")] EditUser editUser)
        {
            var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
            if (role != "Admin" && role != "Owner")
            {
                return RedirectToAction("Unauthorized", "Account");
            }

            if (ModelState.IsValid)
            {
                var user = _context.Users.Where(u => u.Id == editUser.Id).FirstOrDefault();
                if (user.Email != editUser.Email)
                {
                    var user2 = _context.Users.Where(u => u.Email == editUser.Email).FirstOrDefault();
                    if (user2 != null)
                    {
                        ViewData["Error"] = "EmailTaken";
                        return View(editUser);
                    }

                }
                user.FirstName = editUser.FirstName;
                user.LastName = editUser.LastName;
                user.Password = editUser.Password;
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(editUser);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
            if (role != "Admin" && role != "Owner")
            {
                return RedirectToAction("Unauthorized", "Account");
            }
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
            if (role != "Admin" && role != "Owner")
            {
                return RedirectToAction("Unauthorized", "Account");
            }

            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            var userRole = _context.UserRoleMappings.Where(u => u.UserId == user.Id).FirstOrDefault();
            if (user != null)
            {
                _context.UserRoleMappings.Remove(userRole);
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





    }
}
