using TPGI_Trenes.Negocio;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Repository;
using TPGI_Trenes.Services.Interfaces;

namespace TPGI_Trenes.Services
{
    public class TicketsServices : ITicketsServices
    {
        private readonly TicketsRepository repo;

        public TicketsServices(TicketsRepository repo)
        {
            this.repo = repo;
        }

        public Task<bool> Add(TicketsDTO ticket)
        {
            Tickets hola = new Tickets
            {
                IdCliente = ticket.IdCliente,
                IdTren = ticket.IdTren,
                Fecha = ticket.FechaCompra,
                Precio = ticket.Precio,
            };
            return repo.AddTicket(hola);
        }

        public async Task<List<Tickets>> GetById(int id)
        {
            try
            {
                return await repo.GetById(id);
            }
            catch (System.ArgumentNullException)
            {
                throw;
            }
        }
        private static List<DetalleTickets> MapToEntities(List<CreateDetalleTicketsDto> details)
        {
            List<DetalleTickets> detalleTickets = new List<DetalleTickets>();

            foreach (CreateDetalleTicketsDto detail in details)
            {
                DetalleTickets detalleTicket = new DetalleTickets
                {
                    IdTicket = detail.IdTicket,
                    IdAsiento = detail.IdAsiento,
                    IdRecorrido = detail.IdRecorrido,
                    Precio = detail.Precio
                };

                detalleTickets.Add(detalleTicket);
            }

            return detalleTickets;
        }
    }
}
