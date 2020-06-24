using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Business.Entities.BaseEntities;
using CRM.Common.EntityDomain;
using System.Data;
using System.Data.SqlClient;

namespace CRM.Business.DAL
{

    public class PedidoRepository : DelRepository<Pedido, Int32>
    {
        public PedidoRepository()
            : base(new ConfigurationContext())
        {

        }

        private IUserContext UserContext { get; set; }

        public PedidoRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }

      

        public void RemovePedidoSP(int pedidoId)
        {
            try
            {
                DataService.SQLSrv.DataAccess.ExecuteStoredProcedure("RemovePedido", new SqlParameter("@PedidoId", pedidoId));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Pedido> ExecuteQuery(int ClienteID)
        {
            DataTable datos = null;
            List<Pedido> pedidos = new List<Pedido>();
            try
            {
                datos = DataService.SQLSrv.DataAccess.ExecuteQuerry("SELECT * FROM ListPedidos WHERE"// Estado"
                    +"(FechaAutorizadaRecepcion is not null AND Estado <> 0 AND Estado  <> 6  AND Estado <> 9)"
                    +/*" In ( 0 )*/" And Cg_Cli = " + ClienteID + " ORDER BY ID").Tables[0];

                if (datos.Rows.Count > 0)
                {
                    foreach (DataRow row in datos.Rows)
	                {
		                Pedido pedido = new Pedido();
                        pedido.Id = Convert.ToInt32(row["ID"].ToString());
                        pedido.Fe_Ped = Convert.ToDateTime(row["FechaSolicitadaRecepcion"].ToString());
                        pedido.Observaciones = row["Observaciones"].ToString();
                        if(row["Conf_Usu"].ToString() != "")
                            pedido.Conf_Usu = Convert.ToInt32(row["Conf_Usu"].ToString());
                        if (row["FechaAutorizadaRecepcion"].ToString() != "")
                            pedido.Conf_Fecha = Convert.ToDateTime(row["FechaAutorizadaRecepcion"].ToString());
                        if (row["Fe_Retiro"].ToString() != "")
                            pedido.Fe_Retiro = Convert.ToDateTime(row["Fe_Retiro"].ToString());
                        pedido.Cg_Cli = Convert.ToInt32(row["Cg_Cli"].ToString());
                       // pedido.Cuit = row["Cuit"].ToString();
                        if (row["FechaAutorizadaRecepcion"].ToString() != "")
                            pedido.Conf_Fecha = Convert.ToDateTime(row["FechaAutorizadaRecepcion"].ToString());
                        pedido.Nombre_Cliente = row["Des_Cli"].ToString();

                        pedidos.Add(pedido);
	                }
                }

                List<Pedido> UniquePedidos = new List<Pedido>();

                foreach(Pedido pedido in pedidos)
                {
                    if (!UniquePedidos.Any(x => x.Id == pedido.Id))
                    {
                        UniquePedidos.Add(pedido);
                    }
                }

                return UniquePedidos.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
      
    }
}
