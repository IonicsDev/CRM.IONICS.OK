using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Website.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult AccessDenied()
        {
            return View();
        }
    }

}
