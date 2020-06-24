using CRM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vGetListadoPedidos : BaseViews.BaseViewString
    {

        public const string VIEW_NAME = "ListPedidos";



        public vGetListadoPedidos()
            : base(VIEW_NAME)
        {

        }

        public vGetListadoPedidos(IList<UsuarioRolCliente> usuarioClientes)
            : base(VIEW_NAME, usuarioClientes)
        {

        }

        public DataTable GetListadoPedidos()
        {
            
            DataTable datos = base.GetViewModel();
            return datos;
        }

        public DataTable GetListadoPedidos(string filter)
        {

            DataTable datos  = DataService.SQLSrv.DataAccess.ExecuteQuerry("SELECT * FROM " + VIEW_NAME + " WHERE " + filter).Tables[0];
            return datos;
        }
        
    }
}
