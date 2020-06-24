using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    class vDetallePedidos : BaseViews.BaseViewString
    {
        private const string SP_NAME = "DetallePedido";



        public vDetallePedidos()
            : base(SP_NAME, true,null)
        {

        }

        public DataTable DetallePedidos(int PedidoId)
        {
            DataTable datos = base.GetViewModel_SP(new System.Data.SqlClient.SqlParameter("@PedidoId", PedidoId));

            return datos;
        }
    }
}
