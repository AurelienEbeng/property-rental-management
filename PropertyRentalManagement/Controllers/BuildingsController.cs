using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PropertyRentalManagement.Context;
using PropertyRentalManagement.Models;
using PropertyRentalManagement.Requests;

namespace PropertyRentalManagement.Controllers
{
    [Authorize]
    public class BuildingsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public BuildingsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Buildings
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
            if (role != "Admin" && role != "PropertyManager")
            {
                return RedirectToAction("Unauthorized", "Account");
            }

            var applicationDBContext = _context.Buildings.Include(b => b.Address);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Buildings/Create
        public IActionResult Create()
        {

            var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
            if (role != "Admin" && role != "PropertyManager")
            {
                return RedirectToAction("Unauthorized", "Account");
            }

            var createBuilding = new CreateBuilding();

            createBuilding.AmenitiesInBuildingCheckboxes = GetAmenitiesInBuildingCheckboxes();
            createBuilding.AmenitiesNearbyCheckboxes = GetAmenitiesNearbyCheckboxes();
            createBuilding.OwnersCheckboxes = GetOwnersCheckboxes();
            createBuilding.PropertyManagersCheckboxes = GetPropertyManagerCheckboxes();


            ViewData["Error"] = null;


            return View(createBuilding);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBuilding createBuilding)
        {
            var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
            if (role != "Admin" && role != "PropertyManager")
            {
                return RedirectToAction("Unauthorized", "Account");
            }
            createBuilding.AmenitiesInBuildingCheckboxes = GetAmenitiesInBuildingCheckboxes();
            createBuilding.AmenitiesNearbyCheckboxes = GetAmenitiesNearbyCheckboxes();
            createBuilding.OwnersCheckboxes = GetOwnersCheckboxes();
            createBuilding.PropertyManagersCheckboxes = GetPropertyManagerCheckboxes();



            ViewData["Error"] = null;



            var oldAddress = _context.Addresses.Where(a => a.Street == createBuilding.Street && a.City == createBuilding.City &&
                    a.Province == createBuilding.Province && a.PostalCode == createBuilding.PostalCode
                    && a.Country == createBuilding.Country).FirstOrDefault();
            if (oldAddress != null)
            {
                ViewData["Error"] = "BuildingCreated";
                return View(createBuilding);
            }

            AddAddress(createBuilding);


            AddBuilding(createBuilding);

            var newAddress = _context.Addresses.Where(a => a.Street == createBuilding.Street && a.City == createBuilding.City &&
                    a.Province == createBuilding.Province && a.PostalCode == createBuilding.PostalCode
                    && a.Country == createBuilding.Country).FirstOrDefault();

            var newBuilding = _context.Buildings.Where(b => b.NumberOfApartments == createBuilding.NumberOfApartments &&
                                            b.NumberOfFloors == createBuilding.NumberOfFloors && b.AddressId == newAddress.Id).FirstOrDefault();


            AddAmenitiesInBuildingMappings(createBuilding, newBuilding);

            AddAmenitiesNearbyMappings(createBuilding, newBuilding);

            AddOwnerMappings(createBuilding, newBuilding);

            AddPropertyManagerMappings(createBuilding, newBuilding);


            return RedirectToAction(nameof(Index));



            //return View(createBuilding);
        }

        private void AddAddress(CreateBuilding createBuilding)
        {
            _context.Add(new Address()
            {
                Street = createBuilding.Street,
                City = createBuilding.City,
                Province = createBuilding.Province,
                PostalCode = createBuilding.PostalCode,
                Country = createBuilding.Country
            });
            _context.SaveChanges();
        }

        private void AddBuilding(CreateBuilding createBuilding)
        {
            var newAddress = _context.Addresses.Where(a => a.Street == createBuilding.Street && a.City == createBuilding.City &&
                        a.Province == createBuilding.Province && a.PostalCode == createBuilding.PostalCode
                        && a.Country == createBuilding.Country).FirstOrDefault();

            _context.Add(new Building()
            {
                NumberOfApartments = createBuilding.NumberOfApartments,
                NumberOfFloors = createBuilding.NumberOfFloors,
                AddressId = newAddress.Id
            });

            _context.SaveChanges();
        }

