using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoltaAoMundo.DAL;
using VoltaAoMundo.Models;
using System.Security.Cryptography;

namespace VoltaAoMundo.Controllers
{
    public class MainController : Controller
    {    
        VoltaAoMundoDAL db;

        // GET: /Main/
        [HttpGet]
        public ActionResult Index()
        {
            db = new VoltaAoMundoDAL();
            ViewBag.LTemas = db.Temas.ToList();
            ViewBag.Dificuldades = db.Perguntas.Select(x => x.dificuldade).Distinct().ToList();
            ViewBag.Regioes = db.Regioes.OrderBy(x => x.nomeRegiao).ToList();

            return View();
    }

        [HttpPost]
        public ActionResult Index(Jogo jg)
        {
            db = new VoltaAoMundoDAL();

            if (ModelState.IsValid)
            {
                var jogo = db.Jogos.Where(j => j.username == User.Identity.Name).FirstOrDefault();

                if (jogo != null)
                {
                    //actualizar a informação do novo jogo
                    db.Jogos.Single(j => j.username == User.Identity.Name).dificuldade = jg.dificuldade;
                    db.Jogos.Single(j => j.username == User.Identity.Name).regiaoAtual = jg.regiaoAtual;
                    db.Jogos.Single(j => j.username == User.Identity.Name).respostasAcertadas = 0;
                    db.Jogos.Single(j => j.username == User.Identity.Name).respostasDadas = 0;
                    db.Jogos.Single(j => j.username == User.Identity.Name).nomeTema = jg.nomeTema;
                    db.Jogos.Single(j => j.username == User.Identity.Name).numPontos = 0;
                    db.Jogos.Single(j => j.username == User.Identity.Name).fim = false;
                    db.Jogos.Single(j => j.username == User.Identity.Name).regiaoInicial = jg.regiaoAtual;

                    //Limpar a lista de regiões percorridas ao iniciar o jogo
                    var regs = db.RegiaoPercorridas.Where(r => r.jogoId == jogo.idJogo);

                    foreach (var r in regs)
                    {
                        db.RegiaoPercorridas.Remove(r);
                    }

                    db.SaveChanges();
                }
                else
                {
                    var sysJogo = db.Jogos.Create();

                    sysJogo.dificuldade = jg.dificuldade;
                    sysJogo.regiaoAtual = jg.regiaoAtual;
                    sysJogo.respostasAcertadas = 0;
                    sysJogo.respostasDadas = 0;
                    sysJogo.nomeTema = jg.nomeTema;
                    sysJogo.numPontos = 0;
                    sysJogo.username = User.Identity.Name;
                    sysJogo.fim = false;
                    sysJogo.regiaoInicial = jg.regiaoAtual;

                    db.Jogos.Add(sysJogo);
                    db.SaveChanges();
                }

                return RedirectToAction("Mapa", "Main");
                
            }
            return View(jg);
        }

        [HttpGet]
        public ActionResult Mapa()
        {
            db = new VoltaAoMundoDAL(); 
            ViewBag.Jogo = db.Jogos.Where(j => j.username == User.Identity.Name).FirstOrDefault();

            return View();
        }

        [HttpGet]
        public ActionResult Pergunta(string idregiao)
        {
            db = new VoltaAoMundoDAL();
            var flag = 0; 
            var game = db.Jogos.FirstOrDefault(u => u.username == User.Identity.Name); 
            var sysRegPer = db.RegiaoPercorridas.Create(); 
            var sysRegiPer = db.RegiaoPercorridas.Where(r => r.jogoId == game.idJogo).ToList();

            db.Jogos.Single(j => j.username == User.Identity.Name).regiaoAtual = idregiao;

            if (game.fim == false && sysRegiPer.Count > 0)
            {
                var alin = db.Alinhamentos.Where(r => r.idRegiao == game.regiaoInicial);

                foreach (var element in alin) 
                { 
                    if (element.idRegAlin == idregiao) 
                        flag = 1; 
                } 
                if (flag == 0) 
                { 
                    db.Jogos.Single(j => j.username == User.Identity.Name).fim = true; 
                }
            }

            sysRegPer.idRegiao = idregiao; 
            sysRegPer.jogoId = game.idJogo;

            db.RegiaoPercorridas.Add(sysRegPer); 
            db.SaveChanges();

            return View();
        }

