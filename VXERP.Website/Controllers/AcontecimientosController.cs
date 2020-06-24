using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Business.Views;
using CRM.Website.DevExpressHelpers;
using CRM.Website.Security.Infrastructure;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace CRM.Website.Controllers
{
    public class AcontecimientosController : BaseController
    {

        private AcontecimientoRepository acontecimientoRepository = new AcontecimientoRepository();
        private TipoAcontecimientoRepository TipoAcontecimientoRepository = new TipoAcontecimientoRepository();
        private vAcontecimientos vacontecimientos = new vAcontecimientos();
        private AcontecimientoDetalleRepository acontecimientoDetalleRepository = new AcontecimientoDetalleRepository();
        private ArchivoModuloRepository archivoModuloRepository = new ArchivoModuloRepository();
        private OrigenRepository origenRepository = new OrigenRepository();
        //
        // GET: /Acontecimientos/
        [LogonAuthorize(Roles = "VIEW,EDIT")]
        public ActionResult Index()
        {
            Session["firmaGteGral"] = "false";
            DataTable datos = new DataTable();
            datos = vacontecimientos.GetListadoAcontecimientos();
            ViewBag.TiposAcontecimiento = TipoAcontecimientoRepository.GetFiltered(x => x.Estado == true).ToList();
            ViewBag.Origenes = origenRepository.GetFiltered(x => x.Estado == true).ToList();
            vacontecimientos.Datos = datos;
            Session["ListadoAcontecimientos"] = datos;

            return View("Index", vacontecimientos);
        }

        public void SetSessionVariable(string key, string value)
        {
            Session[key] = value;
        }

        public ActionResult GridViewAllPartial()
        {
            vAcontecimientos vacontecimientos = new vAcontecimientos();
            vacontecimientos.Datos = (DataTable)Session["ListadoAcontecimientos"];
            return PartialView("_GridViewAcontecimientos", vacontecimientos);
        }

        //VIEW
        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult View(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.Origenes = origenRepository.GetFiltered(x => x.Estado == true).ToList();
            Acontecimiento acontecimiento = acontecimientoRepository.Get(id.Value).SingleOrDefault();
            //acontecimiento.TipoAcontecimiento_Id = acontecimiento.ti.TipoEventualidad_Id;

            try
            {
                if (acontecimiento == null)
                {
                    throw new Exception(" El Acontecimiento no existe");
                }

                acontecimiento.TipoAcontecimiento = TipoAcontecimientoRepository.GetFiltered(x => x.Id == acontecimiento.TipoAcontecimiento_Id).FirstOrDefault();
                acontecimiento.Origen = origenRepository.GetFiltered(o => o.Id == acontecimiento.Origen_Id).FirstOrDefault();
                ViewBag.TiposAcontecimiento = TipoAcontecimientoRepository.GetFiltered(x => x.Estado == true).ToList();

                string sId_Acontecimiento = acontecimiento.Id.ToString();
                acontecimiento.ArchivosModulo = archivoModuloRepository.GetFiltered(x => x.Entity_Id == sId_Acontecimiento && x.Estado == true
                            && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();


            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                return Index();
            }


            return View("View", acontecimiento);
        }


        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create()
        {
            ViewBag.TiposAcontecimiento = TipoAcontecimientoRepository.GetFiltered(x => x.Estado == true).ToList();
            ViewBag.Origenes = origenRepository.GetFiltered(x => x.Estado == true).ToList();
            Acontecimiento newAcontecimiento = new Acontecimiento();
            newAcontecimiento.Fecha = DateTime.Now.Date;
            newAcontecimiento.FechaApertura = DateTime.Now.Date;
            newAcontecimiento.TipoAcontecimiento_Id = 0;
            newAcontecimiento.ArchivosModulo = new List<ArchivoModulo>().ToArray();
            //Tuple<Acontecimiento, AcontecimientoDetalle> Models = new 
            //    Tuple<Acontecimiento, AcontecimientoDetalle>(newAcontecimiento, new AcontecimientoDetalle());

            return View(newAcontecimiento);
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
                //string fileName = Server.MapPath(@"C:\\inetpub\\wwwroot\\CRM.IONICS\\Content\\TempFiles\\") + name;
                //string fileName = Server.MapPath("~/Content/TempFiles/") + name;
                string fileName = HostingEnvironment.MapPath("~/Content/TempFiles/") + name;
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
        public ActionResult Create(Acontecimiento Item1)
        {
            try
            {

                if (Item1.ArchivosModulo == null)
                    Item1.ArchivosModulo = new List<ArchivoModulo>().ToArray();

                ViewBag.TiposAcontecimiento = TipoAcontecimientoRepository.GetFiltered(x => x.Estado == true).ToList();
                ViewBag.Origenes = origenRepository.GetFiltered(x => x.Estado == true).ToList();
                if (ModelState.IsValid)
                {
                    if (acontecimientoRepository.GetFiltered(x => x.Descripcion.ToLower().Trim().Equals(Item1.Descripcion.ToLower().Trim())).Any())
                    {
                        ModelState.AddModelError(string.Empty, "El Acontecimiento ya ha sido utilizado!");
                        //Tuple<Acontecimiento, AcontecimientoDetalle> Models =
                        //            new Tuple<Acontecimiento, AcontecimientoDetalle>(Item1, new AcontecimientoDetalle());
                        return View(Item1);
                    }
                    else
                    {
                        if (Item1.Descripcion != "" && Item1.TipoAcontecimiento_Id != 0)
                        {
                            //Guardamos los datos
                            acontecimientoRepository.Add(Item1, this.User.UserID);

                            //AcontecimientoDetalle.Acontecimiento_ID = Item1.Id;
                            //acontecimientoDetalleRepository.Add(AcontecimientoDetalle, User.UserID);
                            //Archivos
                            if (Item1.ArchivosModulo != null)
                            {
                                foreach (ArchivoModulo newArchivo in Item1.ArchivosModulo)
                                {
                                    if (newArchivo.Deleted == false)
                                    {
                                        AltaArchivosModulo(newArchivo, Item1.Id.ToString());
                                    }
                                }
                            }


                            SetMessage(SUCCESS, "Acontecimiento Nro " + Item1.Id_Custom + " Guardado.");

                            return Index();
                        }
                        else
                        {
                            //Tuple<Acontecimiento, AcontecimientoDetalle> Models =
                            //        new Tuple<Acontecimiento, AcontecimientoDetalle>(Item1, new AcontecimientoDetalle());
                            return View(Item1);
                        }

                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El Modelo no es valido!");
                    //Tuple<Acontecimiento, AcontecimientoDetalle> Models = 
                    //    new Tuple<Acontecimiento, AcontecimientoDetalle>(Item1, new AcontecimientoDetalle());
                    return View(Item1);
                }
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, "Error al guardar acontecimiento :" + ex.Message + ex.StackTrace);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                //Tuple<Acontecimiento, AcontecimientoDetalle> Models =
                //        new Tuple<Acontecimiento, AcontecimientoDetalle>(Item1, new AcontecimientoDetalle());
                return View(Item1);
            }
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            var acontecimiento = acontecimientoRepository.Get(id.Value).First();
            //AcontecimientoDetalle acontecimiento_detalle = acontecimientoDetalleRepository.GetFiltered(ad => ad.Acontecimiento_ID == id.Value).First();
            ViewBag.TiposAcontecimiento = TipoAcontecimientoRepository.GetFiltered(x => x.Estado == true).ToList();
            ViewBag.Origenes = origenRepository.GetFiltered(x => x.Estado == true).ToList();
            string sId_Acontecimiento = acontecimiento.Id.ToString();
            acontecimiento.ArchivosModulo = archivoModuloRepository.GetFiltered(x => x.Entity_Id == sId_Acontecimiento && x.Estado == true
                        && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

            return View("Edit", acontecimiento);
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(Acontecimiento Item1)
        {
            if (Item1.ArchivosModulo == null)
                Item1.ArchivosModulo = new List<ArchivoModulo>().ToArray();

            ViewBag.Origenes = origenRepository.GetFiltered(x => x.Estado == true).ToList();

            try
            {
                acontecimientoRepository.Modify(Item1, User.UserID);
                //Item2.Acontecimiento_ID = Item1.Id;
                //acontecimientoDetalleRepository.Modify(Item2, User.UserID);
                if (Item1.ArchivosModulo != null)
                {
                    foreach (ArchivoModulo newArchivo in Item1.ArchivosModulo)
                    {
                        var id = Item1.Id.ToString();
                        var existe = archivoModuloRepository.GetAll().Any(a => a.Entity_Id == id);

                        if (newArchivo.Deleted == false 
                            && !archivoModuloRepository.GetAll().Any(a => a.Entity_Id == id && a.TipoArchivoModulo_Id == 19
                            && a.Estado == true))
                        {
                            AltaArchivosModulo(newArchivo, Item1.Id.ToString());
                        }
                        else
                        {
                            archivoModuloRepository.RemoveFromDataBase(newArchivo, User.UserID);
                        }

                    }
                }
                SetMessage(SUCCESS, "Acontecimiento Nro " + Item1.Id_Custom + " Guardado Correctamente.");
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, "Error al guardar acontecimiento :" + ex.Message);
                //Tuple<Acontecimiento, AcontecimientoDetalle> Models = new Tuple<Acontecimiento, AcontecimientoDetalle>(Item1, new AcontecimientoDetalle());
                ViewBag.TiposAcontecimiento = TipoAcontecimientoRepository.GetFiltered(x => x.Estado == true).ToList();
                return View(Item1);
            }
            return Index();
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Delete(int id)
        {
            try
            {
                var dtAcontecimiento = vacontecimientos.GetByID(id.ToString());
                if (vacontecimientos.Datos.Rows.Count == 0)
                    throw new Exception(" El Acontecimiento no existe.");

                Acontecimiento acontecimiento = new Acontecimiento();
                acontecimiento = acontecimientoRepository.Get(id).SingleOrDefault();
                acontecimientoRepository.Remove(acontecimiento, UserContext.UserID);
                SetMessage(SUCCESS, "Guardado Correctamente! Acontecimiento Nro: " + id);
            }
            catch (Exception ex) { SetMessage(ERROR, ex.Message); }


            return Index();
        }

        private static GridViewSettings GetSettingExport()
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = "grid_acontecimientos";
            settings.CallbackRouteValues = new { Controller = "Acontecimientos", Action = "GridViewAllPartial" };
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.SettingsSearchPanel.Visible = true;
            settings.SettingsBehavior.AllowFixedGroups = true;
            settings.SettingsBehavior.AllowDragDrop = true;
            settings.SettingsBehavior.AllowSort = true;

            settings.SettingsCookies.StoreFiltering = true;
            settings.SettingsCookies.StoreGroupingAndSorting = true;
            settings.SettingsCookies.StorePaging = true;
            settings.SettingsCookies.Enabled = true;
            settings.CustomJSProperties = (s, e) =>
            {
                MVCxGridView gridView = (MVCxGridView)s;
                e.Properties["cpClientLayout"] = gridView.SaveClientLayout();
            };

            settings.CommandColumn.Visible = true;
            settings.Settings.UseFixedTableLayout = true;
            settings.CommandColumn.Width = 50;

            settings.CommandColumn.ShowClearFilterButton = true;
            //CheckBox en cada fila y checkbox general
            settings.CommandColumn.ShowSelectCheckbox = true;
            settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
            settings.CommandColumn.HeaderStyle.BackColor = System.Drawing.Color.Orange;

            //settings.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.Control;
            settings.SettingsResizing.ColumnResizeMode = ColumnResizeMode.Control;

            settings.Settings.ShowGroupPanel = true;
            settings.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            settings.Settings.VerticalScrollableHeight = 300;

            settings.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            settings.SettingsPager.FirstPageButton.Visible = true;
            settings.SettingsPager.LastPageButton.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Items = new string[] { "10", "20", "50" };

            settings.KeyFieldName = "Id";
            settings.Columns.Add("Id");
            settings.Columns.Add("Fecha Acont.");
            settings.Columns.Add("Descripcion");
            settings.Columns.Add("TipoAcontecimiento");
            settings.Columns.Add("Fecha Apertura ");
            settings.Columns.Add("Fecha Ocurrencia ");
            settings.Columns.Add("Fecha Implementacion ");
            settings.Columns.Add("Estado"); ;

            //Fila de filtros
            settings.Settings.ShowFilterRow = true;
            //Icono del menu del filtrado
            settings.Settings.ShowFilterRowMenu = false;

            //Setear por defecto la condicion de filtrado
            //de todas las columnas a Contains
            settings.DataBound = (sender, e) =>
            {
                MVCxGridView gv = sender as MVCxGridView;
                gv.Visible = (gv.VisibleRowCount > 0);

                foreach (GridViewColumn column in gv.Columns)
                {
                    var dataColumn = column as GridViewDataColumn;
                    if (dataColumn != null)
                        dataColumn.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                }
            };


            settings.PreRender = (s, e) =>
            {
                MVCxGridView grid = s as MVCxGridView;
                if (grid != null)
                    grid.ExpandAll();
            };

            settings.SettingsDetail.ExportMode = GridViewDetailExportMode.Expanded;
            return settings;
        }

        [HttpPost]
        public ActionResult ExportXLS(GridViewExportedRowType exportRowType)
        {
            vacontecimientos.Datos = (DataTable)Session["ListadoAcontecimientos"];
            var setting = GridHelper.GetSettingExport(vacontecimientos.GetDynamicCollectionList(vacontecimientos.Datos), _ControllerName);
            setting.SettingsExport.ExportedRowType = exportRowType;
            //setting.SettingsExport.ExportSelectedRowsOnly = 
            // retornamos el excel al usuario
            return GridViewExtension.ExportToXls(setting, vacontecimientos.Datos, string.Format("{0}s_{1}.{2}", "Acontemiento", DateTime.Now.ToString("ddMMyyyy_HHmmss"), "xls"));
        }

        public ActionResult Imprimir(int id)
        {
            vacontecimientos.Datos = (DataTable)Session["ListadoAcontecimientos"];
            //var table = vacontecimientos.Datos.AsEnumerable().Where(a => a.Field<int>("Registro") == id).CopyToDataTable();
            var firmaGG = (string)Session["firmaGteGral"];    
            XtraReportNoConformidad reporte = new XtraReportNoConformidad();
            reporte.Parameters["Acontecimiento_Id"].Value = id;
            reporte.Parameters["Acontecimiento_Id"].Visible = false;
            reporte.Parameters["bFirmaGteGral"].Value = firmaGG == "true" ? true : false;
            reporte.Parameters["bFirmaGteGral"].Visible = false;
            reporte.ExportOptions.Html.TableLayout = false; 
            reporte.PaperKind = PaperKind.A4;

            Session["firmaGteGral"] = "false";
            Session["ReporteAcontecimiento"] = reporte;

            return View("Reporte", reporte);
        }
    }
}
