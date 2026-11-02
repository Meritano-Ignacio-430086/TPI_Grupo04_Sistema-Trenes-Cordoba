using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class TrenesRecorridosEstacionesDTO
    {
        public int NroLinea { get; set; }

        public TimeOnly? HoraPartida { get; set; }

        public TimeOnly? HoraLlegada { get; set; }

        public int IdEstacion { get; set; }

        public TimeOnly? HoraEstacion { get; set; }

        public int IdChofer { get; set; }

    }
}