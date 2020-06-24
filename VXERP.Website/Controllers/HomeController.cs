using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Business.DAL;
using CRM.Website.Security.Infrastructure;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System.Web.UI;

namespace CRM.Website.Controllers
{
    public class HomeController : BaseController
    {
        private UsuarioRepository usuarioRepository = new UsuarioRepository();

        [LogonAuthorize(Roles="VIEW")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