        private void AddAmenitiesInBuildingMappings(CreateBuilding createBuilding, Building newBuilding)
        {
            _context.BuildingAmenitiesInBuildingMappings
                .AddRange(GetListOfAmenitiesInBuildingMappings(createBuilding.AmenitiesInBuilding, newBuilding.Id));
            _context.SaveChanges();
        }

        private void AddAmenitiesNearbyMappings(CreateBuilding createBuilding, Building newBuilding)
        {
            _context.BuildingAmenitiesNearbyMappings
            .AddRange(GetListOfAmenitiesNearbyMappings(createBuilding.AmenitiesNearby, newBuilding.Id));

            _context.SaveChanges();
        }

        private void AddOwnerMappings(CreateBuilding createBuilding, Building newBuilding)
        {

            _context.BuildingOwnerMappings
            .AddRange(GetListOfOwnersMappings(createBuilding.Owners, newBuilding.Id));

            _context.SaveChanges();
        }

        private void AddPropertyManagerMappings(CreateBuilding createBuilding, Building newBuilding)
        {

            _context.BuildingPropertyManagerMappings
            .AddRange(GetListOfProprertyManagersMappings(createBuilding.PropertyManagers, newBuilding.Id));

            _context.SaveChanges();
        }

        // GET: Buildings/Edit/5
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

            var building = await _context.Buildings.FindAsync(id);
            if (building == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", building.AddressId);
            return View(building);
        }

        // POST: Buildings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumberOfFloors,NumberOfApartments,AddressId")] Building building)
        {
            var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
            if (role != "Admin" && role != "PropertyManager")
            {
                return RedirectToAction("Unauthorized", "Account");
            }

            if (id != building.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(building);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingExists(building.Id))
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
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", building.AddressId);
            return View(building);
        }

        // GET: Buildings/Delete/5
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

            var building = await _context.Buildings
                .Include(b => b.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "RoleName")?.Value;
            if (role != "Admin" && role != "PropertyManager")
            {
                return RedirectToAction("Unauthorized", "Account");
            }

            var building = await _context.Buildings.FindAsync(id);
            var amenitiesNearby = _context.BuildingAmenitiesNearbyMappings.Where(a=> a.BuildingId==building.Id).ToList();
            var amenitiesInBuilding = _context.BuildingAmenitiesInBuildingMappings.Where(a => a.BuildingId == building.Id).ToList();
            var ownerMappings = _context.BuildingOwnerMappings.Where(a => a.BuildingId == building.Id).ToList();
            var propertyManagerMappings = _context.BuildingPropertyManagerMappings.Where(a => a.BuildingId == building.Id).ToList();

            if (amenitiesInBuilding != null)
            {
                _context.BuildingAmenitiesInBuildingMappings.RemoveRange(amenitiesInBuilding);
            }

            if (amenitiesNearby != null)
            {
                _context.BuildingAmenitiesNearbyMappings.RemoveRange(amenitiesNearby);
            }

            if (ownerMappings != null)
            {
                _context.BuildingOwnerMappings.RemoveRange(ownerMappings);
            }

            if (propertyManagerMappings != null)
            {
                _context.BuildingPropertyManagerMappings.RemoveRange(propertyManagerMappings);
            }

