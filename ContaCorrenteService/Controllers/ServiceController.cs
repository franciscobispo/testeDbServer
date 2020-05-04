using AutoMapper;
using ContaCorrenteService.Interfaces;
using ContaCorrenteService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContaCorrenteService.Controllers
{
    public class ServiceController : Controller
    {
        private readonly InterfaceContaCorrenteLancamentosApp _interfaceContaCorrenteLancamentosApp;
        private readonly InterfaceUsuarioApp _interfaceUsuario;
        private readonly InterfaceContaCorrenteApp _interfaceContaCorrenteApp;
        private IMapper _mapper;

        public ServiceController(IMapper mapper,
            InterfaceContaCorrenteLancamentosApp interfaceContaCorrenteLancamentosApp,
            InterfaceContaCorrenteApp interfaceContaCorrenteApp,
            InterfaceUsuarioApp interfaceUsuario
            )
        {   
            _interfaceContaCorrenteLancamentosApp = interfaceContaCorrenteLancamentosApp;
            _interfaceContaCorrenteApp = interfaceContaCorrenteApp;
            _interfaceUsuario = interfaceUsuario;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("obterSaldo")]
        [Authorize]
        public async Task<ActionResult<decimal>> ObterSaldo(int idContaCorrente)
        {
            try
            {
                var saldo = await Task.Run(() => _interfaceContaCorrenteLancamentosApp.ObterSaldo(idContaCorrente));
                return saldo;
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar obter saldo", null, 500, "ObterSaldo error");
            }
        }

        [HttpGet]
        [Route("obterExtrato")]
        [Authorize]
        public async Task<ActionResult<List<ContaCorrenteLancamentos>>> ObterExtrato(int idContaCorrente)
        {
            try
            {
                var contaCorrenteLancamentos = await Task.Run(() => _interfaceContaCorrenteLancamentosApp.ObterExtrato(idContaCorrente));

                var extrato = _mapper.Map<List<Entities.Entities.ContaCorrenteLancamentos>, List<Models.ContaCorrenteLancamentos>>(contaCorrenteLancamentos);

                return extrato;
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar obter saldo", null, 500, "ObterExtrato error");
            }
        }

        [HttpPost]
        [Route("inserirLancamento")]
        [Authorize]
        public async Task<ActionResult<bool>> InserirContaCorrenteLancamento(ContaCorrenteLancamentos paramContaCorrenteLancamento)
        {
            try
            {
                var contacorrente = await Task.Run(() => _interfaceContaCorrenteApp.GetEntityById(paramContaCorrenteLancamento.idContaCorrente));
                if (paramContaCorrenteLancamento.idContaCorrente == 0 || contacorrente == null)
                    return NotFound(new { message = "Conta corrente não localizada" });


                Entities.Entities.ContaCorrenteLancamentos newContaCorrenteLancamento = new Entities.Entities.ContaCorrenteLancamentos
                {
                    Valor = paramContaCorrenteLancamento.Valor,
                    DataCriacao = DateTime.Now,
                    DataLancamento = paramContaCorrenteLancamento.DataLancamento,
                    TipoLancamento = (Entities.Enums.TipoLancamentoEnum)paramContaCorrenteLancamento.TipoLancamento,
                    ContaCorrente = contacorrente
                };

                return await Task.Run(() => _interfaceContaCorrenteLancamentosApp.Inserir(newContaCorrenteLancamento));
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar obter saldo", null, 500, "ObterExtrato error");
            }
        }

        [HttpPost]
        [Route("inserirTransferencia")]
        [Authorize]
        public async Task<ActionResult<bool>> inserirTransferencia(int idContaCorrenteOrigem, int idContaCorrenteDestino, decimal valor)
        {
            try
            {
                try
                {
                    return await Task.Run(() => _interfaceContaCorrenteLancamentosApp.Transferir(idContaCorrenteOrigem, idContaCorrenteDestino, valor));
                }
                catch (ApplicationException ex)
                {
                    return Problem(ex.Message, null, 400);
                }
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar obter saldo", null, 500, "ObterExtrato error");
            }
        }
    }
}