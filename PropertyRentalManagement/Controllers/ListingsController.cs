using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PropertyRentalManagement.Context;
using PropertyRentalManagement.Models;

namespace PropertyRentalManagement.Controllers
{

    public class ListingsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ListingsController(ApplicationDBContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: Listings
        public async Task<IActionResult> Index()
        {
			var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
			if (role != "Admin" && role != "PropertyManager")
			{
				return RedirectToAction("Unauthorized", "Account");
			}
			var applicationDBContext = _context.Listings.Include(l => l.Apartment).Include(l => l.Creator);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Listings/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
			var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
			if (role != "Admin" && role != "PropertyManager")
			{
				return RedirectToAction("Unauthorized", "Account");
			}
			if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listings
                .Include(l => l.Apartment)
                .Include(l => l.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        // GET: Listings/Create
        [Authorize]
        public IActionResult Create()
        {
			var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
			if (role != "Admin" && role != "PropertyManager")
			{
				return RedirectToAction("Unauthorized", "Account");
			}
			ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id");
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,ApartmentId,Price,AvailableFrom,Email,CreatorId,Description,Title,DateCreated,LeaseDuration,ArePetsAllowed,IsSmokingAllowed")] Listing listing)
        {
			var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
			if (role != "Admin" && role != "PropertyManager")
			{
				return RedirectToAction("Unauthorized", "Account");
			}
			var email = HttpContext.User?.Identity?.Name;
            var user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
            
            if(user != null)
            {
                listing.CreatorId = user.Id;
                listing.Email = user.Email;
                _context.Add(listing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
           
        }

        // GET: Listings/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
			var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
			if (role != "Admin" && role != "PropertyManager")
			{
				return RedirectToAction("Unauthorized", "Account");
			}
			if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listings.FindAsync(id);
            if (listing == null)
            {
                return NotFound();
            }
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", listing.ApartmentId);
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", listing.CreatorId);
            return View(listing);
        }

        // POST: Listings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApartmentId,Price,AvailableFrom,Email,CreatorId,Description,Title,DateCreated,LeaseDuration,ArePetsAllowed,IsSmokingAllowed")] Listing listing)
        {
			var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
			if (role != "Admin" && role != "PropertyManager")
			{
				return RedirectToAction("Unauthorized", "Account");
			}
			if (id != listing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListingExists(listing.Id))
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
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", listing.ApartmentId);
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", listing.CreatorId);
            return View(listing);
        }

        // GET: Listings/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
			var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
			if (role != "Admin" && role != "PropertyManager")
			{
				return RedirectToAction("Unauthorized", "Account");
			}
			if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.Listings
                .Include(l => l.Apartment)
                .Include(l => l.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        // POST: Listings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
			if (role != "Admin" && role != "PropertyManager")
			{
				return RedirectToAction("Unauthorized", "Account");
			}
			var listing = await _context.Listings.FindAsync(id);
            if (listing != null)
            {
                _context.Listings.Remove(listing);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListingExists(int id)
        {
            return _context.Listings.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AvailableApartments()
        {
            var applicationDBContext = _context.Listings.Include(l => l.Apartment).Include(l => l.Creator);

            var availableApartments = from l in _context.Listings
                                      where l.AvailableFrom > DateTime.Now
                                      select new
                                      {
                                          l.AvailableFrom,
                                          l.Title,
                                          l.Description,
                                          l.Id
                                      };
            ViewBag.availableApartments = availableApartments;
            return View();
        }

        public async Task<IActionResult> AvailableApartmentDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = (from l in _context.Listings
                          where l.Id == id
                          select new
                          {
                              l.AvailableFrom,
                              l.Title,
                              l.Description,
                              l.Id,
                              l.DateCreated,
                              l.LeaseDuration,
                              l.ApartmentId,
                              l.ArePetsAllowed,
                              l.CreatorId,
                              l.Price,
                              l.Email,
                              l.IsSmokingAllowed
                          }).FirstOrDefault();

            ViewBag.listing = listing;

            var equipments = from e in _context.ApartmentEquipmentsIncludedMappings
                             from p in _context.EquipmentsIncluded
                             where e.ApartmentId == listing.ApartmentId && e.EquipmentIncludedId == p.Id
                             select new
                             {
                                 p.Name
                             };

            ViewBag.equipments = equipments;

            var outdoorSpace = from e in _context.ApartmentOutdoorSpaceMappings
                            from p in _context.OutdoorSpaces
                            where e.ApartmentId == listing.ApartmentId && e.OutdoorSpaceId == p.Id
                            select new
                            {
                                p.Name
                            };

            ViewBag.outdoorSpace = outdoorSpace;

            var services = from e in _context.ApartmentServiceIncludedMappings
                               from p in _context.OutdoorSpaces
                               where e.ApartmentId == listing.ApartmentId && e.ServiceIncludedId == p.Id
                               select new
                               {
                                   p.Name
                               };

            ViewBag.services = services;

            var apartment = _context.Apartments.Where(u => u.Id == listing.ApartmentId).FirstOrDefault();

            var amenitiesNearby = from e in _context.BuildingAmenitiesNearbyMappings
                                  from p in _context.AmenitiesNearby
                                  where e.BuildingId == apartment.BuildingId && e.AmenitiesNearbyId == p.Id
                                  select new
                                  {
                                      p.Name
                                  };
            ViewBag.amenitiesNearby = amenitiesNearby;

            var amenitiesInBuilding = from e in _context.BuildingAmenitiesInBuildingMappings
                                      from p in _context.AmenitiesInBuilding
                                      where e.BuildingId == apartment.BuildingId && e.AmenitiesInBuildingId == p.Id
                                  select new
                                  {
                                      p.Name
                                  };
            ViewBag.amenitiesInBuilding = amenitiesInBuilding;

            return View();
        }
    }
}
