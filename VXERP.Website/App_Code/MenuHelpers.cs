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
using CRM.Website.Crosscutting;


namespace CRM.Website.App_Code
{
    public static  class MenuHelpers
    {

        private static UsuarioRepository usuarioRepository = new UsuarioRepository();
        private static ModuloRepository moduloRepository = new ModuloRepository();



        public static IHtmlString DrawMenuItems(this HtmlHelper html)
        {
            StringBuilder sb = new StringBuilder();

            var currenUser = AuthenticationFactory.CreateAuthentication().GetUser();
            var currenModulo = GetBaseController(html).GetCurrenModulo();

            currenUser.RolesEmpresa = AppSession.RolesEmpresa;
          //  currenUser.UsuarioRolClientes = AppSession.RolesCliente;
            //var listModulosSistema = moduloRepository.GetFiltered(o=>o.Visible, p=>p.Parent).OrderBy(o=>o.Descripcion);
            var listModulosSistema =  moduloRepository.GetModulosUser(currenUser.RolesEmpresa, AppSession.Modulos);
            var menuVM = MenuViewModel.CreateVM(null, listModulosSistema.ToList());

         //   var listModulosPermiso = usuarioRepository.GetRolesEmpresaByUsuarioId(currenUser.UserID).Select(o=>o.Rol.ModulosPermiso);

            //foreach (var ob in listModulosPermiso)
            //{
            //    foreach (var modulo in ob.GroupBy(o => o.Modulo_Id).Select(group => group.First()))
            //    {
                   
            //  //      menuVM.Where(o => o.Children.se == modulo.Modulo_Id).Any();
                
            //    }
            //}

            string classExpand = "class='hasSub current'";
            string classAExpand = "class='expand rotateOut'";
            string classSubExpandShow = "class='sub expand show'";
            string classCurrent = "class='current'";
            foreach (var menuItem in menuVM)
            {
               
                    if (IsInCurrentModulo(currenModulo, menuItem.MenuId))
                    {
                        sb.Append("<li " + classExpand + ">");

                        sb.Append("<a href='#' " + classAExpand + ">");
                    }
                    else
                    {
                        sb.Append("<li >");
                        sb.Append("<a href='#'>");
                    }
                    sb.Append(" <span class='icon'><i class='" + menuItem.Class + "'></i></span>");
                    sb.Append(" <span class='txt'>" + menuItem.Descripcion + "</span>");
                    sb.Append("</a>");

                    if (menuItem.Children.Count() > 0)
                    {
                      
                            if (IsInCurrentModulo(currenModulo, menuItem.MenuId))
                                sb.Append("<ul " + classSubExpandShow + " >");
                            else
                                sb.Append("<ul class='sub'>");
                        
                        #region Sub Menu 1° Nivel
                        foreach (var child in menuItem.Children)
                        {
                           
                                if (currenModulo.Id == menuItem.MenuId)
                                    sb.Append("<li " + classCurrent + " >");
                                else
                                    sb.Append("<li>");

                                if (child.URL != string.Empty && child.URL != null)
                                    if (child.URL.ToUpper().StartsWith("HTTP"))
                                    {
                                        if (IsInCurrentModulo(currenModulo, child.MenuId))
                                            sb.Append("<a href='" + child.URL + "' " + classExpand + " >");
                                        else
                                            sb.Append("<a href='" + child.URL + "'>");
                                    }
                                    else
                                    {
                                        if (IsInCurrentModulo(currenModulo, child.MenuId))
                                            sb.Append("<a href='/" + child.URL + "' " + classExpand + " >");
                                        else
                                            sb.Append("<a href='/" + child.URL + "'>");
                                    }
                                else
                                    if (IsInCurrentModulo(currenModulo, child.MenuId))
                                        sb.Append("<a href='#'>");
                                    else
                                        sb.Append("<a href='#' " + classExpand + " >");

                                sb.Append("<span class='icon'><i class='" + (child.Class != null && child.Class != string.Empty ? child.Class : "") + "'></i></span>");
                                sb.Append(" <span class='txt'>" + child.Descripcion + "</span>");
                                sb.Append("</a>");

                                //if(child.Children.Count() > 0)
                                //    sb.Append("<ul class='sub'>");

                                if (child.Children.Count() > 0)
                                {
                                    if (IsInCurrentModulo(currenModulo, child.MenuId))
                                        sb.Append("<ul " + classSubExpandShow + " >");
                                    else
                                        sb.Append("<ul class='sub'>");
                                }

                                #region Sub Menu 2° Nivel
                                foreach (var subchild in child.Children)
                                {
                                    //   sb.Append("<li>");
                                        if (currenModulo.Id == menuItem.MenuId)
                                            sb.Append("<li " + classCurrent + " >");
                                        else
                                            sb.Append("<li>");

                                        if (subchild.URL != string.Empty && subchild.URL != null)
                                            if (subchild.URL.ToUpper().StartsWith("HTTP"))
                                            {
                                                if (IsInCurrentModulo(currenModulo, subchild.MenuId))
                                                    sb.Append("<a href='" + subchild.URL + "' " + classExpand + " >");
                                                else
                                                    sb.Append("<a href='" + subchild.URL + "'>");
                                            }
                                            else
                                            {
                                                if (IsInCurrentModulo(currenModulo, subchild.MenuId))
                                                    sb.Append("<a href='/" + subchild.URL + "' " + classExpand + " >");
                                                else
                                                    sb.Append("<a href='/" + subchild.URL + "'>");
                                            }                                            
                                        else
                                            if (IsInCurrentModulo(currenModulo, subchild.MenuId))
                                                sb.Append("<a href='#'>");
                                            else
                                                sb.Append("<a href='#' " + classExpand + " >");

                                        sb.Append("<span class='icon'><i class='" + (subchild.Class != null && subchild.Class != string.Empty ? subchild.Class : "") + "'></i></span>");
                                        sb.Append(" <span class='txt'>" + subchild.Descripcion + "</span>");
                                        sb.Append("</a>");

                                        //if (subchild.Children.Count() > 0)
                                        //    sb.Append("<ul class='sub'>");

                                        if (subchild.Children.Count() > 0)
                                        {
                                            if (IsInCurrentModulo(currenModulo, subchild.MenuId))
                                                sb.Append("<ul " + classSubExpandShow + " >");
                                            else
                                                sb.Append("<ul class='sub'>");
                                        }

                                        #region Sub Menu 3° Nivel
                                        foreach (var subsubchild in subchild.Children)
                                        {
                                            //   sb.Append("<li>");
                                                if (currenModulo.Id == subsubchild.MenuId)
                                                    sb.Append("<li " + classCurrent + " >");
                                                else
                                                    sb.Append("<li>");

                                                if (subsubchild.URL != string.Empty && subsubchild.URL != null)
                                                    if (subsubchild.URL.ToUpper().StartsWith("HTTP"))
                                                    {
                                                        if (IsInCurrentModulo(currenModulo, subsubchild.MenuId))
                                                            sb.Append("<a href='" + subsubchild.URL + "' " + classExpand + " >");
                                                        else
                                                            sb.Append("<a href='" + subsubchild.URL + "'>");
                                                    }
                                                    else
                                                    {
                                                        if (IsInCurrentModulo(currenModulo, subsubchild.MenuId))
                                                            sb.Append("<a href='/" + subsubchild.URL + "' " + classExpand + " >");
                                                        else
                                                            sb.Append("<a href='/" + subsubchild.URL + "'>");
                                                    }
                                                    
                                                else
                                                    if (IsInCurrentModulo(currenModulo, subsubchild.MenuId))
                                                        sb.Append("<a href='#'>");
                                                    else
                                                        sb.Append("<a href='#' " + classExpand + " >");

                                                sb.Append("<span class='icon'><i class='" + (subsubchild.Class != null && subsubchild.Class != string.Empty ? subsubchild.Class : "") + "'></i></span>");
                                                sb.Append(" <span class='txt'>" + subsubchild.Descripcion + "</span>");
                                                sb.Append("</a>");

                                                sb.Append("</li>");
                                            
                                        }
                                        #endregion

                                        if (subchild.Children.Count() > 0)
                                            sb.Append("</ul>");
                                        sb.Append("</li>");
                                    
                                }

                                #endregion

                                if (child.Children.Count() > 0)
                                    sb.Append("</ul>");
                                sb.Append("</li>");
                            
                        }
                        #endregion

                        if (menuItem.Children.Count() > 0)
                            sb.Append("</ul>");
                        sb.Append("</li>");
                    
                }
            }

            //foreach (var ob in listRolEmpresa.Select(o => o.Rol.ModulosPermiso))
            //{
            //    foreach (var modulo in ob.GroupBy(o=>o.Modulo_Id).Select(group => group.First()))
            //    {
            //       var moduloSistema= listModulosSistema.Where(o => o.Id == modulo.Modulo_Id).FirstOrDefault();
            //       var result = moduloSistema.Parent.
            //    }
            //}

            return new HtmlString(sb.ToString());
        }





        public static BaseController GetBaseController(this HtmlHelper htmlHelper)
        {
            var controller = htmlHelper.ViewContext.Controller as BaseController;
            if (controller == null)
            {
                throw new Exception("The controller used to render this view doesn't inherit from BaseContller");
            }
            return controller;
        }

     


        private static bool IsInCurrentModulo(Modulo currentModulo, int idParent)
        {
            ModuloRepository moduloRepository = new ModuloRepository();

            var listParent = moduloRepository.GetParentsList(currentModulo, AppSession.Modulos);//currentModulo.GetParentsList;
            foreach (var item in listParent)
            {
                if (item.Id == idParent)
                    return true;
            }
            return false;
        }

        
    }
}