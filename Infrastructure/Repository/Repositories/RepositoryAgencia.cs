using Domain.Interfaces.InterfaceAgencia;
using Entities.Entities;
using Infrastructure.Repository.Generics;

namespace Infrastructure.Repository.Repositories
{
    public class RepositoryAgencia : RepositoryGenerics<Agencia>, IAgencia
    {
    }
}
