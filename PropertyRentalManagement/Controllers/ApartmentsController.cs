using System;
using System.Collections.Generic;
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

            createApartment.ServicesIncluded = new List<string>();
            createApartment.EquipmentsIncluded = new List<string>();
            createApartment.OutdoorSpaces = new List<string>();

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

            
            return View(createApartment);
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
