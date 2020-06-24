using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using CRM.Business;
using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Website.Models;
using CRM.Website.Security;
using CRM.Website.Security.Infrastructure;
using DevExpress.Web;
using CRM.Website.Crosscutting;
using CRM.Business.Views;
using System.Data;
using System.Drawing.Printing;
using DevExpress.Web.Mvc;

namespace CRM.Website.Controllers
{
    public class NoConformidadDosController : GenericController<NoConformidad>
    {
        vGetListadoNoConformidades vGetListadoEventualidades = new vGetListadoNoConformidades();
        private NoConformidadRepository usuarioRepository = new NoConformidadRepository();
        private UsuarioRolClienteRepository usuarioRolClienteRepository = new UsuarioRolClienteRepository();
        ArchivoModuloRepository archivoRepository = new ArchivoModuloRepository();
        NoConformidadRepository noconformidadRepository = new NoConformidadRepository();
        Tipos_NoConformidadesRepository tipos_NoConformidadesRepository = new Tipos_NoConformidadesRepository();
        Origen_NoConformidadesRepository origen_NoConformidadesRepository = new Origen_NoConformidadesRepository();
        Identificacion_NoConformidadesRepository identificacion_NoConformidadesRepository = new Identificacion_NoConformidadesRepository();

        //MKN
        public ActionResult Imprimir(int id)
        {
            vGetListadoEventualidades.Datos = (DataTable)Session["DtNoConformidadSeleccionada"];
            DataTable DtEventualidad = vGetListadoEventualidades.Datos.Clone();
            DtEventualidad.Columns.Clear();


            DtEventualidad.Columns.Add("ID", typeof(int));
            DtEventualidad.Columns.Add("Tipo", typeof(string));
            DtEventualidad.Columns.Add("Numero", typeof(int));
            DtEventualidad.Columns.Add("Origen", typeof(string));
            DtEventualidad.Columns.Add("Identificacion", typeof(string));
            DtEventualidad.Columns.Add("Descripcion", typeof(string));
            DtEventualidad.Columns.Add("Observaciones", typeof(string));
            DtEventualidad.Columns.Add("AccionInmediata", typeof(string));
            DtEventualidad.Columns.Add("GestionDeAccion", typeof(string));
            DtEventualidad.Columns.Add("ComprobacionEficacia", typeof(string));
            DtEventualidad.Columns.Add("NombreApellido", typeof(string));
            DtEventualidad.Columns.Add("FechaActualizacion", typeof(DateTime));


            foreach (DataRow row in vGetListadoEventualidades.Datos.Rows)
            {
                if (row["ID"].ToString() == id.ToString())
                {
                    DtEventualidad.ImportRow(row);
                }
            }


            vGetListadoNoConformidades vDtEventualidad = new vGetListadoNoConformidades();
            vDtEventualidad.Datos = DtEventualidad;
            Session["DtNoConformidadSeleccionada"] = DtEventualidad;

            return PartialView("_ImpresoraNoConformidad", vDtEventualidad);
        }

        [HttpPost]
        public ActionResult Imprimir(/*string btnImprimir*/)
        {
            vGetListadoEventualidades.Datos = (DataTable)Session["DtNoConformidadSeleccionada"];
            DataTable dtEventualidad = vGetListadoEventualidades.Datos;
            XtraReportNoConformidad reporte = new XtraReportNoConformidad();


            reporte.Parameters["NoConformidad_Id"].Value = Convert.ToInt32(dtEventualidad.Rows[0][0]);
            reporte.Parameters["NoConformidad_Id"].Visible = false;

            reporte.Parameters["NombreApellido"].Value = Convert.ToString(dtEventualidad.Rows[0][10]);
            reporte.Parameters["NombreApellido"].Visible = false;
            reporte.ExportOptions.Html.TableLayout = false;
            reporte.PaperKind = PaperKind.A4;
            Session["ReporteNoConformidad"] = reporte;

            return PartialView("DocumentViewerPartial", reporte);
        }

        public ActionResult GridViewImpresora()
        {
            vGetListadoNoConformidades vDtEventualidad = new vGetListadoNoConformidades();
            vDtEventualidad.Datos = (DataTable)Session["DtNoConformidadSeleccionada"];
            return PartialView("_GridViewNoConformidad", vDtEventualidad);
        }

        public ActionResult GridViewAllPartial()
        {
            return PartialView("_GridViewAllPartial", usuarioRepository.GetAll().ToList());
        }
        
        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult Index()
        {
            //ClearTempFolder();

            vGetListadoNoConformidades listUsuarios = new vGetListadoNoConformidades();
            IQueryable<NoConformidad> usuarios;

            if (base.UserContext.RolesEmpresa.Any(s => s.Rol_Id == 1))
            {
                usuarios = usuarioRepository.GetFiltered(s => s.Estado == true);
            }
            else
            {
                usuarios = usuarioRepository.GetFiltered(s => s.Estado == true);

            }
            ViewBag.ObjectModel = usuarios;

            if (usuarios.Count() > 0)
            {
                return View("Index", listUsuarios);
            }
            else
            {
                return View("Index", listUsuarios);
            }
           
        }

