using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class DetalleTickets_CreateDTO
    {

        public int NroLinea { get; set; }

        public string Estacion { get; set; } = "";

        public string HoraPartida { get; set; } = "";
        public string TipoVagon { get; set; } = "";
    }
}