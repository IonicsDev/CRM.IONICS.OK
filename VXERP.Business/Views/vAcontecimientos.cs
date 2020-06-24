using CRM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
namespace CRM.Business.Views
{
    public class vAcontecimientos : BaseViews.BaseViewString
    {
        public const string VIEW_NAME = "ListAcontecimientos";


        public vAcontecimientos()
            : base(VIEW_NAME)
        {

        }

        public vAcontecimientos(IList<UsuarioRolCliente> usuarioClientes)
            : base(VIEW_NAME,usuarioClientes)
        {

        }

        public DataTable GetListadoAcontecimientos()
        {
            DataTable datos = base.GetViewModel();
            return datos;
        }
    }
}
