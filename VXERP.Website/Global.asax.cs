using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using CRM.Website.Crosscutting;
using CRM.Website.Security;
using System.Web.Script.Serialization;

namespace CRM.Website
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           
            
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            AuthenticationFactory.SetCurrent(new FormsAuthenticationFactory());
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
                return;

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                if (!string.IsNullOrEmpty(authCookie.Value))
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket == null)
                        return;


                    try
                    {
                        Business.DAL.UsuarioRepository usuarioRepo = new Business.DAL.UsuarioRepository();
                        var authenticationService = AuthenticationFactory.CreateAuthentication();
                          var user =  usuarioRepo.GetUserByUserName(ticket.Name, true);
                          var serializeModel = new CustomPrincipalSerializeModel();
                          serializeModel.UserID = user.Id;
                          serializeModel.FirstName = user.NombreApellido;
                          serializeModel.UserName = user.UserName;


                          var serializer = new JavaScriptSerializer();
                          var userData = serializer.Serialize(serializeModel);

                          authenticationService.Login(user.NombreApellido, user.Password, user.Recordarme, userData);

                          user.FechaUltimoAcceso = DateTime.Now;
                          usuarioRepo.Modify(user, user.Id);
                         // AppSession.Init_Session(user.Id);
                    //    u = uow.LoginWithTicket(ticket.Name);
                    }
                    catch (Exception)
                    {
                        HttpContext.Current.User = null;
                        return;
                    }

                  //  HttpContext.Current.User = new Business.UserPrincipal(u);
                }
            }
            /*else
            {
               Business.UnitOfWork uow = new Business.UnitOfWork();
               Business.User u         = null;
               try
               {
                   u = uow.LoginAsAnonymous();
               }
               catch (Exception)
               {
                   HttpContext.Current.User = null;
                   return;
               }

               HttpContext.Current.User = new Business.UserPrincipal(u);
            }*/
        }
        protected void Application_EndRequest()
        {//here breakpoint
            // under debug mode you can find the exceptions at code: this.Context.AllErrors
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            AuthenticationFactory.CreateAuthentication().PostAuthenticateRequest();
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            
            DevExpress.Web.ASPxWebControl.GlobalTheme = "Metropolis";
        }

        void Session_Start(object sender, EventArgs e)
        {
            

            // Set Session timeout to 180 minutes
            Context.Session.Timeout = 180;
        }
    }

     
}