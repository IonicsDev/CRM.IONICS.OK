using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Website.Security.Infrastructure;
using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Business.Views;
using System.Net;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using DevExpress.Web;
using CRM.Website.DevExpressHelpers;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using System.Drawing.Printing;

namespace CRM.Website.Controllers
{
    public class EventualidadController : GenericController<Eventualidad>
    {
        vGetListadoEventualidades vGetListadoEventualidades = new vGetListadoEventualidades();
        EventualidadRepository eventualidadRepository = new EventualidadRepository();
        EventualidadUsuarioRepository eventualidadUsuarioRepository = new EventualidadUsuarioRepository();
        EventualidadContactoRepository eventualidadContactoRepository = new EventualidadContactoRepository();
        TipoEventualidadRepository tipoEventualidadRepository = new TipoEventualidadRepository();
        SubTipoEventualidadRepository subTipoEventualidadRepository = new SubTipoEventualidadRepository();
        RolEmpresaRepository rolEmpresaRepository = new RolEmpresaRepository();
        UsuarioRolClienteRepository usuarioRolClienteRepository = new UsuarioRolClienteRepository();
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        PedidoRepository pedidoRepository = new PedidoRepository();
        vClientes vClientes = new vClientes();
        vGetListadoPedidos vGetListadoPedidos = new vGetListadoPedidos();
        vDetalleEntrega vDetalleEntega = new vDetalleEntrega();
        vContactosSinFoto vContactosSinFoto = new vContactosSinFoto();
        ArchivoModuloRepository archivoRepository = new ArchivoModuloRepository();

        [LogonAuthorize(Roles = "VIEW,EDIT")]
        public ActionResult Index()
        {
            ClearTempFolder();
            GetEventualidades();
            Session["ReporteEventualidad"] = new XtraReportEventualidad();
            var datos = new DataTable();
           
            if (IsCliente)
            {
                List<EventualidadUsuario> eventualidadUsuarios = new List<EventualidadUsuario>();

                if (this.User != null)
                {
                    datos = vGetListadoEventualidades.GetListadoEventualidades_Usuario(this.User.UserID);
                    vGetListadoEventualidades.Datos.Clear();
                    vGetListadoEventualidades.Datos = datos;
                }

                Session["ListadoEventualidades"] = datos;

                return View("Index", vGetListadoEventualidades);
            }
            else
            {
               // List<Eventualidad> eventualidades = eventualidadRepository.GetFiltered(x => x.Estado == true).ToList();
                datos = vGetListadoEventualidades.GetListadoEventualidades();
                vGetListadoEventualidades.Datos.Clear();
                vGetListadoEventualidades.Datos = datos;
                Session["ListadoEventualidades"] = datos;
                return View("Index", vGetListadoEventualidades);
            }
            
        }

        private static GridViewSettings GetSettingExport()
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = "grid_eventualidades";
            settings.CallbackRouteValues = new { Controller = "Eventualidades", Action = "GridViewAllPartial" };
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
            settings.Columns.Add("Pedido");
            settings.Columns.Add("Cliente");
            settings.Columns.Add("Tipo_Eventualidad");
            settings.Columns.Add("SubTipo_Eventualidad");
            settings.Columns.Add("Descripcion_Y_Causas");
            settings.Columns.Add("Acciones_Inmediatas");
            settings.Columns.Add("Fecha_Ocurrencia");
            settings.Columns.Add("Fecha_Apertura");
            settings.Columns.Add("Usuario_Alta");
            
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
        public override ActionResult ExportXLS(GridViewExportedRowType exportRowType)
        {
            var setting = GetSettingExport();
            setting.SettingsExport.ExportedRowType = exportRowType;

            var tblListOfEventualidades = vGetListadoEventualidades.GetListadoEventualidades_Usuario(this.User.UserID);
            var listOfEventualidades = tblListOfEventualidades.AsEnumerable().Select(r => new
            {
                Id = Convert.ToInt32(r["Id"]),
                Pedido = Convert.ToInt32(r["Pedido"]),
                Cliente = Convert.ToString(r["Cliente"]),
                SubTipo_Eventualidad = Convert.ToString(r["Sub Tipo Eventualidad"]),
                Tipo_Eventualidad = Convert.ToString(r["Tipo Eventualidad"]),
                Descripcion_Y_Causas = Convert.ToString(r["Descripcion y Causas"]),
                Acciones_Inmediatas = Convert.ToString(r["Acciones Inmediatas"]),
                Fecha_Ocurrencia = Convert.ToDateTime(r["Fecha Ocurrencia"]),
                Fecha_Apertura = Convert.ToDateTime(r["Fecha Apertura"]),
                Usuario_Alta = Convert.ToString(r["Usuario Alta"])
            }).OrderByDescending(e => e.Id).ToList();
            //tblListOfEventualidades.Columns["ID"].ColumnName = "Id";

            return GridViewExtension.ExportToXls(setting, listOfEventualidades.ToList(), string.Format("Eventualidades_{0}.{1}", DateTime.Now.ToString("ddMMyyyy_HHmmss"), "xls"));
        }

