using Entities.Entities;
using System.Collections.Generic;

namespace ContaCorrenteService.Models
{
    /// <summary>
    /// Classe utilizada para retorno do método de login, pois não precisamos retornar todos os dados de cliente e desta forma fica mais simples a manutenção
    /// </summary>
    public class UsuarioLogin 
    {
        public string Nome { get; set; }
        public string Token { get; set; }
        public List<ContaCorrente> ContasCorrentes { get; set; }
    }
}
