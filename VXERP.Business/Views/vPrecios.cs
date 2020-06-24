using CRM.Business.Entities;
using CRM.Common.EntityDomain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vPrecios : BaseViews.BaseViewString
    {

        public const string VIEW_NAME = "v_Precios";



        public vPrecios()
            : base(VIEW_NAME)
        {
            
        }
        public vPrecios(IList<UsuarioRolCliente> usuarioClientes)
            : base(VIEW_NAME,usuarioClientes)
        {

        }

        public override DataTable GetByID(string Id)
        {
            DataTable resultData = base.GetByID(Id);

            if (base._usuarioClientes != null)
            {
                if (!HasPerm(resultData, base._usuarioClientes.ToList()))
                    throw new Exception(" No tiene Permiso Para ver este Precio");
            }

            return resultData;
        }

        public vPrecios GetById(string id)
        {
            vPrecios ret = null;
            DataTable resultData = base.GetByID(id);

            if (base._usuarioClientes != null)
            {
                if (!HasPerm(resultData, base._usuarioClientes.ToList()))
                    throw new Exception(" No tiene Permiso Para ver este cliente");
            }
            foreach (DataRow row in resultData.Rows)
            {
                ret = new vPrecios();
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
                    if ((int)dr["CodigoCliente"] == clienteRol.Cliente_Id)
                        ret = true;

                }
            }

            return ret;
        }


        public vPrecios GetByUserRol(List<UsuarioRolCliente> listRolCliente)
        {
            vPrecios ret = null;
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
                ret = new vPrecios();
                ret.Id = (string)row["ID"];
            }
            return ret;
        }

       
        

        
    }
}
