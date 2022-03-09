using ClosedXML.Excel;
using Ks.MonitorCPI.Entidades.Produtos;
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
    public class ProdutosController : Controller
    {




        // GET: Produtos
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

        public ActionResult ConsultarProdutos(ViewModelProdutos produtos)
        {


            IntegradorCPI.IntegradorConsumoCPI integradorConsumoCPI = new IntegradorCPI.IntegradorConsumoCPI();

            produtos.ROOT = integradorConsumoCPI.BuscarPayLoadProdutos(produtos.codigoTotvs);
            TempData["row"] = produtos;
            return View(produtos);
        }


        public FileContentResult ExportarXml()
        {
            var xmlNovo = (ViewModelProdutos)TempData["row"];

            XmlSerializer xsSubmit = new XmlSerializer(typeof(List<Row>));
            var subReq = xmlNovo.ROOT.Select_response.Row;
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
            Row = (List<Row>)TempData["row"];
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