using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PropertyRentalManagement.Context;
using PropertyRentalManagement.Models;
using PropertyRentalManagement.Requests;

namespace PropertyRentalManagement.Controllers
{
    public class ApartmentsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ApartmentsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Apartments
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Apartments.Include(a => a.Building);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Apartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments
                .Include(a => a.Building)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // GET: Apartments/Create
        public IActionResult Create()
        {
            var createApartment = new CreateApartment();
            createApartment.ServicesIncludedCheckboxes = GetServicesIncludedCheckboxes();
            createApartment.EquipmentsIncludedCheckboxes = GetEquipmentsIncludedCheckboxes();
            createApartment.OutdoorSpacesCheckboxes = GetOutdoorSpacesCheckboxes();
            createApartment.BuildingSelectItems = GetBuildingSelectItems();
            ViewData["Error"] = null;
            ViewData["FloorExceeded"] = null;
            ViewData["ApartmentsExceeded"] = null;

            return View(createApartment);
        }

        private List<CheckBoxOption> GetEquipmentsIncludedCheckboxes()
        {
            var items = _context.EquipmentsIncluded.ToList();
            List<CheckBoxOption> checkboxes = new List<CheckBoxOption>();
            foreach (var item in items)
            {
                checkboxes.Add(new CheckBoxOption { Description = item.Name, IsChecked = false, Value = "" + item.Id });
            }
            return checkboxes;
        }

        private List<CheckBoxOption> GetOutdoorSpacesCheckboxes()
        {
            var items = _context.OutdoorSpaces.ToList();
            List<CheckBoxOption> checkboxes = new List<CheckBoxOption>();
            foreach (var item in items)
            {
                checkboxes.Add(new CheckBoxOption { Description = item.Name, IsChecked = false, Value = "" + item.Id });
            }
            return checkboxes;
        }

        private List<CheckBoxOption> GetServicesIncludedCheckboxes()
        {
            var items = _context.ServicesIncluded.ToList();
            List<CheckBoxOption> checkboxes = new List<CheckBoxOption>();
            foreach (var item in items)
            {
                checkboxes.Add(new CheckBoxOption { Description = item.Name, IsChecked = false, Value = "" + item.Id });
            }
            return checkboxes;
        }

