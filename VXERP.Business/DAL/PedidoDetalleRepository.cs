using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Business.Entities.BaseEntities;
using CRM.Common.EntityDomain;
using System.Data.SqlClient;

namespace CRM.Business.DAL
{

    public class PedidoDetalleRepository : DelRepository<PedidoDetalle, Int32>
    {
        public PedidoDetalleRepository()
            : base(new ConfigurationContext())
        {

        }

        private IUserContext UserContext { get; set; }

        public PedidoDetalleRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }


        /// <summary>
        /// Inserta el pedido en Solutiion
        /// </summary>
        /// <param name="pedidoId"></param>
        /// <param name="userName"></param>
        /// <remarks>SOLO SI TIENE FECHA DE CONFIRMACION</remarks>
        public void PushPedidoERP(int pedidoId, string userName)
        {
            try
            {
                DataService.SQLSrv.DataAccess.ExecuteStoredProcedure("PushPedido", new SqlParameter("@PedidoId", pedidoId),
                                                                                   new SqlParameter("@username", userName));

            }
            catch (Exception ex)
            {
                throw new Exception("Error al subir pedido a Solutiion :" + ex.Message);
            }
        }
    }
}
