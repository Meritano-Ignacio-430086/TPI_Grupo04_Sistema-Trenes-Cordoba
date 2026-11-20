using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.Repository.Interfaces
{
    public interface ITicketsRepository
    {
        Task<List<Tickets>> GetByClienteId(int id);
        Task<bool> AddTicket(Tickets_CreateDTO ticket);
        Task<bool> GetByDniEmpleado(int dniEmpleado);
        Task<bool> GetByDniCliente(int dniCliente);
        Task<bool> GetDescFormaPago(string descripcion);
        Task<bool> GetByLineasFilter(int nroLinea, string estacion, TimeOnly hora);
        Task<int> GetAsientosLibres(int nroLinea, string estacion, TimeOnly hora, string tipoVagon);
    }
}