        //MKN
        public ActionResult Imprimir(int id)
        {
            vGetListadoEventualidades.Datos = (DataTable)Session["ListadoEventualidades"];
            DataTable DtEventualidad = vGetListadoEventualidades.Datos.Clone();
            DtEventualidad.Columns.Clear();


            DtEventualidad.Columns.Add("ID", typeof(int));
            DtEventualidad.Columns.Add("Pedido", typeof(int));
            DtEventualidad.Columns.Add("Cliente", typeof(string));
            DtEventualidad.Columns.Add("Sub Tipo Eventualidad", typeof(string));
            DtEventualidad.Columns.Add("Tipo Eventualidad", typeof(string));
            DtEventualidad.Columns.Add("Descripcion y Causas", typeof(string));
            DtEventualidad.Columns.Add("Acciones Inmediatas", typeof(string));
            DtEventualidad.Columns.Add("Fecha Ocurrencia", typeof(DateTime));
            DtEventualidad.Columns.Add("Fecha Apertura", typeof(DateTime));
            DtEventualidad.Columns.Add("Usuario Alta", typeof(string));

            foreach (DataRow row in vGetListadoEventualidades.Datos.Rows)
            {
                if (row["ID"].ToString() == id.ToString())
                {
                    DtEventualidad.ImportRow(row);
                }
            }

            vGetListadoEventualidades vDtEventualidad = new vGetListadoEventualidades();
            vDtEventualidad.Datos = DtEventualidad;
            Session["DtEventualidadSeleccionada"] = DtEventualidad;

            return PartialView("_ImpresoraEventualidad", vDtEventualidad);
        }

        public ActionResult GridViewImpresora()
        {
            vGetListadoEventualidades vDtEventualidad = new vGetListadoEventualidades();
            vDtEventualidad.Datos = (DataTable)Session["DtEventualidadSeleccionada"];
            return PartialView("_GridViewEventualidad", vDtEventualidad);
        }

        [HttpPost]
        public ActionResult Imprimir(string btnImprimir)
        {
            DataTable dtEventualidad = (DataTable)Session["DtEventualidadSeleccionada"];
            //var setting = GridHelper.GetSettingExportPDF(vGetListadoPedidos.GetDynamicCollectionList(dtPedido), _ControllerName, true);
            //setting.SettingsExport.PaperKind = PaperKind.A4;
            XtraReportEventualidad reporte = new XtraReportEventualidad();
           
            reporte.Parameters["Eventualidad_Id"].Value = Convert.ToInt32(dtEventualidad.Rows[0][0]);
            reporte.Parameters["Eventualidad_Id"].Visible = false;
            reporte.ExportOptions.Html.TableLayout = false;
            reporte.PaperKind = PaperKind.A4;
            Session["ReporteEventualidad"] = reporte;
            
            return PartialView("DocumentViewerPartial", reporte);
        }

        public ActionResult GridViewAllPartial()
        {
            var datos = new DataTable();
           
            List<Eventualidad> eventualidades = new List<Eventualidad>();
            if (IsCliente )
            {
                List<EventualidadUsuario> eventualidadUsuarios = new List<EventualidadUsuario>();

                if (this.User != null)
                {

                    datos = vGetListadoEventualidades.GetListadoEventualidades_Usuario(this.User.UserID);
                    vGetListadoEventualidades.Datos.Clear();
                    vGetListadoEventualidades.Datos = datos;

                }

            }
            else
            {
                //eventualidades = eventualidadRepository.GetFiltered(x => x.Estado == true).ToList();

                datos = vGetListadoEventualidades.GetListadoEventualidades();
                vGetListadoEventualidades.Datos.Clear();
                vGetListadoEventualidades.Datos = datos;
            }

            return PartialView("_GridForViews", vGetListadoEventualidades);
        }

        //CREATE
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create()
        {
            if (!IsCliente)
                vClientes.Datos = new Business.Views.vClientes().GetViewModel();
            else
                vClientes.Datos = new Business.Views.vClientes(base.UserContext.RolesCliente.ToList()).GetViewModel();

            Eventualidad eventualidad = new Eventualidad();
            //if (vClientes.Datos.Rows.Count > 0)
            //{
            //    eventualidad.Cg_Clie = Convert.ToInt32(vClientes.Datos.Rows[0]["ID"]);
            //    eventualidad.Nombre_Cliente = vClientes.Datos.Rows[0]["Descripcion"].ToString();
            //}

            ViewBag.TiposEventualidad = tipoEventualidadRepository.GetFiltered(x => x.Estado == true).ToList();
            ViewBag.SubTiposEventualidad = new List<SubTipoEventualidad>();

            eventualidad.Pedido_Id = null;
            eventualidad.FechaOcurrencia = null;
            eventualidad.FechaApertura = DateTime.Now;
            eventualidad.ArchivosModulo = new List<ArchivoModulo>().ToArray();

            Usuario admin = usuarioRepository.GetFiltered(x => x.UserName.Trim().ToUpper().Equals("ADMIN")).FirstOrDefault();
            if (admin != null)
            {
                eventualidad.NotificarUsuarios_Id = admin.Id.ToString();
                eventualidad.NotificarUsuarios = admin.UserName + "; ";
            }

            return View("Create", eventualidad);
        }

        public ActionResult GetClientes()
        {
            if (!IsCliente)
                vClientes = new Business.Views.vClientes();
            else
                vClientes = new Business.Views.vClientes(base.UserContext.RolesCliente.ToList());

            return PartialView("_Clientes", vClientes);
        }

