using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class EstacionesRecorridosDTO
    {
        public int IdEstacionRecorrido { get; set; }

        public int NroLinea { get; set; }

        public int IdEstacion { get; set; }

        public EstacionesDTO? IdEstacionNavigation { get; set; }

        public RecorridosDTO? NroLineaNavigation { get; set; }
    }
}