using Microsoft.EntityFrameworkCore;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Repository;
using TPGI_Trenes.Repository.Interfaces;
using TPGI_Trenes.Services.Interfaces;

namespace TPGI_Trenes.Services
{
    public class LineasServices : ILineasServices
    {
        private readonly ILineasRepository repo;
        private readonly QueryRepository queryrepo;

        public LineasServices(ILineasRepository repo,QueryRepository queryrepo)
        {
            this.repo = repo;
            this.queryrepo = queryrepo;
        }

        public async Task<List<ChoferesDTO>> GetChoferesAsync()
        {
            List<ChoferesDTO> choferes = await queryrepo.GetChoferesRendimiento();

            return choferes;
        }

        public async Task<List<ComprasImportantesDTO>> GetComprasImportantesAsync()
        {
            List<ComprasImportantesDTO> compras = await queryrepo.GetComprasImportantes();
            return compras;
        }

        public async Task<List<FacturasCompraDTO>> GetFacturasCompraAsync()
        {
            List<FacturasCompraDTO> facturas = await queryrepo.GetFacturasConMultiplesPagos();
            return facturas;
        }

        public async Task<List<HistorialCompraDTO>> GetHistorialComprasAsync(int nroDoc)
        {
            List<HistorialCompraDTO> compras = await queryrepo.GetHistorialComprasPorDni(nroDoc);
            return compras;
        }

        public async Task<List<LineasDTO>> GetLineasByIdAsync(int NroLinea)
        {
            List<LineasDTO> lineas = await queryrepo.GetProcedimientoLineas(NroLinea);

            return lineas;
        }
    }
}