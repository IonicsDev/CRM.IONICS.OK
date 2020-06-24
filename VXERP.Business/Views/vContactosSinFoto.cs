using CRM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vContactosSinFoto : BaseViews.BaseView
    {

        private const string VIEW_NAME = "v_ContactosSinFoto";



        public vContactosSinFoto()
            : base(VIEW_NAME)
        {

        }
        public vContactosSinFoto GetAll()
        {
            vContactosSinFoto ret = new vContactosSinFoto();
            ret.Datos = GetViewModel();
            return ret;
        }

        public vContactosSinFoto GetByUserRol(List<UsuarioRolCliente> listRolCliente)
        {
            vContactosSinFoto ret = new vContactosSinFoto();
            if (listRolCliente.Any(s => s.UsuarioRol.Rol_Id == 1))
            {
               ret.Datos = GetViewModel();
               return ret;
            }
            if (listRolCliente.Count == 0)
            {
                ret.Datos = GetViewModel();
                return ret;
            }
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
                ret = new vContactosSinFoto();
                ret.Id = (int)row["ID"];
            }
            return this;
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

        public static implicit operator vContactosSinFoto(vContactos v)
        {
            throw new NotImplementedException();
        }
    }
}
