using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class Mantenimientos_CreateDTO
    {
        public int IdTipoMantenimiento { get; set; }
        public int IdEmpleado { get; set; }
        public int IdTren { get; set; }
    }
}
/*
     public int IdMantenimiento { get; set; }

    public int IdTipoMantenimiento { get; set; }

    public DateTime? Fecha { get; set; }

    public int IdEmpleado { get; set; }

    public int IdTren { get; set; }

    public virtual Empleados IdEmpleadoNavigation { get; set; }

    public virtual TiposMantenimientos IdTipoMantenimientoNavigation { get; set; }

    public virtual Trenes IdTrenNavigation { get; set; }
*/