using TPGI_Trenes.DTO;
using TPGI_Trenes.Repository.Interfaces;
using TPGI_Trenes.Services.Interfaces;

namespace TPGI_Trenes.Services
{
    public class TrenesServices : ITrenesServices
    {
        private readonly ITrenesRepository repo;
        public TrenesServices(ITrenesRepository repo)
        {
            this.repo = repo;
        }

        public Task<List<TrenesDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<TrenesDTO>> GetById(int IdTren)
        {
            throw new NotImplementedException();
        }
    }
}
