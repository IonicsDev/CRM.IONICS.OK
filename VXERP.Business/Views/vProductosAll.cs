using CRM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vProductosAll : BaseViews.BaseViewString
    {
        public const string VIEW_NAME = "v_ProductosAll";

        public vProductosAll()
            : base(VIEW_NAME)
        {

        }


        public vProductosAll(IList<UsuarioRolCliente> usuarioClientes)
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

        public vProductosAll GetById(string id)
        {
            vProductosAll ret = null;
            DataTable resultData = base.GetByID(id);

            if (base._usuarioClientes != null)
            {
                if (!HasPerm(resultData, base._usuarioClientes.ToList()))
                    throw new Exception(" No tiene Permiso Para ver este Producto");
            }
            foreach (DataRow row in resultData.Rows)
            {
                ret = new vProductosAll();
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


        public vProductosAll GetByUserRol(List<UsuarioRolCliente> listRolCliente)
        {
            vProductosAll ret = null;
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
                ret = new vProductosAll();
                ret.Id = (string)row["ID"];
            }
            return ret;
        }

    }
}
