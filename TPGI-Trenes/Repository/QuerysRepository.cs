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
            string sql = "EXECUTE dbo.PA_ReporteAvanzadoLineaTren";
            List<LineasDTO> linea = await bd.Database.SqlQueryRaw<LineasDTO>(sql,NroLinea).ToListAsync();
            return linea;
        }
    }
}