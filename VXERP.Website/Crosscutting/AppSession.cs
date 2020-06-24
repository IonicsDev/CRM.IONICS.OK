using CRM.Business.DAL;
using CRM.Website.Models;
using CRM.Website.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Website.Crosscutting
{
    public static class AppSession
    {

        private static UsuarioRolClienteRepository usuarioRolClienteRepository = new UsuarioRolClienteRepository();
        private static UsuarioRepository usuarioRepository = new UsuarioRepository();
        private static ModuloRepository moduloRepository = new ModuloRepository();
        private static ListadoRepository listadoRepository = new ListadoRepository();
        private static PropiedadNavegacionListadoRepository propiedadesNavegacionRepository = new PropiedadNavegacionListadoRepository();

        private static int UserId { get; set; }

        public static void SetUserID(int id)
        {
            UserId = id;
        }

       

        public  static ICustomPrincipal CurrentUser
        {
            get
            {
                CustomPrincipal User = new CustomPrincipal(UserId.ToString());

               if (System.Web.HttpContext.Current.Session == null)
               {
                   User.RolesCliente = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Usuario_Id == UserId && s.Estado == true, s => s.UsuarioRol).ToList();
                   User.RolesEmpresa = usuarioRepository.GetRolesEmpresaByUsuarioId(UserId);
               }
               else
               {
                   User.RolesCliente =RolesCliente;
                   User.RolesEmpresa = RolesEmpresa;
               }
               return (ICustomPrincipal) User;
            }
        }

        public static ICollection<Business.Entities.Listado> Listados
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["UserListados"] == null)
                {
                    System.Web.HttpContext.Current.Session["UserListados"] = listadoRepository.GetAll().ToList();
                }

                return (ICollection<Business.Entities.Listado>)System.Web.HttpContext.Current.Session["UserListados"];
            }
        }


        public static ICollection<Business.Entities.PropiedadNavegacionListado> PropiedadesNavegacion
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["UserPropiedadesNavegacion"] == null)
                {
                    System.Web.HttpContext.Current.Session["UserPropiedadesNavegacion"] = propiedadesNavegacionRepository.GetAll().ToList();
                }

                return (ICollection<Business.Entities.PropiedadNavegacionListado>)System.Web.HttpContext.Current.Session["UserPropiedadesNavegacion"];
            }
        }


        public static ICollection<Business.Entities.UsuarioRolCliente> RolesCliente
        {
            get {
                if (System.Web.HttpContext.Current.Session["UserRolesCliente"] == null)
                {
                    System.Web.HttpContext.Current.Session["UserRolesCliente"] = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Usuario_Id == UserId && s.Estado == true, s => s.UsuarioRol).ToList();
                   
                }
                    return (ICollection<Business.Entities.UsuarioRolCliente>)System.Web.HttpContext.Current.Session["UserRolesCliente"];
            }
        }

        public static ICollection<Business.Entities.RolEmpresa> RolesEmpresa
        {
            get{
                  if(System.Web.HttpContext.Current.Session["RolesEmpresa"] == null)
                      System.Web.HttpContext.Current.Session["RolesEmpresa"] = usuarioRepository.GetRolesEmpresaByUsuarioId(UserId);

                    return (ICollection<Business.Entities.RolEmpresa>)System.Web.HttpContext.Current.Session["RolesEmpresa"];
            }
        }

        public static List<Business.Entities.Modulo> Modulos
        {
            get
            {
                if(System.Web.HttpContext.Current.Session["UserModulos"] == null)
                  System.Web.HttpContext.Current.Session["UserModulos"] =  moduloRepository.GetAll().ToList();

                return  (List<Business.Entities.Modulo>)System.Web.HttpContext.Current.Session["UserModulos"];
            }
           
        }


        public static void Init_Session(int userId)
        {


            System.Web.HttpContext.Current.Session["UserRolesCliente"] = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Usuario_Id == userId && s.Estado == true, s => s.UsuarioRol).ToList();
            System.Web.HttpContext.Current.Session["RolesEmpresa"] = usuarioRepository.GetRolesEmpresaByUsuarioId(userId);
            //var listado = Listados;
            //var propiedadesNav = PropiedadesNavegacion;
          //  Modulos = moduloRepository.GetAll().ToList();

        }
    }
}