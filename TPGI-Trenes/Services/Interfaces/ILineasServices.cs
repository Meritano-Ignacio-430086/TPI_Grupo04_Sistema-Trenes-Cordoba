using TPGI_Trenes.Negocio;
using TPGI_Trenes.DTO;

namespace TPGI_Trenes.Services.Interfaces
{
    public interface ILineasServices
    {
        Task <List<LineasDTO>> GetLineasByIdAsync (int NroLinea);
        Task<List<HistorialCompraDTO>> GetHistorialComprasAsync(int nroDoc);
        Task<List<FacturasCompraDTO>> GetFacturasCompraAsync();
        Task<List<ComprasImportantesDTO>> GetComprasImportantesAsync();
        Task<List<ChoferesDTO>> GetChoferesAsync();
    }

}