        private List<SelectListItem> GetBuildingSelectItems()
        {
            var items = _context.Buildings.ToList();

            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (var item in items)
            {
                listItems.Add(new SelectListItem { Text = item.Id+"", Value = "" + item.Id });
            }

            return listItems;
        }


    

    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateApartment createApartment)
        {
            createApartment.ServicesIncludedCheckboxes = GetServicesIncludedCheckboxes();
            createApartment.EquipmentsIncludedCheckboxes = GetEquipmentsIncludedCheckboxes();
            createApartment.OutdoorSpacesCheckboxes = GetOutdoorSpacesCheckboxes();
            createApartment.BuildingSelectItems = GetBuildingSelectItems();
            ViewData["Error"] = null;
            ViewData["FloorExceeded"] = null;
            ViewData["ApartmentsExceeded"] = null;

            var oldApartment = _context.Apartments.Where(a => a.ApartmentNumber == createApartment.ApartmentNumber &&
                                                    a.BuildingId==createApartment.BuildingId).FirstOrDefault();
            if (oldApartment != null)
            {
                ViewData["Error"] = "Taken";
                return View(createApartment);
            }

            var building = _context.Buildings.Where(b => b.Id ==createApartment.BuildingId).FirstOrDefault();

            if(createApartment.FloorNumber > building.NumberOfFloors)
            {
                ViewData["FloorExceeded"] = "Yes";
                return View(createApartment);
            }

            if (createApartment.ApartmentNumber > building.NumberOfApartments)
            {
                ViewData["ApartmentsExceeded"] = "Yes";
                return View(createApartment);
            }


            AddApartment(createApartment);

            var newApartment = _context.Apartments.Where(apartment =>
            createApartment.ApartmentNumber == apartment.ApartmentNumber &&
                createApartment.BuildingId == apartment.BuildingId &&
                createApartment.Rooms == apartment.Rooms &&
                createApartment.Size == apartment.Size &&
                createApartment.FloorNumber == apartment.FloorNumber &&
                createApartment.IsVacant == apartment.IsVacant).FirstOrDefault();

            AddEquipmentsMappings(createApartment, newApartment);

            AddServicesIncluded(createApartment, newApartment);

            AddOutdoorSpace(createApartment, newApartment);

            return RedirectToAction(nameof(Index));
        }

        private void AddApartment(CreateApartment apartment)
        {
            _context.Apartments.Add(new Apartment()
            {
                ApartmentNumber = apartment.ApartmentNumber,
                BuildingId = apartment.BuildingId,
                Rooms = apartment.Rooms,
                Size = apartment.Size,
                FloorNumber = apartment.FloorNumber,
                IsVacant = apartment.IsVacant
            });

            _context.SaveChanges();
        }


        private void AddEquipmentsMappings(CreateApartment createApartment, Apartment newApartment)
        {
            _context.ApartmentEquipmentsIncludedMappings.AddRange(
                GetListOfEquipmentsMappings(createApartment.EquipmentsIncluded, newApartment.Id));
            _context.SaveChanges();
        }

        private List<ApartmentEquipmentsIncludedMappings> GetListOfEquipmentsMappings(List<string> items, int apartmentId)
        {
            List<ApartmentEquipmentsIncludedMappings> mappings = new List<ApartmentEquipmentsIncludedMappings>();
            foreach(var item in items)
            {
                mappings.Add(new ApartmentEquipmentsIncludedMappings() 
                { ApartmentId = apartmentId, EquipmentIncludedId = Int32.Parse(item) });
            }

            return mappings;
        }

        private void AddServicesIncluded(CreateApartment createApartment, Apartment newApartment)
        {
            _context.ApartmentServiceIncludedMappings.AddRange(
                GetListServicesIncludedMappings(createApartment.EquipmentsIncluded, newApartment.Id));
            _context.SaveChanges();
        }

        private List<ApartmentServiceIncludedMappings> GetListServicesIncludedMappings(List<string> items, int apartmentId)
        {
            List<ApartmentServiceIncludedMappings> mappings = new List<ApartmentServiceIncludedMappings>();
            foreach (var item in items)
            {
                mappings.Add(new ApartmentServiceIncludedMappings()
                { ApartmentId = apartmentId, ServiceIncludedId = Int32.Parse(item) });
            }

            return mappings;
        }

        private void AddOutdoorSpace(CreateApartment createApartment, Apartment newApartment)
        {
            _context.ApartmentOutdoorSpaceMappings.AddRange(
                GetListOutdoorSpace(createApartment.EquipmentsIncluded, newApartment.Id));
            _context.SaveChanges();
        }

        private List<ApartmentOutdoorSpaceMappings> GetListOutdoorSpace(List<string> items, int apartmentId)
        {
            List<ApartmentOutdoorSpaceMappings> mappings = new List<ApartmentOutdoorSpaceMappings>();
            foreach (var item in items)
            {
                mappings.Add(new ApartmentOutdoorSpaceMappings()
                { ApartmentId = apartmentId, OutdoorSpaceId = Int32.Parse(item) });
            }

            return mappings;
        }



        // GET: Apartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return NotFound();
            }
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Id", apartment.BuildingId);
            return View(apartment);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BuildingId,Rooms,Size,FloorNumber,ApartmentNumber,IsVacant")] Apartment apartment)
        {
            if (id != apartment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartmentExists(apartment.Id))
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
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Id", apartment.BuildingId);
            return View(apartment);
        }

        // GET: Apartments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments
                .Include(a => a.Building)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment != null)
            {
                _context.Apartments.Remove(apartment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartmentExists(int id)
        {
            return _context.Apartments.Any(e => e.Id == id);
        }
    }
}