        public ActionResult UsuariosPartial()
        {
            return PartialView("_GridParcial", usuarioRepository.GetAll().ToList());
        }

        //VIEW
        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult View(int? id)
        {
            ViewBag.TiposEventualidad = tipos_NoConformidadesRepository.GetFiltered(x => x.Estado == true).ToList();
            ViewBag.Origen = origen_NoConformidadesRepository.GetFiltered(x => x.Estado == true).ToList();
            ViewBag.Identificacion = identificacion_NoConformidadesRepository.GetFiltered(x => x.Estado == true).ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoConformidad eventualidad = noconformidadRepository.GetFiltered(x => x.Id == id).SingleOrDefault();

            try
            {
                if (eventualidad == null)
                {
                    throw new Exception(" La No Conformidad no existe");
                }

                string sId_Eventualidad = eventualidad.Id.ToString();
                eventualidad.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == sId_Eventualidad && x.Estado == true
                            && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                return Index();
            }

            GetEventualidades();
            return View("View", eventualidad);
        }

        [HttpGet]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create()
        {
            NoConformidad newUsuario = new NoConformidad();
            newUsuario.ArchivosModulo = new List<ArchivoModulo>().ToArray();

            ViewBag.TiposEventualidad = tipos_NoConformidadesRepository.GetFiltered(x => x.Estado == true).ToList();
            ViewBag.Origen = origen_NoConformidadesRepository.GetFiltered(x => x.Estado == true).ToList();
            ViewBag.Identificacion = identificacion_NoConformidadesRepository.GetFiltered(x => x.Estado == true).ToList();
            return View(newUsuario);
        }

        public ActionResult AddArchivoModulo()
        {
            ViewBag.TipoArchivoModulo = _ControllerName;

            return PartialView("_FormArchivo", new ArchivoModulo());
        }

        [HttpPost]
        public string AjaxAddArchivo()
        {
            string descripcion = Request["descripcion"];
            string[] descripciones = Request["descArchivos"].Split(',');
            string[] deletedArchivos = Request["deletedArchivos"].Split(',');

            for (int i = 0; i < descripciones.Length; i++)
            {
                if (deletedArchivos[i].Trim() == "false")
                {
                    if (descripciones[i].ToLower().Trim().Equals(descripcion.ToLower().Trim()))
                    {
                        return "false";
                    }
                }
            }

            HttpPostedFileBase currentFile = Request.Files["file"];

            string extension = Path.GetExtension(currentFile.FileName);
            string name = DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString()
                                + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00")
                                + extension;
            try
            {
                string fileName = Server.MapPath("~/Content/TempFiles/") + name;
                currentFile.SaveAs(fileName);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, "No se pudo guardar el archivo: " + ex.Message);
            }

            return name;
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Numero,Id,FechaOcurrencia,FechaApertura,IDOrigen,Identificacion,Descripcion,Observaciones,AccionInmediata,GestionDeAccion,ComprobacionEficacia,FechaCierre,Id,ArchivosModulo")] NoConformidad Nconformidad)
        {
            try
            {
                if (Nconformidad.ArchivosModulo == null)
                    Nconformidad.ArchivosModulo = new List<ArchivoModulo>().ToArray();

                if (ModelState.IsValid)
                {
                    if (usuarioRepository.GetFiltered(x => x.Descripcion.ToLower().Trim().Equals(Nconformidad.Descripcion.ToLower().Trim())).Any())
                    {
                        ModelState.AddModelError(string.Empty, "El Nombre de Usuario ya ha sido utilizado!");
                        return View(Nconformidad);
                    }
                    else
                    {
                        if (Nconformidad.Descripcion.Length >= 4)
                        {
                            usuarioRepository.Add(Nconformidad, this.User.UserID);

                            if (Nconformidad.ArchivosModulo != null)
                            {
                                foreach (ArchivoModulo newArchivo in Nconformidad.ArchivosModulo)
                                {
                                    if (newArchivo.Deleted == false)
                                    {
                                        AltaArchivosModulo(newArchivo, Nconformidad.Id.ToString());
                                    }
                                }
                            }
                            SetMessage(SUCCESS, "Guardado.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "La Password debe contener más de 3 caracteres");
                            return View(Nconformidad);
                        }
                    }
                }
                else
                {
                    return View(Nconformidad);
                }
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, "Error al guardar usuario :" + ex.Message);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                return View(Nconformidad);
            }

            return Index();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ViewBag.TiposEventualidad = tipos_NoConformidadesRepository.GetFiltered(x => x.Estado == true).ToList();
            ViewBag.Origen = origen_NoConformidadesRepository.GetFiltered(x => x.Estado == true).ToList();
            ViewBag.Identificacion = identificacion_NoConformidadesRepository.GetFiltered(x => x.Estado == true).ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!IsAdmin)
            {
                if (id.Value != User.UserID)
                {
                    SetMessage(ERROR, " Error al intentar modificar un usuario sin permisos.");
                    ModelState.AddModelError("", "No tiene permisis para editar usuarios");
                    return RedirectToAction("Index", "Home");
                }

            }
            NoConformidad usuario = usuarioRepository.Get(id.Value).SingleOrDefault();

