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
        [Route("Clientes/{id}")]
        public async Task<ActionResult<List<Tickets>>> GetTicketsByClienteID([FromBody]int id)
        {
            List<Tickets> tickets = await services.GetByClienteId(id);
            if(tickets == null)
            {
                return NoContent();
            }
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
