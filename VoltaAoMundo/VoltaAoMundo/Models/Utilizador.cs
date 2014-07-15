using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace VoltaAoMundo.Models
{
    public class Utilizador
    {
        [Key]
        [Required]
        [Display(Name = "Username ")]
        public string username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password ")]
        public string password { get; set; }
        public bool admin { get; set; }
        public string passwordSalt { get; set; }

        public virtual ICollection<EstatisticaPessoal> EstatisticaPessoal { get; set; }
        public virtual ICollection<Jogo> Jogo { get; set; }

    }
}