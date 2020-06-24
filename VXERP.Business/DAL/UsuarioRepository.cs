using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Common.EntityDomain;
using CRM.Business.Views;

namespace CRM.Business.DAL
{
    public class UsuarioRepository : DelRepository<Usuario, Int32>
    {
        public UsuarioRepository()
            : base(new ConfigurationContext())
        {

        }

        public string GetPasswordByUserName(string userName)
        {
            return base.GetFiltered(c => c.UserName.ToLower() == userName.ToLower()).Select(m => m.Password).FirstOrDefault();
        }

        public Usuario GetUserByUserName(string userName, bool isThowException = true)
        {
            var user = base.GetFiltered(c => c.UserName.ToLower() == userName.ToLower()).FirstOrDefault();
            if (user == null && isThowException) throw new Exception("Usuario no encontrado");
            
            return user;
        }



        
        /// <summary>
        /// Login App
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool IsValid(Usuario usuario)
        {
           var result = base.GetFiltered(obj => obj.UserName.Equals(usuario.UserName.Trim()) &&
                            obj.Password.Equals(usuario.Password.Trim())).FirstOrDefault();
            
                if (result == null)
                    return false;

                if (GetRolesEmpresaByUsuarioId(result.Id).Any())
                {
                    if (result.Estado)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return false;
            }

        public bool IsClienteActivo(Usuario usuario)
        {
            var user = base.GetFiltered(obj => obj.UserName.Equals(usuario.UserName.Trim()) &&
                            obj.Password.Equals(usuario.Password.Trim())).FirstOrDefault();

            if (user == null)
                return false;

            //user.RolesEmpresa = GetRolesEmpresaByUsuarioId(Convert.ToInt32(user.idUsuario));

            vClientes cliente = new vClientes();
            string filtro = string.Format("Cuit = '{0}'", usuario.UserName);
            var dtcliente = cliente.GetByFilter(filtro);
            if (dtcliente.Rows.Count == 0)
                return false;
            string activo = dtcliente.Rows[0]["ACTIVO"].ToString();

            if (user.RolesEmpresa.Any(i => i.Rol_Id == 25) && activo == "N")
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Obtiene los roles de un usuario
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public ICollection<RolEmpresa> GetRolesEmpresaByUsuarioId(int idUsuerio)
        {
            var result = base.GetFiltered(u => u.Id == idUsuerio,
                a => a.RolesEmpresa.Select(c => c.Rol.ModulosPermiso.Select(o => o.Modulo))
             ).SingleOrDefault();
            

            return result.RolesEmpresa;
        }




        //public void ChangePassword(UserDTO user, string password)
        //{
        //    if (string.IsNullOrWhiteSpace(password)) throw new Exception("password cannot bu null");

        //    var dbUser = GetUserByUserName(user.UserName);
        //    dbUser.Password = password;
        //    Repository.Modify(dbUser);
        //}
    }

    }
    

