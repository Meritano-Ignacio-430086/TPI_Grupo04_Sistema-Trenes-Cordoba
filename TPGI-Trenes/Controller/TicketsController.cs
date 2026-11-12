using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPGI_Trenes.Services.Interfaces;

namespace TPGI_Trenes.Controller
{
    [Route("api/Tickets")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketsServices services;
        public TicketsController(ITicketsServices services)
        {
            this.services = services;
        }

        // GET: HomeController
        public IActionResult GetByClienteID(int id)
        {
            return Ok(services.GetByClienteId(id));
        }

        //// GET: HomeController/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// GET: HomeController/Edit/5
        //public IActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: HomeController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: HomeController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}
    }
}
