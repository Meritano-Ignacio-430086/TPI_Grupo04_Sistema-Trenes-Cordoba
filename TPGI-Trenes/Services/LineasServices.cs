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

<<<<<<< HEAD
        public Task<List<RecorridosDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<RecorridosDTO>> GetById(int NroLinea)
=======
        public async Task<List<LineasDTO>> GetById(int NroLinea)
>>>>>>> origin/TPI_Grupo04_Sistema-Trenes-Cordoba
        {
            List<LineasDTO> lineas = await queryrepo.GetProcedimientoLineas(NroLinea);

            return lineas;
        }
    }
}