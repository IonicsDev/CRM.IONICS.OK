using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Business.Views;
using CRM.Website.DevExpressHelpers;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CRM.Website.Security.Infrastructure;

namespace CRM.Website.Controllers
{
    public class CalendarioRetiroController : BaseController
    {
        vGetScheduleRetiro vGetScheduleRetiro = new vGetScheduleRetiro();
        PedidoRepository pedidoRepository = new PedidoRepository();
        vClientes vClientes = new vClientes();

        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult Index()
        {
            var meses = new List<KeyValuePair<string, int>>();
            meses.Add(new KeyValuePair<string, int>("Enero", 1));
            meses.Add(new KeyValuePair<string, int>("Febrero", 2));
            meses.Add(new KeyValuePair<string, int>("Marzo", 3));
            meses.Add(new KeyValuePair<string, int>("Abril", 4));
            meses.Add(new KeyValuePair<string, int>("Mayo", 5));
            meses.Add(new KeyValuePair<string, int>("Junio", 6));
            meses.Add(new KeyValuePair<string, int>("Julio", 7));
            meses.Add(new KeyValuePair<string, int>("Agosto", 8));
            meses.Add(new KeyValuePair<string, int>("Septiembre", 9));
            meses.Add(new KeyValuePair<string, int>("Octubre", 10));
            meses.Add(new KeyValuePair<string, int>("Noviembre", 11));
            meses.Add(new KeyValuePair<string, int>("Diciembre", 12));

            List<KeyValuePair<int, int>> anios = Enumerable.Range(DateTime.Now.Year - 6, 11)
                                                    .Select(x => new KeyValuePair<int, int>(x, x)).ToList();
            Session["Tipo"] = "A";
            ViewBag.Meses = meses;
            ViewBag.Anios = anios;

            return View("Index");
        }

