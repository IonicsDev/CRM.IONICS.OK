using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using CRM.Business.DAL;
using CRM.Business.Entities;

namespace CRM.Website.Models
{

    public class RolEmpresaViewModelG
    {

        public RolEmpresaViewModelG()
        {

        }

      
        public int Id { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Display(Name = "Nombre Rol")]
        public String NombreRol { get; set; }

         [Display(Name = "Permisos Empresas")]
        public String PermisosEmpresas { get; set; }

        //[Display(Name = "Usuario")]
        //public string UserName { get { return Usuario.UserName; } }
        //public Usuario Usuario { get; set; }

        //[Display(Name = "Empresas")]
        //public string EmpresasRol
        //{
        //    get
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        // Si es para todos
        //        if (this.GrupoEmpresarial == null && this.Compania == null && this.Division == null && this.Sucursal == null)
        //            return "Todos los Grupos y Empresas";

        //        if (this.GrupoEmpresarial != null)
        //            sb.Append("Grupo : " + this.GrupoEmpresarial.Descripcion);

        //        if (this.Compania != null)
        //        {
        //            sb.Append("- Empresa : " + this.Compania.Descripcion);
        //        }

        //        if (this.Division != null)
        //        {
        //            sb.Append("- División : " + this.Division.Descripcion);
        //        }

        //        if (this.Sucursal != null)
        //        {
        //            sb.Append("- Sucursal : " + this.Sucursal.Descripcion);
        //        }

        //        return sb.ToString();

        //    }
        //}


        //[Required]
        //[Display(Name="Nombre Rol")]
        //public string RolNombre { get { return this.Rol.RoleName; } } 

        //public GrupoEmpresarial GrupoEmpresarial { get; set; }
   
        //public Compania Compania { get; set; }
  
        //public Division Division { get; set; }
     
        //public Sucursal Sucursal { get; set; }

        //public Rol Rol { get; set; }



        #region Methods

         public static string GetEmpresasRol(RolEmpresa rolEmpresa)
         {
            
                 StringBuilder sb = new StringBuilder();
                

                 return sb.ToString();

             
         }

         public static List<RolEmpresaViewModelG> MapVM(List<RolEmpresa> listSource)
        {
        
            List<RolEmpresaViewModelG> listVM = new List<RolEmpresaViewModelG>();
            
            foreach (var item in listSource)
            {
               

                var  obj = listVM.Find(o => o.UserId == item.Usuario_Id);
                if (obj == null)
                {
                    RolEmpresaViewModelG rmVM = new RolEmpresaViewModelG();
                    rmVM.Id = item.Id;
                    rmVM.UserId = item.Usuario_Id;
                    rmVM.UserName = item.Usuario.UserName;
                    rmVM.NombreRol = (item.Rol != null ? item.Rol.RoleName : " Undefined");
                    rmVM.PermisosEmpresas = GetEmpresasRol(item);

                    listVM.Add(rmVM);
                }
                else
                {
                    obj.PermisosEmpresas = obj.PermisosEmpresas + " | " + GetEmpresasRol(item);
                }
                

            }

            return listVM;
        }

        #endregion
    }
}