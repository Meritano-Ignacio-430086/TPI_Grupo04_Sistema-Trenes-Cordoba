using TPGI_Trenes.Negocio;
using TPGI_Trenes.DTO;

namespace TPGI_Trenes.Services.Interfaces
{
    public interface ITicketsServices
    {
        Task<bool> Add(TicketsDTO ticket);
        Task<List<Tickets>> GetByClienteId(int id);
    }
}
