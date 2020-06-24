using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Business.Entities;
using CRM.Website.Security.Infrastructure;
using CRM.Business.DAL;
using System.IO;
using CRM.Business.Views;
using System.Net;
using System.Data;
using DevExpress.Web;
using CRM.Website.DevExpressHelpers;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrintingLinks;
using System.Drawing.Printing;
using System.Drawing;
using System.Threading;
using System.Net.Mail;
using System.Configuration;
using System.Threading.Tasks;

namespace CRM.Website.Controllers
{
    public class PedidoProcesoController : BaseController
    {
        PedidoRepository pedidoRepository = new PedidoRepository();
        PedidoDetalleRepository pedidoDetalleRepository = new PedidoDetalleRepository();
        PedidoEstadoRepository pedidoEstadoRepository = new PedidoEstadoRepository();
        vPedidosFechaMaximaAnticipacion vPedidosFechaMaximaAnticipacion = new vPedidosFechaMaximaAnticipacion();
        vClientes vClientes = new vClientes();
        vProductos vProductos = new vProductos();
        vProductosAll vProductosAll = new vProductosAll();
        RolEmpresaRepository rolEmpresaRepository = new RolEmpresaRepository();
        UsuarioRolClienteRepository usuarioRolClienteRepository = new UsuarioRolClienteRepository();
        vGetProductosCliente vGetProductosCliente = new vGetProductosCliente();
        vGetListadoPedidos vGetListadoPedidos = new vGetListadoPedidos();
        vDetalleEntrega vDetalleEntrega = new vDetalleEntrega();
        vDetalleFactura vDetalleFactura = new vDetalleFactura();
        ArchivoModuloRepository archivoRepository = new ArchivoModuloRepository();
        ColorEstadoRepository colorEstadoRepository = new ColorEstadoRepository();
        EstadoPedidoRepository estadoPedidoRepository = new EstadoPedidoRepository();
        ParametroRepository parametroRepository = new ParametroRepository();
        UsuarioRepository usuarioRepository = new UsuarioRepository();

        [LogonAuthorize(Roles = "VIEW,EDIT")]
        public ActionResult IndexRefrech(bool refresh)
        {
            var item = parametroRepository.GetValueByParamName("CantItem");
            Session["CantItem"] = item;
            Session["ListadoPedidos"] = null;
            vGetListadoPedidos vPedidosCliente = new vGetListadoPedidos();

            //** COMENTADO PORQUE TARDA MAS DE 25 SEG EL SP
            //Actualizamos el estado de los pedidos
            pedidoEstadoRepository.ActualizarEstadosPedidosSP();

            var datos = new DataTable();
            if (Session["ddlEstadosFilter"] != null)
            {
                if (Session["ddlEstadosFilter"].ToString().Trim() != string.Empty)
                {
                    ViewBag.ddlEstados = Session["ddlEstadosFilter"].ToString();
                    datos = vPedidosCliente.GetListadoPedidos(Session["ddlEstadosFilter"].ToString());
                }

            }
            else
            {
                datos = vPedidosCliente.GetListadoPedidos();
            }



            //if (!IsAdmin)
            if(IsCliente)
            {
                vPedidosCliente.Datos.Clear();
                foreach (DataRow row in datos.Rows)
                {
                    foreach (UsuarioRolCliente cliente in base.UserContext.RolesCliente)
                    {

                        if (row["Cg_Cli"].ToString() == cliente.Cliente_Id.ToString())
                        {
                            vPedidosCliente.Datos.ImportRow(row);
                        }


                    }
                }
            }
            else
            {
                vPedidosCliente.Datos.Clear();
                vPedidosCliente.Datos = datos;
            }

            SetDropDownEstados();

            Session["ListadoPedidos"] = vPedidosCliente.Datos;
            ViewBag.ColoresEstado = colorEstadoRepository.GetAll().ToList();
            if (IsAdmin || IsOperador || IsProduccion)
                ViewBag.IsAdminOperador = true;
            else
                ViewBag.IsAdminOperador = false;
            return View("Index", vPedidosCliente);
        }

        [LogonAuthorize(Roles = "VIEW,EDIT")]
        public ActionResult Index()
        {
            var item = parametroRepository.GetValueByParamName("CantItem");
            Session["CantItem"] = item;
                
            
            vGetListadoPedidos vPedidosCliente = new vGetListadoPedidos();
            var datos = new DataTable();
            if (Session["ddlEstadosFilter"] != null)
            {
                if (Session["ddlEstadosFilter"].ToString().Trim() != string.Empty)
                {
                    ViewBag.ddlEstados = Session["ddlEstadosFilter"].ToString();
                    datos = vPedidosCliente.GetListadoPedidos(Session["ddlEstadosFilter"].ToString());
                }
                
            }
            else
            {
                datos = vPedidosCliente.GetListadoPedidos();
            }
            
            //if (!IsAdmin )
            if(IsCliente)
            {
                vPedidosCliente.Datos.Clear();
                foreach (DataRow row in datos.Rows)
                {
                    foreach (UsuarioRolCliente cliente in base.UserContext.RolesCliente)
                    {

                        if (row["Cg_Cli"].ToString() == cliente.Cliente_Id.ToString())
                        {
                          
                            vPedidosCliente.Datos.ImportRow(row);
                        }
                        
                    }
                }
            }
            else
            {
                vPedidosCliente.Datos.Clear();
                vPedidosCliente.Datos = datos;
            }

            SetDropDownEstados();

            Session["ListadoPedidos"] = vPedidosCliente.Datos;
            ViewBag.ColoresEstado = colorEstadoRepository.GetAll().ToList();

            if (IsAdmin || IsOperador || IsProduccion)
                ViewBag.IsAdminOperador = true;
            else
                ViewBag.IsAdminOperador = false;

            return View("Index", vPedidosCliente);
        }

