using VoltaAoMundo.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoltaAoMundo.Models;
using System.Data.Entity;

namespace VoltaAoMundo.Controllers
{
    public class AdminController : Controller
    {
        VoltaAoMundoDAL db;
        //
        // GET: /Admin/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NovaPergunta()
        {
            db = new VoltaAoMundoDAL();
            ViewBag.LTemas = db.Temas.ToList();
            ViewBag.Regioes = db.Regioes.OrderBy(x => x.nomeRegiao).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult NovaPergunta(VoltaAoMundo.Models.Pergunta perg)
        {
            db = new VoltaAoMundoDAL();

            if (ModelState.IsValid)
            {      
                var sysPergunta = db.Perguntas.Create();

                sysPergunta.idRegiao = perg.idRegiao;
                sysPergunta.nTema = perg.nTema;
                sysPergunta.dificuldade = perg.dificuldade;
                sysPergunta.pergunta = perg.pergunta;
                sysPergunta.resposta = perg.resposta;
                sysPergunta.opcao1 = perg.opcao1;
                sysPergunta.opcao2 = perg.opcao2;
                sysPergunta.opcao3 = perg.opcao3;

                db.Perguntas.Add(sysPergunta);
                db.SaveChanges();

                return RedirectToAction("Index", "Admin");
              
            }

            return View(perg);
        }

        [HttpGet]
        public ActionResult NovoTema()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NovoTema(VoltaAoMundo.Models.Tema tema)
        {
            db = new VoltaAoMundoDAL();

            if (ModelState.IsValid)
            {
                
                if (db.Temas.Any(t => t.nome == tema.nome))
                {
                    ModelState.AddModelError("", "Tema já existe!!!");
                }
                else
                {
                    var sysTema = db.Temas.Create();

                    sysTema.nome = tema.nome;
                    sysTema.jogavel = true;

                    db.Temas.Add(sysTema);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Admin");
                }

            }
            return View(tema);
        }

        [HttpGet]
        public ActionResult RemoverPerguntas()
        {
              db = new VoltaAoMundoDAL();
              return View(db.Perguntas.ToList());
        }

        

        [HttpGet]
        public ActionResult Remover(int id)
        {
            db = new VoltaAoMundoDAL();
            Pergunta perg = db.Perguntas.Find(id);
            db.Perguntas.Remove(perg);
            db.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }
    

        [HttpGet]
        public ActionResult EditarPerguntas()
        {
             db = new VoltaAoMundoDAL();
             return View(db.Perguntas.ToList());
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            db = new VoltaAoMundoDAL();
            ViewBag.LTemas = db.Temas.ToList();
            ViewBag.Regioes = db.Regioes.OrderBy(x => x.nomeRegiao).ToList();
            Pergunta perg = db.Perguntas.Find(id);

            return View(perg);
        }

        [HttpPost]
        public ActionResult Editar(VoltaAoMundo.Models.Pergunta perg)
        {
            db = new VoltaAoMundoDAL();

            if (ModelState.IsValid)
            {
                db.Entry(perg).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Admin");
            }
            return View(perg);
        }
	}
}