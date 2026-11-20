using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Services.Interfaces;

namespace TPGI_Trenes.Controller
{
    [Route("api/Mantenimientos")]
    [ApiController]
    public class MantenimientosController : ControllerBase
    {
        private readonly IMantenimientosServices _service;
        public MantenimientosController(IMantenimientosServices services)
        {
            this._service = services;
        }

        [HttpPost]
        [Route("Mantenimientos/Create")]
        public async Task<IActionResult> CreateMantenimiento([FromBody] Mantenimientos_CreateDTO mant)
        {
            bool res = await _service.AddMantenimiento(mant);
            if (res)
            {
                return Ok(new { mensaje = "Mantenimiento creado" });
            }
            return BadRequest(new { mensaje = "No se pudo crear: revisá empleado, rol, tipo, tren y estado" });
        }

        [HttpPost]
        [Route("Mantenimientos/Close")]
        public async Task<IActionResult> CloseMantenimiento([FromQuery] int dniResponsable,[FromQuery] string unidad)
        {
            bool res = await _service.CloseMantenimiento(dniResponsable,unidad);
            if (res)
            {
                return Ok(new { mensaje = "Mantenimiento cerrado exitosamente" });
            }
            return BadRequest(new { mensaje = "No se pudo crear el mantenimiento: revisá empleado, rol, tipo, tren y estado" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lista = await _service.GetAll();
            if (lista.Count == 0) return NotFound();
            return Ok(lista);
        }


    }
}
