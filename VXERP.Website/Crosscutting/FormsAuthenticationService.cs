using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using CRM.Business.DAL;
using CRM.Website.Models;
using CRM.Website.Security;
using CRM.Business.Entities;

namespace CRM.Website.Crosscutting
{

    public class FormsAuthenticationService : IAuthentication
    {
       private UsuarioRepository usuarioRepository = new UsuarioRepository();
       private UsuarioRolClienteRepository usuarioRolClienteRepository = new UsuarioRolClienteRepository();

        public void Login(string userName, string password, bool isPersistent, string customData)
        {
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                                     1,
                                                     userName,
                                                     DateTime.Now,
                                                     DateTime.Now.AddDays(15),
                                                     true,
                                                     customData);

            var encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            HttpContext.Current.Response.Cookies.Add(faCookie);

          

        }

        public void Logout()
        {
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
        }

        public ICustomPrincipal GetUser()
        {
            return HttpContext.Current.User as ICustomPrincipal;
        }

        public void PostAuthenticateRequest()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
              
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializer = new JavaScriptSerializer();

                var serializeModel = serializer.Deserialize<CustomPrincipalSerializeModel>(authTicket.UserData);

                CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                AppSession.SetUserID(serializeModel.UserID);
                newUser.UserID = serializeModel.UserID;
                newUser.FirstName = serializeModel.FirstName;
                newUser.LastName = serializeModel.LastName;

                newUser.UserName = serializeModel.UserName;

                HttpContext.Current.User = System.Threading.Thread.CurrentPrincipal = newUser;
            }
        }
    }
}
