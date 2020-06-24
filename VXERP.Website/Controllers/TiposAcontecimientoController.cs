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

    public class TiposAcontecimientoController : GenericController<TipoAcontecimiento>
    {
        private TiposAcontecimientoRepository tiposAcontecimientoRepository = new TiposAcontecimientoRepository();
        //
        // GET: /TiposAcontecimiento/

        public ActionResult Index()
        {
            var lTiposAcontecimiento = tiposAcontecimientoRepository.GetAll().ToList();
            Session["ListadoTiposAcontecimiento"] = lTiposAcontecimiento;
            return View("Index", lTiposAcontecimiento);
        }

        public ActionResult GridViewAllPartial()
        {
            var tipoacont = (IEnumerable<TipoAcontecimiento>)Session["ListadoTiposAcontecimiento"];
            return PartialView("_GridViewAllPartial", tipoacont);
        }

        [HttpGet]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create()
        {
            TipoAcontecimiento newtp = new TipoAcontecimiento();
            return View("Create", newtp);
        }

        //
        // GET: /TiposAcontecimiento/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // POST: /TiposAcontecimiento/Create

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create(TipoAcontecimiento tipoAcontecimiento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (tiposAcontecimientoRepository.GetFiltered(t=> t.Descripcion == tipoAcontecimiento.Descripcion).Any())
                    {
                        ModelState.AddModelError(string.Empty, "Tipo de Acontecimiento ya Utilizado");
                        return View(tipoAcontecimiento);
                    }
                    else
                    {
                        if (tipoAcontecimiento.Descripcion != "")
                        {
                            // TODO: Add insert logic here
                            tiposAcontecimientoRepository.Add(tipoAcontecimiento, this.User.UserID);
                            SetMessage(SUCCESS, "Guardado.");
                        }
                    }
                       
                }

                return Index();
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.ToString());
                return View();
            }
        }

        //
        // GET: /TiposAcontecimiento/Edit/5
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(int id)
        {
            try
            {

                TipoAcontecimiento tipoacontecimiento = tiposAcontecimientoRepository.Get(id).SingleOrDefault();

                return View(tipoacontecimiento);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
            }

            return Index();
        }

        //
        // POST: /TiposAcontecimiento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(TipoAcontecimiento tipoacontecimiento)
        {
            try
            {
                tiposAcontecimientoRepository.Modify(tipoacontecimiento, User.UserID);
                SetMessage(SUCCESS, "Cambio Guardado.");
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.ToString());
            }

            return Index();
        }

        //
        // GET: /TiposAcontecimiento/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /TiposAcontecimiento/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
