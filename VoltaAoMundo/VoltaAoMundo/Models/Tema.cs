using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoltaAoMundo.Models
{
    public class Tema
    {
        [Key]
        [Required]
        [Display(Name = "Tema ")]
        public string nome { get; set; }
        public bool jogavel { get; set; }

        public virtual ICollection<Jogo> Jogos { get; set; }
        public virtual ICollection<Pergunta> Perguntas { get; set; }
    }
}