        public ActionResult GetPedidos(int CliID)
        {
            vGetListadoPedidos vPedidosCliente = new vGetListadoPedidos();
            vPedidosCliente.Datos = vGetListadoPedidos.Datos.Clone();
            vPedidosCliente.Datos.Rows.Clear();
            foreach (DataRow row in vGetListadoPedidos.Datos.Rows)
            {
                if (row["Cg_Cli"].ToString() == CliID.ToString())
                {
                    vPedidosCliente.Datos.ImportRow(row);
                }
                
                //foreach (UsuarioRolCliente cliente in this.User.RolesCliente)
                //{
                //    if (row["Cg_Cli"].ToString() == cliente.Cliente_Id.ToString())
                //    {
                //        vPedidosCliente.Datos.ImportRow(row);
                //    }
                //}
            }

            Session["PedidosCliente"] = vPedidosCliente.Datos;

            return PartialView("_Pedidos", vPedidosCliente);
        }

        public ActionResult GetDatosPedido(string Id_Pedido)
        {
            vGetListadoPedidos vPedidosCliente = new vGetListadoPedidos();
            DataTable dtPedido = vPedidosCliente.GetByID(Id_Pedido.ToString());

            string ordenFacturacion = "", producto = "", lote = "";
            if (dtPedido.Rows.Count > 0)
            {
                ordenFacturacion = dtPedido.Rows[0]["CodigoIonics"].ToString();
                producto = dtPedido.Rows[0]["Descripcion"].ToString();
                lote = dtPedido.Rows[0]["Lotes"].ToString();
            }

            string remito = "";
            DataTable dtDetalleEntrega = vDetalleEntega.DetalleEntrega(Convert.ToInt32(Id_Pedido));
            if (dtDetalleEntrega.Rows.Count > 0)
                remito = dtDetalleEntrega.Rows[0]["Remito"].ToString();

            string[] datos = new string[] { remito, producto, ordenFacturacion, lote };
            return Json(datos.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GridViewModalClientes()
        {
            if (!IsCliente )
                vClientes = new Business.Views.vClientes();
            else
                vClientes = new Business.Views.vClientes(base.UserContext.RolesCliente.ToList());

            return PartialView("_GridViewModalClientes", vClientes);
        }

        public ActionResult GridViewModalPedidos()
        {
            vGetListadoPedidos.Datos = (DataTable)Session["PedidosCliente"];
            return PartialView("_GridViewModalPedidos", vGetListadoPedidos);
        }

        public ActionResult GetSubTiposEventualidad(int TipoEventualidad_Id)
        {
            var subTipos = subTipoEventualidadRepository.GetFiltered(x => x.TipoEventualidad_Id == TipoEventualidad_Id
                && x.Estado == true).ToList();
                
            var listSubTipos = subTipos.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Descripcion }).ToList();

            return Json(listSubTipos.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            //if (IsAdmin)
                usuarios = usuarioRepository.GetFiltered(x => x.Estado == true && x.RolesEmpresa.Any(s => s.Rol_Id != 25)).ToList();
        

            return PartialView("_Usuarios", usuarios);
        }

        public ActionResult GetContactos(int CliID)
        {
            if (IsAdmin)
                vContactosSinFoto.Datos = vContactosSinFoto.GetViewModel();
            else
                vContactosSinFoto = vContactosSinFoto.GetByUserRol(base.UserContext.RolesCliente.ToList());

            vContactosSinFoto vContactosCliente = new vContactosSinFoto();
            vContactosCliente.Datos = vContactosSinFoto.Datos.Clone();
            vContactosCliente.Datos.Rows.Clear();

            foreach (DataRow row in vContactosSinFoto.Datos.Rows)
            {
                if (row["CodigoCliente"].ToString() == CliID.ToString())
                {
                    vContactosCliente.Datos.ImportRow(row);
                }
            }

            Session["vContactosCliente"] = vContactosCliente.Datos;

            return PartialView("_Contactos", vContactosCliente);
        }

        public ActionResult GridViewModalUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();
            //if (IsAdmin)
                usuarios = usuarioRepository.GetFiltered(x => x.Estado == true && x.RolesEmpresa.Any(s => s.Rol_Id != 25)).ToList();
            //else
            //    usuarios = GetUsuariosPermitidosByRolEmpresa().ToList();

            return PartialView("_GridViewModalUsuarios", usuarios);
        }