        public JsonResult devolvePerguntas()
        {
            db = new VoltaAoMundoDAL();
            
            var game = db.Jogos.FirstOrDefault(u => u.username == User.Identity.Name);
            var perguntas = db.Perguntas.Where( p => p.dificuldade == game.dificuldade && p.nTema == game.nomeTema && p.idRegiao == game.regiaoAtual).ToList();
            var viewModel = perguntas.Select(x => new
            {
                idPergunta = x.idPergunta,
                idRegiao = x.idRegiao,
                nTema = x.nTema,
                pergunta = x.pergunta,
                resposta = x.resposta,
                dificuldade = x.dificuldade,
                opcao1 = x.opcao1,
                opcao2 = x.opcao2,
                opcao3 = x.opcao3
            });

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult devolveEstPessoal()
        {
            db = new VoltaAoMundoDAL();
            var estPessoal = db.EstatisticaPessoais.Where(e => e.username == User.Identity.Name);
            var viewModel = estPessoal.Select(x => new
            {
                username = x.username,
                numTotalJogos = x.numTotalJogos,
                numTotalPontos = x.numTotalPontos,
                recordPessoal = x.recordPessoal,
                respostasDadas = x.respostasDadas,
                respostasAcertadas = x.respostasAcertadas,
                pontosj = x.pontosj
            });
            var estPessoalSend = viewModel.FirstOrDefault(e => e.username == User.Identity.Name);

            return Json(estPessoalSend, JsonRequestBehavior.AllowGet);
        }

        public JsonResult devolveRegioes()
        {
            db = new VoltaAoMundoDAL();
            var regioes = db.Regioes.ToList();   
            var viewModel = regioes.Select(x => new { 
                id = x.id, 
                nomeRegiao = x.nomeRegiao, 
                latLng = x.latLng, 
                icon = x.icon, 
                icon2 = x.icon2 
            });

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult devolveRegioesPerc()
        {
            db = new VoltaAoMundoDAL();
            var jogoact = db.Jogos.Where(j => j.username == User.Identity.Name).FirstOrDefault();
            var regioesPerc = db.RegiaoPercorridas.ToList();
            var viewModel = regioesPerc.Select(x => new
            {
                idRegiao = x.idRegiao,
                jogoId = x.jogoId
            });

            var regPerc = viewModel.Where(j => j.jogoId == jogoact.idJogo);

            return Json(regPerc, JsonRequestBehavior.AllowGet);
        }

        public JsonResult devolveJogo()
        {
            db = new VoltaAoMundoDAL();
            var jogos = db.Jogos.ToList();
            var viewModel = jogos.Select(x => new
            {
                idJogo = x.idJogo,
                username = x.username,
                nomeTema = x.nomeTema,
                regiaoAtual = x.regiaoAtual,
                dificuldade = x.dificuldade,
                respostasDadas = x.respostasDadas,
                respostasAcertadas = x.respostasAcertadas,
                numPontos = x.numPontos,
                regiaoInicial = x.regiaoInicial,
                fim = x.fim
            });
            var jogoact = viewModel.Where(j => j.username == User.Identity.Name).FirstOrDefault();

            return Json(jogoact, JsonRequestBehavior.AllowGet);
        }

        public JsonResult devolveFronteiras()
        {
            db = new VoltaAoMundoDAL();
            var jogoact = db.Jogos.Where(j => j.username == User.Identity.Name).FirstOrDefault();
            var fronts = db.Fronteiras.ToList();
            var viewModel = fronts.Select(x => new
            {
                idRegiao = x.idRegiao,
                idFronteira = x.idFronteira
            });
            var fronteiras = viewModel.Where(j => j.idRegiao == jogoact.regiaoAtual);

            return Json(fronteiras, JsonRequestBehavior.AllowGet);
        }

        public JsonResult devolveAlinhamentos()
        {
            db = new VoltaAoMundoDAL();
            var alins = db.Alinhamentos;

            var viewModel = alins.Select(x => new
            {
                idRegiao = x.idRegiao,
                idRegAlin = x.idRegAlin
            });

            return Json(viewModel.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult devolveEstatisticaOrdByRecord()
        {
            db = new VoltaAoMundoDAL();
            var records = db.EstatisticaPessoais.ToList().OrderBy(r => r.recordPessoal).Reverse().Take(10);
            var viewModel = records.Select(x => new
            {
                username = x.username,
                recordPessoal = x.recordPessoal
            });

                return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult devolveEstatisticaOrdByTotal()
        {
            db = new VoltaAoMundoDAL();
            var records = db.EstatisticaPessoais.ToList().OrderBy(r => r.numTotalPontos).Reverse().Take(10);
            var viewModel = records.Select(x => new
            {
                username = x.username,
                numTotalPontos = x.numTotalPontos,
                numTotalJogos = x.numTotalJogos,
            });

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public void actualizaJogo(int pontos, int certas)
        {
            db = new VoltaAoMundoDAL();
            var jogoact = db.Jogos.Where(j => j.username == User.Identity.Name).FirstOrDefault();
            var respDadas = jogoact.respostasDadas + 3;
            var respCertas = jogoact.respostasAcertadas + certas;
            var totalPontos = jogoact.numPontos + pontos;

            db.Jogos.Single(u => u.username == User.Identity.Name).numPontos = totalPontos;
            db.Jogos.Single(u => u.username == User.Identity.Name).respostasAcertadas = respCertas;
            db.Jogos.Single(u => u.username == User.Identity.Name).respostasDadas = respDadas;

            db.SaveChanges();
            checkDificuldade();
        }

        public void checkDificuldade() {
            db = new VoltaAoMundoDAL();
            var jogoact = db.Jogos.Where(j => j.username == User.Identity.Name).FirstOrDefault();
            var valor = ((double)jogoact.respostasAcertadas / (double)jogoact.respostasDadas);

            if (valor > 0.8 && jogoact.dificuldade < 3)
            {
                db.Jogos.Single(u => u.username == User.Identity.Name).dificuldade += 1;
                db.SaveChanges();
            }
            else if (valor < 0.6 && jogoact.dificuldade > 1)
            {
                db.Jogos.Single(u => u.username == User.Identity.Name).dificuldade -= 1;
                db.SaveChanges();
            }
        }

        public ActionResult MapaFim() 
        {
            db = new VoltaAoMundoDAL();
            ViewBag.Jogo = db.Jogos.Where(j => j.username == User.Identity.Name).FirstOrDefault();

            return View(); 
        }

        public ActionResult FinalJogo() 
        { 
            db = new VoltaAoMundoDAL(); 
            ViewBag.Jogo = db.Jogos.Where(j => j.username == User.Identity.Name).FirstOrDefault();

            return View(); 
        }


        public ActionResult FinalizaJogo()
        {
            db = new VoltaAoMundoDAL();

            var jogoact = db.Jogos.Where(j => j.username == User.Identity.Name).FirstOrDefault();
            var estpessoal = db.EstatisticaPessoais.Where(j => j.username == User.Identity.Name).FirstOrDefault();

            var respDadas = jogoact.respostasDadas;
            var respCertas = jogoact.respostasAcertadas;
            var totalPontos = jogoact.numPontos;

            var numTJogos = estpessoal.numTotalJogos + 1;
            var numTPontos = estpessoal.numTotalPontos + totalPontos;
            var recPessoal = estpessoal.recordPessoal;
            var respTDadas = estpessoal.respostasDadas + respDadas;
            var respTCertas = estpessoal.respostasAcertadas + respCertas;
            var pontosUltJ = totalPontos;

            db.EstatisticaPessoais.Single(u => u.username == User.Identity.Name).numTotalJogos = numTJogos;
            db.EstatisticaPessoais.Single(u => u.username == User.Identity.Name).numTotalPontos = numTPontos;
            if (totalPontos > recPessoal)
            {
                db.EstatisticaPessoais.Single(u => u.username == User.Identity.Name).recordPessoal = totalPontos;
            }
            db.EstatisticaPessoais.Single(u => u.username == User.Identity.Name).respostasDadas = respTDadas;
            db.EstatisticaPessoais.Single(u => u.username == User.Identity.Name).respostasAcertadas = respTCertas;
            db.EstatisticaPessoais.Single(u => u.username == User.Identity.Name).pontosj = pontosUltJ;

            db.SaveChanges();

            return RedirectToAction("FinalJogo", "Main");
        }


        public ActionResult PerguntaFinal(string idregiao)
        {
            db = new VoltaAoMundoDAL();
            var flag = 0;
            var game = db.Jogos.FirstOrDefault(u => u.username == User.Identity.Name);
            var sysRegPer = db.RegiaoPercorridas.Create();
            var sysRegiPer = db.RegiaoPercorridas.Where(r => r.jogoId == game.idJogo).ToList();

            db.Jogos.Single(j => j.username == User.Identity.Name).regiaoAtual = idregiao;

            if (game.fim == false && sysRegiPer.Count > 0)
            {
                var alin = db.Alinhamentos.Where(r => r.idRegiao == game.regiaoInicial);

                foreach (var element in alin)
                {
                    if (element.idRegAlin == idregiao)
                        flag = 1;
                }
                if (flag == 0)
                {
                    db.Jogos.Single(j => j.username == User.Identity.Name).fim = true;
                }
            }

            sysRegPer.idRegiao = idregiao;
            sysRegPer.jogoId = game.idJogo;

            db.RegiaoPercorridas.Add(sysRegPer);
            db.SaveChanges();

            return View();
        }
	}
}