using ClosedXML.Excel;
using Ks.MonitorCPI.Entidades;
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
using System.Xml;
using System.Xml.Serialization;

namespace Ks.MonitorCPI.Controllers
{
    public class TransactionlogController : Controller
    {
        public List<Row> Row { get; set; }

        public ActionResult Index()
        {
            ViewModelLogin viewModelLogin = new ViewModelLogin();

            if (Session.Keys.Count != 0)
            {
                viewModelLogin.usuario = Session[0].ToString();
                viewModelLogin.senha = Session[1].ToString();

                if (viewModelLogin.VerificaSeLoginExiste(viewModelLogin))
                {
                    return View(new ViewModelTransactionLog());
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

        [HttpPost]
        public ActionResult Transactionlog(ViewModelTransactionLog viewModelTransactionLog)
        {
            if (!string.IsNullOrEmpty(viewModelTransactionLog.tableid))
            {
                if (viewModelTransactionLog.tableid.Equals("Todos"))
                {
                    viewModelTransactionLog.tableid = "";
                }
                IntegradorCPI.IntegradorConsumoCPI CPI = new IntegradorCPI.IntegradorConsumoCPI();
                viewModelTransactionLog.ROOT = CPI.BuscarLogs(viewModelTransactionLog.status, viewModelTransactionLog.datefrom, viewModelTransactionLog.tableid, viewModelTransactionLog.hourFrom);

                TempData["row"] = viewModelTransactionLog.ROOT.Select_response.Row.OrderByDescending(f => f.PROC_DATE_RETORNO).ToList();


            }

            return View(viewModelTransactionLog);
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
                            wb.Worksheets.Add(odt, "ExportaExcelMonitorCPI");

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=ExportaExcelMonitorCPI" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
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


        public FileContentResult ExportarXml()
        {
            var xmlNovo = (List<Row>)TempData["row"];

            string[] novaMsg;

        

            foreach (var items in xmlNovo)
            {
                novaMsg = items.MESSAGE.ToString().Split('@');
                items.MESSAGE = string.Empty; 
                foreach (var item in novaMsg)
                {
                    
                    if (item.Contains("false"))
                    {
                        items.MESSAGE += item.ToString() + "\n";
                    }
                    else if(item.Contains("true"))
                    {

                    }
                    else
                    {
                        items.MESSAGE += item.ToString() + "\n";
                    }
                }
            }
          
            



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


        public ActionResult TransactionPayLoad(string data)
        {
            TempData["data"] = data;
            return Json(data);

        }

        [HttpGet]
        public FileResult Download()
        {
            string data = TempData["data"].ToString();

            IntegradorCPI.IntegradorConsumoCPI CPI = new IntegradorCPI.IntegradorConsumoCPI();

            XmlDocument xdoc = CPI.BuscarPayLoad(data.ToString());

            MemoryStream memoryStream = new MemoryStream();
            TextWriter tw = new StreamWriter(memoryStream);

            if (xdoc != null)
            {
                tw.WriteLine(xdoc.InnerXml);
                tw.Flush();
                tw.Close();
            }

            return File(memoryStream.GetBuffer(), "text/xml", data.ToString() + "_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xml");
        }
    }
}