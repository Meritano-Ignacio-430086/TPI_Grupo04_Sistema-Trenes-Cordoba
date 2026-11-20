using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.Services.Interfaces
{
    public interface IMantenimientosServices
    {
        Task<List<Mantenimientos_ResponseDTO>> GetAll();
        Task<bool> AddMantenimiento(Mantenimientos_CreateDTO mant);
        Task<bool> CloseMantenimiento(int dniResponsable, string unidad);
    }
}
