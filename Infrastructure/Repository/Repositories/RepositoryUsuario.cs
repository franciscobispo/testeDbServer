using Domain.Interfaces.InterfaceUsuario;
using Entities.Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Repositories
{
    public class RepositoryUsuario : RepositoryGenerics<Usuario>, IUsuario
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;
        public RepositoryUsuario()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }
        public async Task<Usuario> Get(string userName, string password)
        {
            byte[] passwordHash = new byte[64], passwordSalt = new byte[128];

            using (var data = new ContextBase(_OptionsBuilder))
            {

                var usuario = await Task.Run(() => data.Usuario.Include(c => c.ContaCorrentes).ThenInclude(p => p.Agencia).Where(u => u.Username == userName).SingleOrDefaultAsync());

                // check if username exists
                if (usuario == null)
                    return null;

                // check if password is correct
                if (!VerifyPasswordHash(password, usuario.PasswordHash, usuario.PasswordSalt))
                    return null;

                // authentication successful
                return usuario;
            }            
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
