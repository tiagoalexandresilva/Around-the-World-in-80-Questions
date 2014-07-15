using VoltaAoMundo.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace VoltaAoMundo.Controllers
{
    public class UserController : Controller
    {
        VoltaAoMundoDAL db;

        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Registration(VoltaAoMundo.Models.Utilizador user)
        {
            db = new VoltaAoMundoDAL();

            if (ModelState.IsValid)
            {
                //se já existe o utilizador não insere
                if(db.Utilizadores.Any(u => u.username == user.username))
                {
                    ModelState.AddModelError("", "Username em uso!!!");
                }
                else
                {
                    var crypto = new SimpleCrypto.PBKDF2();
                    var encrpPass = crypto.Compute(user.password);
                    var sysUser = db.Utilizadores.Create();
                    var sysEstP = db.EstatisticaPessoais.Create();

                    sysEstP.numTotalJogos = 0;
                    sysEstP.numTotalPontos = 0;
                    sysEstP.recordPessoal = 0;
                    sysEstP.respostasAcertadas = 0;
                    sysEstP.respostasDadas = 0;
                    sysEstP.username = user.username;
                    sysEstP.pontosj = 0;

                    sysUser.username = user.username;
                    sysUser.password = encrpPass;
                    sysUser.passwordSalt = crypto.Salt;
                    sysUser.admin = false;

                    db.EstatisticaPessoais.Add(sysEstP);
                    db.Utilizadores.Add(sysUser);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult AlterarPassword()
        {
            db = new VoltaAoMundoDAL();

            return View();
        }

        [HttpPost]
        public ActionResult AlterarPassword(VoltaAoMundo.Models.Utilizador user)
        {
             db = new VoltaAoMundoDAL();

             ModelState.Remove("username");

             if (ModelState.IsValid)
             {
                 var crypto = new SimpleCrypto.PBKDF2();
                 var encrpPass = crypto.Compute(user.password);

                 db.Utilizadores.Single(u => u.username == User.Identity.Name).password = encrpPass;
                 db.Utilizadores.Single(u => u.username == User.Identity.Name).passwordSalt = crypto.Salt;
                 db.SaveChanges();

                 return RedirectToAction("Mapa", "Main");
             }

            return View(user);
        }

	}
}