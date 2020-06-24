using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Business.Views;
using CRM.Website.DevExpressHelpers;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CRM.Website.Security.Infrastructure;

namespace CRM.Website.Controllers
{
    public class CronogramaPedidoController : BaseController
    {
        vGetSchedule vGetSchedule = new vGetSchedule();

        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult Index()
        {
            var tipos = new List<KeyValuePair<string, string>>();
            tipos.Add(new KeyValuePair<string, string>("Autorizados", "A"));
            tipos.Add(new KeyValuePair<string, string>("Sin Tratar en Planta", "S"));
            tipos.Add(new KeyValuePair<string, string>("Tratados en Planta", "T"));
            tipos.Add(new KeyValuePair<string, string>("No Recepcionados", "N"));

            var meses = new List<KeyValuePair<string, int>>();
            meses.Add(new KeyValuePair<string, int>("Enero", 1));
            meses.Add(new KeyValuePair<string, int>("Febrero", 2));
            meses.Add(new KeyValuePair<string, int>("Marzo", 3));
            meses.Add(new KeyValuePair<string, int>("Abril", 4));
            meses.Add(new KeyValuePair<string, int>("Mayo", 5));
            meses.Add(new KeyValuePair<string, int>("Junio", 6));
            meses.Add(new KeyValuePair<string, int>("Julio", 7));
            meses.Add(new KeyValuePair<string, int>("Agosto", 8));
            meses.Add(new KeyValuePair<string, int>("Septiembre", 9));
            meses.Add(new KeyValuePair<string, int>("Octubre", 10));
            meses.Add(new KeyValuePair<string, int>("Noviembre", 11));
            meses.Add(new KeyValuePair<string, int>("Diciembre", 12));

            List<KeyValuePair<int, int>> anios = Enumerable.Range(DateTime.Now.Year - 6, 11)
                                                    .Select(x => new KeyValuePair<int, int>(x, x)).ToList();
            Session["Tipo"] = "A";
            ViewBag.Tipos = tipos;
            ViewBag.Meses = meses;
            ViewBag.Anios = anios;

            return View("Index");
        }

        [LogonAuthorize(Roles = "VIEW")]
        public JsonResult GetEvents(double start, double end)
        {
            DateTime dateStart = ConvertFromUnixTimestamp(start);
            DateTime dateEnd = ConvertFromUnixTimestamp(end);

            TimeSpan ts = dateEnd.Subtract(dateStart);
            DateTime dateMiddle = dateStart.AddMinutes(ts.TotalMinutes / 2);

            int mes = dateMiddle.Month;
            int anio = dateMiddle.Year;
            string tipo = Session["Tipo"].ToString();

            DataTable dt = vGetSchedule.GetSchedule(mes, anio, tipo);

            //Formato de la fecha del FullCalendar = "anio-mes-dia"
            var random = new Random();
            var list = dt.AsEnumerable()
                .Select(dr => new
                {
                    title = "(" + dr.Field<int>("pedido") + ") " + dr.Field<string>("Des_Cli") + " - " + dr.Field<string>("Cantidad"),
                    //start = Convert.ToDateTime(dr.Field<string>("fe_conf")).Year + "-" + Convert.ToDateTime(dr.Field<string>("fe_conf")).Month + "-" + Convert.ToDateTime(dr.Field<string>("fe_conf")).Day,
                    start = Convert.ToDateTime(dr.Field<string>("fe_conf")).ToString("yyy-MM-dd"),
                    allDay = true,
                    textColor =dr.Field<string>("HexaColor"),
                    color = "#f6f6f6"
                }).ToList();

            var rows = list.ToArray();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetSessionVariable(string key, string value)
        {
            Session[key] = value;

            return this.Json(new { success = true });
        }

        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

    }
}
