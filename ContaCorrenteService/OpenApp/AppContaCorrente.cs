using ContaCorrenteService.Interfaces;
using Domain.Interfaces.InterfaceContaCorrente;
using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContaCorrenteService.OpenApp
{
    public class AppContaCorrente : InterfaceContaCorrenteApp
    {
        IContaCorrente _contaCorrente;

        public AppContaCorrente(IContaCorrente contaCorrente)
        {
            _contaCorrente = contaCorrente;
        }

        public async Task<ContaCorrente> GetEntityById(int Id)
        {
            return await _contaCorrente.GetEntityById(Id);
        }

        public async Task<List<ContaCorrente>> List()
        {
            return await _contaCorrente.List();
        }
    }
}
