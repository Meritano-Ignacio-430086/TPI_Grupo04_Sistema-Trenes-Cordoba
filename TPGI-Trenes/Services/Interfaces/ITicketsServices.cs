using TPGI_Trenes.Negocio;
using TPGI_Trenes.DTO;

namespace TPGI_Trenes.Services.Interfaces
{
    public interface ITicketsServices
    {
        Task<List<Tickets>> GetById(int id);
        Task<bool> Add(TicketsDTO ticket);
    }
}
