using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Website.Security.Infrastructure;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CRM.Website.Controllers
{
    public class ParametrosController : GenericController<Parametro>
    {

        ParametroRepository parametroRepository = new ParametroRepository();

        [LogonAuthorize(Roles = "VIEW,EDIT")]
        public ActionResult Index()
        {
            ViewData["exportRowType"] = GridViewExportedRowType.All;
            var lparametros = parametroRepository.GetAll().ToList();
            Session["ListadoParametros"] = lparametros;
            return View("Index", lparametros);
        }

        public ActionResult Refresh()
        {
            var lparametros = (List<Parametro>)Session["ListadoParametros"];
            return View("Index", lparametros);
        }

        public ActionResult GridViewAllPartial()
        {
            ViewData["exportRowType"] = GridViewExportedRowType.All;
            var parametros = (IEnumerable<Parametro>)Session["ListadoParametros"];
            return PartialView("_GridViewParametros", parametros);
        }

        public ActionResult GridViewPedidoRefresh()
        {
            if (IsAdmin)
                ViewBag.IsAdminOperador = true;
            else
                ViewBag.IsAdminOperador = false;

            var datos = (IEnumerable<Parametro>)Session["ListadoParametros"];
            datos = null;
            Session["ListadoParametros"] = null;

            if (datos == null)
            {
                datos = parametroRepository.GetFiltered(x => x.Estado == true).ToList();
                Session["ListadoParametros"] = datos;
            }


            return PartialView("_GridViewParametros", datos);
        }

        [HttpGet]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create()
        {
            Parametro newPramaetro = new Parametro();
            return View(newPramaetro);
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Parametro parametro)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (parametroRepository.GetFiltered(x => x.ParamName.ToLower().Trim().Equals(parametro.ParamName.ToLower().Trim())).Any())
                    {
                        ModelState.AddModelError(string.Empty, "El Nombre de Parametro ya ha sido utilizado!");
                        return View(parametro);
                    }
                    else
                    {
                        if (parametro.ParamName != "")
                        {
                            parametroRepository.Add(parametro, this.User.UserID);

                            //if (parametro.ArchivosModulo != null)
                            //{
                            //    foreach (ArchivoModulo newArchivo in parametro.ArchivosModulo)
                            //    {
                            //        if (newArchivo.Deleted == false)
                            //        {
                            //            AltaArchivosModulo(newArchivo, parametro.Id.ToString());
                            //        }
                            //    }
                            //}
                            SetMessage(SUCCESS, "Guardado.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "El nombre de Parámetro debe contener más de 1 caracter");
                            return View(parametro);
                        }
                    }
                }
                else
                {
                    return View(parametro);
                }
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, "Error al guardar parámetro :" + ex.Message);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                return View(parametro);
            }

            return Index();
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            try
            {
                var isCliente = false;

                if (base.UserContext.RolesEmpresa.Any(s => s.Rol.RoleName.ToLower().Equals("cliente")))
                    isCliente = true;

                if (isCliente)
                    Session["isCliente"] = true;
                else
                    Session["isCliente"] = false;


                if (base.IsAdmin)
                {
                    Session["IsAdmin"] = true;
                    Session["isCliente"] = false;
                }
                else
                    Session["IsAdmin"] = false;

                Parametro parametro = parametroRepository.Get(id.Value).SingleOrDefault();

                /*Tuple<Pedido, PedidoDetalle> Models = new Tuple<Pedido, PedidoDetalle>(parametro, new PedidoDetalle())*/
                
                return View(parametro);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
            }

            return Index();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(Parametro parametro)
        {
            try
            {
                var isCliente = false;

                if (base.UserContext.RolesEmpresa.Any(s => s.Rol.RoleName.ToLower().Equals("cliente")))
                    isCliente = true;

                if (isCliente)
                    Session["isCliente"] = true;
                else
                    Session["isCliente"] = false;


                if (base.IsAdmin)
                {
                    Session["IsAdmin"] = true;
                    Session["isCliente"] = false;
                }
                else
                    Session["IsAdmin"] = false;

                try
                {
                    parametroRepository.Modify(parametro, User.UserID);
                    SetMessage(SUCCESS, "Cambio Guardado.");
                }
                catch (Exception ex)
                {
                    SetMessage(ERROR, ex.ToString());
                }



            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
            }

            return Index();
        }
    }
}
