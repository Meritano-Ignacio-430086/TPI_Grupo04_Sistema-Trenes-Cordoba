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

        public async Task<List<Tickets>> GetByClienteId(int? dni)
        {
            return await repo.GetByDniCliente(dni);
        }
        public async Task<List<HistorialCompraDTO>> GetAllCliente()
        {
            return await repo.GetAllCliente();
        }
    }
}