        public void SetDropDownEstados()
        {

            List<KeyValuePair<string, string>> estados = new List<KeyValuePair<string, string>>();

            estados.Add(new KeyValuePair<string, string>("(1 = 1)", "Todos"));
            estados.Add(new KeyValuePair<string, string>("(FechaAutorizadaRecepcion is null AND Estado = 0)", "Sin autorizar"));
            estados.Add(new KeyValuePair<string, string>("(FechaAutorizadaRecepcion is not null  AND Estado = 0)", "Autorizados"));
            estados.Add(new KeyValuePair<string, string>("(FechaAutorizadaRecepcion is not null AND Estado in (1,4, 9) )", "En Ionics"));


            SelectList sEstadosPedido = new SelectList(estados);
            ViewBag.EstadosPedido = estados;

        }

        [LogonAuthorize(Roles = "VIEW,EDIT")]
        [HttpPost]
        public ActionResult Index(string ddlEstados)
        {
            var item = parametroRepository.GetValueByParamName("CantItem");
            Session["CantItem"] = item;

            vGetListadoPedidos vPedidosCliente = new vGetListadoPedidos();
            DataTable datos = new DataTable();
            if (ddlEstados.Trim() != string.Empty)
            {
                Session["ddlEstadosFilter"] = ddlEstados;
                datos = vPedidosCliente.GetListadoPedidos(ddlEstados);
            }
            else
                datos = vPedidosCliente.GetListadoPedidos();

            //if (!IsAdmin)
            if(IsCliente)
            {
                vPedidosCliente.Datos.Clear();
                foreach (DataRow row in datos.Rows)
                {
                    foreach (UsuarioRolCliente cliente in base.UserContext.RolesCliente)
                    {

                        if (row["Cg_Cli"].ToString() == cliente.Cliente_Id.ToString())
                        {
                            vPedidosCliente.Datos.ImportRow(row);
                        }
                    }
                }
            }
            else
            {
                vPedidosCliente.Datos.Clear();
                vPedidosCliente.Datos = datos;
            }
           // vPedidosCliente.Datos = datos;
            Session["ListadoPedidos"] = vPedidosCliente.Datos;
            ViewBag.ColoresEstado = colorEstadoRepository.GetAll().ToList();
            SetDropDownEstados();

            if (IsAdmin || IsOperador || IsProduccion)
                ViewBag.IsAdminOperador = true;
            else
                ViewBag.IsAdminOperador = false;
            

            return View("Index", vPedidosCliente);

        }

        public ActionResult Refresh()
        {
            var item = parametroRepository.GetValueByParamName("CantItem");
            Session["CantItem"] = item;
            Session["ListadoPedidos"] = null;
            vGetListadoPedidos vPedidosCliente = new vGetListadoPedidos();
            
            //** COMENTADO PORQUE TARDA MAS DE 25 SEG EL SP
            //Actualizamos el estado de los pedidos
           pedidoEstadoRepository.ActualizarEstadosPedidosSP();

            var datos = new DataTable();
            if (Session["ddlEstadosFilter"] != null)
            {
                if (Session["ddlEstadosFilter"].ToString().Trim() != string.Empty)
                {
                    ViewBag.ddlEstados = Session["ddlEstadosFilter"].ToString();
                    datos = vPedidosCliente.GetListadoPedidos(Session["ddlEstadosFilter"].ToString());
                }

            }
            else
            {
                datos = vPedidosCliente.GetListadoPedidos();
            }

            //if (!IsAdmin)
            if(IsCliente)
            {
                vPedidosCliente.Datos.Clear();
                foreach (DataRow row in datos.Rows)
                {
                    foreach (UsuarioRolCliente cliente in base.UserContext.RolesCliente)
                    {

                        if (row["Cg_Cli"].ToString() == cliente.Cliente_Id.ToString())
                        {
                            vPedidosCliente.Datos.ImportRow(row);
                        }
                    }
                }
            }
            else
            {
                vPedidosCliente.Datos.Clear();
                vPedidosCliente.Datos = datos;
            }

            SetDropDownEstados();

            Session["ListadoPedidos"] = vPedidosCliente.Datos;
            ViewBag.ColoresEstado = colorEstadoRepository.GetAll().ToList();
            
            if (IsAdmin || IsOperador || IsProduccion)
                ViewBag.IsAdminOperador = true;
            else
                ViewBag.IsAdminOperador = false;
            return View("Index", vPedidosCliente);
        }


