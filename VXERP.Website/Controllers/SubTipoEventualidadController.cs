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
    public class SubTipoEventualidadController : GenericController<SubTipoEventualidad>
    {
        SubTipoEventualidadRepository subTipoEventualidadRepository = new SubTipoEventualidadRepository();
        TipoEventualidadRepository tipoEventualidadRepository = new TipoEventualidadRepository();
        UsuarioRepository usuarioRepository = new UsuarioRepository();

        [LogonAuthorize(Roles = "VIEW,EDIT")]
        public ActionResult Index()
        {
            List<SubTipoEventualidad> subTiposEventualidad = subTipoEventualidadRepository.GetAll(x => x.TipoEventualidad).ToList();
            
            return View("Index", subTiposEventualidad);
        }

        public ActionResult GridViewAllPartial()
        {
            return PartialView("_GridViewAllPartial", subTipoEventualidadRepository.GetAll().ToList());
        }

        //CREATE
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create(int? TipoEventualidad_Id)
        {
            ViewBag.TiposEventualidad = tipoEventualidadRepository.GetFiltered(x => x.Estado == true).ToList();
            SubTipoEventualidad subTipo = new SubTipoEventualidad();

            if(TipoEventualidad_Id != null)
                subTipo.TipoEventualidad_Id = TipoEventualidad_Id.Value;
            
            return View("Create", subTipo);
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create(SubTipoEventualidad subTipoEventualidad)
        {
            if (ModelState.IsValid)
            {
                if (subTipoEventualidadRepository.GetFiltered(x => x.Descripcion.Trim().ToLower()
                    .Equals(subTipoEventualidad.Descripcion.ToLower().Trim())).Any())
                {
                    ModelState.AddModelError("Descripcion", "El Nombre Sub Tipo Eventualidad ya ha sido utilizado!");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.TiposEventualidad = tipoEventualidadRepository.GetFiltered(x => x.Estado == true).ToList();
                return View(subTipoEventualidad);
            }

            try
            {
                subTipoEventualidadRepository.Add(subTipoEventualidad, this.User.UserID);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                ViewBag.TiposEventualidad = tipoEventualidadRepository.GetFiltered(x => x.Estado == true).ToList();
                return View(subTipoEventualidad);
            }

            SetMessage(SUCCESS, "Guardado.");
            return Index();
        }

        //EDIT
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubTipoEventualidad subTipoEventualidad = subTipoEventualidadRepository.Get(id.Value,
                x => x.TipoEventualidad, x => x.Usuario).SingleOrDefault();

            if (subTipoEventualidad == null)
            {
                return HttpNotFound();
            }

            ViewBag.TiposEventualidad = tipoEventualidadRepository.GetFiltered(x => x.Estado == true).ToList();

            return View("Edit", subTipoEventualidad);
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(SubTipoEventualidad subTipoEventualidad)
        {
            Usuario usuario = usuarioRepository.Get(subTipoEventualidad.idUsuario.Value).FirstOrDefault();
            subTipoEventualidad.Usuario = usuario;

            if (ModelState.IsValid)
            {
                if (subTipoEventualidadRepository.GetFiltered(x => x.Descripcion.Trim().ToLower()
                    .Equals(subTipoEventualidad.Descripcion.ToLower().Trim()) && x.Id != subTipoEventualidad.Id).Any())
                {
                    ModelState.AddModelError("Descripcion", "El Nombre Sub Tipo Eventualidad ya ha sido utilizado!");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.TiposEventualidad = tipoEventualidadRepository.GetFiltered(x => x.Estado == true).ToList();
                return View(subTipoEventualidad);
            }

            try
            {
                subTipoEventualidadRepository.Modify(subTipoEventualidad, this.User.UserID);
                SetMessage(SUCCESS, "Guardado.");
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                ViewBag.TiposEventualidad = tipoEventualidadRepository.GetFiltered(x => x.Estado == true).ToList();
                return View(subTipoEventualidad);
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
            SubTipoEventualidad subTipoEventualidad = subTipoEventualidadRepository.Get(id.Value,
                x => x.TipoEventualidad, x => x.Usuario).SingleOrDefault();

            if (subTipoEventualidad == null)
            {
                return HttpNotFound();
            }

            ViewBag.TiposEventualidad = tipoEventualidadRepository.GetFiltered(x => x.Estado == true).ToList();

            return View("View", subTipoEventualidad);
        }

        [HttpGet]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SubTipoEventualidad subTipoEventualidad = subTipoEventualidadRepository.Get(id.Value).SingleOrDefault();
            if (subTipoEventualidad == null)
            {
                return HttpNotFound();
            }
            subTipoEventualidadRepository.Remove(subTipoEventualidad, this.User.UserID);

            return RedirectToAction("Index");
        }

    }
}
