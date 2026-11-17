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
            var empleado = await _context.Empleados
                    .FirstOrDefaultAsync(e => e.NroDoc == mant.EmpleadoDNI);
            if (empleado == null) return false;

            var tipo = await _context.TiposMantenimientos
                .FirstOrDefaultAsync(t => t.Descripcion == mant.TipoMantenimiento);
            if (tipo == null) return false;

            var tren = await _context.Trenes
                .Include(t => t.IdEstadoNavigation)
                .FirstOrDefaultAsync(t => t.NroUnidad == mant.UnidadTren);
            if (tren == null) return false;

            if (tren.IdEstadoNavigation.Descripcion != "Operativo") return false;

            var estadoMant = await _context.Estados
                .FirstOrDefaultAsync(e => e.Descripcion == "En mantenimiento");
            if (estadoMant == null) return false;

            //if (empleado.IdRolNavigation.Descripcion != "Mecánico") return false; //Preguntar sobre eso

                _context.Mantenimientos.Add(new Mantenimientos
            {
                Fecha = DateTime.Now,
                IdEmpleado = empleado.IdEmpleado,
                IdTren = tren.IdTren,
                IdTipoMantenimiento = tipo.IdTipoMantenimiento
            });

            tren.IdEstado = estadoMant.IdEstado;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CloseMantenimiento(int dniEmpleado, string unidad)
        {
            var mantenimiento = await _context.Mantenimientos
                .Include(m=>m.IdTrenNavigation)
                .FirstOrDefaultAsync(t => t.IdTrenNavigation.NroUnidad == unidad);
            if (mantenimiento == null) return false;
            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(e => e.NroDoc == dniEmpleado);
            if (empleado == null) return false;

            var tren = await _context.Trenes.FirstOrDefaultAsync(t=>t.NroUnidad==unidad);
            if (tren == null) return false;

            var estadoMant = await _context.Estados
                .FirstOrDefaultAsync(e => e.Descripcion == "Operativo");
            if (estadoMant == null) return false;

            _context.Mantenimientos.Add(new Mantenimientos
            {
                Fecha = DateTime.Now,
                IdEmpleado = empleado.IdEmpleado,
                IdTren = mantenimiento.IdTren,
                IdTipoMantenimiento = mantenimiento.IdTipoMantenimiento
            });

            tren.IdEstado = estadoMant.IdEstado;

            await _context.SaveChangesAsync();
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