        public ActionResult GridViewModalContactos()
        {
            //if (IsAdmin)
            //    vContactosSinFoto.Datos = vContactosSinFoto.GetViewModel();
            //else
            //    vContactosSinFoto = vContactosSinFoto.GetByUserRol(base.UserContext.RolesCliente.ToList());
            vContactosSinFoto.Datos = ((DataTable)Session["vContactosCliente"]);
            return PartialView("_GridViewModalContactos", vContactosSinFoto);
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT" )]
        public ActionResult Create(Eventualidad eventualidad)
        {
            if (eventualidad.ArchivosModulo == null)
                eventualidad.ArchivosModulo = new List<ArchivoModulo>().ToArray();

            if (!ModelState.IsValid)
            {
                ViewBag.TiposEventualidad = tipoEventualidadRepository.GetFiltered(x => x.Estado == true).ToList();
                if (eventualidad.TipoEventualidad_Id == null)
                {
                    ViewBag.SubTiposEventualidad = new List<SubTipoEventualidad>();
                }
                else
                {
                    ViewBag.SubTiposEventualidad = subTipoEventualidadRepository
                        .GetFiltered(x => x.TipoEventualidad_Id == eventualidad.TipoEventualidad_Id && x.Estado == true).ToList();
                }

                return View(eventualidad);
            }

            try
            {
                eventualidadRepository.Add(eventualidad, this.User.UserID);

                Usuario userRemitente = usuarioRepository.Get(this.User.UserID).FirstOrDefault();
                string cuerpoMail = ArmarCuerpoMail(eventualidad);

                //Archivos
                if (eventualidad.ArchivosModulo != null)
                {
                    foreach (ArchivoModulo newArchivo in eventualidad.ArchivosModulo)
                    {
                        if (newArchivo.Deleted == false)
                        {
                            AltaArchivosModulo(newArchivo, eventualidad.Id.ToString());
                        }
                    }
                }

                //Guardo los usuarios y contactos.
                if (eventualidad.NotificarUsuarios_Id != null)
                {
                    string[] usuarios = eventualidad.NotificarUsuarios_Id.Split(',');
                    foreach (string User_Id in usuarios)
                    {
                        if (User_Id != "")
                        {
                            EventualidadUsuario eventUsuario = new EventualidadUsuario();
                            eventUsuario.Eventualidad_Id = eventualidad.Id;
                            eventUsuario.Usuario_Id = Convert.ToInt32(User_Id);
                            eventUsuario.Visto = false;

                            eventualidadUsuarioRepository.Add(eventUsuario, this.User.UserID);

                            Usuario userDestinatario = usuarioRepository.Get(Convert.ToInt32(User_Id)).FirstOrDefault();
                            if (userDestinatario != null)
                            {
                                try
                                {
                                    SendEmails("Eventualidad: N° " + eventualidad.Id.ToString() + " del cliente " + eventualidad.Nombre_Cliente, userRemitente.Email, userDestinatario.Email, cuerpoMail, eventualidad.ArchivosModulo.ToList());
                                }
                                catch (Exception) { }
                            }
                        }
                    } 
                }

                if (eventualidad.NotificarContactos_Id != null)
                {
                    string[] contactos = eventualidad.NotificarContactos_Id.Split(',');
                    foreach (string Contacto_Id in contactos)
                    {
                        if (Contacto_Id != "")
                        {
                            EventualidadContacto eventContacto = new EventualidadContacto();
                            eventContacto.Eventualidad_Id = eventualidad.Id;
                            eventContacto.Contacto_Id = Convert.ToInt32(Contacto_Id);
                            eventContacto.Visto = false;

                            eventualidadContactoRepository.Add(eventContacto, this.User.UserID);

                            DataTable dtContacto = vContactosSinFoto.GetByID(Convert.ToInt32(Contacto_Id));
                            if (dtContacto.Rows.Count > 0)
                            {
                                string emailContacto = dtContacto.Rows[0]["Email"].ToString().Trim();
                                try
                                {
                                    string[] emailContactos_Array = emailContacto.Split(';');

                                    List<string> validContactos = new List<string>();
                                    foreach (string cont in emailContactos_Array)
                                    {
                                        if (IsEmail(cont.Trim()))
                                            validContactos.Add(cont.Trim());
                                    }
                                    string recipients = string.Join(",", validContactos.ToArray());

                                    SendEmails("Eventualidad: N° " + eventualidad.Id.ToString() + " del cliente " + eventualidad.Nombre_Cliente, userRemitente.Email, recipients, cuerpoMail, eventualidad.ArchivosModulo.ToList());
                                }
                                catch (Exception) { }
                            }
                        }
                    } 
                }
                SendEmails("Eventualidad: N° " + eventualidad.Id.ToString() + " del cliente " + eventualidad.Nombre_Cliente, userRemitente.Email, ConfigurationManager.AppSettings["COMERCIALES.EVENTUALIDAD"].ToString(), cuerpoMail, eventualidad.ArchivosModulo.ToList());                
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                ViewBag.TiposEventualidad = tipoEventualidadRepository.GetFiltered(x => x.Estado == true).ToList();
                if (eventualidad.TipoEventualidad_Id == null)
                {
                    ViewBag.SubTiposEventualidad = new List<SubTipoEventualidad>();
                }
                else
                {
                    ViewBag.SubTiposEventualidad = subTipoEventualidadRepository
                        .GetFiltered(x => x.TipoEventualidad_Id == eventualidad.TipoEventualidad_Id && x.Estado == true).ToList();
                }

                if (eventualidad.Id > 0)
                {
                    string sEventualidad_Id = eventualidad.Id.ToString();
                    eventualidad.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == sEventualidad_Id && x.Estado == true
                        && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();
                }

                return View(eventualidad);
            }

            SetMessage(SUCCESS, "Guardado.");
            return Index();
        }

        public string ArmarCuerpoMail(Eventualidad eventualidad)
        {
            //El mail va a contener el id del pedido, el/los productos, la desc de la eventualidad
                //y el link que lleve a la eventualidad.
            var fullUrl = this.Url.Action("View", "Eventualidad", new { Id = eventualidad.Id }, this.Request.Url.Scheme);

            string causas = "";
            string descripcion = "";
            string accionInmediata = "";

            if (eventualidad.Descripcion != null)
                descripcion = eventualidad.Descripcion;

            if (eventualidad.AccionesInmediatas != null)
                accionInmediata = eventualidad.AccionesInmediatas;

            if (eventualidad.Causas != null)
                causas = eventualidad.Causas;

            string cuerpo = "<b>Eventualidad: N° </b>" + eventualidad.Id.ToString() + " <b>del cliente </b> " + eventualidad.Nombre_Cliente + "<br>" +
                "<b>Pedido: </b>" + eventualidad.Pedido_Id + "<br>" +
                "<b>Producto: </b>" + eventualidad.NombreProducto + "<br>" +
                "<b>Tipo: </b>" + eventualidad.NombreTipoEventualidad + "<br>" +
                "<b>Sub Tipo: </b>" + eventualidad.NombreSubTipoEventualidad + "<br>" +
                "<b>Descripcion: </b>" + descripcion + "<br>" +
                "<b>Accion Inmediata: </b>" + accionInmediata + "<br>" +
                "<b>Fecha Ocurrencia: </b>" + eventualidad.FechaOcurrencia.Value.ToString("dd/MM/yyyy") + "<br>" +
                "<b>Link: </b>" + fullUrl.ToString() + "<br>";

            return cuerpo;
        }

        private void SendEmails(string asunto, string remitente, string destinatario, string cuerpo, List<ArchivoModulo> archivosModulo)
        {
            //string[] ems = { "fbriolfo@rt-solutions.com.ar", "prueba.ionics@gmail.com", "facu.riolfo@gmail.com", "facu.riolfo@hotmail.com", "facundobarriosriolfo@hotmail.com"};
            if (destinatario != "")
            {
            //    foreach(string em in ems)
            //{
               // var asunto = ConfigurationManager.AppSettings["ASUNTO.EVENTUALIDAD"];
                MailMessage mm = new MailMessage(remitente, destinatario, asunto, cuerpo);
                //MailMessage mm = new MailMessage(remitente, em, asunto, cuerpo);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mm.IsBodyHtml = true;
                //mm.CC.Add(ConfigurationManager.AppSettings["COMERCIALES.EVENTUALIDAD"].ToString());

                foreach (ArchivoModulo adjunto in archivosModulo)
                {
                    string fileName = Server.MapPath("~/Content/Files/") + adjunto.Path;
                    mm.Attachments.Add(new Attachment(fileName));
                }

                SendEmailInBackgroundThread(mm);
            }
        }

        public void SendEmailInBackgroundThread(object mm)
        {
            Task beginSendEmailTask = new Task(SendEmail, mm);
            beginSendEmailTask.Start();
        }

        public void SendEmail(object mm)
        {
            Thread.Sleep(5000);
            MailMessage mailMessage = (MailMessage)mm;
            SmtpClient ss = new SmtpClient("smtp.gmail.com", 587);
            ss.EnableSsl = true;
            ss.Timeout = 10000;
            ss.DeliveryMethod = SmtpDeliveryMethod.Network;
            ss.UseDefaultCredentials = true;
            var mail = ConfigurationManager.AppSettings["ADMIN.MAIL"];
            var password = ConfigurationManager.AppSettings["ADMIN.MAILPASS"];

            ss.Credentials = new NetworkCredential(mail, password);

            ss.SendCompleted += (s, e) =>
            {
                ss.Dispose();
                mailMessage.Dispose();
            };

            ss.SendAsync(mailMessage, mailMessage);
        }

        //EDIT
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventualidad eventualidad = eventualidadRepository.Get(id.Value, x => x.Usuario, x => x.SubTipoEventualidad).SingleOrDefault();
            eventualidad.TipoEventualidad_Id = eventualidad.SubTipoEventualidad.TipoEventualidad_Id;

            try
            {
                if (eventualidad == null)
                {
                    throw new Exception(" La Eventualidad no existe");
                }

                //if ((!IsAdmin && !IsCalidad ) && !(eventualidad.idUsuario.Value == base.UserContext.UserID))
                //    throw new Exception(" No tiene permiso para Editar esta eventualidad");

                DataTable dtCliente = vClientes.GetByID(eventualidad.Cg_Clie);
                if (dtCliente.Rows.Count > 0)
                    eventualidad.Nombre_Cliente = dtCliente.Rows[0]["Descripcion"].ToString();

                vGetListadoPedidos vPedidosCliente = new vGetListadoPedidos();
                DataTable dtPedido = vPedidosCliente.GetByID(eventualidad.Pedido_Id.ToString());

                string ordenFacturacion = "", producto = "", lote = "";
                if (dtPedido.Rows.Count > 0)
                {
                    ordenFacturacion = dtPedido.Rows[0]["CodigoIonics"].ToString();
                    producto = dtPedido.Rows[0]["Descripcion"].ToString();
                    lote = dtPedido.Rows[0]["Lotes"].ToString();
                }

                string remito = "";
                DataTable dtDetalleEntrega = vDetalleEntega.DetalleEntrega(eventualidad.Pedido_Id.Value);
                if (dtDetalleEntrega.Rows.Count > 0)
                    remito = dtDetalleEntrega.Rows[0]["Remito"].ToString();

                eventualidad.Remito = remito;
                eventualidad.NombreProducto = producto;
                eventualidad.LoteCliente = lote;
                eventualidad.OrdenFabricacion = ordenFacturacion;

                ViewBag.TiposEventualidad = tipoEventualidadRepository.GetFiltered(x => x.Estado == true).ToList();
                ViewBag.SubTiposEventualidad = subTipoEventualidadRepository.GetFiltered(x => x.Estado == true
                    && x.TipoEventualidad_Id == eventualidad.SubTipoEventualidad.TipoEventualidad_Id).ToList();

                string sId_Eventualidad = eventualidad.Id.ToString();
                eventualidad.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == sId_Eventualidad && x.Estado == true
                            && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

                List<EventualidadUsuario> eventUsuarios = eventualidadUsuarioRepository
                    .GetFiltered(x => x.Eventualidad_Id == eventualidad.Id, x => x.Usuario).ToList();

                List<string> arrayUsuariosId = new List<string>();
                foreach (var eventUser in eventUsuarios)
                {
                    arrayUsuariosId.Add(eventUser.Usuario_Id.ToString());
                    eventualidad.NotificarUsuarios += eventUser.Usuario.UserName + "; ";
                }
                eventualidad.NotificarUsuarios_Id = string.Join(",", arrayUsuariosId.ToArray());

                if (eventualidad.NotificarUsuarios == null)
                {
                    Usuario admin = usuarioRepository.GetFiltered(x => x.UserName.Trim().ToUpper().Equals("ADMIN")).FirstOrDefault();
                    if (admin != null)
                    {
                        eventualidad.NotificarUsuarios_Id = admin.Id.ToString();
                        eventualidad.NotificarUsuarios = admin.UserName + "; ";
                    }
                }

                List<EventualidadContacto> eventContactos = eventualidadContactoRepository
                    .GetFiltered(x => x.Eventualidad_Id == eventualidad.Id).ToList();

                List<string> arrayContactosId = new List<string>();
                foreach (var eventContacto in eventContactos)
                {
                    DataTable dtContacto = vContactosSinFoto.GetByID(eventContacto.Contacto_Id);
                    if (dtCliente.Rows.Count > 0)
                    {
                        string Id_Contacto = dtCliente.Rows[0]["Id"].ToString();
                        arrayContactosId.Add(Id_Contacto);
                        string contacto = dtCliente.Rows[0]["Descripcion"].ToString();
                        eventualidad.NotificarUsuarios += contacto + "; ";
                    }
                }
                eventualidad.NotificarContactos_Id = string.Join(",", arrayContactosId.ToArray());
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                return Index();
            }

            return View("Edit", eventualidad);
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(Eventualidad eventualidad)
        {
            if (eventualidad.ArchivosModulo == null)
                eventualidad.ArchivosModulo = new List<ArchivoModulo>().ToArray();

            if (!ModelState.IsValid)
            {
                ViewBag.TiposEventualidad = tipoEventualidadRepository.GetFiltered(x => x.Estado == true).ToList();
                if (eventualidad.TipoEventualidad_Id == null)
                {
                    ViewBag.SubTiposEventualidad = new List<SubTipoEventualidad>();
                }
                else
                {
                    ViewBag.SubTiposEventualidad = subTipoEventualidadRepository
                        .GetFiltered(x => x.TipoEventualidad_Id == eventualidad.TipoEventualidad_Id && x.Estado == true).ToList();
                }

                return View(eventualidad);
            }

            try
            {
                eventualidadRepository.Modify(eventualidad, this.User.UserID);

                Usuario userRemitente = usuarioRepository.Get(this.User.UserID).FirstOrDefault();
                string cuerpoMail = ArmarCuerpoMail(eventualidad);

                //Modifico los Archivos, los que ya no estan en el listado los elimino
                //y los nuevos los agrego
                if (eventualidad.ArchivosModulo != null)
                {
                    foreach (ArchivoModulo archivoModulo in eventualidad.ArchivosModulo)
                    {
                        if ((archivoRepository.GetFiltered(x => x.Id == archivoModulo.Id && x.Estado == true).Any() == false) 
                            && archivoModulo.Deleted != true)
                        {
                            AltaArchivosModulo(archivoModulo, eventualidad.Id.ToString());
                        }

                        if (archivoModulo.Deleted == true)
                        {
                            archivoRepository.Remove(archivoModulo, this.User.UserID);
                        }
                    }
                }

                //Elimino los usuarios y contactos antes existentes
                List<EventualidadUsuario> eventUsuarios = eventualidadUsuarioRepository
                    .GetFiltered(x => x.Eventualidad_Id == eventualidad.Id).ToList();
                foreach(var eventUser in eventUsuarios)
                {
                    eventualidadUsuarioRepository.RemoveFromDataBase(eventUser, this.User.UserID);
                }
                List<EventualidadContacto> eventContactos = eventualidadContactoRepository
                    .GetFiltered(x => x.Eventualidad_Id == eventualidad.Id).ToList();
                foreach (var eventContacto in eventContactos)
                {
                    eventualidadContactoRepository.RemoveFromDataBase(eventContacto, this.User.UserID);
                }

                //Guardo los usuarios y contactos.
                if (eventualidad.NotificarUsuarios_Id != null)
                {
                    string[] usuarios = eventualidad.NotificarUsuarios_Id.Split(',');
                    foreach (string User_Id in usuarios)
                    {
                        if (User_Id != "")
                        {
                            EventualidadUsuario eventUsuario = new EventualidadUsuario();
                            eventUsuario.Eventualidad_Id = eventualidad.Id;
                            eventUsuario.Usuario_Id = Convert.ToInt32(User_Id);
                            eventUsuario.Visto = false;

                            eventualidadUsuarioRepository.Add(eventUsuario, this.User.UserID);

                            Usuario userDestinatario = usuarioRepository.Get(Convert.ToInt32(User_Id)).FirstOrDefault();
                            if (userDestinatario != null)
                            {
                                try
                                {
                                    SendEmails("Eventualidad: N° " + eventualidad.Id.ToString() + " del cliente " + eventualidad.Nombre_Cliente, userRemitente.Email, userDestinatario.Email, cuerpoMail, eventualidad.ArchivosModulo.ToList());
                                }
                                catch (Exception) { }
                            }
                        }
                    }
                }

                if (eventualidad.NotificarContactos_Id != null)
                {
                    string[] contactos = eventualidad.NotificarContactos_Id.Split(',');
                    foreach (string Contacto_Id in contactos)
                    {
                        if (Contacto_Id != "")
                        {
                            EventualidadContacto eventContacto = new EventualidadContacto();
                            eventContacto.Eventualidad_Id = eventualidad.Id;
                            eventContacto.Contacto_Id = Convert.ToInt32(Contacto_Id);
                            eventContacto.Visto = false;

                            eventualidadContactoRepository.Add(eventContacto, this.User.UserID);

                            DataTable dtContacto = vContactosSinFoto.GetByID(Convert.ToInt32(Contacto_Id));
                            if (dtContacto.Rows.Count > 0)
                            {
                                string emailContacto = dtContacto.Rows[0]["Email"].ToString().Trim();
                                try
                                {
                                    string[] emailContactos_Array = emailContacto.Split(';');

                                    List<string> validContactos = new List<string>();
                                    foreach (string cont in emailContactos_Array)
                                    {
                                        if (IsEmail(cont.Trim()))
                                            validContactos.Add(cont.Trim());
                                    }
                                    string recipients = string.Join(",", validContactos.ToArray());

                                    SendEmails("Eventualidad: N° " + eventualidad.Id.ToString() + " del cliente " + eventualidad.Nombre_Cliente ,userRemitente.Email, recipients, cuerpoMail, eventualidad.ArchivosModulo.ToList());
                                }
                                catch (Exception) { }
                            }
                        }
                    }
                }

                SendEmails("Eventualidad: N° " + eventualidad.Id.ToString() + " del cliente " + eventualidad.Nombre_Cliente, userRemitente.Email, ConfigurationManager.AppSettings["COMERCIALES.EVENTUALIDAD"].ToString(), cuerpoMail, eventualidad.ArchivosModulo.ToList());
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                ViewBag.TiposEventualidad = tipoEventualidadRepository.GetFiltered(x => x.Estado == true).ToList();
                if (eventualidad.TipoEventualidad_Id == null)
                {
                    ViewBag.SubTiposEventualidad = new List<SubTipoEventualidad>();
                }
                else
                {
                    ViewBag.SubTiposEventualidad = subTipoEventualidadRepository
                        .GetFiltered(x => x.TipoEventualidad_Id == eventualidad.TipoEventualidad_Id && x.Estado == true).ToList();
                }

                if (eventualidad.Id > 0)
                {
                    string sEventualidad_Id = eventualidad.Id.ToString();
                    eventualidad.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == sEventualidad_Id && x.Estado == true
                        && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();
                }

                return View(eventualidad);
            }

            SetMessage(SUCCESS, "Guardado.");
            return Index();
        }

        //VIEW
        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventualidad eventualidad = eventualidadRepository.Get(id.Value, x => x.Usuario, x => x.SubTipoEventualidad).SingleOrDefault();
            eventualidad.TipoEventualidad_Id = eventualidad.SubTipoEventualidad.TipoEventualidad_Id;

            try
            {
                if (eventualidad == null)
                {
                    throw new Exception(" La Eventualidad no existe");
                }

                List<EventualidadUsuario> eventUsuarios = eventualidadUsuarioRepository
                        .GetFiltered(x => x.Eventualidad_Id == eventualidad.Id, x => x.Usuario).ToList();


                //Permiso en Eventualidad sobre usuarios
                //if (!IsAdmin && eventUsuarios.Any(s => s.Usuario_Id == this.User.UserID))
                //    throw new Exception(" No tiene permiso para ver esta eventualidad");

                //Marco como vista a la eventualidad por parte del usuario
                List<EventualidadUsuario> eventualidadUsuario = eventualidadUsuarioRepository
                    .GetFiltered(x => x.Eventualidad_Id == eventualidad.Id && x.Usuario_Id == this.User.UserID).ToList();
                foreach (var even in eventualidadUsuario)
                {
                    even.Visto = true;
                    eventualidadUsuarioRepository.Modify(even, this.User.UserID);
                }
              

                DataTable dtCliente = vClientes.GetByID(eventualidad.Cg_Clie);
                if (dtCliente.Rows.Count > 0)
                    eventualidad.Nombre_Cliente = dtCliente.Rows[0]["Descripcion"].ToString();

                vGetListadoPedidos vPedidosCliente = new vGetListadoPedidos();
                DataTable dtPedido = vPedidosCliente.GetByID(eventualidad.Pedido_Id.ToString());

                string ordenFacturacion = "", producto = "", lote = "";
                if (dtPedido.Rows.Count > 0)
                {
                    ordenFacturacion = dtPedido.Rows[0]["CodigoIonics"].ToString();
                    producto = dtPedido.Rows[0]["Descripcion"].ToString();
                    lote = dtPedido.Rows[0]["Lotes"].ToString();
                }

                string remito = "";
                DataTable dtDetalleEntrega = vDetalleEntega.DetalleEntrega(eventualidad.Pedido_Id.Value);
                if (dtDetalleEntrega.Rows.Count > 0)
                    remito = dtDetalleEntrega.Rows[0]["Remito"].ToString();

                eventualidad.Remito = remito;
                eventualidad.NombreProducto = producto;
                eventualidad.LoteCliente = lote;
                eventualidad.OrdenFabricacion = ordenFacturacion;

                ViewBag.TiposEventualidad = tipoEventualidadRepository.GetFiltered(x => x.Estado == true).ToList();
                ViewBag.SubTiposEventualidad = subTipoEventualidadRepository.GetFiltered(x => x.Estado == true
                    && x.TipoEventualidad_Id == eventualidad.SubTipoEventualidad.TipoEventualidad_Id).ToList();

                string sId_Eventualidad = eventualidad.Id.ToString();
                eventualidad.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == sId_Eventualidad && x.Estado == true
                            && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

                foreach (var eventUser in eventUsuarios)
                {
                    eventualidad.NotificarUsuarios += eventUser.Usuario.UserName + "; ";
                }

                List<EventualidadContacto> eventContactos = eventualidadContactoRepository
                    .GetFiltered(x => x.Eventualidad_Id == eventualidad.Id).ToList();
                foreach (var eventContacto in eventContactos)
                {
                    DataTable dtContacto = vContactosSinFoto.GetByID(eventContacto.Contacto_Id);
                    if (dtCliente.Rows.Count > 0)
                    {
                        string contacto = dtCliente.Rows[0]["Descripcion"].ToString();
                        eventualidad.NotificarUsuarios += contacto + "; ";
                    }
                }
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
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                Eventualidad eventualidad = eventualidadRepository.Get(id.Value).SingleOrDefault();
                if (eventualidad == null)
                    throw new Exception(" La Eventualidad no existe");

                if (!IsAdmin && !(eventualidad.idUsuario.Value == base.UserContext.UserID))
                    throw new Exception(" No tiene permiso para Eliminar esta eventualidad");

                eventualidadRepository.Remove(eventualidad, this.User.UserID);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                return Index();
            }

            return RedirectToAction("Index");
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

        public ActionResult AddArchivoModulo()
        {
            ViewBag.TipoArchivoModulo = _ControllerName;

            return PartialView("_FormArchivo", new ArchivoModulo());
        }
        
        public ActionResult DocumentViewerPartialbak()
        {
            var dtEventualidad = (DataTable)Session["DtEventualidadSeleccionada"];
            var reporte = (XtraReportEventualidad)Session["ReporteEventualidad"];
            reporte.Parameters["Eventualidad_Id"].Value = Convert.ToInt32(dtEventualidad.Rows[0][0]);
            reporte.Parameters["Eventualidad_Id"].Visible = false;
            reporte.PaperKind = PaperKind.A4;
            return PartialView("DocumentViewerPartial", reporte);
        }

        public ActionResult ExportDocumentViewer()
        {
            var dtEventualidad = (DataTable)Session["DtEventualidadSeleccionada"];
            var reporte = (XtraReportEventualidad)Session["ReporteEventualidad"];
            reporte.ExportOptions.Html.TableLayout = false;
            reporte.Parameters["Eventualidad_Id"].Value = Convert.ToInt32(dtEventualidad.Rows[0][0]);
            reporte.Parameters["Eventualidad_Id"].Visible = false;
            reporte.PaperKind = PaperKind.A4;
            return DocumentViewerExtension.ExportTo(reporte, Request);
        }

        
        public ActionResult DocumentViewerPartial()
        {
            //var dtEventualidad = (DataTable)Session["DtEventualidadSeleccionada"];
            var reporte = (XtraReportEventualidad)Session["ReporteEventualidad"];
            //reporte.Parameters["Eventualidad_Id"].Value = Convert.ToInt32(dtEventualidad.Rows[0][0]);
            //reporte.Parameters["Eventualidad_Id"].Visible = false;
            reporte.ExportOptions.Html.TableLayout = false;
            reporte.PaperKind = PaperKind.A4;
            return PartialView("_DocumentViewerPartial", reporte);
        }

        public ActionResult DocumentViewerPartialExport()
        {
            var reporte = (XtraReportEventualidad)Session["ReporteEventualidad"];
            reporte.ExportOptions.Html.TableLayout = false;
            reporte.PaperKind = PaperKind.A4;
            return DocumentViewerExtension.ExportTo(reporte, Request);
        }
    }
}
