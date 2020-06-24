using CRM.Business.Entities;
using CRM.Common.EntityDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Common.ContextDomain;
using CRM.Business.Contexts;
using CRM.Business.Entities.BaseEntities;
using CRM.Business.DataService.SQLSrv;

namespace CRM.Business.DAL
{
    public class ParametroRepository : DelRepository<Parametro, Int32>
    {
        private IUserContext UserContext { get; set; }

        public ParametroRepository()
            : base(new ConfigurationContext())
        {

        }

        public ParametroRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }

        public string GetValueByParamName(string paramName)
        {
          return base.GetFiltered(c => c.ParamName.ToLower() == paramName.ToLower()).Select(m => m.ParamValue).FirstOrDefault();
        }

        public object GetValueBySqlQuery(string query)
        {
            var c = DataAccess.ExecuteQuerry(query).Tables[0];

            return c.Rows[0][0];
        }
    }
}
