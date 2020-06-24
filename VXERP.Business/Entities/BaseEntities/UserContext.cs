using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Business.DAL;

namespace CRM.Business.Entities.BaseEntities
{
    public class UserContext : IUserContext
    {

        public int UserID { get; set; }

        public string UserName { get; set; }

        public ICollection<RolEmpresa> RolesEmpresa { get; set; }

        public ICollection<UsuarioRolCliente> RolesCliente { get; set; }

        public List<Modulo> Modulos { get; set; }

        public Modulo CurrentModulo { get; set; }


       


       
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

            foreach (var ob in this.RolesEmpresa.Select(o => o.Rol.ModulosPermiso))
            {
                foreach (var ob2 in ob.ToList())
                {
                    if (ob2.Modulo.URL == modulo + "/" + accion && (permiso == string.Empty ? true : ob2.Accion == permiso))
                        return true;
                }
            }
            return false;
        }

        public bool Has_Perm(int modulo)
        {
            //Validacion para visualizar los Modulos correspondientes por cada RolEmpresa

            foreach (var ob in this.RolesEmpresa.Select(o => o.Rol.ModulosPermiso))
            {
                foreach (var ob2 in ob.ToList())
                {
                    if (ob2.Modulo.Id == modulo)
                        return true;
                }
            }
            return false;
        }
    }
}
