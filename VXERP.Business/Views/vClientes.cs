using CRM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vClientes : BaseViews.BaseView
    {

        public const string VIEW_NAME = "v_Clientes";
       

        public vClientes()
            : base(VIEW_NAME)
        {

        }

        public vClientes(IList<UsuarioRolCliente> usuarioClientes)
            : base(VIEW_NAME,usuarioClientes)
        {

        }

        public vClientes GetByUserRol(List<UsuarioRolCliente> listRolCliente)
        {
            vClientes ret = null;
            string queryFilter =" ID in ( ";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(queryFilter);

            foreach (var clienteRol in listRolCliente)
            {
                sb.Append(clienteRol.Cliente_Id.ToString());
                sb.Append(",");

            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(") ");
            ret = new vClientes();
            ret.Datos = base.GetByFilter(sb.ToString());

            foreach (DataRow row in base.Datos.Rows )
            {
                
                ret.Id = (int)row["ID"];
            }
            return ret;
        }

        public override DataTable GetByID(int Id)
        {
            DataTable resultData = base.GetByID(Id);

            if (base._usuarioClientes != null)
            {
                if (!HasPerm(resultData, base._usuarioClientes.ToList()))
                    throw new Exception(" No tiene Permiso Para ver este cliente");
            }

            return resultData;
        }

        public vClientes GetById(int id)
        {
            vClientes ret= null;
            DataTable resultData = base.GetByID(id);

            if (base._usuarioClientes != null)
            {
                if (!HasPerm(resultData, base._usuarioClientes.ToList()))
                    throw new Exception(" No tiene Permiso Para ver este cliente");
            }
            foreach (DataRow row  in resultData.Rows)
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
                    if ((int)dr["ID"] == clienteRol.Cliente_Id)
                        ret = true;

                }
            }

            return ret;
        }

        
    }
}
