using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContaCorrenteService.Interfaces.Generics
{
    public interface InterfaceGenericApp<T> where T : class
    {
        Task<T> GetEntityById(int Id);
        Task<List<T>> List();
    }
}
