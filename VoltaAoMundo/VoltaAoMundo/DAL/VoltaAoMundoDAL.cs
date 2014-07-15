using VoltaAoMundo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace VoltaAoMundo.DAL
{
    public class VoltaAoMundoDAL : DbContext
    {
        public VoltaAoMundoDAL() : base("VoltaAoMundoDBContext")
        {
        }
        public DbSet<EstatisticaPessoal> EstatisticaPessoais { get; set; }
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<Pergunta> Perguntas { get; set; }
        public DbSet<Regiao> Regioes { get; set; }
        public DbSet<Tema> Temas { get; set; }
        public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<RegiaoPercorrida> RegiaoPercorridas { get; set; }
        public DbSet<Alinhamento> Alinhamentos { get; set; }
        public DbSet<Fronteira> Fronteiras { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        } 
    }
}