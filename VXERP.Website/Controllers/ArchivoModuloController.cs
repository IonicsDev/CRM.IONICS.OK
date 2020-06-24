using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Website.Security.Infrastructure;
using System.Configuration;

namespace CRM.Website.Controllers
{
    public class ArchivoModuloController : BaseController
    {
        ArchivoModuloRepository objectRepository = new ArchivoModuloRepository();

        public JsonResult PopulateDropDownListTiposArchivoModulo(object selectedItem = null)
        {
            TipoArchivoModuloRepository tipoArchivoModulo = new TipoArchivoModuloRepository();
            var listQuery = tipoArchivoModulo.GetAll();
            ViewBag.TipoArchivoModulo_Id = new SelectList(listQuery.ToList(), "Id", "Tipo", selectedItem);

            //Add JsonRequest behavior to allow retrieving states over http get
            return Json(ViewBag.TipoArchivoModulo_Id, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
      
        public ActionResult Index()
        {
            var listobjectViewModel = new List<ArchivoModulo>();
            var objectViewModel = objectRepository.GetFiltered(o => o.Estado == true ,p => p.TipoArchivoModulo);

            PopulateDropDownListTiposArchivoModulo();

            if (objectViewModel.Count() > 0)
            {
                return View(objectViewModel.ToList());
            }
            else
            {
                return View(listobjectViewModel);
            }
        }

        public void AltaArchivosModulo(ArchivoModulo newArchivo, int countFiles, int entityId)
        {
            ArchivoModuloRepository objectRepository = new ArchivoModuloRepository();

            //Consigo el objeto TipoArchivoModulo
            TipoArchivoModuloRepository tipoArchivoModulo = new TipoArchivoModuloRepository();
            TipoArchivoModulo tipoArchivo = tipoArchivoModulo.GetFiltered(x => x.Tipo.ToLower().Trim().Equals(newArchivo.NombreTipoArchivoModulo.ToLower().Trim())).First();

            newArchivo.TipoArchivoModulo_Id = tipoArchivo.Id;

            var currentFile = Request.Files[countFiles];
            if (currentFile != null && currentFile.ContentLength > 0)
            {
                string extension = Path.GetExtension(newArchivo.Path);
                string name = DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString()
                                    + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00")
                                    + "_T" + newArchivo.TipoArchivoModulo_Id + extension;

                string fileName = Server.MapPath(ConfigurationManager.AppSettings["FILES.PATH"]) + name;
                currentFile.SaveAs(fileName);

                newArchivo.Path = name;
            }


            //Si no hay paises va a retornar cero, nunca va a retornar cero porque primero guardo el pais
            //y entonces voy a conseguir el ultimo id que va a corresponder al pais que acabo de guardar
            //if(lastPaisId == null)
            //    lastPaisId = (paisRepository.GetAll().Max(r => r == null ? 0 : r.Id));

            newArchivo.Entity_Id = entityId.ToString();

            objectRepository.Add(newArchivo, this.User.UserID);
        }

        [HttpPost]
        
        public ActionResult Index(string Descripcion, string DescripcionCorta, int? TipoArchivoModulo_Id)
        {
            var listobjectViewModel = new List<ArchivoModulo>();

            var objectViewModel = objectRepository.GetFiltered(q => q.TipoArchivoModulo.Id == (TipoArchivoModulo_Id != null ? TipoArchivoModulo_Id : q.TipoArchivoModulo.Id)
                                                               && q.Descripcion.Contains(!string.IsNullOrEmpty(Descripcion) ? Descripcion : q.Descripcion)
                                                               && q.DescripcionCorta.Contains(!string.IsNullOrEmpty(DescripcionCorta) ? DescripcionCorta : q.DescripcionCorta),
                                                               p => p.TipoArchivoModulo, p => p.Usuario);

            PopulateDropDownListTiposArchivoModulo(TipoArchivoModulo_Id);

            if (objectViewModel.Count() > 0)
            {
                return View(objectViewModel.ToList());
            }
            else
            {
                return View(listobjectViewModel);
            }
        }

        [HttpGet]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create()
        {
            PopulateDropDownListTiposArchivoModulo();
            return View();
        }

        [HttpPost]
     
        public ActionResult Create([Bind(Include = "Descripcion,DescripcionCorta,Path,TipoArchivoModulo_Id")]ArchivoModulo archivoModulo, HttpPostedFileBase file)
        {
            try
            {
                if (Request.Files.Count != 1 || Request.Files[0].ContentLength == 0)
                {
                    ModelState.AddModelError("PathError", "Debe seleccionar un Archivo");
                    PopulateDropDownListTiposArchivoModulo(archivoModulo.TipoArchivoModulo_Id);
                    return View(archivoModulo);
                }

                if (ModelState.IsValid)
                {
                    //Guardar el archivo en la carpeta Files, renombrarlo "ddmmyyyyhhmmss_T1.extension" T1(TipoArchivoModulo)
                    if (Request.Files.Count > 0)
                    {
                        var currentFile = Request.Files[0];

                        if (currentFile != null && currentFile.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file.FileName);
                            string name = DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString()
                                                + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00")
                                                + "_T" + archivoModulo.TipoArchivoModulo_Id + extension;
                            string fileName = Server.MapPath(ConfigurationManager.AppSettings["FILES.PATH"]) + name;

                            currentFile.SaveAs(fileName);

                            archivoModulo.Path = name;
                        }
                    }

                    objectRepository.Add(archivoModulo, this.User.UserID);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            PopulateDropDownListTiposArchivoModulo(archivoModulo.TipoArchivoModulo_Id);

            return View(archivoModulo);
        }

        [HttpGet]

        public ActionResult Download(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var objectViewModel = objectRepository.Get(id.Value, u => u.Usuario, p => p.TipoArchivoModulo).SingleOrDefault();

            if (objectViewModel == null)
            {
                return HttpNotFound();
            }

            PopulateDropDownListTiposArchivoModulo(objectViewModel.TipoArchivoModulo.Id);

            string fileName = Server.MapPath(ConfigurationManager.AppSettings["FILES.PATH"]) + objectViewModel.Path;
            byte[] fileBytes = new byte[0];
            string fileNameSave = objectViewModel.Path;
            try
            {
                fileBytes = System.IO.File.ReadAllBytes(fileName);
            }
            catch (FileNotFoundException)
            {
                SetMessage(ERROR, " El Archivo no se encuentra en el sistema");
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes("El Archivo que quiere bajar no se encuentra en el sistema");
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, "archivoVacio.txt");
            }

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameSave);
        }

        [HttpGet]
     
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var objectViewModel = objectRepository.Get(id.Value, u => u.Usuario, p => p.TipoArchivoModulo).SingleOrDefault();

            if (objectViewModel == null)
            {
                return HttpNotFound();
            }



            PopulateDropDownListTiposArchivoModulo(objectViewModel.TipoArchivoModulo.Id);
            
            return View(objectViewModel);
        }

        [HttpPost]
       
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,DescripcionCorta,Estado,FechaCreacion,FechaActualizacion,idUsuario,Path,TipoArchivoModulo_Id")]ArchivoModulo archivoModulo, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objectRepository.Modify(archivoModulo, this.User.UserID);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            
            return View(archivoModulo);
        }

        [HttpPost]
        public bool ValidateExistingArchivo(string descripcion)
        {
            objectRepository = new ArchivoModuloRepository();
            List<ArchivoModulo> list = objectRepository.GetFiltered(x => x.Descripcion.ToLower().Trim().Equals(descripcion.ToLower().Trim())).ToList();

            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }

        [HttpGet]
      
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var objectViewModel = objectRepository.Get(id.Value).SingleOrDefault();
            if (objectViewModel == null)
            {
                return HttpNotFound();
            }
            objectViewModel.Estado = false;

            objectRepository.Modify(objectViewModel, this.User.UserID);

            return RedirectToAction("Index");
        }
    }
}
