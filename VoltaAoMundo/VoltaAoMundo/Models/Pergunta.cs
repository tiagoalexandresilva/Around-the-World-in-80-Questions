using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaAoMundo.Models
{
    public class Pergunta
    {
        [Key]
        public int idPergunta { get; set; }

        [ForeignKey("Regiao")]
        [Required]
        [Display(Name = "Região ")]
        public string idRegiao { get; set; }

        [ForeignKey("Tema")]
        [Required]
        [Display(Name = "Tema ")]
        public string nTema { get; set; }

        [Required]
        [Display(Name = "Pergunta ")]
        public string pergunta { get; set; }

        [Required]
        [Display(Name = "Opção Correcta ")]
        public string resposta { get; set; }

        [Required]
        [Display(Name = "Dificuldade ")]
        public int dificuldade { get; set; }

        [Required]
        [Display(Name = "Opção 1 ")]
        public string opcao1 { get; set; }

        [Required]
        [Display(Name = "Opção 2 ")]
        public string opcao2 { get; set; }

        [Required]
        [Display(Name = "Opção 3 ")]
        public string opcao3 { get; set; }
    
        public virtual Regiao Regiao { get; set; }
        public virtual Tema Tema { get; set; }
    }
}