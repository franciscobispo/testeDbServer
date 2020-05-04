using Domain.Interfaces.Generics;
using Entities.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces.InterfaceUsuario
{
    public interface IUsuario : IGeneric<Usuario>
    {
        Task<Usuario> Get(string userName, string password);
    }
}
