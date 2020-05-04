using Domain.Interfaces.InterfaceContaCorrenteLancamentos;
using Entities.Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Repositories
{
    public class RepositoryContaCorrenteLancamentos : RepositoryGenerics<ContaCorrenteLancamentos>, IContaCorrenteLancamentos
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;
        public RepositoryContaCorrenteLancamentos()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<List<ContaCorrenteLancamentos>> ObterExtrato(int idContaCorrente)
        {
            using (var data = new ContextBase(_OptionsBuilder))
            {
                var contaCorrenteLancamentos = await Task.Run(() => data.ContaCorrenteLancamentos.Include(c => c.ContaCorrente).ThenInclude(p => p.Agencia).Where(u => u.ContaCorrente.Id == idContaCorrente).ToList());

                return contaCorrenteLancamentos.OrderBy(c => c.DataLancamento).ToList(); //ordenada do mais recente para o mais antigo
            }
        }

        public async Task<decimal> ObterSaldo(int idContaCorrente)
        {
            using (var data = new ContextBase(_OptionsBuilder))
            {
                decimal creditos = await Task.Run(() => data.ContaCorrenteLancamentos.Where(ccl => ccl.ContaCorrente.Id == idContaCorrente && ccl.TipoLancamento == Entities.Enums.TipoLancamentoEnum.Credito).Sum(ccl => ccl.Valor));
                decimal debitos = await Task.Run(() => data.ContaCorrenteLancamentos.Where(ccl => ccl.ContaCorrente.Id == idContaCorrente && ccl.TipoLancamento == Entities.Enums.TipoLancamentoEnum.Debito).Sum(ccl => ccl.Valor));
                return creditos - debitos;
            }
        }

        public async Task<bool> Inserir(ContaCorrenteLancamentos contaCorrenteLancamento)
        {
            using (var data = new ContextBase(_OptionsBuilder))
            {
                var contacorrente = await Task.Run(() => data.ContaCorrente.Include(c => c.Agencia).Where(cc => cc.Id == contaCorrenteLancamento.ContaCorrente.Id).FirstOrDefaultAsync());
                contaCorrenteLancamento.ContaCorrente = contacorrente;
                await data.ContaCorrenteLancamentos.AddAsync(contaCorrenteLancamento);
                int id = await data.SaveChangesAsync();
                return id > 0;
            }
        }
        
        public async Task<bool> Transferir(int idContaCorrenteOrigem, int idContaCorrenteDestino, decimal valor)
        {
            if (valor <= 0)
                throw new ApplicationException("Valor da transferência deve ser maior que zero");
            
            if (idContaCorrenteOrigem == idContaCorrenteDestino)            
                throw new ApplicationException("Conta de origem não pode ser igual a de destino");            

            using (var data = new ContextBase(_OptionsBuilder))
            {
                var contacorrenteOrigem = await Task.Run(() => data.ContaCorrente.Include(c => c.Agencia).Where(cc => cc.Id == idContaCorrenteOrigem).FirstOrDefaultAsync());
                var contacorrenteDestino = await Task.Run(() => data.ContaCorrente.Include(c => c.Agencia).Where(cc => cc.Id == idContaCorrenteDestino).FirstOrDefaultAsync());

                if (idContaCorrenteOrigem == 0 || contacorrenteOrigem == null)
                    throw new ApplicationException("Conta corrente de origem não localizada");

                if (idContaCorrenteDestino == 0 || contacorrenteDestino == null)
                    throw new ApplicationException("Conta corrente de destino não localizada");


                bool contaOrigemPossuiSaldoSuficiente = await Task.Run(() => ObterSaldo(idContaCorrenteOrigem)) < valor;
                if (contaOrigemPossuiSaldoSuficiente)
                    throw new ApplicationException("Conta de origem possui saldo insuficiente");

                ContaCorrenteLancamentos contaCorrenteLancamentosOrigem = new ContaCorrenteLancamentos
                {
                    ContaCorrente = contacorrenteOrigem,
                    DataCriacao = DateTime.Now,
                    DataLancamento = DateTime.Now,
                    TipoLancamento = Entities.Enums.TipoLancamentoEnum.Debito,
                    Valor = valor
                };
                await data.ContaCorrenteLancamentos.AddAsync(contaCorrenteLancamentosOrigem);

                ContaCorrenteLancamentos contaCorrenteLancamentosDestino = new ContaCorrenteLancamentos
                {
                    ContaCorrente = contacorrenteDestino,
                    DataCriacao = DateTime.Now,
                    DataLancamento = DateTime.Now,
                    TipoLancamento = Entities.Enums.TipoLancamentoEnum.Credito,
                    Valor = valor
                };
                await data.ContaCorrenteLancamentos.AddAsync(contaCorrenteLancamentosDestino);

                int id = await data.SaveChangesAsync();
                return id > 0;
            }
        }
    }
}
