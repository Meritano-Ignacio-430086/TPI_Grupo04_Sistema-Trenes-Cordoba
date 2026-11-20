using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class Mantenimientos_CreateDTO
    {
        public string? TipoMantenimiento { get; set; }
        public int EmpleadoDNI { get; set; }
        public string? UnidadTren { get; set; }
    }
}
