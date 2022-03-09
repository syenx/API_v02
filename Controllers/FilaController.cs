using ClosedXML.Excel;
using Ks.MonitorCPI.Entity;
using Ks.MonitorCPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace Ks.MonitorCPI.Controllers
{
    public class FilaController : Controller
    {

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



        #region CONSULTAS
        public ActionResult ConsultarFilaCliente(ViewModelFila fila)
        {
            if (!string.IsNullOrEmpty(fila.nvc1))
            {
                fila.tabelaId = "CLIENTE";
                FilaEntidade filaEntidade = new FilaEntidade();

                foreach (var item in fila.nvc1.Split(','))
                {
                    fila.FilaEntidade.Add(ConverDataTableToList(filaEntidade.Listar()).Where(f => f.nvc1.Trim() == item.Trim() && f.tabelaId.ToUpper().Trim() == fila.tabelaId.ToUpper().Trim()).FirstOrDefault());
                    TempData["fila"] = fila;
                }
                return View(fila);
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult ConsultarFilaPedido(ViewModelFila fila)
        {
            if (!string.IsNullOrEmpty(fila.nvc1))
            {
                fila.tabelaId = "PEDIDO";
                FilaEntidade filaEntidade = new FilaEntidade();
                foreach (var item in fila.nvc1.Split(','))
                {
                    fila.FilaEntidade.Add(ConverDataTableToList(filaEntidade.Listar()).Where(f => f.nvc1.Trim() == item.Trim() && f.tabelaId.ToUpper().Trim() == fila.tabelaId.ToUpper().Trim()).FirstOrDefault());
                    TempData["fila"] = fila;
                }
                return View(fila);
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult ConsultarFilaMedico(ViewModelFila fila)
        {
            if (fila.nvc1 != null)
            {
                fila.tabelaId = "MEDICO";
                FilaEntidade filaEntidade = new FilaEntidade();
                foreach (var item in fila.nvc1.Split(','))
                {
                    fila.FilaEntidade.Add(ConverDataTableToList(filaEntidade.Listar()).Where(f => f.nvc1.Trim() == item.Trim() && f.tabelaId.ToUpper().Trim() == fila.tabelaId.ToUpper().Trim()).FirstOrDefault());
                    TempData["fila"] = fila;
                }
                return View(fila);
            }
            else
            {
                return View("Index");
            }
        }

        #endregion



        private List<FilaEntidade> ConverDataTableToList(DataTable dt)
        {
            var convertedList = (from rw in dt.AsEnumerable()
                                 select new FilaEntidade()
                                 {
                                     nvc1 = Convert.ToString(rw["nvc1"]),
                                     nvc2 = Convert.ToString(rw["nvc2"]),
                                     nvc3 = Convert.ToString(rw["nvc3"]),
                                     tabelaId = Convert.ToString(rw["tabelaId"])
                                 }).ToList();
            return convertedList;
        }

        public FileContentResult ExportarXml()
        {
            var xmlNovo = (ViewModelFila)TempData["fila"];

            XmlSerializer xsSubmit = new XmlSerializer(typeof(FilaEntidade));

            var subReq = ConvertToDataTable(xmlNovo.FilaEntidade);

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

        List<FilaEntidade> filaEntidade = new List<FilaEntidade>();
        public ActionResult ExportarExcel()
        {
            filaEntidade = (List<FilaEntidade>)TempData["row"];
            if (filaEntidade != null)
            {
                if (filaEntidade.Count() != 0)
                {
                    DataTable odt = ConvertToDataTable(filaEntidade);


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



        public ActionResult IncluirFila(ViewModelFila fila)
        {
            if (fila.nvc1 != null)
            {
                foreach (var item in fila.nvc1.Split(','))
                {
                   
                    FilaEntidade filaEntidade = new FilaEntidade();
                    filaEntidade.nvc1 = item;
                    filaEntidade.tabelaId = fila.tabelaId;
                    filaEntidade.nvc3 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    FilaEntidade listEntidade = ConverDataTableToList(filaEntidade.Listar())
                        .Where(f => f.nvc1.Trim() == item.Trim() && f.tabelaId.ToUpper().Trim() == fila.tabelaId.ToUpper().Trim()).FirstOrDefault();

                    if (listEntidade == null)
                    {
                        filaEntidade.Incluir();
                    }
                }



                return Json("");
            }
            else
            {
                return View("Index");
            }
        }

        public ActionResult IncluirFilaMedico(ViewModelFila fila)
        {
            if (fila.nvc1 != null)
            {
                var listaItenInclusao = fila.nvc1.Split(',');

                foreach (var item in listaItenInclusao)
                {
                    fila.tabelaId = "MEDICO";
                    FilaEntidade filaEntidade = new FilaEntidade();
                    filaEntidade.nvc1 = item;
                    filaEntidade.tabelaId = "MEDICO";
                    filaEntidade.nvc3 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    filaEntidade.Incluir();
                }


                return View(fila);
            }
            else
            {
                return View("Index");
            }
        }

        public ActionResult IncluirFilaPedido(ViewModelFila pedidoId)
        {
            if (!string.IsNullOrEmpty(pedidoId.nvc1))
            {
                var listaItenInclusao = pedidoId.nvc1.Split(',');

                foreach (var item in listaItenInclusao)
                {

                    FilaEntidade filaEntidade = new FilaEntidade();
                    filaEntidade.nvc1 = item;
                    filaEntidade.tabelaId = "PEDIDO";
                    filaEntidade.nvc3 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    filaEntidade.Incluir();
                }


                return View(new ViewModelFila());
            }
            else
            {
                return View("Index");
            }
        }



    }
}