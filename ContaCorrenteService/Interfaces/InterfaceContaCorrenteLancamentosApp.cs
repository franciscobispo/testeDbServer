using ContaCorrenteService.Interfaces.Generics;
using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContaCorrenteService.Interfaces
{
    public interface InterfaceContaCorrenteLancamentosApp : InterfaceGenericApp<ContaCorrenteLancamentos>
    {
        Task<decimal> ObterSaldo(int idContaCorrente);
        Task<List<ContaCorrenteLancamentos>> ObterExtrato(int idContaCorrente);
        Task<bool> Inserir(ContaCorrenteLancamentos contaCorrenteLancamento);
        Task<bool> Transferir(int idContaCorrenteOrigem, int idContaCorrenteDestino, decimal valor);
    }
}
