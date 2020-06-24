using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CRM.Business.Entities;
using System.IO;

namespace CRM.Business.Views
{
    public class vGuardarContacto : BaseViews.BaseViewString
    {

        private const string SP_NAME = "GuardarContacto";



        public vGuardarContacto()
            : base(SP_NAME, true,null)
        {

        }

        public DataTable GuardarContacto(Contacto contacto, string UserName)
        {
            object fotoByte = null;
            if (contacto.Foto != null)
            {
                MemoryStream target = new MemoryStream();
                contacto.Foto.InputStream.CopyTo(target);
                fotoByte = target.ToArray();
            }
            else
            {
                fotoByte = contacto.FotoByte;
            }

            contacto.Cargo = (contacto.Cargo == null) ? " " : contacto.Cargo;
            contacto.Telefono = (contacto.Telefono == null) ? "0" : contacto.Telefono;
            contacto.Movil = (contacto.Movil == null) ? "0" : contacto.Movil;
            contacto.Email = (contacto.Email == null) ? " " : contacto.Email;
            contacto.Interno = (contacto.Interno == null) ? 0 : contacto.Interno;
           

            //@CgCli, @CgDep, @Nombre, @Apellido, @Cargo, @Nivel, @TelOf, @TelMov, @Email, @Responde, @Usuario, @CgPredio, @Fax, @Interno, @CgCount
            DataTable datos = base.GetViewModel_SP(new System.Data.SqlClient.SqlParameter("@CgCli", contacto.CodigoCliente),
                                                    new System.Data.SqlClient.SqlParameter("@Nombre", contacto.Nombre),
                                                    new System.Data.SqlClient.SqlParameter("@Apellido", contacto.Apellido),
                                                    new System.Data.SqlClient.SqlParameter("@Cargo", contacto.Cargo),
                                                    new System.Data.SqlClient.SqlParameter("@TelOf", contacto.Telefono),
                                                    new System.Data.SqlClient.SqlParameter("@TelMov", contacto.Movil),
                                                    new System.Data.SqlClient.SqlParameter("@Email", contacto.Email),
                                                    new System.Data.SqlClient.SqlParameter("@Interno", contacto.Interno),
                                                    new System.Data.SqlClient.SqlParameter("@CgCont", contacto.Id),
                                                    new System.Data.SqlClient.SqlParameter("@Dni", contacto.Dni),
                                                    new System.Data.SqlClient.SqlParameter("@Foto", fotoByte));
            return datos;
        }
        
    }
}
