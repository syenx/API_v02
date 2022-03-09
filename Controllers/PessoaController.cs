using ClosedXML.Excel;
using Ks.MonitorCPI.Entidades.Pessoa;
using Ks.MonitorCPI.Entity;
using Ks.MonitorCPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace Ks.MonitorCPI.Controllers
{
    public class PessoaController : Controller
    {
        // GET: Pessoa
        public ActionResult Index()
        {
            ViewModelLogin viewModelLogin = new ViewModelLogin();

            if (Session.Keys.Count != 0)
            {
                viewModelLogin.usuario = Session[0].ToString();
                viewModelLogin.senha = Session[1].ToString();

                if (viewModelLogin.VerificaSeLoginExiste(viewModelLogin))
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


        public List<Row> Row { get; set; }

        public ActionResult ConsultarPessoa(ViewModelPessoa pessoa)
        {
            if (pessoa.cnpj != null)
            {

                pessoa.ROOT = new ROOT();
                pessoa.ROOT.Select_response = new Select_response();
                pessoa.ROOT.Select_response.Row = new List<Row>();
                foreach (var item in pessoa.cnpj.Split(','))
                {

                    if (!string.IsNullOrEmpty(item.Trim()))
                    {
                        IntegradorCPI.IntegradorConsumoCPI integradorConsumoCPI = new IntegradorCPI.IntegradorConsumoCPI();
                        var root = integradorConsumoCPI.BuscarPessoa(item);

                        if (root.Select_response.Row.Count >= 1)
                        {
                            foreach (var itemLS in root.Select_response.Row)
                            {
                                pessoa.ROOT.Select_response.Row.Add(itemLS);
                            }

                        }
                    }
                }

                TempData["fila"] = pessoa.ROOT.Select_response.Row;
                return View(pessoa);
            }
            else
            {
                return View("Index");
            }


        }


        public FileContentResult ExportarXml()
        {
            var xmlNovo = (List<Row>)TempData["fila"];

            XmlSerializer xsSubmit = new XmlSerializer(typeof(List<Row>));
            var subReq = xmlNovo;
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, subReq);
                    xml = sww.ToString(); // Your XML
                }
            }


            XmlDocument xdoc = new XmlDocument();

            xdoc.LoadXml(xml);

            MemoryStream stream = new MemoryStream();

            StreamWriter objstreamwriter = new StreamWriter(stream);
            objstreamwriter.Write(xdoc.InnerXml);
            objstreamwriter.Flush();
            objstreamwriter.Close();
            return File(stream.GetBuffer(), "text/xml", "arquivo" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xml");
        }


        public ActionResult ExportarExcel()
        {
            Row = (List<Row>)TempData["fila"];
            if (Row != null)
            {
                if (Row.Count() != 0)
                {
                    DataTable odt = ConvertToDataTable(Row);



                    if (odt != null && odt.Rows.Count > 0)
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(odt, "ExportaExcelMonitorCPIProdutos");

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=ExportaExcelMonitorCPIProdutos" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);
                                Response.Flush();
                                Response.End();
                            }
                        }
                    }
                }
            }

            return View();
        }
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            try
            {
                PropertyDescriptorCollection properties =
             TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
                return table;
            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}