using TPGI_Trenes.Negocio;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Repository;
using TPGI_Trenes.Repository.Interfaces;
using TPGI_Trenes.Services.Interfaces;

namespace TPGI_Trenes.Services
{
    public class TicketsServices : ITicketsServices
    {
        private readonly ITicketsRepository repo;

        public TicketsServices(ITicketsRepository repo)
        {
            this.repo = repo;
        }

        public async Task<bool> AddTicket(Tickets_CreateDTO ticket)
        {
            return await repo.AddTicket(ticket);
        }

        public async Task<List<Tickets>> GetByClienteId(int id)
        {
            List<Tickets> list = await repo.GetByClienteId(id);
            return list;
        }
/*        private static List<DetalleTickets> MapToTicketsDTO(Tickets tickets)
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
 */    
 }
}
