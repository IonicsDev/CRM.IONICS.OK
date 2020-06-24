using System.Web;
using System.Web.Mvc;
using CRM.Website.Security.Infrastructure;


namespace CRM.Website
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new InternationalizationAttribute());
            filters.Add(new LogonAuthorize());
            //filters.Add(new RequireHttpsAttribute()); 
            filters.Add(new HandleErrorAttribute());
        }
    }
}