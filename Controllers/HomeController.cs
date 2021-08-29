using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace LoginUsuario.Controllers
{

    public class HomeController : Controller
    {

        string JSONresult;

        public ActionResult Login()
        {
            ViewBag.Alerta = string.Empty;

            return View();
        }

        public ActionResult Cadastro()
        {

            return View();
        }

        public ActionResult ResetSenha()
        {
            return View();
        }

        public JsonResult CadastraNovoUsuario(string nome, string email, string login, string senha)
        {
            List<dynamic> novoUsuario = Models.Login.CadastraNovoUsuario(nome, email, login, senha);
            JSONresult = JsonConvert.SerializeObject(novoUsuario);
            return Json(JSONresult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AlterarSenhaUsuario(string login, string senha)
        {
            List<dynamic> novaSenha = Models.Login.AlterarSenhaUsuario(login, senha);
            JSONresult = JsonConvert.SerializeObject(novaSenha);
            return Json(JSONresult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Login(Models.LoginParameters loginParameters)
        {

            Models.LoginParameters parameters = new Models.LoginParameters();
            DataTable dt = Models.Login.RetornaLogin(loginParameters.Login, loginParameters.Senha);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                parameters.Login = Convert.ToString(dr["Login"]);
                parameters.Senha = Convert.ToString(dr["Senha"]);

                return RedirectToAction("Index", "Home");

            }
            else
            {

                ViewBag.Alerta = "Login Inválido";

            }


            return View();

        }

        public ActionResult Index()
        {
            return View();
        }


    }
}