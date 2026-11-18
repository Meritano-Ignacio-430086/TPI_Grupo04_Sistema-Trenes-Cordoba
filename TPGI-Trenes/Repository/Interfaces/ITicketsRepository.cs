using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.Repository.Interfaces
{
    public interface ITicketsRepository
    {
        Task<List<Tickets>> GetByClienteId(int id);
        Task<bool> AddTicket(Tickets_CreateDTO ticket);
        Task<Empleados> GetEmpleado(int dniEmpleado);
        Task<Clientes> GetCliente(int dniCliente);
        Task<FormasPago> GetFormaPago(string descripcion);
    }
}
