using Entities.Enums;
using System;

namespace Entities.Entities
{
    public class ContaCorrenteLancamentos : Base
    {
        public decimal Valor { get; set; }
        public DateTime DataLancamento { get; set; }
        public TipoLancamentoEnum TipoLancamento { get; set; }
        public ContaCorrente ContaCorrente { get; set; }
    }
}
