using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Repository.Interfaces;

namespace TPGI_Trenes.Repository
{
    public class TicketsRepository : ITicketsRepository
    {
        private readonly TrenesContext bd;

        public TicketsRepository(TrenesContext bd)
        {
            this.bd = bd;
        }

        public async Task<bool> AddTicket(Tickets_CreateDTO ticket)
        {
            Tickets ticketaux = new Tickets
            {
                IdEmpleado = ticket.IdEmpleado,
                IdCliente = ticket.IdCliente,
                Fecha = ticket.FechaCompra,
                DetalleTickets = ticket.DetalleTickets?.Select //Detalle tickets podría ser null
                (
                    dt=>new DetalleTickets
                    {
                        IdTrenesVagones=dt.IdTrenesVagones,
                        IdTrenRecorridoEst=dt.IdTrenRecorridoEst,
                        Precio = dt.Precio
                    }
                    ).ToList()
            };
            bd.Tickets.Add(ticketaux);
            await bd.SaveChangesAsync();
            return true;
        }

        public async Task<List<Tickets>> GetByClienteId(int id)
        {
            List<Tickets> tickets =  await bd.Tickets.Where(p=>p.IdCliente==id).ToListAsync();
            return tickets;
        }
    }
}
