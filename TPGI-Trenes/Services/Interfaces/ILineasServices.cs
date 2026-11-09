using TPGI_Trenes.Negocio;
using TPGI_Trenes.DTO;

namespace TPGI_Trenes.Services.Interfaces
{
    public interface ILineasServices
    {
        Task <List<LineasDTO>> GetById (int NroLinea);
    }

}