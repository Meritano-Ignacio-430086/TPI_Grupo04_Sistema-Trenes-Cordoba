namespace TPGI_Trenes.DTO
{
    public class FacturasCompraDTO
    {
        public int Nro_Factura { get; set; }
        public string? Proveedor { get; set; }
        public DateTime Fecha { get; set; }
        public decimal MontoDeLaFactura { get; set; }
        public decimal TotalDelDetalle { get; set; }
        public decimal Diferencia { get; set; }
        public int RegistrosDePago { get; set; }
        public int FormasDePagoDistintas { get; set; }
        public string? Observacion { get; set; }
    }
}