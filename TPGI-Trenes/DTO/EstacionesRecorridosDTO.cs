using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class EstacionesRecorridosDTO
    {
        public int IdEstacionRecorrido { get; set; }

        public int NroLinea { get; set; }

        public int IdEstacion { get; set; }

        public virtual Estaciones? IdEstacionNavigation { get; set; }

        public virtual Recorridos? NroLineaNavigation { get; set; }
    }
}