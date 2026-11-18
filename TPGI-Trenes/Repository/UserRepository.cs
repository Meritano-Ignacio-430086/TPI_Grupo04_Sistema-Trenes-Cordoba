using Microsoft.EntityFrameworkCore;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Repository.Interfaces;

namespace TPGI_Trenes.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TrenesContext bd;
        public UserRepository(TrenesContext bd)
        {
            this.bd = bd;
        }

        public async Task<Usuario?> ObtenerCorreoAsync(string email)
        {
            return await bd.Usuarios.FirstOrDefaultAsync(u=>u.Email == email);
        }

        public async Task RegistroUsuarios(Usuario usuario)
        {
            await bd.Usuarios.AddAsync(usuario);
            await bd.SaveChangesAsync();
        }
    } 
}