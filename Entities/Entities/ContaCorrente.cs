using System.Collections.Generic;

namespace Entities.Entities
{
    public class ContaCorrente : Base
    {
        public string Digito { get; set; }
        public virtual Agencia Agencia { get; set; }
    }
}
