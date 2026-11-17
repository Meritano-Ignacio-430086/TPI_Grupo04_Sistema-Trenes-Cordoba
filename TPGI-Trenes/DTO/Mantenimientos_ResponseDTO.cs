using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class Mantenimientos_ResponseDTO
    {
        public string? TipoMantenimiento { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Empleado { get; set; }
        public string? Unidad { get; set; }
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
    "idTipoMantenimiento": 1,
    "fecha": "2026-07-01T00:00:00",
    "idEmpleado": 31,
    "idTren": 35,
*/