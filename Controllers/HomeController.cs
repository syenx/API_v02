using ClosedXML.Excel;
using Ks.MonitorCPI.Entidades;
using Ks.MonitorCPI.Entity;
using Ks.MonitorCPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using System.Xml.Serialization;

namespace Ks.MonitorCPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewModelLogin viewModelLogin = new ViewModelLogin();

            if (Session.Keys.Count != 0)
            {
                viewModelLogin.usuario = Session[0].ToString();
                viewModelLogin.senha = Session[1].ToString();

                if (VerificaSeLoginExiste(viewModelLogin))
                {

                    return View();
                }
                else
                {
                    Response.Redirect("~/Login/Logar");
                    return View("Login");
                }
            }
            else
            {
                Response.Redirect("~/Login/Logar");
                return View("Login");
            }
        }


        public ActionResult EnviaProdutosAPI()
        {
            ViewModelLogin viewModelLogin = new ViewModelLogin();

            if (Session.Keys.Count != 0)
            {
                viewModelLogin.usuario = Session[0].ToString();
                viewModelLogin.senha = Session[1].ToString();

                if (VerificaSeLoginExiste(viewModelLogin))
                {
                    IntegradorCPI.IntegradorConsumoCPI integrador = new IntegradorCPI.IntegradorConsumoCPI();
                    integrador.EnviaProduto();
                    return Json("Ok");
                }
                else
                {
                    Response.Redirect("~/Login/Logar");
                    return View("Login");
                }
            }
            else
            {
                Response.Redirect("~/Login/Logar");
                return View("Login");
            }
        }




        private bool VerificaSeLoginExiste(ViewModelLogin login)
        {
            try
            {

                DataTable oDt = new DataTable();

                Usuario oUsr = new Usuario();

                oUsr.usuarioLogin = login.usuario;
                oUsr.usuarioSenha = FormsAuthentication.HashPasswordForStoringInConfigFile(login.senha, "SHA1");

                oDt = oUsr.ListarLogin();
                if (oDt != null)
                {
                    if (oDt.Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }



    }
}