using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaAoMundo.Models
{
    public class Fronteira
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Regiao")]
        public string idRegiao { get; set; }
        
        [Key]
        [Column(Order = 2)] 
        public string idFronteira { get; set; }

        public virtual Regiao Regiao { get; set; }
    }
}