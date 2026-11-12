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
        private static List<DetalleTickets> MapToTicketsDTO(Tickets tickets)
        {
            List<DetalleTicketsDTO> detalleTickets = new List<DetalleTicketsDTO>();
            ClientesDTO clientes = new ClientesDTO();
            EmpleadosDTO empleados = new EmpleadosDTO();
            List<TicketsXFormapagoDTO> ticketsXFormapagos = new List<TicketsXFormapagoDTO>();

            foreach (DetalleTickets detail in tickets.DetalleTickets)
            {
                detalleTickets.Add(MapDetalleTicketsToDTO(detail));
            }

            return new TicketsDTO
            {
                IdCliente = tickets.IdCliente,
                IdTren = tickets.IdTren,
                FechaCompra = tickets.Fecha,
                Precio = tickets.Precio,
                DetalleTickets = detalleTickets
                IdClienteNavigation = clientes,
                IdEmpleadoNavigation = empleados,
                TicketsXFormapago = ticketsXFormapagos
            };
        }

        private static DetalleTicketsDTO MapDetalleTicketsToDTO(DetalleTickets detail)
        {
            throw new NotImplementedException();
        }
    }
}
