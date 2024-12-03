using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyRentalManagement.Context;
using PropertyRentalManagement.Models;
using PropertyRentalManagement.Requests;

namespace PropertyRentalManagement.Controllers
{
    public class PropertyManagerAccountController : Controller
    {
        private readonly ApplicationDBContext _context;

        public PropertyManagerAccountController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: PropertyManagerAccount
        public async Task<IActionResult> Index()
        {
            var propertyManagers = from ur in _context.UserRoleMappings
                                   from r in _context.Roles
                                   from u in _context.Users
                                   where r.Name=="PropertyManager" && ur.RoleId==r.Id && ur.UserId==u.Id
                                   select new 
                                   {
                                       u.Id,
                                       u.FirstName,
                                       u.LastName,
                                       u.Email,
                                   };
            ViewBag.propertyManagers = propertyManagers;
            return View();
        }

        // GET: PropertyManagerAccount/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: PropertyManagerAccount/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PropertyManagerAccount/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

                _context.Add(new User() { Email=createUserRequest.Email, Password=createUserRequest.Password,FirstName=createUserRequest.FirstName, 
                    LastName=createUserRequest.LastName});
                await _context.SaveChangesAsync().ContinueWith(async task =>
                {
                    var createdUser = _context.Users.Where(u => u.FirstName==createUserRequest.FirstName && u.LastName==createUserRequest.LastName && 
                                        u.Email==createUserRequest.Email).First();
                    var role = _context.Roles.Where(p => p.Name == "PropertyManager").First();
                    _context.Add(new UserRoleMappings() { RoleId = role.Id, UserId = createdUser.Id });
                    await _context.SaveChangesAsync();
                });
                ViewData["Error"] = null;
                return RedirectToAction(nameof(Index)); 
            }
            return View(createUserRequest);
        }


        // GET: PropertyManagerAccount/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: PropertyManagerAccount/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,FirstName,LastName")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
            return View(user);
        }

        // GET: PropertyManagerAccount/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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

        // POST: PropertyManagerAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
