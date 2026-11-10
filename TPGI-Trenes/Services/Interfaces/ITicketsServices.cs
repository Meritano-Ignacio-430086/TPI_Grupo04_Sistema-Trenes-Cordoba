using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.Services.Interfaces
{
    public interface ITicketsServices
    {
        Task<List<Tickets>> GetById(int id);
    }
}
