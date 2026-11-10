using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.Repository.Interfaces
{
    public interface ITicketsRepository
    {
        Task<List<Tickets>> GetById(int id);
        Task<bool> AddTicket(Tickets ticket);
    }
}
