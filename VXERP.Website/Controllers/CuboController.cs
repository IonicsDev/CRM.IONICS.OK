using CRM.Website.DevExpressHelpers;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Website.Controllers
{
    public class CuboController : BaseController
    {
        //
        // GET: /Cubo/
        public ActionResult Index()
        {
            Session["Options"] = PivotGridHelper.PivotGridSettings;
            return View();
        }


        public ActionResult ChartPartial()
        {
            var model = new object[0];
            return PartialView("_ChartPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult PivotGridPartial()
        {
            var model = new object[0];
            return PartialView("_PivotGridPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult ChartsIntegrationChartPartial()
        {
            var chartModel = PivotGridExtension.GetDataObject(PivotGridHelper.PivotGridSettings, PivotGridHelper.OlapConexionString);
           
            return PartialView("_ChartPartial", chartModel);
        }
    }
}
