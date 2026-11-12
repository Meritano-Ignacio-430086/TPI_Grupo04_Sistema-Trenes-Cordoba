using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class LineasDTO
    {
        public int NumeroLinea {get;set;}
        public string? DescripcionRecorrido {get;set;}
        public string? UnidadTren {get;set;}
       	public string? ModeloTren {get;set;}
        public int TotalMantenimientosHistoricos {get;set;}
        public DateTime? UltimaFechaMantenimiento {get;set;}
        public int CantidadEstacionesEnRecorrido {get;set;}
        public int CantidadVagonesAsignados	{get;set;}
        public int CapacidadTotalVagones {get;set;}
        public string? EstadoOperativoTren {get;set;}
    }
}