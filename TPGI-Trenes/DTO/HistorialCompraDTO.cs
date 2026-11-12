namespace TPGI_Trenes.DTO
{
    public class HistorialCompraDTO
    {
        public int Id_Cliente { get; set; }
        public int NroDoc { get; set; }
        public string? Tipo_Documento { get; set; }
        public int Id_Ticket { get; set; }
        public DateTime Fecha_Compra { get; set; }
        public int? Id_Det_Ticket { get; set; }
        public int? Nro_Linea { get; set; }
        public string? Recorrido { get; set; }
        public string? Estacion { get; set; }
        public string? Tren { get; set; }
        public TimeSpan Hora_Partida { get; set; }
        public TimeSpan Hora_Llegada { get; set; }
        public decimal Precio { get; set; }
    }
}