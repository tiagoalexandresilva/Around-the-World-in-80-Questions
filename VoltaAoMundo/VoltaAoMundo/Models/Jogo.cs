using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaAoMundo.Models
{
    public class Jogo
    {
        [Key]
        public int idJogo { get; set; }

        [ForeignKey("Utilizador")]
        public string username { get; set; }

        [ForeignKey("Tema")]
        [Required]
        [Display(Name = "Tema ")]
        public string nomeTema { get; set; }

        [ForeignKey("Regiao")]
        [Required]
        [Display(Name = "Região Inicial ")]
        public string regiaoAtual { get; set; }

        [Required]
        [Display(Name = "Dificuldade ")]
        public int dificuldade { get; set; }
        public int respostasDadas { get; set; }
        public int respostasAcertadas { get; set; }
        public int numPontos { get; set; }
        public bool fim { get; set; }
        public string regiaoInicial { get; set; }


        public virtual Tema Tema { get; set; }
        public virtual Utilizador Utilizador { get; set; }
        public virtual ICollection<RegiaoPercorrida> RegiaoPercorridas { get; set; }
        public virtual Regiao Regiao { get; set; }
    }
}