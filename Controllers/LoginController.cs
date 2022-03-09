using Ks.MonitorCPI.Entity;
using Ks.MonitorCPI.Models;
using KS.SimuladorPrecos.DataEntities.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Ks.MonitorCPI.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Logar()
        {
            Session.Remove("login");
            Session.Remove("senha");
            return View(new ViewModelLogin());
        }


        public void ProcessRequest(HttpContext context)
        {
            context.Session.Abandon();
            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
            System.Web.Security.FormsAuthentication.SignOut();
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        [HttpPost]
        public ActionResult Logar(ViewModelLogin login)
        {
            string returnUrl = "~/home/index";
            if (ModelState.IsValid)
            {

                Usuario vLogin = EfetuaLogin(login);

                if (vLogin.usuarioSenha != null)
                {
                    /*Código abaixo verifica se a senha digitada no site é igual a
                    senha que está sendo retornada
                     do banco. Caso não cai direto no else*/
                    if (Equals(vLogin.usuarioSenha, login.senha))
                    {
                        FormsAuthentication.SetAuthCookie(vLogin.usuarioLogin, false);
                        if (Url.IsLocalUrl(returnUrl)
                        && returnUrl.Length > 1
                        && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//")
                        && returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        /*código abaixo cria uma session para armazenar o nome do usuário*/
                        Session["login"] = vLogin.usuarioLogin;
                        /*código abaixo cria uma session para armazenar o sobrenome do usuário*/
                        Session["senha"] = vLogin.usuarioSenha;
                        /*retorna para a tela inicial do Home*/


                        return RedirectToAction("Index", "Home");


                    }
                    /*Else responsável da validação da senha*/
                    else
                    {
                        /*Escreve na tela a mensagem de erro informada*/
                        ModelState.AddModelError("", "Senha informada Inválida!!!");
                        /*Retorna a tela de login*/
                        return View();
                    }
                }
                /*Else responsável por verificar se o usuário está ativo*/
                else
                {
                    /*Escreve na tela a mensagem de erro informada*/
                    ModelState.AddModelError("", "Usuário sem acesso para usar o sistema!!!");
                    /*Retorna a tela de login*/
                    return View();
                }


            }
            /*Caso os campos não esteja de acordo com a solicitação retorna a tela de login
            com as mensagem dos campos*/
            return View(login);
        }


        private Usuario EfetuaLogin(ViewModelLogin login)
        {
            try
            {
                DataTable oDt = new DataTable();
                AcessoCargaSimulador acessoCargaSumulador = new AcessoCargaSimulador();
                Usuario oUsr = new Usuario();

                oUsr.usuarioLogin = login.usuario;
                oUsr.usuarioSenha = FormsAuthentication.HashPasswordForStoringInConfigFile(login.senha, "SHA1");

                oDt = oUsr.ListarLogin();
                if (oDt != null)
                {
                    if (oDt.Rows.Count > 0)
                    {
                        return new Usuario()
                        {
                            usuarioLogin = oDt.Rows[0]["usuarioLogin"].ToString(),
                            usuarioSenha = login.senha
                        };
                    }
                    return new Usuario();

                }
                return new Usuario();
            }
            catch
            {
                return new Usuario();
            }
        }



    }
}