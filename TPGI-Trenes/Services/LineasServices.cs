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

        public LineasServices(LineasRepository repo)
        {
            this.repo = repo;
        }

        public Task<List<RecorridosDTO>> GetById(int NroLinea)
        {
            throw new NotImplementedException();
        }
    }
}