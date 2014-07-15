using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoltaAoMundo.Models
{
    public class Regiao
    {
        [Key]
        public string id { get; set; }
        public string nomeRegiao { get; set; }
        public string latLng { get; set; }
        public string icon { get; set; }
        public string icon2 { get; set; }

        public virtual ICollection<Alinhamento> Alinhamentos { get; set; }
        public virtual ICollection<Fronteira> Fronteiras { get; set; }
        public virtual ICollection<Pergunta> Perguntas { get; set; }
        public virtual ICollection<RegiaoPercorrida> RegiaoPercorridas { get; set; }
        public virtual ICollection<Jogo> Jogos { get; set; }

        
    }
}