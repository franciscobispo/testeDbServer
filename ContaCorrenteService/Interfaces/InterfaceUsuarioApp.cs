using ContaCorrenteService.Interfaces.Generics;
using Entities.Entities;
using System.Threading.Tasks;

namespace ContaCorrenteService.Interfaces
{
    public interface InterfaceUsuarioApp : InterfaceGenericApp<Usuario>
    {
        Task<Usuario> Get(string userName, string password);
    }
}
