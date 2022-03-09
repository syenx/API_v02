using Ks.MonitorCPI.Entidades;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace Ks.MonitorCPI.IntegradorCPI
{
    public class IntegradorConsumoCPI
    {
        public bool producao = bool.Parse(WebConfigurationManager.AppSettings["MonitorCPI_Controle"].ToString());
        public ROOT BuscarLogs(string status, DateTime datefrom, string tableid, string hour)
        {
            string url = string.Empty;
            string apiKey = string.Empty;

            if (producao)
            {
                url = WebConfigurationManager.AppSettings["TransactionLogs_Prod"].ToString().Trim();
                apiKey = WebConfigurationManager.AppSettings["apiKey_Prod"].ToString().Trim();
            }
            else
            {
                url = WebConfigurationManager.AppSettings["TransactionLogs_HOM"].ToString().Trim();
                apiKey = WebConfigurationManager.AppSettings["apiKey_HOM"].ToString().Trim();

            }

            var HoraCompleta = hour.Split(':');



            Double hora = double.Parse(HoraCompleta[0].ToString());
            Double minuto = double.Parse(HoraCompleta[1].ToString());
            Double segundo = double.Parse(HoraCompleta[2].ToString());

            XmlDocument doc = new XmlDocument();
            ROOT root = new ROOT();
            //   string APIKey = @"PNSmJP8lIXv2yqLu50l0rGrOkbsvGTMn";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent("", Encoding.UTF8, "application/xml");
                content.Headers.Clear();

                content.Headers.Add("status", status);
                content.Headers.Add("datefrom", datefrom.AddHours(hora).AddMinutes(minuto).AddSeconds(segundo).ToString("yyyy-MM-ddTHH:mm:ss"));//"2020-01-01T09:24:30.806"


                if (!string.IsNullOrEmpty(tableid))
                {
                    content.Headers.Add("tableid", tableid);
                }

                content.Headers.Add("APIKey", apiKey);

                var result = httpClient.PostAsync(url, content).Result;
                string resultXml = result.Content.ReadAsStringAsync().Result.Remove(0, 54);

                if (!string.IsNullOrEmpty(resultXml))
                {
                    if (!resultXml.Contains("errorcode"))
                    {
                        doc.LoadXml(resultXml);
                        root = Deserialize<ROOT>(doc);
                    }

                   
                }
            }
            return root;
        }
        public XmlDocument BuscarLogsXml(string status, DateTime datefrom, string tableid, string hour)
        {
            string url,
                   apiKey = string.Empty;

            if (producao)
            {
                url = WebConfigurationManager.AppSettings["TransactionLogs_Prod"].ToString().Trim();
                apiKey = WebConfigurationManager.AppSettings["apiKey_Prod"].ToString().Trim();
            }
            else
            {
                url = WebConfigurationManager.AppSettings["TransactionLogs_HOM"].ToString().Trim();
                apiKey = WebConfigurationManager.AppSettings["apiKey_HOM"].ToString().Trim();

            }


            var HoraCompleta = hour.Split(':');

            Double hora = double.Parse(HoraCompleta[0].ToString());
            Double minuto = double.Parse(HoraCompleta[1].ToString());
            Double segundo = double.Parse(HoraCompleta[2].ToString());

            XmlDocument doc = new XmlDocument();
            ROOT root = new ROOT();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent("", Encoding.UTF8, "application/xml");
                content.Headers.Clear();

                content.Headers.Add("status", status);
                content.Headers.Add("datefrom", datefrom.AddHours(hora).AddMinutes(minuto).AddSeconds(segundo).ToString("yyyy-MM-ddTHH:mm:ss"));//"2020-01-01T09:24:30.806"

                if (!string.IsNullOrEmpty(tableid))
                {
                    content.Headers.Add("tableid", tableid);
                }
                content.Headers.Add("APIKey", apiKey);

                var result = httpClient.PostAsync(url, content).Result;
                string resultXml = result.Content.ReadAsStringAsync().Result.Remove(0, 54);

                if (!string.IsNullOrEmpty(resultXml))
                {
                    doc.LoadXml(resultXml);

                }
            }
            return doc;
        }
        internal XmlDocument BuscarPayLoad(string batchId)
        {
            string url,
                   apiKey = string.Empty;

            if (producao)
            {
                url = WebConfigurationManager.AppSettings["TransactionLogsPayload_Prod"].ToString().Trim();
                apiKey = WebConfigurationManager.AppSettings["apiKey_Prod"].ToString().Trim();
            }
            else
            {
                url = WebConfigurationManager.AppSettings["TransactionLogsPayload_HOM"].ToString().Trim();
                apiKey = WebConfigurationManager.AppSettings["apiKey_HOM"].ToString().Trim();

            }

            XmlDocument doc = new XmlDocument();
            ROOT root = new ROOT();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            using (var httpClient = new HttpClient())
            {


                var content = new StringContent("", Encoding.UTF8, "application/xml");
                content.Headers.Clear();

                content.Headers.Add("APIKey", apiKey);
                content.Headers.Add("batchid", batchId);
                content.Headers.Add("payload", "sent");

                var result = httpClient.PostAsync(url, content).Result;
                string resultXml = result.Content.ReadAsStringAsync().Result;


                if (!string.IsNullOrEmpty(resultXml))
                {
                    doc.LoadXml(resultXml);
                }
            }
            return doc;
        }
        internal Entidades.Produtos.ROOT BuscarPayLoadProdutos(List<string> codigosTotvs)
        {

            string url,
                   apiKey = string.Empty;

            string[] listaItens = codigosTotvs[0].Split(',');
            string codigoTotvs = string.Empty;

            foreach (var item in listaItens)
            {
                codigoTotvs += "'" + item + "'" + ",";

            }

            codigoTotvs = codigoTotvs.Remove(codigoTotvs.Length - 1);


            if (producao)
            {
                url = WebConfigurationManager.AppSettings["CheckProduct_Prod"].ToString().Trim();
                apiKey = WebConfigurationManager.AppSettings["apiKey_Prod"].ToString().Trim();
            }
            else
            {
                url = WebConfigurationManager.AppSettings["CheckProduct_HOM"].ToString().Trim();
                apiKey = WebConfigurationManager.AppSettings["apiKey_HOM"].ToString().Trim();

            }

            XmlDocument doc = new XmlDocument();
            Entidades.Produtos.ROOT root = new Entidades.Produtos.ROOT();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent("", Encoding.UTF8, "application/xml");
                content.Headers.Clear();

                content.Headers.Add("codigoTotvs", codigoTotvs);
                content.Headers.Add("APIKey", apiKey);

                var result = httpClient.PostAsync(url, content).Result;
                string resultXml = result.Content.ReadAsStringAsync().Result.Remove(0, 54);

                if (!string.IsNullOrEmpty(resultXml))
                {
                    doc.LoadXml(resultXml);
                    root = Deserialize<Entidades.Produtos.ROOT>(doc);
                }
            }
            return root;

        }


        internal Entidades.Pessoa.ROOT BuscarPessoa(string cnpj)
        {
            string url,
                   apiKey = string.Empty;


            if (producao)
            {
                url = WebConfigurationManager.AppSettings["CheckCustomer_Prod"].ToString().Trim();
                apiKey = WebConfigurationManager.AppSettings["apiKey_Prod"].ToString().Trim();
            }
            else
            {
                url = WebConfigurationManager.AppSettings["CheckCustomer_HOM"].ToString().Trim();
                apiKey = WebConfigurationManager.AppSettings["apiKey_HOM"].ToString().Trim();
            }

            XmlDocument doc = new XmlDocument();
            Entidades.Pessoa.ROOT root = new Entidades.Pessoa.ROOT();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent("", Encoding.UTF8, "application/xml");
                content.Headers.Clear();

                content.Headers.Add("cnpj", "'" + cnpj + "'");
                content.Headers.Add("APIKey", apiKey);

                var result = httpClient.PostAsync(url, content).Result;
                string resultXml = result.Content.ReadAsStringAsync().Result.Remove(0, 54);

                if (!string.IsNullOrEmpty(resultXml))
                {
                    doc.LoadXml(resultXml);
                    root = Deserialize<Entidades.Pessoa.ROOT>(doc);
                }
            }
            root.Select_response.Row = root.Select_response.Row.Distinct().ToList();

            return root;

        }

        internal void EnviaProduto()
        {

            string url,
                   apiKey = string.Empty;


            if (producao)
            {
                url = WebConfigurationManager.AppSettings["ProductAPI_Prod"].ToString().Trim();
                apiKey = WebConfigurationManager.AppSettings["apiKey_Prod"].ToString().Trim();
            }
            else
            {
                url = WebConfigurationManager.AppSettings["ProductAPI_HOM"].ToString().Trim();
                apiKey = WebConfigurationManager.AppSettings["apiKey_HOM"].ToString().Trim();
            }

            XmlDocument doc = new XmlDocument();
            Entidades.Pessoa.ROOT root = new Entidades.Pessoa.ROOT();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent("", Encoding.UTF8, "application/xml");
                content.Headers.Clear();

                content.Headers.Add("APIKey", apiKey);

               var result = httpClient.PostAsync(url, content).Result;
            }

        }


        public T Deserialize<T>(XmlDocument document) where T : class
        {
            XmlReader reader = new XmlNodeReader(document);
            var serializer = new XmlSerializer(typeof(T));
            T result = (T)serializer.Deserialize(reader);
            return result;
        }
    }

}
