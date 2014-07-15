using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaAoMundo.Models
{
    public class EstatisticaPessoal
    {
        [Key]
        public int idEstatistica { get; set; }

        [ForeignKey("Utilizador")]
        public string username { get; set; }
        public int numTotalJogos { get; set; }
        public int numTotalPontos { get; set; }
        public int recordPessoal { get; set; }
        public int respostasDadas { get; set; }
        public int respostasAcertadas { get; set; }
        public int pontosj { get; set; }
    
        public virtual Utilizador Utilizador { get; set; }
    }
}