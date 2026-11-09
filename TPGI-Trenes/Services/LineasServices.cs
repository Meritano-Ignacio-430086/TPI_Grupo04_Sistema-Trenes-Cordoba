using Microsoft.EntityFrameworkCore;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Repository;
using TPGI_Trenes.Services.Interfaces;

namespace TPGI_Trenes.Services
{
    public class LineasServices : ILineasServices
    {
        private readonly LineasRepository repo;
        private readonly QueryRepository queryrepo;

        public LineasServices(LineasRepository repo,QueryRepository queryrepo)
        {
            this.repo = repo;
            this.queryrepo = queryrepo;
        }

        public async Task<List<LineasDTO>> GetById(int NroLinea)
        {
            List<LineasDTO> lineas = await queryrepo.GetProcedimientoLineas(NroLinea);

            return lineas;
        }
    }
}