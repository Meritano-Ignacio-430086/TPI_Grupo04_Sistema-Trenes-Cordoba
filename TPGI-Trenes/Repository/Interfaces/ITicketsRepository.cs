using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.Repository.Interfaces
{
    public interface ITicketsRepository
    {
        Task<List<Tickets>> GetByClienteId(int id);
    }
}
