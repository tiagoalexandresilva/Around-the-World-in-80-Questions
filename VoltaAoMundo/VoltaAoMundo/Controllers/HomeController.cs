using VoltaAoMundo.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace VoltaAoMundo.Controllers
{
    public class HomeController : Controller
    {
        VoltaAoMundoDAL db;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(VoltaAoMundo.Models.Utilizador user)
        {
            if (ModelState.IsValid)
            { 
                if (IsValid(user.username, user.password))
                {
                    if (user.username == "admin")
                    {
                        FormsAuthentication.SetAuthCookie(user.username, false);
                        return RedirectToAction("Index", "Admin");
                    }            
                    else{
                        FormsAuthentication.SetAuthCookie(user.username, false);
                        return RedirectToAction("Index", "Main");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Login errado!!!");
                }
            }

            return View(user);
        }

        private bool IsValid(string userName, string passWord)
        {
            db = new VoltaAoMundoDAL();
            var crypto = new SimpleCrypto.PBKDF2();
            bool isValid = false;
            var user = db.Utilizadores.FirstOrDefault(u => u.username == userName);

            if (user != null)
            {
                if (user.password == crypto.Compute(passWord, user.passwordSalt))
                {
                    isValid = true;
                }
            }

            return isValid;
        }

    }
}