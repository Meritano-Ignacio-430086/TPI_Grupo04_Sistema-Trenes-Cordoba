using System.ComponentModel.DataAnnotations;

namespace TPGI_Trenes.DTO
{
    public class UsuarioLoginDTO
    {
        public required string UserEmail { get; set; }
        
        public required string Password { get; set; }
        
    }
}
