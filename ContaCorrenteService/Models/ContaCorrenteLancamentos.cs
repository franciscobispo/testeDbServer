using System;

namespace ContaCorrenteService.Models
{
    public class ContaCorrenteLancamentos
    {
        public decimal Valor { get; set; }
        public DateTime DataLancamento { get; set; }
        public int TipoLancamento { get; set; }        
        public int idContaCorrente { get; set; }
    }
}
