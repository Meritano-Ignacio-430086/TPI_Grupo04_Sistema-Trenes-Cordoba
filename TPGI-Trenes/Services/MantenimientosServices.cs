using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Repository;
using TPGI_Trenes.Repository.Interfaces;
using TPGI_Trenes.Services.Interfaces;

namespace TPGI_Trenes.Services
{
    public class MantenimientosServices: IMantenimientosServices
    {
        private readonly IMantenimientosRepository _repo;
        public MantenimientosServices (IMantenimientosRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> AddMantenimiento(Mantenimientos_CreateDTO mant)
        {
            return await _repo.AddMantenimiento(mant);
        }

        public async Task<bool> CloseMantenimiento(int dniEmpleado, string unidad)
        {
            return await _repo.CloseMantenimiento(dniEmpleado,unidad);
        }

        public async Task<List<Mantenimientos_ResponseDTO>> GetAll()
        {
            return await _repo.GetAll();
        }
    }
}
