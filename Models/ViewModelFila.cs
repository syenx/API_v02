using Ks.MonitorCPI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ks.MonitorCPI.Models
{
    public class ViewModelFila
    {

  

        public string[] listaDeTabelaId { get; set; }
        public string tabelaId { get; set; }
        public string nvc1 { get; set; }
        public string nvc3 { get; set; }
        public List<FilaEntidade> FilaEntidade { get; set; }


        string[] listaTabelaId = { "PEDIDO", "MEDICO", "CLIENTE" };
        public ViewModelFila()
        {
            FilaEntidade = new List<FilaEntidade>();

            listaDeTabelaId = listaTabelaId;
        }

    }
}