using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.Repository.Interfaces
{
    public interface ILineasRepository
    {
        Task <List<Recorridos>> GetById (int NroLinea);
        Task<List<Recorridos>> GetAllLineas();
    }
}
