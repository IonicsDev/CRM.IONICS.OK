using CRM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Website.Security.Infrastructure;
using CRM.Business.DAL;

namespace CRM.Website.Controllers
{
    public class TiposNovedadController : GenericController<TipoNovedad>
    {
        TipoNovedadRepository tipoNovedadRepository = new TipoNovedadRepository();
        UsuarioRepository usuarioRepository = new UsuarioRepository();

        public ActionResult GridViewAllPartial()
        {
            return PartialView("_GridViewAllPartial", tipoNovedadRepository.GetAll().ToList());
        }

        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult Index()
        {
            return View("Index", tipoNovedadRepository.GetAll().ToList());
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create([Bind(Include = "Descripcion")] TipoNovedad tipoNovedad)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoNovedad);
            }

            if (!tipoNovedadRepository.GetFiltered(x => x.Descripcion.Trim().ToLower().Equals(tipoNovedad.Descripcion.Trim().ToLower())).Any())
            {
                try
                {
                    tipoNovedadRepository.Add(tipoNovedad, this.User.UserID);

                    SetMessage(SUCCESS, " Guardado.");
                }
                catch (Exception ex)
                {
                    SetMessage(ERROR, " Error al guardar el Tipo de Novedad: " + ex.Message);
                    return View(tipoNovedad);
                }
            }
            else
            {
                ModelState.AddModelError("", "El nombre de Tipo de Novedad ya ha sido utilizado");
                return View(tipoNovedad);
            }

            return Index();
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            TipoNovedad tipoNovedad = null;
            try
            {
                tipoNovedad = tipoNovedadRepository.Get(id.Value, x => x.Usuario).FirstOrDefault();
                if (tipoNovedad == null)
                    throw new Exception(" El Tipo de Novedad no existe");

            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                return Index();
            }

            return View(tipoNovedad);
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,DescripcionEstado,FechaCreacion,FechaActualizacion,Estado,idUsuario")] TipoNovedad tipoNovedad)
        {
            Usuario usuario = usuarioRepository.Get(tipoNovedad.idUsuario.Value).FirstOrDefault();
            tipoNovedad.Usuario = usuario;
            if (!ModelState.IsValid)
            {
                return View(tipoNovedad);
            }

            if (!tipoNovedadRepository.GetFiltered(x => (x.Descripcion.Trim().ToLower().Equals(tipoNovedad.Descripcion.Trim().ToLower())) && (x.Id != tipoNovedad.Id)).Any())
            {
                try
                {
                    tipoNovedadRepository.Modify(tipoNovedad, this.User.UserID);

                    SetMessage(SUCCESS, " Guardado.");
                }
                catch (Exception ex)
                {
                    SetMessage(ERROR, " Error al guardar el Tipo de Novedad: " + ex.Message);
                    return View(tipoNovedad);
                }
            }
            else
            {
                ModelState.AddModelError("", "El nombre de Tipo de Novedad ya ha sido utilizado");
                return View(tipoNovedad);
            }

            return Index();
        }

        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult View(int? id)
        {
            TipoNovedad tipoNovedad = null;
            try
            {
                tipoNovedad = tipoNovedadRepository.Get(id.Value, x => x.Usuario).FirstOrDefault();
                if (tipoNovedad == null)
                    throw new Exception(" El Tipo de Novedad no existe");

            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
            }

            return View(tipoNovedad);
        }

        [HttpGet]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var objectViewModel = tipoNovedadRepository.Get(id.Value).SingleOrDefault();
            if (objectViewModel == null)
            {
                return HttpNotFound();
            }
            tipoNovedadRepository.Remove(objectViewModel, this.User.UserID);

            return RedirectToAction("Index");
        }
    }
}
