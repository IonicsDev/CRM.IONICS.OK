using CRM.Business.Entities;
using CRM.Common.EntityDomain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vProductos : BaseViews.BaseViewString
    {

        public const string VIEW_NAME = "v_Productos";



        public vProductos()
            : base(VIEW_NAME)
        {
            
        }


        public vProductos(IList<UsuarioRolCliente> usuarioClientes)
            : base(VIEW_NAME,usuarioClientes)
        {

        }

        public override DataTable GetByID(string Id)
        {
            DataTable resultData = base.GetByID(Id);

            if (base._usuarioClientes != null)
            {
                if (!HasPerm(resultData, base._usuarioClientes.ToList()))
                    throw new Exception(" No tiene Permiso Para ver este Producto");
            }

            return resultData;
        }

        public vProductos GetById(string id)
        {
            vProductos ret = null;
            DataTable resultData = base.GetByID(id);

            if (base._usuarioClientes != null)
            {
                if (!HasPerm(resultData, base._usuarioClientes.ToList()))
                    throw new Exception(" No tiene Permiso Para ver este Producto");
            }
            foreach (DataRow row in resultData.Rows)
            {
                ret = new vProductos();
                ret.Id = (string)row["ID"];

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
                    if (int.Parse(dr["CodigoCliente"].ToString()) == clienteRol.Cliente_Id)
                        ret = true;

                }
            }

            return ret;
        }


        public vProductos GetByUserRol(List<UsuarioRolCliente> listRolCliente)
        {
            vProductos ret = null;
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

            foreach (DataRow row in base.GetByFilter(sb.ToString()).Rows)
            {
                ret = new vProductos();
                ret.Id = (string)row["ID"];
            }
            return ret;
        }


        

        
    }
}
