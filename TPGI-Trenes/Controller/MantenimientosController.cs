/*using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TPGI_Trenes.Controller
{
    public class MantenimientosController : ControllerBase
    {
        // GET: MantenimientosController
        public ActionResult Index()
        {
            return View();
        }

        // POST: MantenimientosController/Create
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

        // GET: MantenimientosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // GET: MantenimientosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

    }
}
*/