using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Business.Views;
using CRM.Website.DevExpressHelpers;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Globalization;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CRM.Website.Controllers
{
    public class PanelDeControlController : BaseController
    {
        private static System.Data.SqlClient.SqlParameter[] parameters = { new System.Data.SqlClient.SqlParameter("@FeHasta", DateTime.Now.Date) };

        vStockValorizadoSinTratar vStockValorizadosSinTratar = new vStockValorizadoSinTratar(parameters);
        vStockValorizadoTratados vStockValorizadoTratados = new vStockValorizadoTratados();
        vHorasEnCartera vHorasEnCartera = new vHorasEnCartera();
        vPaneldeControlActualizarValor vPaneldeControlActualizarValor = new vPaneldeControlActualizarValor();
        vRPT_PanelControl_Equivalente vRPT_PanelControl_Equivalente = new vRPT_PanelControl_Equivalente();
        vRPT_PanelControl_Fisico vRPT_PanelControl_Fisico = new vRPT_PanelControl_Fisico();
        vRPT_PanelControl_Valorizado vRPT_PanelControl_Valorizado = new vRPT_PanelControl_Valorizado();
        vRPT_PanelControl_Horas vRPT_PanelControl_Horas = new vRPT_PanelControl_Horas();

        public ActionResult Index(string dateStockSinTratar = default(string), string dateStockTratados = default(string), string dateHorasCartera = default(string))
        {
            Session["DateStockSinTratar"] = DateTime.Now.Date.ToShortDateString();
            Session["DateStockTratados"] = DateTime.Now.Date.ToShortDateString();
            Session["DateHorasCartera"] = DateTime.Now.Date.ToShortDateString();

            vStockValorizadosSinTratar.Datos = vStockValorizadosSinTratar.Get_Datos(DateTime.Now.Date.ToShortDateString());
            vStockValorizadoTratados.Datos = vStockValorizadoTratados.Get_Datos(DateTime.Now.Date.ToShortDateString());
            vHorasEnCartera.Datos = vHorasEnCartera.Get_Datos(DateTime.Now.Date.ToShortDateString());

            Session["vStockValorizadosSinTratar"] = vStockValorizadosSinTratar.Datos;
            Session["vStockValorizadoTratados"] = vStockValorizadoTratados.Datos;
            Session["vHorasEnCartera"] = vHorasEnCartera.Datos;
            ViewBag.SegundosPorPaso = vPaneldeControlActualizarValor.ObtenerValor();


            //DATOS PARA EL GRAFICO DE HORAS
            Session["FechaDesde_Horas"] = DateTime.Now.ToShortDateString(); Session["FechaHasta_Horas"] = DateTime.Now.ToShortDateString();
            DataTable dtRPT_PanelControl_Horas = vRPT_PanelControl_Horas.Get_Datos(DateTime.Now.ToShortDateString(), DateTime.Now.ToShortDateString());
            ViewBag.ValuesChartHoras_SinTratar = CalculateValuesChart(dtRPT_PanelControl_Horas, "HorasSinTratar");
            ViewBag.ValuesChartHoras_Cartera = CalculateValuesChart(dtRPT_PanelControl_Horas, "HorasCartera");
            Session["ValuesChartHoras_SinTratar"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartHoras_SinTratar;
            Session["ValuesChartHoras_Cartera"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartHoras_Cartera;
       


            //DATOS PARA EL GRAFICO EQUIVALENTE
            Session["FechaDesde_Equivalente"] = DateTime.Now.ToShortDateString(); Session["FechaHasta_Equivalente"] = DateTime.Now.ToShortDateString();
            DataTable dtRPT_PanelControl_Equivalente = vRPT_PanelControl_Equivalente.Get_Datos(DateTime.Now.ToShortDateString(), DateTime.Now.ToShortDateString());
            ViewBag.ValuesChartEquivalente_SinTratar = CalculateValuesChart(dtRPT_PanelControl_Equivalente, "StockEquivSinTratar");
            ViewBag.ValuesChartEquivalente_Tratados = CalculateValuesChart(dtRPT_PanelControl_Equivalente, "StockEquivTratado");
            Session["ValuesChartEquivalente_SinTratar"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartEquivalente_SinTratar;
            Session["ValuesChartEquivalente_Tratados"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartEquivalente_Tratados;

            //DATOS PARA EL GRAFICO FISICO
            Session["FechaDesde_Fisico"] = DateTime.Now.ToShortDateString(); Session["FechaHasta_Fisico"] = DateTime.Now.ToShortDateString();
            DataTable dtRPT_PanelControl_Fisico = vRPT_PanelControl_Fisico.Get_Datos(DateTime.Now.ToShortDateString(), DateTime.Now.ToShortDateString());
            ViewBag.ValuesChartFisico_SinTratar = CalculateValuesChart(dtRPT_PanelControl_Fisico, "StockFisSinTratar");
            ViewBag.ValuesChartFisico_Tratados = CalculateValuesChart(dtRPT_PanelControl_Fisico, "StockFisTratado");
            Session["ValuesChartFisico_SinTratar"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartFisico_SinTratar;
            Session["ValuesChartFisico_Tratados"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartFisico_Tratados;

            //DATOS PARA EL GRAFICO STOCKVAL
            Session["FechaDesde_StockVal"] = DateTime.Now.ToShortDateString(); Session["FechaHasta_StockVal"] = DateTime.Now.ToShortDateString();
            DataTable dtRPT_PanelControl_Valorizado = vRPT_PanelControl_Valorizado.Get_Datos(DateTime.Now.ToShortDateString(), DateTime.Now.ToShortDateString());
            ViewBag.ValuesChartStockVal_SinTratar = CalculateValuesChart(dtRPT_PanelControl_Valorizado, "ValSinTratar");
            ViewBag.ValuesChartStockVal_Tratados = CalculateValuesChart(dtRPT_PanelControl_Valorizado, "ValTratado");
            Session["ValuesChartStockVal_SinTratar"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartStockVal_SinTratar;
            Session["ValuesChartStockVal_Tratados"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartStockVal_Tratados;

           
            var vistas = new Tuple<vStockValorizadoSinTratar, vStockValorizadoTratados, vHorasEnCartera>(vStockValorizadosSinTratar, vStockValorizadoTratados, vHorasEnCartera);
            return View("Index", vistas);
        }

        [HttpPost]
        public ActionResult Index(string dateStockSinTratar, string dateStockTratados, string dateHorasCartera, string exportarStockSinTratar, int? segundosPorPaso,
                                        string exportarStockTratados, string exportarHorasCartera, string filtrarStockSinTratar, string filtrarStockTratados, string filtrarHorasCartera)
        {
            if (dateStockSinTratar == "")
                dateStockSinTratar = DateTime.Now.Date.ToShortDateString();
            if (dateStockTratados == "")
                dateStockTratados = DateTime.Now.Date.ToShortDateString();
            if (dateHorasCartera == "")
                dateHorasCartera = DateTime.Now.Date.ToShortDateString();

            try
            {
                if (exportarStockSinTratar != null)
                {
                    var setting = GridHelper.GetEspecialSettingExport(vStockValorizadosSinTratar.GetDynamicCollectionList(vStockValorizadosSinTratar.Get_Datos(dateStockSinTratar)), _ControllerName);

                    // retornamos el excel al usuario
                    return GridViewExtension.ExportToXls(setting, vStockValorizadosSinTratar.Get_Datos(dateStockSinTratar), string.Format("{0}s_{1}.{2}",
                                                        typeof(vStockValorizadoSinTratar).Name, DateTime.Now.ToString("ddMMyyyy_HHmmss"), "xls"));
                }

                if (exportarStockTratados != null)
                {
                    var setting = GridHelper.GetEspecialSettingExport(vStockValorizadoTratados.GetDynamicCollectionList(vStockValorizadoTratados.Get_Datos(dateStockTratados)), _ControllerName);

                    // retornamos el excel al usuario
                    return GridViewExtension.ExportToXls(setting, vStockValorizadoTratados.Get_Datos(dateStockTratados), string.Format("{0}s_{1}.{2}",
                                                        typeof(vStockValorizadoTratados).Name, DateTime.Now.ToString("ddMMyyyy_HHmmss"), "xls"));
                }

                if (exportarHorasCartera != null)
                {
                    var setting2 = GridHelper.GetEspecialSettingExport(vHorasEnCartera.GetDynamicCollectionList(vHorasEnCartera.Get_Datos(dateHorasCartera)), _ControllerName);

                    // retornamos el excel al usuario
                    return GridViewExtension.ExportToXls(setting2, vHorasEnCartera.Get_Datos(dateHorasCartera), string.Format("{0}s_{1}.{2}",
                                                        typeof(vHorasEnCartera).Name, DateTime.Now.ToString("ddMMyyyy_HHmmss"), "xls")); 
                }

                if (segundosPorPaso != null)
                {
                    vPaneldeControlActualizarValor.ActualizarValores(segundosPorPaso.Value);
                    ViewBag.SegundosPorPaso = segundosPorPaso;
                }

                vStockValorizadosSinTratar.Datos = vStockValorizadosSinTratar.Get_Datos(dateStockSinTratar);
                Session["DateStockSinTratar"] = dateStockSinTratar;
                Session["vStockValorizadosSinTratar"] = vStockValorizadosSinTratar.Datos;

                vStockValorizadoTratados.Datos = vStockValorizadoTratados.Get_Datos(dateStockTratados);
                Session["DateStockTratados"] = dateStockTratados;
                Session["vStockValorizadoTratados"] = vStockValorizadoTratados.Datos;

                vHorasEnCartera.Datos = vHorasEnCartera.Get_Datos(dateHorasCartera);
                Session["DateHorasCartera"] = dateHorasCartera;
                Session["vHorasEnCartera"] = vHorasEnCartera.Datos;

                //DATOS PARA EL GRAFICO EQUIVALENTE
                ViewBag.ValuesChartEquivalente_SinTratar = (SortedDictionary<DateTime, double>)Session["ValuesChartEquivalente_SinTratar"];
                ViewBag.ValuesChartEquivalente_Tratados = (SortedDictionary<DateTime, double>)Session["ValuesChartEquivalente_Tratados"];

                //DATOS PARA EL GRAFICO FISICO
                ViewBag.ValuesChartFisico_SinTratar = (SortedDictionary<DateTime, double>)Session["ValuesChartFisico_SinTratar"];
                ViewBag.ValuesChartFisico_Tratados = (SortedDictionary<DateTime, double>)Session["ValuesChartFisico_Tratados"];

                //DATOS PARA EL GRAFICO STOCKVAL
                ViewBag.ValuesChartStockVal_SinTratar = (SortedDictionary<DateTime, double>)Session["ValuesChartStockVal_SinTratar"];
                ViewBag.ValuesChartStockVal_Tratados = (SortedDictionary<DateTime, double>)Session["ValuesChartStockVal_Tratados"];

                //DATOS PARA EL GRAFICO DE HORAS
                ViewBag.ValuesChartHoras_SinTratar = (SortedDictionary<DateTime, double>)Session["ValuesChartHoras_SinTratar"];
                ViewBag.ValuesChartHoras_Cartera = (SortedDictionary<DateTime, double>)Session["ValuesChartHoras_Cartera"];
                
            }
            catch (Exception ex)
            {
            }

            var vistas = new Tuple<vStockValorizadoSinTratar, vStockValorizadoTratados, vHorasEnCartera>(vStockValorizadosSinTratar, vStockValorizadoTratados, vHorasEnCartera);
            return View("Index", vistas);
        }

        public SortedDictionary<DateTime, double> CalculateValuesChart(DataTable dt, string campo)
        {
            SortedDictionary<DateTime, double> values = new SortedDictionary<DateTime, double>();

            if (dt.Rows.Count == 0)
                values.Add(DateTime.Now, 0);

            foreach (DataRow row in dt.Rows)
            {
                DateTime dia = Convert.ToDateTime(row["Fecha"].ToString());
                values.Add(dia, Convert.ToDouble(row[campo].ToString()));
            }

            return values;
        }

        public ActionResult GridViewAllPartial_StockSinTratar()
        {
            ViewBag.Id = "Articulo";
            vStockValorizadosSinTratar.Datos = (DataTable)Session["vStockValorizadosSinTratar"];
            return PartialView("_GridViewStockSinTratar", vStockValorizadosSinTratar);
        }

        public ActionResult GridViewAllPartial_StockTratados()
        {
            ViewBag.Id = "Cg_Art";
            vStockValorizadoTratados.Datos = (DataTable)Session["vStockValorizadoTratados"];
            return PartialView("_GridViewStockTratados", vStockValorizadoTratados);
        }

        public ActionResult GridViewAllPartial_HorasEnCartera()
        {
            ViewBag.Id = "Articulo";
            vHorasEnCartera.Datos = (DataTable)Session["vHorasEnCartera"];
            return PartialView("_GridViewHorasEnCartera", vHorasEnCartera);
        }

        [HttpPost]
        public ActionResult Reportes(string dpDesde_Equivalente, string dpHasta_Equivalente, string dpDesde_Fisico, string dpHasta_Fisico,
                                        string dpDesde_StockVal, string dpHasta_StockVal, string dpDesde_Horas, string dpHasta_Horas)
        {
            if (dpDesde_Equivalente == "") dpDesde_Equivalente = DateTime.Now.ToShortDateString();
            if (dpHasta_Equivalente == "") dpHasta_Equivalente = DateTime.Now.ToShortDateString();
            Session["FechaDesde_Equivalente"] = dpDesde_Equivalente; Session["FechaHasta_Equivalente"] = dpHasta_Equivalente;

            if (dpDesde_Fisico == "") dpDesde_Fisico = DateTime.Now.ToShortDateString();
            if (dpHasta_Fisico == "") dpHasta_Fisico = DateTime.Now.ToShortDateString();
            Session["FechaDesde_Fisico"] = dpDesde_Fisico; Session["FechaHasta_Fisico"] = dpHasta_Fisico;

            if (dpDesde_StockVal == "") dpDesde_StockVal = DateTime.Now.ToShortDateString();
            if (dpHasta_StockVal == "") dpHasta_StockVal = DateTime.Now.ToShortDateString();
            Session["FechaDesde_StockVal"] = dpDesde_StockVal; Session["FechaHasta_StockVal"] = dpHasta_StockVal;

            if (dpDesde_Horas == "") dpDesde_Horas = DateTime.Now.ToShortDateString();
            if (dpHasta_Horas == "") dpHasta_Horas = DateTime.Now.ToShortDateString();
            Session["FechaDesde_Horas"] = dpDesde_Horas; Session["FechaHasta_Horas"] = dpHasta_Horas;

            //DATOS PARA EL GRAFICO EQUIVALENTE
            DataTable dtRPT_PanelControl_Equivalente = vRPT_PanelControl_Equivalente.Get_Datos(dpDesde_Equivalente, dpHasta_Equivalente);
            ViewBag.ValuesChartEquivalente_SinTratar = CalculateValuesChart(dtRPT_PanelControl_Equivalente, "StockEquivSinTratar");
            ViewBag.ValuesChartEquivalente_Tratados = CalculateValuesChart(dtRPT_PanelControl_Equivalente, "StockEquivTratado");
            Session["ValuesChartEquivalente_SinTratar"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartEquivalente_SinTratar;
            Session["ValuesChartEquivalente_Tratados"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartEquivalente_Tratados;

            //DATOS PARA EL GRAFICO FISICO
            DataTable dtRPT_PanelControl_Fisico = vRPT_PanelControl_Fisico.Get_Datos(dpDesde_Fisico, dpHasta_Fisico);
            ViewBag.ValuesChartFisico_SinTratar = CalculateValuesChart(dtRPT_PanelControl_Fisico, "StockFisSinTratar");
            ViewBag.ValuesChartFisico_Tratados = CalculateValuesChart(dtRPT_PanelControl_Fisico, "StockFisTratado");
            Session["ValuesChartFisico_SinTratar"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartFisico_SinTratar;
            Session["ValuesChartFisico_Tratados"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartFisico_Tratados;

            //DATOS PARA EL GRAFICO STOCKVAL
            DataTable dtRPT_PanelControl_Valorizado = vRPT_PanelControl_Valorizado.Get_Datos(dpDesde_StockVal, dpHasta_StockVal);
            ViewBag.ValuesChartStockVal_SinTratar = CalculateValuesChart(dtRPT_PanelControl_Valorizado, "ValSinTratar");
            ViewBag.ValuesChartStockVal_Tratados = CalculateValuesChart(dtRPT_PanelControl_Valorizado, "ValTratado");
            Session["ValuesChartStockVal_SinTratar"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartStockVal_SinTratar;
            Session["ValuesChartStockVal_Tratados"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartStockVal_Tratados;

            //DATOS PARA EL GRAFICO STOCKVAL
            DataTable dtRPT_PanelControl_Horas = vRPT_PanelControl_Horas.Get_Datos(dpDesde_Horas, dpHasta_Horas);
            ViewBag.ValuesChartHoras_SinTratar = CalculateValuesChart(dtRPT_PanelControl_Horas, "HorasSinTratar");
            ViewBag.ValuesChartHoras_Cartera = CalculateValuesChart(dtRPT_PanelControl_Horas, "HorasCartera");
            Session["ValuesChartHoras_SinTratar"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartHoras_SinTratar;
            Session["ValuesChartHoras_Cartera"] = (SortedDictionary<DateTime, double>)ViewBag.ValuesChartHoras_Cartera;

            vStockValorizadosSinTratar.Datos = (DataTable)Session["vStockValorizadosSinTratar"];
            vStockValorizadoTratados.Datos = (DataTable)Session["vStockValorizadoTratados"];
            vHorasEnCartera.Datos = (DataTable)Session["vHorasEnCartera"];

            var vistas = new Tuple<vStockValorizadoSinTratar, vStockValorizadoTratados, vHorasEnCartera>(vStockValorizadosSinTratar, vStockValorizadoTratados, vHorasEnCartera);
            return View("Index", vistas);
        }

        [HttpPost]
        public ActionResult Export(string dataUri, string Title, string BlueReference, string RedReference, string FechaDesde, string FechaHasta)
        {
            string base64 = dataUri.Substring(dataUri.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            byte[] data = Convert.FromBase64String(base64);

            //string path = "~/Content/Files/";
            //var file = Server.MapPath(path + "Chart.jpg");
            //System.IO.File.WriteAllBytes(file, data); //Guarda la imagen

            System.Drawing.Image image = byteArrayToImage(data);
            iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Png);
            pic.ScalePercent(45f, 50f);

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4.Rotate(), 60f, 60f, 20f, 20f);
                iTextSharp.text.Font NormalFont = FontFactory.GetFont("Arial", 9, BaseColor.GRAY);

                string pdfName = DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString()
                               + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00")
                               + "_" + "NombreTemporario" + ".pdf";
                string pathPdf = Server.MapPath("~/Content/TempFiles/") + pdfName;
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.Open();
                doc.AddTitle(Title);
                Paragraph pTitle = new Paragraph(Title, FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.UNDERLINE));
                pTitle.Alignment = Element.ALIGN_CENTER;
                doc.Add(pTitle);

                doc.Add(new Paragraph(" ", NormalFont)); doc.Add(new Paragraph(" ", NormalFont)); doc.Add(new Paragraph(" ", NormalFont));

                Paragraph fechas = new Paragraph("Desde: " + FechaDesde + ", Hasta: " + FechaHasta, NormalFont);
                doc.Add(fechas);

                Paragraph pReferences = new Paragraph();
                Chunk c1 = new Chunk("aaaaaaaaaaaaa", FontFactory.GetFont("Arial", 5, iTextSharp.text.Font.NORMAL, new BaseColor(98, 174, 239)));
                c1.SetBackground(new BaseColor(98, 174, 239), 0f, 1f, 0f, 0f);
                Chunk c2 = new Chunk(" " + BlueReference + "   ", NormalFont);
                Chunk c3 = new Chunk("aaaaaaaaaaaaa", FontFactory.GetFont("Arial", 5, iTextSharp.text.Font.NORMAL, new BaseColor(216, 65, 96)));
                c3.SetBackground(new BaseColor(216, 65, 96), 0f, 1f, 0f, 0f);
                Chunk c4 = new Chunk(" " + RedReference + "   ", NormalFont);
                pReferences.Add(c1); pReferences.Add(c2); pReferences.Add(c3); pReferences.Add(c4);
                pReferences.Alignment = Element.ALIGN_RIGHT;
                doc.Add(pReferences);

                doc.Add(pic);
                doc.Close();
                byte[] bytes = ms.ToArray();

                string handle = Guid.NewGuid().ToString();
                Session[handle] = bytes;
                
                ms.Close();

                return new JsonResult()
                {
                    Data = new { FileGuid = handle, FileName = pdfName }
                };
            }
        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }

        public ActionResult Download(string fileName, string fileGuid)
        {
            if (Session[fileGuid] != null)
            {
                byte[] data = Session[fileGuid] as byte[];
                return File(data, fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}
