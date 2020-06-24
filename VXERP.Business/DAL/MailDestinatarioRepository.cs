using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Business.Entities.BaseEntities;
using CRM.Common.EntityDomain;

namespace CRM.Business.DAL
{

    public class MailDestinatarioRepository : DelRepository<MailDestinatario, Int32>
    {
        public MailDestinatarioRepository()
            : base(new ConfigurationContext())
        {

        }

        private IUserContext UserContext { get; set; }

        public MailDestinatarioRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }


        public string GetDestinatariosMail(int mailID)
        {
            StringBuilder sb = new StringBuilder();
            var destinatarios = GetFiltered(f => f.Mail_Id == mailID);

            foreach (var destinatario in destinatarios)
            {
                sb.Append(destinatario.DestinatarioMail);
                sb.Append(";");

            }
            if(sb.Length > 1)
                sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
    }
}
