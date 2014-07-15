using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaAoMundo.Models
{
    public class Alinhamento
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Regiao")]
        public string idRegiao { get; set; }
        
        [Key]
        [Column(Order = 2)]
        public string idRegAlin { get; set; }

        public virtual Regiao Regiao { get; set; }
    }
}