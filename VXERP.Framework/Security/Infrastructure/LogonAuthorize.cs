using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VXERP.Framework.Security.Infrastructure
{
    public sealed class LogonAuthorize : AuthorizeAttribute
    {
      
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (!skipAuthorization)
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    if (this.Roles == string.Empty)
                        return;

                    ICustomPrincipal principalUser = HttpContext.Current.User as ICustomPrincipal; 
                    if(principalUser == null)
                        base.OnAuthorization(filterContext);

                    if (!principalUser.Has_Perm(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName, this.Roles))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                    }
                

                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Usuario", action = "Login" }));
                }
              
            }
            base.OnAuthorization(filterContext);
        }



        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
            }
        }

       
    }
}