        public ActionResult GridViewPedidos()
        {
            if (IsAdmin || IsOperador || IsProduccion)
                ViewBag.IsAdminOperador = true;
            else
                ViewBag.IsAdminOperador = false;

            ViewBag.ColoresEstado = colorEstadoRepository.GetAll().ToList();
            
            var datos = (DataTable)Session["ListadoPedidos"];
            if (datos == null)
            {
                datos = vGetListadoPedidos.GetListadoPedidos();
            }

            //if (!IsAdmin)
            if(IsCliente)
            {
                vGetListadoPedidos.Datos.Clear();
                foreach (DataRow row in datos.Rows)
                {
                    foreach (UsuarioRolCliente cliente in base.UserContext.RolesCliente)
                    {
                        if (row["Cg_Cli"].ToString() == cliente.Cliente_Id.ToString())
                        {
                            vGetListadoPedidos.Datos.ImportRow(row);
                        }
                    }
                }
            }
            else
            {
                vGetListadoPedidos.Datos.Clear();
                vGetListadoPedidos.Datos = datos;
            }

            return PartialView("_GridViewPedidos", vGetListadoPedidos);
        }
        public ActionResult GridViewPedidoRefresh()
        {
            if (IsAdmin || IsOperador || IsProduccion)
                ViewBag.IsAdminOperador = true;
            else
                ViewBag.IsAdminOperador = false;

            ViewBag.ColoresEstado = colorEstadoRepository.GetAll().ToList();
            var datos = (DataTable)Session["ListadoPedidos"];
            datos = null;
            Session["ListadoPedidos"] = null;
            if (datos == null)
            {
                datos = vGetListadoPedidos.GetListadoPedidos();
                Session["ListadoPedidos"] = datos;
            }
            //if (!IsAdmin)

            if(IsCliente)
            {
                vGetListadoPedidos.Datos.Clear();
                foreach (DataRow row in datos.Rows)
                {
                    foreach (UsuarioRolCliente cliente in base.UserContext.RolesCliente)
                    {

                        if (row["Cg_Cli"].ToString() == cliente.Cliente_Id.ToString())
                        {
                            vGetListadoPedidos.Datos.ImportRow(row);
                        }
                    }
                }
            }
            else
            {
                vGetListadoPedidos.Datos.Clear();
                vGetListadoPedidos.Datos = datos;
                Session["ListadoPedidos"] = datos;
            }

            return PartialView("_GridViewPedidos", vGetListadoPedidos);
        }
      

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create()
        {
            //List<RolEmpresa> rolesUsuario = rolEmpresaRepository.GetFiltered(x => x.Usuario_Id == this.User.UserID 
            //                                    && x.Rol.Estado == true && x.Usuario.Estado == true, x => x.Rol).ToList();

            List<RolEmpresa> rolesUsuario = UserContext.RolesEmpresa.ToList();
            List<UsuarioRolCliente> userRolClient = UserContext.RolesCliente.ToList();
           
            //List<UsuarioRolCliente> userRolClient = new List<UsuarioRolCliente>();
            //foreach (RolEmpresa rol in rolesUsuario)
            //{
            //    List<UsuarioRolCliente> clientesRol = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Rol_Id == rol.Rol_Id
            //                                            && s.UsuarioRol.Usuario_Id == this.User.UserID && s.Estado == true).ToList();

            //    if (rol.Rol.RoleName.Trim().ToLower().Contains("admin"))
            //        isAdmin = true;

            //    foreach (UsuarioRolCliente clienteRol in clientesRol)
            //    {
            //        userRolClient.Add(clienteRol);
            //    }
            //}

            if (base.IsAdmin|| base.IsOperador  || base.IsProduccion )
                Session["IsAdmin"] = true;
            else
                Session["IsAdmin"] = false;

            foreach (var item in userRolClient)
            {
                var listRow = (new vClientes()).GetViewModel().AsEnumerable()
                        .Where(r => r.Field<int>("ID") == item.Cliente_Id).ToList();
                foreach (DataRow dr in listRow)
                {
                    vClientes.Datos.Rows.Add(dr.ItemArray);
                    break;
                }
                break;
            }

            Pedido pedido = new Pedido();
            if (vClientes.Datos.Rows.Count > 0) 
            {
                pedido.Cg_Cli = Convert.ToInt32(vClientes.Datos.Rows[0]["ID"]);
                pedido.Nombre_Cliente = vClientes.Datos.Rows[0]["Descripcion"].ToString();
                pedido.Cuit = vClientes.Datos.Rows[0]["Cuit"].ToString();
            }
            
            Tuple<Pedido, PedidoDetalle> Models = new Tuple<Pedido, PedidoDetalle>(pedido, new PedidoDetalle());
            return View(Models);
        }

        [HttpPost]
        public ActionResult Create(Pedido Item1, PedidoDetalle[] PedidoDetalles)
        {
            if (PedidoDetalles == null)
                ModelState.AddModelError("", "El Pedido debe tener al menos un Producto");
            else
            {
                foreach (PedidoDetalle pedDetalle in PedidoDetalles)
                {
                    if (pedDetalle.Deleted == false)
                        Item1.PedidoDetalles.Add(pedDetalle);
                }

                if(Item1.PedidoDetalles.Count == 0)
                    ModelState.AddModelError("", "El Pedido debe tener al menos un Producto");
            }

            if (Item1.Conf_Fecha != null)
            {
                DateTime FechaConf = Item1.Conf_Fecha.Value;
                int result = FechaConf.CompareTo(Item1.Fe_Ped);
                if (result < 0)
                    ModelState.AddModelError("FechaConfError", "La Fecha Autorizada para Recepción en Planta debe ser mayor o igual a la Fecha Solicitada");
            }

            try
            {
                if (Item1.Fe_Ped != null)
                {
                    int result = Item1.Fe_Ped.Value.CompareTo(DateTime.Now.Date);
                    if (result < 0)
                        ModelState.AddModelError("FechaPedError", "Debe seleccionar una Fecha mayor o igual al día de hoy.");
                    else
                    {
                        DataTable dt = vPedidosFechaMaximaAnticipacion.PedidosFechaMaximaAnticipacion();
                        int DiasMaxAnticipacion = Convert.ToInt32(dt.Rows[0][0]);
                        DateTime FechaMaxPedido = DateTime.Parse(dt.Rows[0][1].ToString());

                        result = FechaMaxPedido.CompareTo(Item1.Fe_Ped);
                        if (result < 0)
                            ModelState.AddModelError("FechaPedError", "No se puede cargar pedidos con mas de " + DiasMaxAnticipacion + " días de anticipación.");
                    }
                }

                if (!ModelState.IsValid)
                {
                    Tuple<Pedido, PedidoDetalle> Models = new Tuple<Pedido, PedidoDetalle>(Item1, new PedidoDetalle());
                    return View(Models);
                }

                //Guardamos los datos
                if (Item1.Conf_Fecha != null)
                    Item1.Conf_Usu = this.User.UserID;

                pedidoRepository.Add(Item1, this.User.UserID);

                foreach (PedidoDetalle pedDetalle in Item1.PedidoDetalles)
                {
                    pedDetalle.PedidoID = Item1.Id;
                    if (pedDetalle.Deleted == false)
                    {
                        pedidoDetalleRepository.Add(pedDetalle, this.User.UserID);

                        PedidoEstado pedidoEstado = new PedidoEstado();
                        pedidoEstado.Cg_Prod = pedDetalle.Cg_Prod;
                        pedidoEstado.Cantidad = pedDetalle.Cantidad;
                        pedidoEstado.PedidoID = pedDetalle.PedidoID;
                        pedidoEstado.EstadoInt = 0;
                        pedidoEstado.Devol = false;

                        pedidoEstadoRepository.Add(pedidoEstado, this.User.UserID);
                    }
                }

                if (Item1.Conf_Fecha != null)
                    pedidoDetalleRepository.PushPedidoERP(Item1.Id, this.User.UserName);

                
                SetMessage(SUCCESS, "Pedido Numero "+Item1.Id+" Guardado.");
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, "ERROR: " + ex.Message);
                Tuple<Pedido, PedidoDetalle> Models = new Tuple<Pedido, PedidoDetalle>(Item1, new PedidoDetalle());
                return View(Models);
            }

