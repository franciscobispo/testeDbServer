using System.Collections.Generic;

namespace Entities.Entities
{
    public class Usuario : Base
    {
        public string Username { get; set; }
        public string Nome { get; set; }        
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public virtual ICollection<ContaCorrente> ContaCorrentes { get; set; }
    }
}
