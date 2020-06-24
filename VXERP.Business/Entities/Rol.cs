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

    [Table("Roles")]
    public class Rol : BaseEntity
    {
        public Rol()
        {
            this.ModulosPermiso = new HashSet<ModuloPermiso>();
        }
       
        [Required]
        [Display(Name = "Nombre Rol")]
        [ColumnNameGridView(Name = "Nombre Rol")]
        public string RoleName { get; set; }      
       
        [NotMappedAttribute]
        [Display(Name = "Estado")]
        [InvisibleAttribute]
        public string DescripcionEstado
        {
            get
            {
                return (Estado ? "Activo" : "Inactivo");
            }
        }

        //[ForeignKey("IdUsuario")]
        //public virtual Usuario Usuario { get; set; }
        [InvisibleAttribute]
        public virtual ICollection<ModuloPermiso> ModulosPermiso { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        public bool Deleted { get; set; }
    }
}
