using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.DAL;
using CRM.Business.Entities.BaseEntities;
using CRM.Common.EntityDomain;

namespace CRM.Business.Entities
{
    [Table("Usuarios")]
    public class Usuario : BaseEntity
    {
        public Usuario()
        {
            this.RolesEmpresa = new HashSet<RolEmpresa>();
        }

        [Required(ErrorMessage = "El Nombre y Apellido es requerido")]
        [Display(Name = "Nombre y Apellido")]
        [ColumnNameGridView(Name = "Nombre y Apellido")]
        public string NombreApellido { get; set; }

        [NotMapped]
        [Invisible]
        public new int? idUsuario { get; set; }

        [Required(ErrorMessage = "El Nombre de Usuario es requerido")]
        [Display(Name = "Nombre de Usuario")]
        [ColumnNameGridView(Name = "Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La Password es requerida")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
      
        public string Password { get; set; }

        [Required(ErrorMessage = "El Email es requerido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Cambiar contraseña en el primer Login")]
        [InvisibleAttribute]
        public bool CambiarPass { get; set; }

        [InvisibleAttribute]
        public DateTime? FechaUltimoAcceso { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Estado")]
        [ColumnNameGridView(Name = "Estado")]
        public string DescripcionEstado
        {
            get
            {
                return (Estado ? "Activo" : "Inactivo");
            }
        }

        [Display(Name = "Recordarme?")]
        [NotMapped]
        [InvisibleAttribute]
        public bool Recordarme { get; set; }

        [InvisibleAttribute]
        public virtual ICollection<RolEmpresa> RolesEmpresa { get; set; }


        [NotMappedAttribute]
        [Display(Name = "Archivos")]
        [InvisibleAttribute]
        public ArchivoModulo[] ArchivosModulo { get; set; }

    }
}