        [LogonAuthorize(Roles = "VIEW")]
        public JsonResult GetEvents(double start, double end)
        {
            DateTime dateStart = ConvertFromUnixTimestamp(start);
            DateTime dateEnd = ConvertFromUnixTimestamp(end);

            TimeSpan ts = dateEnd.Subtract(dateStart);
            DateTime dateMiddle = dateStart.AddMinutes(ts.TotalMinutes / 2);

            int mes = dateMiddle.Month;
            int anio = dateMiddle.Year;

            DataTable dt = vGetScheduleRetiro.GetScheduleRetiro(mes, anio);

            var random = new Random();
            var list = dt.AsEnumerable()
                .Select(dr => new
                {
                    title = "(" + dr.Field<int>("Pedido") + ") " + dr.Field<string>("Des_Cli") + " - " + dr.Field<int>("Cantidad").ToString(),
                    start = dr.Field<DateTime>("fe_retiro").ToString("yyy-MM-dd"),
                    allDay = true,
                      textColor =(dr.Field<string>("HexaColor") == null || dr.Field<string>("HexaColor") == string.Empty ? "#FFFFFF" :dr.Field<string>("HexaColor")),
                    color = "#f6f6f6" }).ToList();

            var rows = list.ToArray();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetSessionVariable(string key, string value)
        {
            Session[key] = value;

            return this.Json(new { success = true });
        }

        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(int Id)
        {
            try
            {
                Pedido pedido = pedidoRepository.Get(Id).FirstOrDefault();

                if (pedido == null)
                    throw new Exception(" El Pedido seleccionado no existe.");

                DataTable dtCliente = vClientes.GetByID(pedido.Cg_Cli);
                pedido.Nombre_Cliente = dtCliente.Rows[0]["Descripcion"].ToString();

                return View(pedido);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                return Index();
            }
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(Pedido pedido)
        {
            try
            {
                if (pedido.Fe_Retiro == null)
                {
                    ModelState.AddModelError("FechaRetiroError", "La Fecha de Retiro es requerida");
                    return View(pedido);
                }

                int result = pedido.Fe_Retiro.Value.CompareTo(pedido.Fe_Ped);
                if (result < 0)
                {
                    ModelState.AddModelError("FechaRetiroError", "La Fecha de Retiro no puede ser menor a la Fecha de Pedido");
                    return View(pedido);
                }

                pedidoRepository.Modify(pedido, this.User.UserID);
                SetMessage(SUCCESS, " Guardado.");
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
            }

            return Index();
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Delete(int Id)
        {
            try
            {
                Pedido pedido = pedidoRepository.Get(Id).FirstOrDefault();

                if (pedido == null)
                    throw new Exception(" El Pedido seleccionado no existe.");

                pedido.Fe_Retiro = null;
                pedidoRepository.Modify(pedido, this.User.UserID);

                SetMessage(SUCCESS, " Eliminado.");
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                
            }

            return Index();
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create()
        {
            return View(new Pedido());
        }

        [LogonAuthorize(Roles = "EDIT")]
        [HttpPost]
        public ActionResult Create(Pedido pedido)
        {
            try
            {
                if (pedido.Nombre_Cliente == "" || pedido.Nombre_Cliente == null)
                {
                    ModelState.AddModelError("ErrorCliente", "El Cliente es requerido");
                }

                if (pedido.Id == 0)
                {
                    ModelState.AddModelError("ErrorPedido", "El Pedido es requerido");
                }

                Pedido pedidoBD = new Pedido();
                if (pedido.Fe_Retiro != null)
                {
                    if (pedido.Id != 0)
                        pedidoBD = pedidoRepository.Get(pedido.Id).FirstOrDefault();

                    if (pedidoBD != null)
                    {
                        int result = pedido.Fe_Retiro.Value.CompareTo(pedidoBD.Fe_Ped);
                        if (result < 0)
                        {
                            ModelState.AddModelError("FechaRetiroError", "La Fecha de Retiro no puede ser menor a la Fecha de Pedido");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("FechaRetiroError", "La Fecha de Retiro es requerida");
                }

                if (!ModelState.IsValid)
                {
                    return View(pedido);
                }

                pedidoBD.Fe_Retiro = pedido.Fe_Retiro;
                pedidoRepository.Modify(pedidoBD, this.User.UserID);

                SetMessage(SUCCESS, " Guardado.");
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, " " + ex.Message);
                return View(pedido);
            }

            return Index();
        }

        public ActionResult GetClientes()
        {
            if (Session["ClientesUsuarioPedido"] != null)
            {
                vClientes.Datos = (DataTable)Session["ClientesUsuarioPedido"];
            }
            else
            {
                if (IsAdmin)
                    vClientes = new Business.Views.vClientes();
                else
                    vClientes = new Business.Views.vClientes(base.UserContext.RolesCliente.ToList());

                Session["ClientesUsuarioPedido"] = vClientes.Datos = vClientes.GetViewModel();
            }


        //    vClientes.Datos = GetClientesFromUsuario(this.User.UserID, false);
            

            return PartialView("_Clientes", vClientes);
        }

        public ActionResult GridViewModalClientes()
        {
            if (Session["ClientesUsuarioPedido"] != null)
            {
                vClientes.Datos = (DataTable)Session["ClientesUsuarioPedido"];
            }
            else
            {
                if (IsAdmin)
                    vClientes = new Business.Views.vClientes();
                else
                    vClientes = new Business.Views.vClientes(base.UserContext.RolesCliente.ToList());

                Session["ClientesUsuarioPedido"] = vClientes.Datos = vClientes.GetViewModel();
            }
           
            return PartialView("_GridViewModalClientes", vClientes);
        }

        public ActionResult GetPedidos(int CliID)
        {
            List<Pedido> pedidos = pedidoRepository.ExecuteQuery(CliID);
            Session["PedidosClienteCalRetiro"] = pedidos.ToList();

            return PartialView("_Pedidos", pedidos.ToList());
        }

        public ActionResult GridViewModalPedidos()
        {
            List<Pedido> pedidos = (List<Pedido>)Session["PedidosClienteCalRetiro"];
            return PartialView("_GridViewModalClientes", pedidos);
        }

    }
}
