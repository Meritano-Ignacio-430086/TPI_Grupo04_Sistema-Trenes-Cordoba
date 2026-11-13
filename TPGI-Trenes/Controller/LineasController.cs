using Microsoft.AspNetCore.Mvc;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Services;
using TPGI_Trenes.Services.Interfaces;

namespace TPGI_Trenes.Controller
{
    [Route("api/Lineas")]
    [ApiController]
    
    public class LineasController : ControllerBase
    {
        private readonly ILineasServices services;
        public LineasController(ILineasServices services)
        {
            this.services = services;
        }

        [HttpGet]
        public async Task<ActionResult<List<LineasDTO>>> GetLineasByIdAsync([FromBody]int NroLinea)
        {
            List<LineasDTO> lineas = await services.GetLineasByIdAsync(NroLinea);
            if(lineas == null)
            {
                return NoContent();
            }
            return Ok(lineas);
        }
        
        [HttpGet("historial-cliente/{nroDoc}")]
        public async Task<ActionResult<List<HistorialCompraDTO>>> GetHistorialCliente(int nroDoc)
        {
        var historial = await services.GetHistorialComprasAsync(nroDoc);
            if (historial == null || !historial.Any())
                return NotFound($"No se encontraron compras para el DNI {nroDoc}.");
            return Ok(historial);
        }

        [HttpGet("facturas")]
        public async Task<ActionResult<List<FacturasCompraDTO>>> GetFacturasMultiplesPagos()
        {   
            var resultado = await services.GetFacturasCompraAsync();
            return Ok(resultado);
        }

        [HttpGet("compras-importantes")]
        public async Task<ActionResult<List<ComprasImportantesDTO>>> GetComprasImportantes()
        {
            var compras = await services.GetComprasImportantesAsync();
            return Ok(compras);
        }

        [HttpGet("choferes")]
        public async Task<ActionResult<List<ChoferesDTO>>> GetChoferesRendimiento()
        {
            var rendimiento = await services.GetChoferesAsync();
            return Ok(rendimiento);
        }
        
    }
}