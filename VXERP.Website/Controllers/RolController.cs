using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Website.Models;
using CRM.Website.Security.Infrastructure;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using CRM.Website.DevExpressHelpers;

namespace CRM.Website.Controllers
{
    public class RolController : GenericController<Rol>
    {
        private RolRepository objectRepository = new RolRepository();

      

        public ActionResult GridViewAllPartial()
        {
            return PartialView("_GridViewAllPartial", objectRepository.GetAll().ToList());
        }

        public void PopulateTreeModulosEmpty()
        {
            ModuloRepository moduloRepository = new ModuloRepository();
            StringBuilder sb = new StringBuilder();
            var listModulosSistema = moduloRepository.GetAll(p => p.Parent).OrderBy(o => o.Descripcion);

            var menuVM = MenuViewModel.CreateVM(null, listModulosSistema.ToList());


            foreach (var menuItem in menuVM)
            {
                sb.Append("<li>");
                sb.Append("<p  >");
                sb.Append(" <span class='icon'><i class='" + menuItem.Class + "'></i></span>");
                sb.Append(" <label class='control-label'>" + menuItem.Descripcion + "</label>");
                sb.Append("</p>");

                if (menuItem.URL != string.Empty && menuItem.URL != null)
                {
                    var checked_edit = "";
                    var checked_view = "";

                    if (menuItem.MenuId == 28 || menuItem.MenuId==50) 
                     {
                         checked_edit = "checked";
                         checked_view = "checked";
                     }
                    sb.Append("<div style='float:right; margin-right:150px'><input " + checked_edit + " name='chk_EDIT_" + menuItem.MenuId + "'   type='checkbox'  ></input></div>");
                    sb.Append("<div style='float:right; margin-right:180px'><input " + checked_view + " name='chk_VIEW_" + menuItem.MenuId + "'   type='checkbox' ></input></div>");
                }


                if (menuItem.Children.Count() > 0)
                    sb.Append("<ul class='sub'>");

                #region Sub Menu 1° Nivel
                foreach (var child in menuItem.Children)
                {

                    sb.Append("<li>");
                    if (child.URL != string.Empty && child.URL != null)
                        sb.Append("<p>");
                    else
                        sb.Append("<p >");

                    sb.Append("<span class='icon'><i class='" + (child.Class != null && child.Class != string.Empty ? child.Class : "") + "'></i></span>");
                    sb.Append(" <label class='control-label'>" + child.Descripcion + "</label>");
                    if (child.URL != string.Empty && child.URL != null)
                    {
                        sb.Append("<div style='float:right; margin-right:150px'><input name='chk_EDIT_" + child.MenuId + "'   type='checkbox' ></input></div>");
                        sb.Append("<div style='float:right; margin-right:180px'><input name='chk_VIEW_" + child.MenuId + "'   type='checkbox' ></input></div>");
                    }
                    sb.Append("</p>");

                    if (child.Children.Count() > 0)
                        sb.Append("<ul class='sub'>");

                    #region Sub Menu 2° Nivel
                    foreach (var subchild in child.Children)
                    {
                        sb.Append("<li>");
                        if (subchild.URL != string.Empty && subchild.URL != null)
                            sb.Append("<span >");
                        else
                            sb.Append("<span >");

                        sb.Append("<span class='icon'><i class='" + (subchild.Class != null && subchild.Class != string.Empty ? subchild.Class : "") + "'></i></span>");
                        sb.Append(" <label class='control-label'>" + subchild.Descripcion + "</label>");

                        if (subchild.URL != string.Empty && subchild.URL != null)
                        {
                            sb.Append("<div style='float:right; margin-right:150px'><input name='chk_EDIT_" + subchild.MenuId + "'   type='checkbox' ></input></div>");
                            sb.Append("<div style='float:right; margin-right:180px'><input name='chk_VIEW_" + subchild.MenuId + "'   type='checkbox' ></input></div>");
                        }


                        sb.Append("</span>");

                        if (subchild.Children.Count() > 0)
                            sb.Append("<ul class='sub'>");

                          #region Sub Menu 3° Nivel
                        foreach (var subsubchild in subchild.Children)
                        {
                            sb.Append("<li>");
                            if (subsubchild.URL != string.Empty && subsubchild.URL != null)
                                sb.Append("<span >");
                            else
                                sb.Append("<span >");

                            sb.Append("<span class='icon'><i class='" + (subsubchild.Class != null && subsubchild.Class != string.Empty ? subsubchild.Class : "") + "'></i></span>");
                            sb.Append(" <label class='control-label'>" + subsubchild.Descripcion + "</label>");

                            if (subsubchild.URL != string.Empty && subsubchild.URL != null)
                            {
                                sb.Append("<div style='float:right; margin-right:150px'><input name='chk_EDIT_" + subsubchild.MenuId + "'   type='checkbox' ></input></div>");
                                sb.Append("<div style='float:right; margin-right:180px'><input name='chk_VIEW_" + subsubchild.MenuId + "'   type='checkbox' ></input></div>");
                            }


                            sb.Append("</span>");
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




            ViewBag.Modulos = sb.ToString();
        }

        public void PopulateTreeModulosFill( int idRol)
        {
            ModuloRepository moduloRepository = new ModuloRepository();
            ModuloPermisoRepository chequeadoRepository  = new ModuloPermisoRepository();

            StringBuilder sb = new StringBuilder();
            var listModulosSistema = moduloRepository.GetAll(p => p.Parent).OrderBy(o => o.Descripcion);
            var listChequeados = chequeadoRepository.GetFiltered(p => p.Rol_Id== idRol).ToList();
         
            var menuVM = MenuViewModel.CreateVM(null, listModulosSistema.ToList());

            

            foreach (var menuItem in menuVM)
            {
                sb.Append("<li>");
                sb.Append("<p  >");
                sb.Append(" <span class='icon'><i class='" + menuItem.Class + "'></i></span>");
                sb.Append(" <label class='control-label'>" + menuItem.Descripcion + "</label>");
                sb.Append("</p>");

                if (menuItem.URL != string.Empty && menuItem.URL != null)
                {
                    var checked_edit = "";
                    var checked_view = "";
                    foreach (var item in listChequeados)
                    {
                        if (item.Modulo_Id == menuItem.MenuId && item.Accion == "VIEW") { checked_view = "checked"; }
                        if (item.Modulo_Id == menuItem.MenuId && item.Accion == "EDIT") { checked_edit = "checked"; }
 
                    }
                    sb.Append("<div style='float:right; margin-right:150px'><input " + checked_edit + " name='chk_EDIT_" + menuItem.MenuId + "'   type='checkbox'  ></input></div>");
                    sb.Append("<div style='float:right; margin-right:180px'><input " + checked_view + " name='chk_VIEW_" + menuItem.MenuId + "'   type='checkbox' ></input></div>");
                }


                if (menuItem.Children.Count() > 0)
                    sb.Append("<ul class='sub'>");

                #region Sub Menu 1° Nivel
                foreach (var child in menuItem.Children)
                {

                    sb.Append("<li>");
                    if (child.URL != string.Empty && child.URL != null)
                        sb.Append("<p>");
                    else
                        sb.Append("<p >");

                    sb.Append("<span class='icon'><i class='" + (child.Class != null && child.Class != string.Empty ? child.Class : "") + "'></i></span>");
                    sb.Append(" <label class='control-label'>" + child.Descripcion + "</label>");

                    if (child.URL != string.Empty && child.URL != null)
                    {
                        var checked_edit = "";
                        var checked_view = "";
                        foreach (var item in listChequeados)
                        {
                            if (item.Modulo_Id == child.MenuId && item.Accion == "VIEW") { checked_view = "checked"; }
                            if (item.Modulo_Id == child.MenuId && item.Accion == "EDIT") { checked_edit = "checked"; }

                        }
                        sb.Append("<div style='float:right; margin-right:150px'><input name='chk_EDIT_" + child.MenuId + "'  "+checked_edit+" type='checkbox' ></input></div>");
                        sb.Append("<div style='float:right; margin-right:180px'><input name='chk_VIEW_" + child.MenuId + "'  "+checked_view+" type='checkbox' ></input></div>");
                    }
                    sb.Append("</p>");

                    if (child.Children.Count() > 0)
                        sb.Append("<ul class='sub'>");

                    #region Sub Menu 2° Nivel
                    foreach (var subchild in child.Children)
                    {
                        sb.Append("<li>");
                        if (subchild.URL != string.Empty && subchild.URL != null)
                            sb.Append("<span >");
                        else
                            sb.Append("<span >");

                        sb.Append("<span class='icon'><i class='" + (subchild.Class != null && subchild.Class != string.Empty ? subchild.Class : "") + "'></i></span>");
                        sb.Append(" <label class='control-label'>" + subchild.Descripcion + "</label>");

                        
                        if (subchild.URL!= string.Empty && subchild.URL != null )
                        {
                            var checked_edit = "";
                            var checked_view = "";
                            foreach (var item in listChequeados)
                            {
                                if (item.Modulo_Id == subchild.MenuId && item.Accion == "VIEW") { checked_view = "checked"; }
                                if (item.Modulo_Id == subchild.MenuId && item.Accion == "EDIT") { checked_edit = "checked"; }
                           }

                            sb.Append("<div style='float:right; margin-right:150px'><input name='chk_EDIT_" + subchild.MenuId + "' " + checked_edit + "  type='checkbox' ></input></div>");
                            sb.Append("<div style='float:right; margin-right:180px'><input name='chk_VIEW_" + subchild.MenuId + "' " + checked_view + " type='checkbox' ></input></div>");
                         }

                        sb.Append("</span>");

                        if (subchild.Children.Count() > 0)
                            sb.Append("<ul class='sub'>");

                        #region Sub Menu 3° Nivel
                        foreach (var subsubchild in subchild.Children)
                        {
                            sb.Append("<li>");
                            if (subsubchild.URL != string.Empty && subsubchild.URL != null)
                                sb.Append("<span >");
                            else
                                sb.Append("<span >");

                            sb.Append("<span class='icon'><i class='" + (subsubchild.Class != null && subsubchild.Class != string.Empty ? subsubchild.Class : "") + "'></i></span>");
                            sb.Append(" <label class='control-label'>" + subsubchild.Descripcion + "</label>");


                            if (subsubchild.URL != string.Empty && subsubchild.URL != null)
                            {
                                var checked_edit = "";
                                var checked_view = "";
                                foreach (var item in listChequeados)
                                {
                                    if (item.Modulo_Id == subsubchild.MenuId && item.Accion == "VIEW") { checked_view = "checked"; }
                                    if (item.Modulo_Id == subsubchild.MenuId && item.Accion == "EDIT") { checked_edit = "checked"; }
                                }

                                sb.Append("<div style='float:right; margin-right:150px'><input name='chk_EDIT_" + subsubchild.MenuId + "' " + checked_edit + "  type='checkbox' ></input></div>");
                                sb.Append("<div style='float:right; margin-right:180px'><input name='chk_VIEW_" + subsubchild.MenuId + "' " + checked_view + " type='checkbox' ></input></div>");
                            }



                            sb.Append("</span>");
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




            ViewBag.Modulos = sb.ToString();
        }

        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult Index()
        {
            var listobjectViewModel = new List<Rol>();
            var objectViewModel = objectRepository.GetAll();
         
            if (objectViewModel.Count() > 0)
            {
                return View(objectViewModel.ToList());
            }
            else
            {
                return View(listobjectViewModel);
            }
        }

        [HttpGet]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create()
        {
            PopulateTreeModulosEmpty();
           
            return View();
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create([Bind(Include = "RoleName")]Rol objectViewModel)
        {
            List<KeyValuePair<int, string>> listModulosChk = new List<KeyValuePair<int, string>>();
            try
            {

                if (ModelState.IsValid)
                {

                    ModuloPermisoRepository moduloPermisoRepository = new ModuloPermisoRepository();
                   
                    objectRepository.Add(objectViewModel, this.User.UserID);

                   
                    foreach (var item in Request.Form)
                    {
                        if (item.ToString().StartsWith("chk"))
                        {
                            string[] arrItem = item.ToString().Split('_');

                            listModulosChk.Add(new KeyValuePair<int, string>(int.Parse(arrItem[2]), arrItem[1]));
                        }
                    }

                    foreach (var item in listModulosChk)
                    {
                        ModuloPermiso mPermiso = new ModuloPermiso();
                        mPermiso.Accion = item.Value;
                        mPermiso.Modulo_Id = item.Key;
                        mPermiso.Rol_Id = objectViewModel.Id;
                        moduloPermisoRepository.Add(mPermiso, this.User.UserID);
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar datos del Rol, comuniquese con su admnistrador :" + ex.Message);
            }

            PopulateTreeModulosEmpty();
            return View(objectViewModel);
        }

        [HttpGet]
        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult View(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var objectViewModel = objectRepository.Get(id.Value, p => p.ModulosPermiso).SingleOrDefault();

            if (objectViewModel == null)
            {
                return HttpNotFound();
            }
            PopulateTreeModulosFill(objectViewModel.Id);

            return View(objectViewModel);
        }

        [HttpGet]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
               //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var objectViewModel = objectRepository.Get(id.Value,p => p.ModulosPermiso).SingleOrDefault();

            if (objectViewModel == null)
            {
                return HttpNotFound();
            }
            PopulateTreeModulosFill(objectViewModel.Id);

            return View(objectViewModel);
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoleName,Estado,FechaCreacion,FechaActualizacion,idUsuario")]Rol objectViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ModuloPermisoRepository moduloPermisoRepository = new ModuloPermisoRepository();

                    objectRepository.Modify(objectViewModel, this.User.UserID);

                    //Elimino todos los permisos asociados al Rol
                    moduloPermisoRepository.DeleteByRol(objectViewModel.Id);
                    
                    List<KeyValuePair<int, string>> listModulosChk = new List<KeyValuePair<int, string>>();
                    foreach (var item in Request.Form)
                    {
                        if (item.ToString().StartsWith("chk"))
                        {
                            string[] arrItem = item.ToString().Split('_');

                            listModulosChk.Add(new KeyValuePair<int, string>(int.Parse(arrItem[2]), arrItem[1]));
                        }
                    }

                    foreach (var item in listModulosChk)
                    {
                        ModuloPermiso mPermiso = new ModuloPermiso();
                        mPermiso.Accion = item.Value;
                        mPermiso.Modulo_Id = item.Key;
                        mPermiso.Rol_Id = objectViewModel.Id;
                        moduloPermisoRepository.Add(mPermiso, this.User.UserID);
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar datos del Rol, comuniquese con su admnistrador :" + ex.Message);
            }
            PopulateTreeModulosFill(objectViewModel.Id);
            return View(objectViewModel);
        }

        [HttpGet]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Delete(int? id)
        {
            if (id == null )
            {
                ModelState.AddModelError("", "No se encontro el rol");
                return View("Index");
            }

            if (id == (int)RolRepository.SystemROL.ADMIN || id == (int)RolRepository.SystemROL.CLIENTE)
            {
                ModelState.AddModelError("", "No se pueden eliminar los Roles Default del Sistema");
                SetMessage(ERROR, "No se pueden eliminar los Roles Default del Sistema :");
                return RedirectToAction("Index");
            }

            var objectViewModel = objectRepository.Get(id.Value).SingleOrDefault();
            if (objectViewModel == null)
            {
                return HttpNotFound();
            }
            objectRepository.RemoveFromDataBase(objectViewModel, this.User.UserID);

            return RedirectToAction("Index");

        }

    }
}
