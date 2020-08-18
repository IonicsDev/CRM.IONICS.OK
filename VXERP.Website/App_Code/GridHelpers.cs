using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CRM.Business.Entities;
using CRM.Business.DAL;

namespace CRM.Website.App_Code
{
    public static class GridHelper
    {
        public static IHtmlString MyActionLink(this HtmlHelper html)
        {
            // call the base ActionLink helper:
            return html.ActionLink("some text", "someAction");
        }

        public static string GridManageItemColumn(this HtmlHelper html, int id, bool estado)
        {


            StringBuilder sb = new StringBuilder();
            sb.Append(html.ActionLink(" ", "Edit", new { Id = id }, new { @class = " i-pencil-5 ", title = "Editar" }).ToHtmlString());
            sb.Append("<span> |</span>");


            if (estado)
            {

                sb.Append(html.ActionLink(" ", "Delete", new { Id = id }, new { Class = "confirm i-close-4", title = "Baja" }).ToHtmlString());

            }
            else
            {
                sb.Append(html.ActionLink(" ", "Delete", new { Id = id }, new { @class = "confirm  i-loop ", title = "Restablecer" }).ToHtmlString());
            }

            return sb.ToString();
        }

        public static string GridManageItemColumnwithView(this HtmlHelper html, int id, bool estado)
        {


            StringBuilder sb = new StringBuilder();
            sb.Append(html.ActionLink(" ", "Ver", new { Id = id }, new { @class = " i-search-2", title = "Ver" }).ToHtmlString());
            sb.Append("<span> |</span>");
            sb.Append(html.ActionLink(" ", "Edit", new { Id = id }, new { @class = " i-pencil-5 ", title = "Editar" }).ToHtmlString());
            sb.Append("<span> |</span>");


            if (estado)
            {
                sb.Append(html.ActionLink(" ", "Delete", new { Id = id }, new { Class = "confirm i-close-4", title = "Baja" }).ToHtmlString());

            }
            else
            {
                sb.Append(html.ActionLink(" ", "Delete", new { Id = id }, new { @class = "confirm  i-loop ", title = "Restablecer" }).ToHtmlString());
            }

            return sb.ToString();
        }

        public static string GridManageItemColumnwithoutDelete(this HtmlHelper html, int id)
        {


            StringBuilder sb = new StringBuilder();
            sb.Append(html.ActionLink(" ", "Edit", new { Id = id }, new { @class = " i-pencil-5 ", title = "Editar" }).ToHtmlString());

            return sb.ToString();
        }


        public static string GridManageItemColumn(this HtmlHelper html, int id, bool estado, Type type)
        {
            StringBuilder sb = new StringBuilder();
          
            


            sb.Append(html.ActionLink(" ", "Edit", new { Id = id }, new { @class = " i-pencil-5 ", title = "Editar" }).ToHtmlString());
            sb.Append("<span> |</span>");


            if (estado)
            {
                sb.Append(html.ActionLink(" ", "Delete", new { Id = id }, new { Class = "confirm i-close-4", title = "Baja" }).ToHtmlString());

            }
            else
            {
                sb.Append(html.ActionLink(" ", "Delete", new { Id = id }, new { @class = "confirm  i-loop ", title = "Restablecer" }).ToHtmlString());
            }

            return sb.ToString();
        }


        public static string GridManageItemColumnDiarioContable(this HtmlHelper html, int id, int estado)
        {


            StringBuilder sb = new StringBuilder();
            sb.Append(html.ActionLink(" ", "Do", new { Id = id }, new { @class = " i-search-2", title = "Ver" }).ToHtmlString());

            if (estado.Equals(3) || estado.Equals(4)) // Pendiente o Descuadrada se puede editar
            {
                sb.Append("<span> |</span>");
                sb.Append(html.ActionLink(" ", "Do", new { Id = id, msgError = "" }, new { @class = " i-pencil-5 ", title = "Editar" }).ToHtmlString());
                
                sb.Append("<span> |</span>");
                sb.Append(html.ActionLink(" ", "Anular", new { Id = id }, new { @class = " i-close-4 ", title = "Anular" }).ToHtmlString());

            }

            if (estado.Equals(5))
            {
                sb.Append("<span> |</span>");
                sb.Append(html.ActionLink(" ", "Restablecer", new { Id = id }, new { @class = "  i-loop ", title = "Restablecer" }).ToHtmlString());

            }
            sb.Append("<span> |</span>");
            sb.Append(html.ActionLink(" ", "ComprobanteDiario", "ReportesContables", new { Id = id, msgError = "" }, new { @class = "i-download-2 ", title = "Imprimir" }).ToHtmlString());
            return sb.ToString();
        }

        //public static string GridManageItemColumnDiarioContableFC(this HtmlHelper html, int id, int estado)
        //{


        //    StringBuilder sb = new StringBuilder();
            

        //    if (estado.Equals((int)FacturaCompra.STATUSFACTURACOMPRA.NO_ASENTADA) ) // Pendiente o Descuadrada se puede editar
        //    {
               
        //        sb.Append(html.ActionLink(" ", "Edit", new { Id = id, msgError = "" }, new { @class = " i-pencil-5 ", title = "Editar" }).ToHtmlString());

        //        sb.Append("<span> |</span>");
        //        sb.Append(html.ActionLink(" ", "Delete", new { Id = id }, new { @class = " i-close-4 ", title = "Anular" }).ToHtmlString());

        //    }

        //    if (estado.Equals((int)FacturaCompra.STATUSFACTURACOMPRA.ASENTADA))
        //    {
        //        sb.Append(html.ActionLink(" ", "Edit", new { Id = id }, new { @class = " i-search-2", title = "Ver" }).ToHtmlString());
        //    }

        //    if (estado.Equals((int)FacturaCompra.STATUSFACTURACOMPRA.ANULADA))
        //    {
        //        sb.Append(html.ActionLink(" ", "Edit", new { Id = id, msgError = "" }, new { @class = " i-pencil-5 ", title = "Ver" }).ToHtmlString());

        //        sb.Append("<span> |</span>");
        //        sb.Append(html.ActionLink(" ", "Restablecer", new { Id = id }, new { @class = "  i-loop ", title = "Restablecer" }).ToHtmlString());

        //    }
       
        //    return sb.ToString();
        //}

    }
}