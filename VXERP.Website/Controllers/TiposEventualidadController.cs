using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Website.Security.Infrastructure;
using CRM.Business.DAL;
using CRM.Business.Entities;
using System.Net;

namespace CRM.Website.Controllers
{
    public class TiposEventualidadController : GenericController<TipoEventualidad>
    {
        TipoEventualidadRepository tipoEventualidadRepository = new TipoEventualidadRepository();
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        SubTipoEventualidadRepository subTipoEventualidadRepository = new SubTipoEventualidadRepository();

        [LogonAuthorize(Roles = "VIEW,EDIT")]
        public ActionResult Index()
        {
            List<TipoEventualidad> tiposEventualidad = tipoEventualidadRepository.GetAll().ToList();
            //Filtradas por algo mas??

            return View("Index", tiposEventualidad);
        }

        public ActionResult GridViewAllPartial()
        {
            return PartialView("_GridViewAllPartial", tipoEventualidadRepository.GetAll().ToList());
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create(TipoEventualidad tipoEventualidad)
        {
            if (ModelState.IsValid)
            {
                if (tipoEventualidadRepository.GetFiltered(x => x.Descripcion.Trim().ToLower()
                    .Equals(tipoEventualidad.Descripcion.ToLower().Trim())).Any())
                {
                    ModelState.AddModelError("Descripcion", "El Nombre Tipo Eventualidad ya ha sido utilizado!");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(tipoEventualidad);
            }

            try
            {
                tipoEventualidadRepository.Add(tipoEventualidad, this.User.UserID);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                return View(tipoEventualidad);
            }

            return Edit(tipoEventualidad.Id);
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEventualidad tipoEventualidad = tipoEventualidadRepository.Get(id.Value, x => x.Usuario).SingleOrDefault();

            if (tipoEventualidad == null)
            {
                return HttpNotFound();
            }
            tipoEventualidad.SubTiposEventualidad = subTipoEventualidadRepository.GetFiltered(x => x.TipoEventualidad_Id == tipoEventualidad.Id
                && x.Estado == true).ToList();

            Session["SubTiposEventualidad"] = tipoEventualidad.SubTiposEventualidad;
            Session["IsView"] = false;

            return View("Edit", tipoEventualidad);
        }

        public ActionResult GridViewSubTipos()
        {
            List<SubTipoEventualidad> subTiposEventualidad = (List<SubTipoEventualidad>)Session["SubTiposEventualidad"];
            return PartialView("_GridViewSubTipos", subTiposEventualidad.ToList());
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(TipoEventualidad tipoEventualidad)
        {
            Usuario usuario = usuarioRepository.Get(tipoEventualidad.idUsuario.Value).FirstOrDefault();
            tipoEventualidad.Usuario = usuario;

            if (ModelState.IsValid)
            {
                if (tipoEventualidadRepository.GetFiltered(x => x.Descripcion.Trim().ToLower()
                    .Equals(tipoEventualidad.Descripcion.ToLower().Trim()) && x.Id != tipoEventualidad.Id).Any())
                {
                    ModelState.AddModelError("Descripcion", "El Nombre Tipo Eventualidad ya ha sido utilizado!");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(tipoEventualidad);
            }

            try
            {
                tipoEventualidadRepository.Modify(tipoEventualidad, this.User.UserID);
                SetMessage(SUCCESS, "Guardado.");
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                return View(tipoEventualidad);
            }

            return Index();
        }

        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEventualidad tipoEventualidad = tipoEventualidadRepository.Get(id.Value, x => x.Usuario).SingleOrDefault();

            if (tipoEventualidad == null)
            {
                return HttpNotFound();
            }
            tipoEventualidad.SubTiposEventualidad = subTipoEventualidadRepository.GetFiltered(x => x.TipoEventualidad_Id == tipoEventualidad.Id
                && x.Estado == true).ToList();

            Session["SubTiposEventualidad"] = tipoEventualidad.SubTiposEventualidad;
            Session["IsView"] = true;

            return View("View", tipoEventualidad);
        }

        [HttpGet]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TipoEventualidad tipoEventualidad = tipoEventualidadRepository.Get(id.Value).SingleOrDefault();
            if (tipoEventualidad == null)
            {
                return HttpNotFound();
            }
            tipoEventualidadRepository.Remove(tipoEventualidad, this.User.UserID);

            return RedirectToAction("Index");
        }

    }
}
