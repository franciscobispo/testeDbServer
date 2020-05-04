using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Generics
{
    public interface IGeneric<T> where T : class
    {
        Task<T> GetEntityById(int Id);
        Task<List<T>> List();
    }
}
