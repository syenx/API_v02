using Ks.MonitorCPI.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Ks.MonitorCPI.Models
{
    public class ViewModelLogin
    {

        public string senha { get; set; }
        public string usuario { get; set; }


        internal bool VerificaSeLoginExiste(ViewModelLogin login)
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