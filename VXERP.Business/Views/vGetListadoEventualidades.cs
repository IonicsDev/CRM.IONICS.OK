using CRM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vGetListadoEventualidades : BaseViews.BaseViewString
    {

        public const string VIEW_NAME = "ListEventualidades";



        public vGetListadoEventualidades()
            : base(VIEW_NAME)
        {

        }

        public vGetListadoEventualidades(IList<UsuarioRolCliente> usuarioClientes)
            : base(VIEW_NAME, usuarioClientes)
        {

        }

        public DataTable GetListadoEventualidades()
        {
            
            DataTable datos = base.GetViewModel();
            return datos;
        }

        public DataTable GetListadoEventualidades_Usuario(int userId)
        {

            DataTable datos = DataService.SQLSrv.DataAccess.GetDataSet("GetListadoEventualidades_Usuario", new SqlParameter("@UserId", userId)).Tables[0];
            return datos;
        }
        
    }
}
