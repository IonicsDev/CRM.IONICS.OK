using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace CRM.Website.Models
{

    public class LoginModel
    {
     
        [Required(ErrorMessage="El Nombre de Usuario es requerido")]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La Password es requerida")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recordarme?")]
        
        public bool Recordarme { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "El Nombre de Usuario es requerido")]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La Password Actual es requerida")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "La Nueva Password es requerida")]
        [Display(Name = "Nueva Password")]
        [DataType(DataType.Password)]
        public string NuevaPassword { get; set; }

        [Required(ErrorMessage = "Se requiere ingresar nuevamente la Password")]
        [Display(Name = "Verificar la Nueva Password")]
        [DataType(DataType.Password)]
        public string NuevaPasswordAgain { get; set; }
    }
}
