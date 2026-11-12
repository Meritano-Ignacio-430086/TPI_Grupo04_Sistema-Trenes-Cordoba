using TPGI_Trenes.Negocio;
using TPGI_Trenes.DTO;

namespace TPGI_Trenes.Services.Interfaces
{
    public interface ITicketsServices
    {
<<<<<<< HEAD
        Task<List<Tickets>> GetById(int id);
        Task<bool> Add(TicketsDTO ticket);
=======
        Task<List<Tickets>> GetByClienteId(int id);
>>>>>>> origin/TPI_Grupo04_Sistema-Trenes-Cordoba
    }
}
