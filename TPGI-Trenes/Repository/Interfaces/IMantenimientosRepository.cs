using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.Repository.Interfaces
{
    public interface IMantenimientosRepository
    {
        Task<bool> AddMantenimiento(Mantenimientos_CreateDTO mant); //Cambia el estado del tren a 'en mantenimiento'
        Task<List<Mantenimientos_ResponseDTO>> GetAll();
    }
}
