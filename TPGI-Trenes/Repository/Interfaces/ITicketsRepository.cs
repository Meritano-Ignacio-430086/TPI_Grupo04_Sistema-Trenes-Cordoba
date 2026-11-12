using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.Repository.Interfaces
{
    public interface ITicketsRepository
    {
<<<<<<< HEAD
        Task<List<Tickets>> GetById(int id);
        Task<bool> AddTicket(Tickets ticket);
=======
        Task<List<Tickets>> GetByClienteId(int id);
>>>>>>> origin/TPI_Grupo04_Sistema-Trenes-Cordoba
    }
}
