using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaAoMundo.Models
{
    public class RegiaoPercorrida
    {
        [Key]
        [Column(Order = 1)]
        [Required]
        [Display(Name = "Região inicial ")]
        [ForeignKey("Regiao")]
        public string idRegiao { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("Jogo")]
        public int jogoId { get; set; }

        public virtual Jogo Jogo { get; set; }
        public virtual Regiao Regiao { get; set; }
    }
}