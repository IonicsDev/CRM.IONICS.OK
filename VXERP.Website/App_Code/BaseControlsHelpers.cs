using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CRM.Business.DAL;
using CRM.Website.Security;
using System.Linq;
using CRM.Website.Models;
using CRM.Business.Entities;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using CRM.Website.Controllers;


namespace CRM.Website.App_Code
{
    public static class BaseControlsHelper
    {

        public static BaseController GetBaseController(this HtmlHelper htmlHelper)
        {
            var controller = htmlHelper.ViewContext.Controller as BaseController;
            if (controller == null)
            {
                throw new Exception("The controller used to render this view doesn't inherit from BaseContller");
            }
            return controller;
        }

        private static Modulo GetCurrenModulo(this HtmlHelper html)
        {
            try
            {
                RouteData routeData = html.ViewContext.RouteData;
                var _ControllerName = routeData.GetRequiredString("controller");
                if (_ControllerName == null)
                    return null;

                ModuloRepository moduloRepository = new ModuloRepository();
                var result = moduloRepository.GetFiltered(o => o.URL != null && o.URL.ToUpper().StartsWith(_ControllerName.ToUpper()), p => p.Parent);

                var listResult = result.ToList();

                if (listResult.Count > 1)
                {
                    foreach (var item in result.ToList())
                    {
                        if (item.URL.Split('/')[0].Equals(_ControllerName))
                            return item;
                    }
                }

                if (result == null)
                    throw new Exception("Error al buscar el Modulo actual en el controlador: No se encontró en la consulta");

                return result.First();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el Modulo actual en el controlador:" + ex.Message);
            }

        }

    }
}