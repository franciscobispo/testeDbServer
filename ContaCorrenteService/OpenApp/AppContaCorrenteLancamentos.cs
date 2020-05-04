using ContaCorrenteService.Interfaces;
using Domain.Interfaces.InterfaceContaCorrenteLancamentos;
using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContaCorrenteService.OpenApp
{
    public class AppContaCorrenteLancamentos : InterfaceContaCorrenteLancamentosApp
    {
        IContaCorrenteLancamentos _contaCorrenteLancamentos;

        public AppContaCorrenteLancamentos(IContaCorrenteLancamentos contaCorrenteLancamentos)
        {
            _contaCorrenteLancamentos = contaCorrenteLancamentos;
        }

        public async Task<ContaCorrenteLancamentos> GetEntityById(int Id)
        {
            return await _contaCorrenteLancamentos.GetEntityById(Id);
        }

        public async Task<List<ContaCorrenteLancamentos>> List()
        {
            return await _contaCorrenteLancamentos.List();
        }

        public async Task<List<ContaCorrenteLancamentos>> ObterExtrato(int idContaCorrente)
        {
            return await _contaCorrenteLancamentos.ObterExtrato(idContaCorrente);
        }

        public async Task<decimal> ObterSaldo(int idContaCorrente)
        {
            return await _contaCorrenteLancamentos.ObterSaldo(idContaCorrente);
        }
        public async Task<bool> Inserir(ContaCorrenteLancamentos contaCorrenteLancamento)
        {
            return await _contaCorrenteLancamentos.Inserir(contaCorrenteLancamento);
        }

        public async Task<bool> Transferir(int idContaCorrenteOrigem, int idContaCorrenteDestino, decimal valor)
        {
            return await _contaCorrenteLancamentos.Transferir(idContaCorrenteOrigem, idContaCorrenteDestino, valor);
        }
    }
}
