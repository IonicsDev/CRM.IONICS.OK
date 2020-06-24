using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Website.Crosscutting;
using System.Web.Mvc;


namespace CRM.Website.Models
{

    public class CustomPrincipal : ICustomPrincipal
    {
        public CustomPrincipal(string email)
        {
            this.Identity = new GenericIdentity(email);
        }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return true;
        }
        public bool IsInRole(string role, string _ControllerName)
        {
            if (this.RolesEmpresa == null)
                return true;

            foreach (var item in this.RolesEmpresa)
            {
                foreach (var modPermiso in item.Rol.ModulosPermiso)
                {
                    if (modPermiso.Modulo.URL.ToUpper().StartsWith(_ControllerName.ToUpper()))
                        if (modPermiso.Accion == role)
                            return true;
                }
            }
            return false;
        }

       public ICollection<RolEmpresa> RolesEmpresa { get; set; }

        public ICollection<UsuarioRolCliente> RolesCliente
        {
            get;
            set;
        }

        public Boolean IsAdmin { get; set; }

      
        /// <summary>
        /// Determina si el usuario tiene permiso sobre el Modulo y la Accion
        /// </summary>
        /// <param name="modulo"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        public bool Has_Perm(string modulo, string accion, string permiso)
        {
            //Valido si no existe dos Modulos iguales con diferente accion.
           // var query = this.RolesEmpresa.Select(p=>p.Rol.ModulosPermiso.Select(p.));

          //  return this.RolesEmpresa.Any(p => p.Rol.ModulosPermiso.Select(o => o.Modulo.URL.ToLower() == modulo.ToLower() + "/" + accion.ToLower() && o.Accion == permiso).FirstOrDefault());
            RolEmpresa re = null;
            List<string> listPerm = new List<string>();

            if (permiso.Contains(','))
            {
                foreach (var per in permiso.Split(',').ToList())
                {
                    listPerm.Add(per);
                }
            }
            else
            {
                listPerm.Add(permiso);
            }

                re = RolesEmpresa.First();

            if (re == null && RolesEmpresa.Count() > 0)
            {
                re = re = RolesEmpresa.First();
            }
            foreach (var ob2 in re.Rol.ModulosPermiso)
            {
                foreach (var per in listPerm)
                {
                    if (ob2.Modulo.URL.ToUpper().StartsWith(modulo.ToUpper()) && (permiso == string.Empty ? true : ob2.Accion == per))
                        return true;
                }
            }
            //foreach (var ob in this.RolesEmpresa.Select(o => o.Rol.ModulosPermiso))
            //{
            //    foreach (var ob2 in ob.ToList())
            //    {
            //        if (ob2.Modulo.URL.ToUpper().StartsWith(modulo.ToUpper()) && (permiso == string.Empty ?  true : ob2.Accion == permiso))
            //            return true;
            //    }
            //}
            return false;
        }

        /// <summary>
        /// Verifica el permiso sobre un modulo o un padre de modulo
        /// </summary>
        /// <param name="modulo_id"></param>
        /// <returns></returns>
        public bool Has_Perm(int modulo_id)
        {
            Modulo modulo = (new ModuloRepository()).Get(modulo_id).FirstOrDefault();
            if(modulo == null)
                return false;

            foreach (var ob in this.RolesEmpresa.Select(o => o.Rol.ModulosPermiso))
            {
                foreach (var ob2 in ob.ToList())
                {
                    if (ob2.Modulo_Id == modulo.Id)
                        return true;
                    else
                    {
                        //Tiene permiso en alguno de sus hijos?
                      var listChilds =   (new ModuloRepository()).GetChildsList(modulo,AppSession.Modulos);
                        foreach( var item in listChilds)
                        {
                            if(item.Id == ob2.Modulo_Id)
                                return true;
                        }
                    }
                }
            }

            return false;
        }

      
    }
}