            if (usuario == null)
            {
                return HttpNotFound();
            }
            string idString = id.ToString();
            ArchivoModuloRepository archivoRepository = new ArchivoModuloRepository();
            usuario.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true
                                                                        && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Numero,IDTipo,FechaOcurrencia,FechaApertura,IDOrigen,Identificacion,Descripcion,Observaciones,AccionInmediata,GestionDeAccion,ComprobacionEficacia,FechaCierre,Id,ArchivosModulo")]NoConformidad Nconformidad)
        {

            ViewBag.TiposEventualidad = tipos_NoConformidadesRepository.GetFiltered(x => x.Estado == true).ToList();
            ViewBag.Origen = origen_NoConformidadesRepository.GetFiltered(x => x.Estado == true).ToList();
            ViewBag.Identificacion = identificacion_NoConformidadesRepository.GetFiltered(x => x.Estado == true).ToList();

            if (Nconformidad.ArchivosModulo == null)
                Nconformidad.ArchivosModulo = new List<ArchivoModulo>().ToArray();

            try
            {
                noconformidadRepository.Modify(Nconformidad, this.User.UserID);

                //Modifico los Archivos, los que ya no estan en el listado los elimino
                //y los nuevos los agrego
                if (Nconformidad.ArchivosModulo != null)
                {
                    foreach (ArchivoModulo archivoModulo in Nconformidad.ArchivosModulo)
                    {
                        if ((archivoRepository.GetFiltered(x => x.Id == archivoModulo.Id && x.Estado == true).Any() == false) && archivoModulo.Deleted != true)
                        {
                            AltaArchivosModulo(archivoModulo, Nconformidad.Id.ToString());
                        }

                        if (archivoModulo.Deleted == true)
                        {
                            archivoRepository.Remove(archivoModulo, this.User.UserID);
                        }
                    }
                }
       }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);

                if (Nconformidad.Id > 0)
                {
                    string sEventualidad_Id = Nconformidad.Id.ToString();
                    Nconformidad.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == sEventualidad_Id && x.Estado == true
                        && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();
                }

                return View(Nconformidad);
            }

            SetMessage(SUCCESS, "Guardado.");
            return Index();
        }

        [HttpGet]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NoConformidad usuario = usuarioRepository.Get(id.Value).SingleOrDefault();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            usuarioRepository.Remove(usuario, this.User.UserID);

            return RedirectToAction("Index");
        }

        public ActionResult DocumentViewerPartialbak()
        {
            var dtEventualidad = (DataTable)Session["DtNoConformidadSeleccionada"];
            var reporte = (XtraReportEventualidad)Session["ReporteNoConformidad"];
            reporte.Parameters["NoConformidad_Id"].Value = Convert.ToInt32(dtEventualidad.Rows[0][0]);
            reporte.Parameters["NoConformidad_Id"].Visible = false;
            reporte.PaperKind = PaperKind.A4;
            return PartialView("DocumentViewerPartial", reporte);
        }

        public ActionResult DocumentViewerPartial()
        {
            var reporte = (XtraReportEventualidad)Session["ReporteNoConformidad"];
            reporte.ExportOptions.Html.TableLayout = false;
            reporte.PaperKind = PaperKind.A4;
            return PartialView("_DocumentViewerPartial", reporte);
        }

        public ActionResult ExportDocumentViewer()
        {
            var dtEventualidad = (DataTable)Session["DtNoConformidadSeleccionada"];
            var reporte = (XtraReportNoConformidad)Session["ReporteNoConformidad"];
            reporte.ExportOptions.Html.TableLayout = false;
            reporte.Parameters["NoConformidad_Id"].Value = Convert.ToInt32(dtEventualidad.Rows[0][0]);
            reporte.Parameters["NoConformidad_Id"].Visible = false;
            reporte.PaperKind = PaperKind.A4;
            return DocumentViewerExtension.ExportTo(reporte, Request);
        }


    }
}
