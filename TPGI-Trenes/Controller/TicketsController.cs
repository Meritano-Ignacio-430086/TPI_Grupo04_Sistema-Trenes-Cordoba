using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPGI_Trenes.Services.Interfaces;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.DTO;
using Microsoft.Identity.Client;

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

        [HttpGet]
        [Route("Clientes/{dni}")]
        public async Task<ActionResult<List<HistorialCompraDTO>>> GetTicketsByClienteID([FromRoute] int? dni)
        {
            List<HistorialCompraDTO> tickets = await services.GetByClienteId(dni);
            return Ok(tickets);
        }
        [HttpGet]
        [Route("Clientes")]
        public async Task<ActionResult<List<HistorialCompraDTO>>> Getall()
        {

            List<HistorialCompraDTO> tickets = await services.GetAllCliente();
            return Ok(tickets);

        }

        [HttpPost]
        [Route("Ticket/Create")]
        public async Task<ActionResult<List<Tickets>>> CreateTicket([FromBody] Tickets_CreateDTO ticket)
        {
            bool res = await services.AddTicket(ticket);
            if (res)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }
    }
}
