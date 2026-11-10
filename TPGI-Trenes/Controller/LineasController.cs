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
        public async Task<ActionResult<List<LineasDTO>>> GetLineasByIdAsync([FromQuery]int NroLinea)
        {
            List<LineasDTO> lineas = await services.GetLineasByIdAsync(NroLinea);
            if(lineas == null)
            {
                return NoContent();
            }
            return Ok(lineas);
        }

        
    }
}