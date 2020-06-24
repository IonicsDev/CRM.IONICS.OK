using CRM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vContactos : BaseViews.BaseView
    {

        public const string VIEW_NAME = "v_Contactos";



        public vContactos()
            : base(VIEW_NAME)
        {

        }
        public vClientes GetByUserRol(List<UsuarioRolCliente> listRolCliente)
        {
            vClientes ret = new vClientes();

            string queryFilter = " [CodigoCliente] in ( ";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(queryFilter);

            foreach (var clienteRol in listRolCliente)
            {
                sb.Append(clienteRol.Cliente_Id.ToString());
                sb.Append(",");

            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(") ");
            base.Datos = base.GetByFilter(sb.ToString());

            foreach (DataRow row in Datos.Rows)
            {
                ret = new vClientes();
                ret.Id = (int)row["ID"];
            }
            return ret;
        }

        public Boolean HasPerm(DataTable datos, List<UsuarioRolCliente> listRolCliente)
        {
            Boolean ret = false;

            foreach (DataRow dr in datos.Rows)
            {
                foreach (var clienteRol in listRolCliente)
                {
                    if ((int)dr["CodigoCliente"] == clienteRol.Cliente_Id)
                        ret = true;

                }
            }

            return ret;
        }

        public vContactos GetById(int id)
        {
            vContactos ret = null;

            foreach (DataRow row in base.GetByID(id).Rows)
            {
                ret = new vContactos();
                ret.Id = (int)row["ID"];
            }

            return ret;
        }

        

        
    }
}
