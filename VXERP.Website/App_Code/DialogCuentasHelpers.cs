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
using GridMvc.Html;

namespace CRM.Website.App_Code
{
    public static  class DialogCuentasHelper
    {


        //public static IHtmlString ShowDialog(this HtmlHelper html, string dialogID, string autoOpen, string title, string message, string jsFunctionSelected)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    var controller = BaseControlsHelper.GetBaseController(html);
        //    IEnumerable<Cuenta> listCuentas = (new CuentaRepository(controller.UserContext)).GetAll(p => p.Compania, p => p.Parent, p => p.ClaseCuenta);

        //  //  sb.Append(" <a class='i-search-2' data-toggle='modal' href='#" + dialogID + "'> </a>");

        //    sb.Append("<div class='modal fade' id='" + dialogID + "' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'>");
        //    sb.Append("<div class='modal-dialog'>");
        //    sb.Append("<div class='modal-content'>");
        //    sb.Append("<div class='modal-header'>");
        //    sb.Append("<button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button>");
        //    sb.Append("<h4 class='modal-title'>" + title  + "</h4>");
        //    sb.Append("</div>");
        //    sb.Append("<div class='modal-body'>");
        //    sb.Append(" Busqueda de Cuentas Contables:  ");
        //    sb.Append(" <table cellpadding='0' cellspacing='0' border='0' class='table table-striped table-bordered table-hover' id='dataTable'> ");
        //    sb.Append("<thead>");
        //    sb.Append("<tr>");
        //    sb.Append("<th>Compañia</th>");
        //    sb.Append("<th>Numeración</th>");
        //    sb.Append("<th>Descripción</th>");
        //    sb.Append("<th>Clase Cuenta</th>");
        //    sb.Append("<th></th>");
        //    sb.Append("</tr>");
        //    sb.Append(" </thead>");
        //    sb.Append("<tbody>");
        //    foreach (var item in listCuentas)
        //    {
        //        sb.Append(" <tr class='gradeA'>");
        //        sb.Append(" <td>" + item.Compania.Descripcion+ "</td>");
        //        sb.Append(" <td>" + item.Numeracion + "</td>");
        //        sb.Append(" <td>" + item.Descripcion + "</td>");
        //        sb.Append(" <td>" + item.ClaseCuenta.Descripcion + "</td>");
        //        if (!item.IsAgrupacion)
        //            sb.Append(" <td> <button type='button' class='btn btn-success' data-dismiss='modal' onclick='javascript:" + jsFunctionSelected + "(" + item.Id.ToString() + ");' >seleccionar</button> </td>");
        //        else
        //            sb.Append(" <td> </td>");

        //        sb.Append("</tr>");
        //    }
        //    sb.Append("</tbody>");

        //    sb.Append("<tfoot>");
        //   sb.Append("<tr>");
        //   sb.Append("<th>Compañia</th>");
        //   sb.Append("<th>Numeración</th>");
        //   sb.Append("<th>Descripción</th>");
        //   sb.Append("<th>Clase Cuenta</th>");
        //   sb.Append("<th></th>");
        //  sb.Append("</tr>");
        //  sb.Append("</tfoot>");
        //  sb.Append("</table>");

        //    sb.Append(" </div>");
        //    sb.Append("<div class='modal-footer'>");
        //    sb.Append(" <button type='button' class='btn btn-default' data-dismiss='modal'>Cerrar</button>");
        //  //  sb.Append("<button type='button' class='btn btn-primary' data-dismiss='modal' onclick='javascript:" + jsFunctionSelected + "(12);'>Aceptar</button>");
        //    sb.Append("</div>");
        //    sb.Append("</div><!-- /.modal-content -->");
        //    sb.Append(" </div><!-- /.modal-dialog -->");
        //    sb.Append("</div><!-- /.modal -->   ");

        //    return new HtmlString(sb.ToString());
        //}

       
        
    }
}