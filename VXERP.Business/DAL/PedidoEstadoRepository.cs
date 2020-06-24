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

    public class PedidoEstadoRepository : DelRepository<PedidoEstado, Int32>
    {
        public PedidoEstadoRepository()
            : base(new ConfigurationContext())
        {

        }

        private IUserContext UserContext { get; set; }

        public PedidoEstadoRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }

        public void ActualizarEstadosPedidosSP()
        {
            try
            {
                DataService.SQLSrv.DataAccess.ExecuteStoredProcedure("ActualizacionEstadosPed");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
