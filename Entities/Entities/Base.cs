using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Entities.Entities
{
    public class Base
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), DataMember]
        public DateTime DataCriacao { get; set; }
    }
}
