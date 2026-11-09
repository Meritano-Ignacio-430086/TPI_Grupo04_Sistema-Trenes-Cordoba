using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class RecorridosDTO
    {
        public int NroLinea { get; set; }

        public string? Descripcion { get; set; }

        public List<EstacionesRecorridosDTO>? EstacionesRecorridos { get; set; } = new ();

        public List<TrenesRecorridosEstacionesDTO>? TrenesRecorridosEstaciones { get; set; } = new();
    }
}