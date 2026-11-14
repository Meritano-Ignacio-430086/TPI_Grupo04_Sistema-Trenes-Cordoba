using TPGI_Trenes.Negocio;
using TPGI_Trenes.DTO;

namespace TPGI_Trenes.Services.Interfaces
{
    public interface ITicketsServices
    {
        Task<bool> AddTicket(Tickets_CreateDTO ticket);
        Task<List<Tickets>> GetByClienteId(int id);
    }
}
