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
        [Route("Mantenimientos/Close")]
        public async Task<ActionResult<List<Mantenimientos>>> CloseMantenimiento([FromQuery] int dniResponsable,[FromQuery] string unidad)
        {
            bool res = await _service.CloseMantenimiento(dniResponsable,unidad);
            if (res)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("Mantenimientos/Create")]
        public async Task<ActionResult<List<Mantenimientos>>> CreateMantenimiento([FromBody] Mantenimientos_CreateDTO mant)
        {
            bool res = await _service.AddMantenimiento(mant);
            if (res)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<ActionResult<List<Mantenimientos_ResponseDTO>>> GetAll()
        {
            var lista = await _service.GetAll();
            if (lista.Count == 0) return NotFound();
            return lista;
        }


    }
}
