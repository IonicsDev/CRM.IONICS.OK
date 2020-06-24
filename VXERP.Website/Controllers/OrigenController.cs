using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRM.Business.Contexts;
using CRM.Business.DAL;
using CRM.Business.Entities;

namespace CRM.Website.Controllers
{
    public class OrigenController : BaseController
    {
        OrigenRepository origenRepository = new OrigenRepository();

        // GET: Origen
        public ActionResult Index()
        {
            var lOrigen = origenRepository.GetAll().ToList();
            return View(lOrigen);
        }

        // GET: Origen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            return View();
        }

        // GET: Origen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Origen/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,FechaActualizacion,FechaCreacion,Estado,idUsuario")] 
            Origen origen)
        {
            if (ModelState.IsValid)
            {
                
                return RedirectToAction("Index");
            }

            return View(origen);
        }

        // GET: Origen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            return View();
        }

        // POST: Origen/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,FechaActualizacion,FechaCreacion,Estado,idUsuario")] Origen origen)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(origen);
        }

        // GET: Origen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            return View();
        }

        // POST: Origen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                origenRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