            if (building != null)
            {
                _context.Buildings.Remove(building);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildingExists(int id)
        {
            return _context.Buildings.Any(e => e.Id == id);
        }


        private List<CheckBoxOption> GetAmenitiesInBuildingCheckboxes()
        {
            var amenitiesInBuilding = _context.AmenitiesInBuilding.ToList();
            List<CheckBoxOption> amenitiesInBuildingCheckboxes = new List<CheckBoxOption>();
            foreach (var amenity in amenitiesInBuilding)
            {
                amenitiesInBuildingCheckboxes.Add(new CheckBoxOption { Description = amenity.Name, IsChecked = false, Value = "" + amenity.Id });
            }
            return amenitiesInBuildingCheckboxes;
        }

        private List<CheckBoxOption> GetAmenitiesNearbyCheckboxes()
        {
            var amenities = _context.AmenitiesNearby.ToList();
            List<CheckBoxOption> amenitiesCheckboxes = new List<CheckBoxOption>();
            foreach (var amenity in amenities)
            {
                amenitiesCheckboxes.Add(new CheckBoxOption { Description = amenity.Name, IsChecked = false, Value = "" + amenity.Id });
            }
            return amenitiesCheckboxes;
        }

        private List<CheckBoxOption> GetOwnersCheckboxes()
        {
            var owners = from ur in _context.UserRoleMappings
                         from r in _context.Roles
                         from u in _context.Users
                         where r.Name == "Owner" && ur.RoleId == r.Id && ur.UserId == u.Id
                         select new
                         {
                             u.Id,
                             u.FirstName,
                             u.LastName,
                             u.Email,
                         };
            List<CheckBoxOption> ownersCheckboxes = new List<CheckBoxOption>();
            foreach (var owner in owners)
            {
                ownersCheckboxes.Add(new CheckBoxOption { Description = owner.FirstName + " " + owner.LastName, Value = "" + owner.Id, IsChecked = false });
            }
            return ownersCheckboxes;
        }

        private List<CheckBoxOption> GetPropertyManagerCheckboxes()
        {
            var propertyManagers = from ur in _context.UserRoleMappings
                                   from r in _context.Roles
                                   from u in _context.Users
                                   where r.Name == "PropertyManager" && ur.RoleId == r.Id && ur.UserId == u.Id
                                   select new
                                   {
                                       u.Id,
                                       u.FirstName,
                                       u.LastName,
                                       u.Email,
                                   };
            List<CheckBoxOption> propertyManagersCheckboxes = new List<CheckBoxOption>();
            foreach (var manager in propertyManagers)
            {
                propertyManagersCheckboxes.Add(new CheckBoxOption { Description = manager.FirstName + " " + manager.LastName, Value = "" + manager.Id, IsChecked = false });
            }
            return propertyManagersCheckboxes;
        }

        private List<BuildingAmemitiesInBuildingMappings> GetListOfAmenitiesInBuildingMappings(List<string> amenitiesIds, int buildingId)
        {
            List<BuildingAmemitiesInBuildingMappings> amenities = new List<BuildingAmemitiesInBuildingMappings>();

            foreach (var id in amenitiesIds)
            {
                amenities.Add(new BuildingAmemitiesInBuildingMappings() { AmenitiesInBuildingId = Int32.Parse(id), BuildingId = buildingId });
            }

            return amenities;
        }

        private List<BuildingAmenitiesNearbyMappings> GetListOfAmenitiesNearbyMappings(List<string> amenitiesIds, int buildingId)
        {
            List<BuildingAmenitiesNearbyMappings> amenities = new List<BuildingAmenitiesNearbyMappings>();

            foreach (var id in amenitiesIds)
            {
                amenities.Add(new BuildingAmenitiesNearbyMappings() { AmenitiesNearbyId = Int32.Parse(id), BuildingId = buildingId });
            }

            return amenities;
        }

        private List<BuildingOwnerMappings> GetListOfOwnersMappings(List<string> users, int buildingId)
        {
            List<BuildingOwnerMappings> userMappings = new List<BuildingOwnerMappings>();

            foreach (var id in users)
            {
                userMappings.Add(new BuildingOwnerMappings() { OwnerId = Int32.Parse(id), BuildingId = buildingId });
            }

            return userMappings;
        }

        private List<BuildingPropertyManagerMappings> GetListOfProprertyManagersMappings(List<string> users, int buildingId)
        {
            List<BuildingPropertyManagerMappings> userMappings = new List<BuildingPropertyManagerMappings>();

            foreach (var id in users)
            {
                userMappings.Add(new BuildingPropertyManagerMappings() { PropertyManagerId = Int32.Parse(id), BuildingId = buildingId });
            }

            return userMappings;
        }
    }
}
