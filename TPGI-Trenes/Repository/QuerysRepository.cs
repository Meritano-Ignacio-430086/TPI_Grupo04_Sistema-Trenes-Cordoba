using Microsoft.EntityFrameworkCore;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.Repository
{
    public class QueryRepository
    {
        private readonly TrenesContext bd;

        public QueryRepository(TrenesContext bd)
        {
            this.bd = bd;
        }

        public async Task<List<LineasDTO>> GetProcedimientoLineas(int NroLinea)
        {
        List<LineasDTO> linea = await bd.Database.SqlQuery<LineasDTO>(
            $"EXECUTE dbo.PA_ReporteAvanzadoLineaTren @Nro_Linea = {NroLinea}"
            ).ToListAsync();
    
        return linea;
        }

        public async Task<List<HistorialCompraDTO>> GetHistorialComprasPorDni(int nroDoc)
        {
            return await bd.Database.SqlQuery<HistorialCompraDTO>($"SELECT * FROM dbo.fn_Historial_Compras_Cliente_DNI({nroDoc})").ToListAsync();
        }
        public async Task<List<FacturasCompraDTO>> GetFacturasConMultiplesPagos()
        {
            string sql = @"
                SELECT  f.Nro_Factura,
                        p.Nombre AS Proveedor,
                        CAST(f.Fecha AS DATE) AS Fecha,
                        f.Monto AS MontoDeLaFactura,
                        d.Total_Detalle AS TotalDelDetalle,
                        f.Monto - d.Total_Detalle AS Diferencia,
                        pg.Registros_Pago AS RegistrosDePago,
                        pg.Formas_Distintas AS FormasDePagoDistintas,
                        CASE WHEN pg.Formas_Distintas < pg.Registros_Pago
                            THEN 'Forma de pago repetida (error de carga)'
                            ELSE 'Pago combinado' END AS Observacion
                FROM Facturas f
                JOIN Proveedores p ON p.Id_Proveedor = f.Id_Proveedor
                JOIN (SELECT Nro_Factura, SUM(Cantidad * Precio) AS Total_Detalle FROM Detalles_Facturas GROUP BY Nro_Factura) d ON d.Nro_Factura = f.Nro_Factura
                JOIN (SELECT Nro_Factura, COUNT(*) AS Registros_Pago, COUNT(DISTINCT Id_Forma_Pago) AS Formas_Distintas FROM Facturas_x_Formapago GROUP BY Nro_Factura HAVING COUNT(*) > 1) pg ON pg.Nro_Factura = f.Nro_Factura
                ORDER BY pg.Formas_Distintas ASC, f.Monto DESC;";
    
                return await bd.Database.SqlQueryRaw<FacturasCompraDTO>(sql).ToListAsync();
        }
        public async Task<List<ComprasImportantesDTO>> GetComprasImportantes()
        {
            string sql = "SELECT Nro_Factura, Fecha, Proveedor, Empleado, Cant_Facturas, Unidades, Total FROM vw_Compras_Importantes";
        
            return await bd.Database.SqlQueryRaw<ComprasImportantesDTO>(sql).ToListAsync();
        }

        public async Task<List<ChoferesDTO>> GetChoferesRendimiento()
        {
            string sql = @"SELECT
            c.Id_Chofer AS Nro_Chofer,
            e.Nombre + ' ' + e.Apellido AS Chofer,
            e.NroDoc AS DNI,
            b.Descripcion AS Barrio,
            (SELECT TOP 1 ce.Contacto FROM dbo.Contactos_Empleados ce WHERE ce.Id_Empleado = e.Id_Empleado AND ce.Id_Tipo_Contacto = 2) AS Telefono,
            COUNT(DISTINCT tre.Id_Tren_Recorrido_Est) AS Viajes_Realizados,
            COUNT(DISTINCT t.Id_Tren) AS Trenes_Conducidos,
            SUM(DATEDIFF(MINUTE, tre.Hora_Partida, tre.Hora_Llegada)) AS Minutos_Conducidos,
            (SELECT AVG(DATEDIFF(MINUTE, Hora_Partida, Hora_Llegada)) FROM dbo.Trenes_Recorridos_Estaciones) AS Promedio_Minutos_Viaje,
            CASE
                WHEN SUM(DATEDIFF(MINUTE, tre.Hora_Partida, tre.Hora_Llegada)) > (SELECT AVG(DATEDIFF(MINUTE, Hora_Partida, Hora_Llegada)) FROM dbo.Trenes_Recorridos_Estaciones)
                THEN 'Por encima del promedio'
                ELSE 'En el promedio'
                END AS Estado_Carga_Horaria
        FROM dbo.Choferes c
        INNER JOIN dbo.Empleados e ON c.Id_Empleado = e.Id_Empleado
        INNER JOIN dbo.Barrios b ON e.Id_Barrio = b.Id_Barrio
        INNER JOIN dbo.Trenes_Recorridos_Estaciones tre ON c.Id_Chofer = tre.Id_Chofer
        INNER JOIN dbo.TrenesxVagones txv ON tre.Id_Trenes_Vagones = txv.Id_Trenes_Vagones
        INNER JOIN dbo.Trenes t ON txv.Id_Tren = t.Id_Tren
        WHERE c.Activo = 1
        GROUP BY c.Id_Chofer, e.Id_Empleado, e.Nombre, e.Apellido, e.NroDoc, b.Descripcion
        ORDER BY Minutos_Conducidos DESC;";
    
        return await bd.Database.SqlQueryRaw<ChoferesDTO>(sql).ToListAsync();
        }
    }
}