using TPGI_Trenes.Negocio;
using TPGI_Trenes.Repository;
using TPGI_Trenes.Repository.Interfaces;

namespace TPGI_Trenes.Services
{
    public class TicketsServices : ITicketsRepository
    {
        private readonly TicketsRepository repo;

        public TicketsServices(TicketsRepository repo)
        {
            this.repo = repo;
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
    }
}
