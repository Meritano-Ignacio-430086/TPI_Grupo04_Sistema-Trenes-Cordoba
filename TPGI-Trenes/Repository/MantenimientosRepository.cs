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
                    .Include(e=>e.IdRolNavigation)
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

            if (empleado.IdRolNavigation.Descripcion != "Mecánico") return false; //Preguntar sobre eso

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
            var c_estadoOperativo = await _context.Estados
            .FirstOrDefaultAsync(e => e.Descripcion == "Operativo");
            if (c_estadoOperativo == null) return false;

            var c_estadoMantenimientoFinalizado = await _context.TiposMantenimientos
                .FirstOrDefaultAsync(tm => tm.Descripcion == "Completado");

            if (c_estadoMantenimientoFinalizado == null) return false;
            if (c_estadoOperativo == null) return false;

            var empleadoResponsable = await _context.Empleados
                .Include(e=>e.IdRolNavigation)
                .FirstOrDefaultAsync(e => e.NroDoc == dniEmpleado);
            if (empleadoResponsable == null) return false;

            var tren = await _context.Trenes
                .Include(t=>t.IdEstadoNavigation)
                .FirstOrDefaultAsync(t => t.NroUnidad == unidad);
            if (tren == null) return false;

            var u_mantenimiento = await _context.Mantenimientos
                .Where(m=>m.IdTren==tren.IdTren)
                .OrderByDescending(m=>m.Fecha)
                .FirstOrDefaultAsync();
            if (u_mantenimiento == null) return false;

            string[] privUsers = { "Mecánico", "Supervisor", "Administrativo" };

            if (!privUsers.Contains(empleadoResponsable.IdRolNavigation.Descripcion)) return false;

            _context.Mantenimientos.Add(new Mantenimientos
            {
                Fecha = DateTime.Now,
                IdEmpleado = empleadoResponsable.IdEmpleado,
                IdTren = u_mantenimiento.IdTren,
                IdTipoMantenimiento = c_estadoMantenimientoFinalizado.IdTipoMantenimiento
            });

            if (tren.IdEstado == c_estadoOperativo.IdEstado)
            {
                return false;
            }

            tren.IdEstado = c_estadoOperativo.IdEstado;

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