using ContaCorrenteService.Interfaces;
using Domain.Interfaces.InterfaceUsuario;
using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContaCorrenteService.OpenApp
{
    public class AppUsuario : InterfaceUsuarioApp
    {
        IUsuario _usuario;

        public AppUsuario(IUsuario usuario)
        {
            _usuario = usuario;
        }

        public async Task<Usuario> Get(string userName, string password)
        {
            return await _usuario.Get(userName, password);
        }

        public async Task<Usuario> GetEntityById(int Id)
        {
            return await _usuario.GetEntityById(Id);
        }

        public async Task<List<Usuario>> List()
        {
            return await _usuario.List();
        }        
    }
}
