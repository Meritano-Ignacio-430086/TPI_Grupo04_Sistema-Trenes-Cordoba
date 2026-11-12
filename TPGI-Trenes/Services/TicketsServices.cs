using TPGI_Trenes.Negocio;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Repository;
<<<<<<< HEAD
=======
using TPGI_Trenes.Repository.Interfaces;
>>>>>>> origin/TPI_Grupo04_Sistema-Trenes-Cordoba
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
<<<<<<< HEAD

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
=======
        public async Task<List<Tickets>> GetByClienteId(int id)
>>>>>>> origin/TPI_Grupo04_Sistema-Trenes-Cordoba
        {
            List<Tickets> list = await repo.GetByClienteId(id);
            return list;
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
