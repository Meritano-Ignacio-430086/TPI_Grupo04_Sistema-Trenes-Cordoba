using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.Repository.Interfaces
{
    public interface ITicketsRepository
    {
        //Task<List<Tickets>> GetByClienteId(int dni);
        Task<bool> AddTicket(Tickets_CreateDTO ticket);
        // Task<bool> GetByDniEmpleado(int dniEmpleado);
        Task<List<Tickets>> GetByDniCliente(int? dniCliente);
        Task<List<Tickets>> GetAllCliente();
        // Task<bool> GetDescFormaPago(string descripcion);
        // Task<bool> GetByLineasFilter(int nroLinea, string estacion, TimeOnly hora);
        // Task<int> GetAsientosLibres(int nroLinea, string estacion, TimeOnly hora, string tipoVagon);
    }
}