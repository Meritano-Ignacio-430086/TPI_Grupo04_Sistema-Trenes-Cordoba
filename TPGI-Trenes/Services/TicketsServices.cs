using TPGI_Trenes.Negocio;
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
        public async Task<List<Tickets>> GetByClienteId(int id)
        {
            List<Tickets> list = await repo.GetByClienteId(id);
            return list;
        }
    }
}
