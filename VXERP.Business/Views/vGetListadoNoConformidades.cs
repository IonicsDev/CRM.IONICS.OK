using CRM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vGetListadoNoConformidades : BaseViews.BaseViewString
    {

        public const string VIEW_NAME = "ListNoConformidades";



        public vGetListadoNoConformidades()
            : base(VIEW_NAME)
        {

        }

        public vGetListadoNoConformidades(IList<UsuarioRolCliente> usuarioClientes)
            : base(VIEW_NAME, usuarioClientes)
        {

        }

        public DataTable GetListadoNoConformidades()
        {
            
            DataTable datos = base.GetViewModel();
            return datos;
        }

        public DataTable vGetListadoNoConformidades_Usuario(int userId)
        {

            DataTable datos = DataService.SQLSrv.DataAccess.GetDataSet("GetListadoNoConformidades_Usuario", new SqlParameter("@UserId", userId)).Tables[0];
            return datos;
        }
        
    }
}
