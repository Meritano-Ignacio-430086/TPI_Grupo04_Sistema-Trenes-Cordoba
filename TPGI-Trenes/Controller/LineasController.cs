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

        public async Task<ActionResult<List<LineasDTO>>> GetById(int NroLinea)
        {
            List<LineasDTO> lineas = await services.GetById(NroLinea);
            if(lineas == null)
            {
                return NoContent();
            }
            return Ok(lineas);
        }
    }
}