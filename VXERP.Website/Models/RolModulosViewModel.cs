using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using CRM.Business.Entities;

namespace CRM.Website.Models
{

    public class RolEmpresaViewModel
    {

        public RolEmpresaViewModel()
        {

        }

        public RolEmpresaViewModel(int Id, Usuario usuario,  Rol rol)
        {
            this.Id = Id;
           
            this.Rol = rol;

            this.Usuario = usuario;
        }

        public RolEmpresaViewModel(RolEmpresa rolempresa)
        {
            this.Id                 = rolempresa.Id;

            this.Rol                = rolempresa.Rol;
        
            this.Usuario            = rolempresa.Usuario;
        }

        public int Id { get; set; }

        [Display(Name = "Usuario")]
        public string UserName { get { return Usuario.UserName; } }
        public Usuario Usuario { get; set; }

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


        [Required]
        [Display(Name="Nombre Rol")]
        public string RolNombre { get { return this.Rol.RoleName; } } 

      

        public Rol Rol { get; set; }



        #region Methods
        public static List<RolEmpresaViewModel> MapVM(List<RolEmpresa> listSource)
        {
            List<RolEmpresaViewModel> listVM = new List<RolEmpresaViewModel>();
            foreach (var item in listSource)
            {
                listVM.Add(new RolEmpresaViewModel(item.Id, item.Usuario,  item.Rol));
            }

            return listVM;
        }

        


        #endregion
    }
}