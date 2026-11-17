using Microsoft.EntityFrameworkCore;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Repository.Interfaces;

namespace TPGI_Trenes.Repository
{
    public class MantenimientosRepository : IMantenimientosRepository
    {
        private readonly TrenesContext _context;
        public MantenimientosRepository(TrenesContext context)
        {
             this._context = context;
        }

        public async Task<bool> AddMantenimiento(Mantenimientos_CreateDTO mant)
        {
            //new Mantenimientos n_mant = new Mantenimientos
            //{
            //    IdEmpleado =,
            //    Fecha =,
            //    IdMantenimiento =,
            //    IdTipoMantenimiento =,
            //    IdTren =,

            //};
            return true;
        }

        public async Task<List<Mantenimientos_ResponseDTO>> GetAll()
        {
            return await _context.Mantenimientos.Select(m=>
            new Mantenimientos_ResponseDTO
            {
                Fecha = m.Fecha,
                Empleado = m.IdEmpleadoNavigation.Nombre+' '+m.IdEmpleadoNavigation.Apellido,
                TipoMantenimiento = m.IdTipoMantenimientoNavigation.Descripcion,
                Unidad = m.IdTrenNavigation.NroUnidad
            }).ToListAsync()
            ;
        }
    }
}