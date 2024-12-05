using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PropertyRentalManagement.Controllers
{
    public class ListingsController : Controller
    {
        // GET: ListingsController
        public ActionResult Index()
        {
            return View();
        }

       public ActionResult AvailableApartments()
        {
            return View();
        }

        // GET: ListingsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ListingsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ListingsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ListingsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ListingsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ListingsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ListingsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
