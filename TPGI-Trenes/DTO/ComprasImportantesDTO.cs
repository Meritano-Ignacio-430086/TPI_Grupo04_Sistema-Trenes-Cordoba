namespace TPGI_Trenes.DTO
{
    public class ComprasImportantesDTO
    {
        public int Nro_Factura { get; set; }
        public DateTime Fecha { get; set; }
        public string? Proveedor { get; set; }
        public string? Empleado { get; set; }
        public int Cant_Facturas { get; set; }
        public int Unidades { get; set; }
        public decimal Total { get; set; }
    }
}