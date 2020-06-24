using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Business.Entities.BaseEntities;
using CRM.Common.EntityDomain;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CRM.Business.DAL
{

    public class MailRepository : DelRepository<Mail, Int32>
    {
        public MailRepository()
            : base(new ConfigurationContext())
        {

        }

        private IUserContext UserContext { get; set; }

        public MailRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }


    }
}
