using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.Repository.Interfaces
{
    public interface ITicketsRepository
    {
        Task<bool> AddTicket(Tickets_CreateDTO ticket);
        Task<List<HistorialCompraDTO>> GetByDniCliente(int? dniCliente);
        Task<List<HistorialCompraDTO>> GetAllCliente();
        
    }
}