            return IndexRefrech(true);
        }

        public ActionResult GetProductos(string CliID)
        {
            try
            {
                int CliIDInt = Convert.ToInt32(CliID);
                vProductos.Datos = vGetProductosCliente.Get_ProductosCliente(CliIDInt);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, "No se han podido cargar los productos correctamente");
            }
            Session["ProductosClientePedido"] = vProductos.Datos;

            return PartialView("_Productos", vProductos);
        }

        public ActionResult GetClientes()
        {
           

            if (IsAdmin || IsProduccion || IsOperador)
                vClientes = new Business.Views.vClientes();
            else
                vClientes = new Business.Views.vClientes(base.UserContext.RolesCliente.ToList());


            Session["ClientesUsuarioPedido"] = vClientes.Datos = vClientes.GetViewModel();

            return PartialView("_Clientes", vClientes);
        }

        public ActionResult GridViewModalProductos()
        {
            vProductos.Datos = (DataTable)Session["ProductosClientePedido"];
            return PartialView("_GridViewModalProductos", vProductos);
        }

        public ActionResult GridViewModalClientes()
        {
            vClientes.Datos = (DataTable)Session["ClientesUsuarioPedido"];
            return PartialView("_GridViewModalClientes", vClientes);
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            try
            {
                //List<RolEmpresa> rolesUsuario = rolEmpresaRepository.GetFiltered(x => x.Usuario_Id == this.User.UserID
                //                                && x.Rol.Estado == true && x.Usuario.Estado == true, x => x.Rol).ToList();
                List<RolEmpresa> rolesUsuario = UserContext.RolesEmpresa.ToList();
                List<UsuarioRolCliente> userRolClient = UserContext.RolesCliente.ToList();
              
                //foreach (RolEmpresa rol in rolesUsuario)
                //{
                //    List<UsuarioRolCliente> clientesRol = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Rol_Id == rol.Rol_Id
                //                                            && s.UsuarioRol.Usuario_Id == this.User.UserID && s.Estado == true).ToList();

                //    if (rol.Rol.RoleName.Trim().ToLower().Contains("admin"))
                //        isAdmin = true;

                //    if (rol.Rol.RoleName.Trim().ToLower().Equals("cliente"))
                //        isCliente = true;

                //    foreach (UsuarioRolCliente clienteRol in clientesRol)
                //    {
                //        userRolClient.Add(clienteRol);
                //    }
                //}
                var isCliente = false;

               if( base.UserContext.RolesEmpresa.Any(s=> s.Rol.RoleName.ToLower().Equals("cliente")))
                   isCliente = true;

                if (isCliente)
                    Session["isCliente"] = true;
                else
                    Session["isCliente"] = false;


                if (base.IsAdmin || base.IsOperador || base.IsProduccion)
                {
                    Session["IsAdmin"] = true;
                    Session["isCliente"] = false;
                }
                else
                    Session["IsAdmin"] = false;

                Pedido pedido = pedidoRepository.Get(id.Value).First();

                DataTable dtCli = vClientes.GetByID(pedido.Cg_Cli);
                pedido.Nombre_Cliente = dtCli.Rows[0]["Descripcion"].ToString();
                

                List<PedidoDetalle> pedidoDetalles = pedidoDetalleRepository.GetFiltered(x => x.PedidoID == pedido.Id).ToList();
                foreach (PedidoDetalle pedDetalle in pedidoDetalles)
                {
                    DataTable dtProd = vProductosAll.GetByID(pedDetalle.Cg_Prod);
                    pedDetalle.NombreProducto = dtProd.Rows[0]["Descripcion"].ToString();

                    DataTable detPedSel = vGetListadoPedidos.GetListadoPedidos("ID = " + id.Value + " And CodigoIonics = '"+pedDetalle.Cg_Prod+"'");
                    var dosis = detPedSel.Rows[0]["Dosis"];
                    pedDetalle.Dosis = dosis.ToString();
                }
                pedido.PedidoDetalles = pedidoDetalles.ToList();

                if(pedido.Conf_Fecha != null)
                    Session["ProductosEditablePedido"] = "NoEditable";
                else
                    Session["ProductosEditablePedido"] = null;

                Tuple<Pedido, PedidoDetalle> Models = new Tuple<Pedido, PedidoDetalle>(pedido, new PedidoDetalle());
                return View("Edit", Models);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
            }

            return Index();
        }

        [HttpPost]
        public ActionResult Edit(Pedido Item1, PedidoDetalle[] PedidoDetalles)
        {
            if (PedidoDetalles == null)
                ModelState.AddModelError("", "El Pedido debe tener al menos un Producto");
            else
            {
                foreach (PedidoDetalle pedDetalle in PedidoDetalles)
                {
                    if (pedDetalle.Deleted == false)
                        Item1.PedidoDetalles.Add(pedDetalle);
                }

                if (Item1.PedidoDetalles.Count == 0)
                    ModelState.AddModelError("", "El Pedido debe tener al menos un Producto");
            }


            if (Item1.Conf_Fecha != null && UserContext.RolesEmpresa.Any(s => s.Rol_Id != 26))
            {
                Session["ProductosEditablePedido"] = "NoEditable";
                DateTime FechaConf = Item1.Conf_Fecha.Value;
                int result = FechaConf.CompareTo(Item1.Fe_Ped);
                if (result < 0)
                    ModelState.AddModelError("FechaConfError", "La Fecha Autorizada para Recepción en Planta debe ser mayor o igual a la Fecha Solicitada");
            }
            else
            { 
                Session["ProductosEditablePedido"] = null;
            }

            try
            {
                Pedido pedBD = pedidoRepository.Get(Item1.Id).First();
                if (Item1.Fe_Ped != null)
                {
                    if ((Item1.Fe_Ped.Value.CompareTo(pedBD.Fe_Ped.Value)) != 0)
                    {
                        int result = Item1.Fe_Ped.Value.CompareTo(DateTime.Now.Date);
                        if (result < 0)
                            ModelState.AddModelError("FechaPedError", "Debe seleccionar una Fecha mayor o igual al día de hoy.");
                        else
                        {
                            DataTable dt = vPedidosFechaMaximaAnticipacion.PedidosFechaMaximaAnticipacion();
                            int DiasMaxAnticipacion = Convert.ToInt32(dt.Rows[0][0]);
                            DateTime FechaMaxPedido = DateTime.Parse(dt.Rows[0][1].ToString());

                            result = FechaMaxPedido.CompareTo(Item1.Fe_Ped);
                            if (result < 0)
                                ModelState.AddModelError("FechaPedError", "No se puede cargar pedidos con mas de " + DiasMaxAnticipacion + " días de anticipación.");
                        }
                    }
                    else
                    {
                        DataTable dt = vPedidosFechaMaximaAnticipacion.PedidosFechaMaximaAnticipacion();
                        int DiasMaxAnticipacion = Convert.ToInt32(dt.Rows[0][0]);
                        DateTime FechaMaxPedido = DateTime.Parse(dt.Rows[0][1].ToString());

                        int result = FechaMaxPedido.CompareTo(Item1.Fe_Ped);
                        if (result < 0)
                            ModelState.AddModelError("FechaPedError", "No se puede cargar pedidos con mas de " + DiasMaxAnticipacion + " días de anticipación.");
                    }
                }

                if (!ModelState.IsValid)
                {
                    Tuple<Pedido, PedidoDetalle> Models = new Tuple<Pedido, PedidoDetalle>(Item1, new PedidoDetalle());
                    return View(Models);
                }

                //Guardamos los datos
                if (Item1.Conf_Fecha != null)
                    Item1.Conf_Usu = this.User.UserID;

                pedidoRepository.Modify(Item1, this.User.UserID);

                List<PedidoDetalle> pedidoDetallesBD = pedidoDetalleRepository.GetFiltered(x => x.PedidoID == Item1.Id).ToList();
                foreach (PedidoDetalle pedidoDetalleBd in pedidoDetallesBD)
                {
                    pedidoDetalleRepository.RemoveFromDataBase(pedidoDetalleBd, this.User.UserID);
                }

                
                List<PedidoEstado> pedidosEstadoBD = pedidoEstadoRepository.GetFiltered(x => x.PedidoID == Item1.Id).ToList();
                //foreach (PedidoEstado pedidoEstadoBd in pedidosEstadoBD)
                //{
                //    pedidoEstadoRepository.RemoveFromDataBase(pedidoEstadoBd, this.User.UserID);
                //}

                foreach (PedidoDetalle pedDetalle in Item1.PedidoDetalles)
                {
                    pedDetalle.PedidoID = Item1.Id;
                    if (pedDetalle.Deleted == false)
                    {
                        pedidoDetalleRepository.Add(pedDetalle, this.User.UserID);

                        //PedidoEstado pedidoEstado = new PedidoEstado();
                        //pedidoEstado.Cg_Prod = pedDetalle.Cg_Prod;
                        //pedidoEstado.Cantidad = pedDetalle.Cantidad;
                        //pedidoEstado.PedidoID = pedDetalle.PedidoID;
                        //pedidoEstado.EstadoInt = 0;
                        //pedidoEstado.Devol = false;

                        //pedidoEstadoRepository.Add(pedidoEstado, this.User.UserID);
                    }
                }

                if (Item1.Conf_Fecha != null)
                    pedidoDetalleRepository.PushPedidoERP(Item1.Id, this.User.UserName);

                //Actualizar Estados
                //pedidoEstadoRepository.ActualizarEstadosPedidosSP(); POR UN TEMA DE VELOCIDAD SE COMENTA: ANALIZAR ESE SP PARA MEJORAR
                SetMessage(SUCCESS, "Pedido Numero " + Item1.Id + " Guardado.");
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, "ERROR: " + ex.Message);
                Tuple<Pedido, PedidoDetalle> Models = new Tuple<Pedido, PedidoDetalle>(Item1, new PedidoDetalle());
                return View(Models);
            }
            
            return Index();
        }

        [HttpPost]
        public string EditFecha(int id, string fecha_autorizada)
        {
            try
            {
                Pedido pedBD = pedidoRepository.Get(id).First();
                pedBD.Conf_Fecha = Convert.ToDateTime(fecha_autorizada);

                pedidoRepository.Modify(pedBD, this.User.UserID);
                //Mandar mensaje de que se guardo
                pedidoDetalleRepository.PushPedidoERP(pedBD.Id, this.User.UserName);
            }
            catch (Exception ex)
            {
                return "ERROR";
            }

            return "OK";
        }

        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult View(int? id)
        {
            try
            {
                List<RolEmpresa> rolesUsuario = rolEmpresaRepository.GetFiltered(x => x.Usuario_Id == this.User.UserID
                                                && x.Rol.Estado == true && x.Usuario.Estado == true, x => x.Rol).ToList();

                bool isAdmin = false;
                List<UsuarioRolCliente> userRolClient = new List<UsuarioRolCliente>();
                foreach (RolEmpresa rol in rolesUsuario)
                {
                    List<UsuarioRolCliente> clientesRol = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Rol_Id == rol.Rol_Id
                                                            && s.UsuarioRol.Usuario_Id == this.User.UserID && s.Estado == true).ToList();

                    if (rol.Rol.RoleName.Trim().ToLower().Contains("admin"))
                        isAdmin = true;

                    foreach (UsuarioRolCliente clienteRol in clientesRol)
                    {
                        userRolClient.Add(clienteRol);
                    }
                }

                if (isAdmin)
                    Session["IsAdmin"] = true;
                else
                    Session["IsAdmin"] = false;

                Pedido pedido = pedidoRepository.Get(id.Value).First();

                DataTable dtCli = vClientes.GetByID(pedido.Cg_Cli);
                pedido.Nombre_Cliente = dtCli.Rows[0]["Descripcion"].ToString();

                List<PedidoDetalle> pedidoDetalles = pedidoDetalleRepository.GetFiltered(x => x.PedidoID == pedido.Id).ToList();
                foreach (PedidoDetalle pedDetalle in pedidoDetalles)
                {
                    DataTable dtProd = vProductos.GetByID(pedDetalle.Cg_Prod);
                    pedDetalle.NombreProducto = dtProd.Rows[0]["Descripcion"].ToString();
                }
                pedido.PedidoDetalles = pedidoDetalles.ToList();

                Tuple<Pedido, PedidoDetalle> Models = new Tuple<Pedido, PedidoDetalle>(pedido, new PedidoDetalle());
                return View("View", Models);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
            }

            return Index();
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Delete(int? id)
        {
            try
            {
                Pedido pedido = pedidoRepository.Get(id.Value).First();
                pedidoRepository.Remove(pedido, this.User.UserID);
                pedidoRepository.RemovePedidoSP(pedido.Id);


                SetMessage(SUCCESS, " Se elimino correctamente el Pedido Numero "+id+".");
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, " Se produjo un error al eliminar el Pedido." + ex.Message);
            }

            return Index();
        }

        public ActionResult DetalleEntrega(int id)
        {
            vDetalleEntrega.Datos = vDetalleEntrega.DetalleEntrega(id);
            Session["DetalleEntregaPedido"] = vDetalleEntrega.Datos;

            string idPedidoString = id.ToString();
            ViewBag.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == idPedidoString && x.Estado == true && x.Descripcion.StartsWith("REMITO_")
                                        && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

            return PartialView("_DetalleEntrega", vDetalleEntrega);
        }

        public ActionResult GridViewDetalleEntrega()
        {
            vDetalleEntrega.Datos = (DataTable)Session["DetalleEntregaPedido"];
            return PartialView("_GridViewDetalleEntrega", vDetalleEntrega);
        }

        [HttpPost]
        public ActionResult DetalleEntrega(string btnDetEntAceptar, string btnExportar)
        {
            vDetalleEntrega.Datos = (DataTable)Session["DetalleEntregaPedido"];
            var setting = GridHelper.GetSettingExportWithoutId(vDetalleEntrega.GetDynamicCollectionList(vDetalleEntrega.Datos), _ControllerName);

            if (btnExportar != null)
            {
                return GridViewExtension.ExportToXls(setting, vDetalleEntrega.Datos, string.Format("{0}s_{1}.{2}", "Detalles_Entrega", DateTime.Now.ToString("ddMMyyyy_HHmmss"), "xls"));
            }
            else
            {
                setting.SettingsExport.PaperKind = PaperKind.A4;
                setting.SettingsExport.Landscape = true;
                setting.SettingsExport.PageHeader.Center = "DETALLE DE ENTREGA";
                setting.SettingsExport.PageHeader.Font.Size = 15;
                setting.SettingsExport.PageHeader.Font.Name = "Times New Roman";
                setting.SettingsExport.PageHeader.Font.Bold = true;
                PdfExportOptions pdfExportOptions = new PdfExportOptions();
                pdfExportOptions.ShowPrintDialogOnOpen = true;
                pdfExportOptions.DocumentOptions.Title = "Detalle de Entrega";
                return GridViewExtension.ExportToPdf(setting, vDetalleEntrega.Datos, string.Format("{0}s_{1}.{2}", "Detalles_Entrega", DateTime.Now.ToString("ddMMyyyy_HHmmss"), "pdf"), pdfExportOptions);
            }
        }

        public ActionResult Print()
        {
            vDetalleEntrega.Datos = (DataTable)Session["DetalleEntregaPedido"];
            var setting = GridHelper.GetSettingExportWithoutId(vDetalleEntrega.GetDynamicCollectionList(vDetalleEntrega.Datos), _ControllerName);

            XtraReport report = new XtraReport();
            report.Landscape = true;
            report.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            report.DataSource = vDetalleEntrega.Datos;
            ViewBag.Report = report;

            return PartialView("_DocumentViewerPartial");
        }

        [HttpPost]
        public ActionResult ExportXLS(GridViewExportedRowType exportRowType)
        {
            vGetListadoPedidos.Datos = (DataTable)Session["ListadoPedidos"];
            var setting = GridHelper.GetSettingExport(vGetListadoPedidos.GetDynamicCollectionList(vGetListadoPedidos.Datos), _ControllerName);
            setting.SettingsExport.ExportedRowType = exportRowType;
            //setting.SettingsExport.ExportSelectedRowsOnly = 
            // retornamos el excel al usuario
            return GridViewExtension.ExportToXls(setting, vGetListadoPedidos.Datos, string.Format("{0}s_{1}.{2}", "Pedido", DateTime.Now.ToString("ddMMyyyy_HHmmss"), "xls"));
        }

        public ActionResult Imprimir(int id)
        {
            vGetListadoPedidos.Datos = (DataTable)Session["ListadoPedidos"];
            DataTable DtPedido = vGetListadoPedidos.Datos.Clone();
            DtPedido.Columns.Clear();
            

            DtPedido.Columns.Add("ID",typeof(int));
            DtPedido.Columns.Add("CodigoIonics", typeof(string));
            DtPedido.Columns.Add("CodigoProductoCliente", typeof(string));
            DtPedido.Columns.Add("Descripcion", typeof(string));
            DtPedido.Columns.Add("CantidadPedida", typeof(int));
            DtPedido.Columns.Add("Cg_Cli", typeof(int));
            DtPedido.Columns.Add("Des_Cli", typeof(string));
            DtPedido.Columns.Add("CantidadRecibida", typeof(int));
            DtPedido.Columns.Add("UnidadFac", typeof(string));
            DtPedido.Columns.Add("Dosis", typeof(string));
            DtPedido.Columns.Add("Ancho", typeof(string));
            DtPedido.Columns.Add("Largo", typeof(string));
            DtPedido.Columns.Add("Altura", typeof(string));
            DtPedido.Columns.Add("Diametro", typeof(string));
            DtPedido.Columns.Add("Observaciones", typeof(string));
            DtPedido.Columns.Add("Lotes", typeof(string));

            foreach (DataRow row in vGetListadoPedidos.Datos.Rows)
            {
                if (row["Id"].ToString() == id.ToString())
                {
                    DtPedido.ImportRow(row);
                }
            }

            vGetListadoPedidos vDtPedido = new vGetListadoPedidos();
            vDtPedido.Datos = DtPedido;
            Session["DtPedidoSeleccionado"] = DtPedido;

            return PartialView("_Impresora", vDtPedido);
        }

        Image headerImage;
        [HttpPost]
        public ActionResult Imprimir(string btnImprimir)
        {
            DataTable dtPedido = (DataTable)Session["DtPedidoSeleccionado"];
            var setting = GridHelper.GetSettingExportPDF(vGetListadoPedidos.GetDynamicCollectionList(dtPedido), _ControllerName, true);
            setting.SettingsExport.PaperKind = PaperKind.A4;

            var printable = GridViewExtension.CreatePrintableObject(setting, dtPedido);
            
            PrintingSystem ps = new PrintingSystem();

            using (headerImage = Image.FromFile(Server.MapPath("~\\Images\\Logo.gif")))
            {
                Link header = new Link();
                header.CreateDetailArea += new CreateAreaEventHandler(header_CreateDetailArea);

                Link footer = new Link();
                footer.CreateDetailArea += footer_CreateDetailArea;

                PrintableComponentLink link1 = new PrintableComponentLink(ps);
                link1.Component = printable;

                CompositeLink compositeLink = new CompositeLink(ps);
                compositeLink.Links.AddRange(new object[] { header, link1, footer });
                compositeLink.MinMargins.Left = 10;
                compositeLink.MinMargins.Right = 5;
                compositeLink.Margins.Left = 0;
                compositeLink.Margins.Right = 0;
                compositeLink.Landscape = true;
                compositeLink.CreateDocument();
                using (MemoryStream stream = new MemoryStream())
                {
                    string nombreArchivo = string.Format("{0}s_{1}", "Pedido", DateTime.Now.ToString("ddMMyyyy_HHmmss"));
                    compositeLink.PrintingSystem.ExportToPdf(stream);
                    WriteToResponse(nombreArchivo, true, "pdf", stream);
                }

            }

            //setting.SettingsExport.PaperKind = PaperKind.A4;
            //setting.SettingsExport.Landscape = true;
            //setting.SettingsExport.PageHeader.Center = "DETALLE DE PEDIDO";
            //setting.SettingsExport.PageHeader.Font.Size = 25;
            //setting.SettingsExport.PageHeader.Font.Name = "Times New Roman";
            //setting.SettingsExport.PageHeader.Font.Bold = true;
            PdfExportOptions pdfExportOptions = new PdfExportOptions();
            //pdfExportOptions.ShowPrintDialogOnOpen = true;
            //pdfExportOptions.DocumentOptions.Title = "Detalle de Pedido";

            return GridViewExtension.ExportToPdf(setting, dtPedido, string.Format("{0}s_{1}.{2}", "Pedido", DateTime.Now.ToString("ddMMyyyy_HHmmss"), "pdf"), pdfExportOptions);
        }

        private void footer_CreateDetailArea(object sender, CreateAreaEventArgs e)
        {
            Rectangle r = new Rectangle(0, 0, 250, 120);

            DataTable tbl = (DataTable)Session["DtPedidoSeleccionado"];
            string footer = string.Format("Observaciones: " + tbl.Rows[0]["Observaciones"].ToString().Trim());
            
            r = new Rectangle(0, 50, 550, 50);
            e.Graph.DrawString(footer, Color.Black, r, BorderSide.Right);
        }

        private void header_CreateDetailArea(object sender, CreateAreaEventArgs e)
        {
            e.Graph.BorderWidth = 0;

            Rectangle r = new Rectangle(0, 0, 250, 120);
            e.Graph.DrawImage(headerImage, r);

            DataTable tbl = (DataTable)Session["DtPedidoSeleccionado"];
            string SubHeader = string.Format("PEDIDO: " + tbl.Rows[0]["ID"].ToString() +"\nCLIENTE: (" + tbl.Rows[0]["Cg_Cli"].ToString() +") " + tbl.Rows[0]["Des_Cli"].ToString().Trim() + "\n");
            e.Graph.Font = new Font("Tahoma", 12, FontStyle.Bold);
            e.Graph.MeasureString(SubHeader);
            r = new Rectangle(300,50, 500, 50);
            e.Graph.DrawString( SubHeader, Color.Green, r, BorderSide.Left);

            r = new Rectangle(0, 150, 500, 50);
            e.Graph.DrawString("", Color.White, r, BorderSide.None);
        }

        private void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            string disposition = saveAsFile ? "attachment" : "inline";
            Response.Clear();
            Response.Buffer = false;
            Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Response.AppendHeader("Content-Disposition",
            string.Format("{0}; filename={1}.{2}", disposition, fileName, fileFormat));
            Response.BinaryWrite(stream.GetBuffer());
            Response.End();
        }

        public ActionResult GridViewImpresora()
        {
            vGetListadoPedidos vDtPedido = new vGetListadoPedidos();
            vDtPedido.Datos = (DataTable)Session["DtPedidoSeleccionado"];
            return PartialView("_GridViewImpresora", vDtPedido);
        }

        public ActionResult DetalleFactura(int id)
        {
            //vDetalleEntrega por vDetalleFactura
            vDetalleFactura.Datos = vDetalleFactura.DetalleFactura(id);
            Session["DetalleFacturaPedido"] = vDetalleFactura.Datos;

            string idPedidoString = id.ToString();
            ViewBag.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == idPedidoString && x.Estado == true && x.Descripcion.StartsWith("FC_")
                                        && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

            return PartialView("_DetalleFactura", vDetalleFactura);
        }

        public ActionResult GridViewDetalleFactura()
        {
            vDetalleFactura.Datos = (DataTable)Session["DetalleFacturaPedido"];
            return PartialView("_GridViewDetalleFactura", vDetalleFactura);
        }

        public ActionResult EnviarEmailCotizacionVencida(string name)
        {

            Usuario currentUser = usuarioRepository.Get(this.User.UserID).First();
            string asunto = currentUser.NombreApellido + " - Solicita cotización actualizada";
            string remitente = "comercial@ionics.com.ar";
            string destinatario = "comercial@ionics.com.ar";
            string cuerpo = "<b>Cotización para:.</b>" + currentUser.Email + "<br>" +
            "<b>Cliente: </b>" + currentUser.NombreApellido + "<br>" +
            "<b>Fecha de Registro: </b>" + DateTime.Now + "<br>" +
            "<b>Producto: </b>" + name + "<br>";
             //string[] ems = { "fbriolfo@rt-solutions.com.ar", "prueba.ionics@gmail.com", "facu.riolfo@gmail.com", "facu.riolfo@hotmail.com", "facundobarriosriolfo@hotmail.com"};
            if (destinatario != "")
            {
                //    foreach(string em in ems)
                //{
                // var asunto = ConfigurationManager.AppSettings["ASUNTO.EVENTUALIDAD"];
                MailMessage mm = new MailMessage(remitente, destinatario, asunto, cuerpo);
                //MailMessage mm = new MailMessage(remitente, em, asunto, cuerpo);
                mm.BodyEncoding = System.Text.UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mm.IsBodyHtml = true;

                SendEmailInBackgroundThread(mm);
            }
            return View();
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

        [HttpPost]
        public ActionResult DetalleFactura(string btnDetFacAceptar, string btnExportar)
        {
            vDetalleFactura.Datos = (DataTable)Session["DetalleFacturaPedido"];
            var setting = GridHelper.GetSettingExportWithoutId(vDetalleFactura.GetDynamicCollectionList(vDetalleFactura.Datos), _ControllerName);

            if (btnExportar != null)
            {
                return GridViewExtension.ExportToXls(setting, vDetalleFactura.Datos, string.Format("{0}s_{1}.{2}", "Detalles_Factura", DateTime.Now.ToString("ddMMyyyy_HHmmss"), "xls"));
            }
            else
            {
                setting.SettingsExport.PaperKind = PaperKind.A4;
                setting.SettingsExport.Landscape = true;
                setting.SettingsExport.PageHeader.Center = "DETALLE DE FACTURA";
                setting.SettingsExport.PageHeader.Font.Size = 15;
                setting.SettingsExport.PageHeader.Font.Name = "Times New Roman";
                setting.SettingsExport.PageHeader.Font.Bold = true;
                PdfExportOptions pdfExportOptions = new PdfExportOptions();
                pdfExportOptions.ShowPrintDialogOnOpen = true;
                pdfExportOptions.DocumentOptions.Title = "Detalle de Factura";
                return GridViewExtension.ExportToPdf(setting, vDetalleFactura.Datos, string.Format("{0}s_{1}.{2}", "Detalles_Factura", DateTime.Now.ToString("ddMMyyyy_HHmmss"), "pdf"), pdfExportOptions);
            }
        }
    }
}
