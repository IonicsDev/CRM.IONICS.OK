using System;
using System.Collections;
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
    [Table("RolModulos")]
    public class ModuloPermiso : BaseEntity
    {
        public ModuloPermiso()
         {
            
         }

         [Required]
         [Display(Name = "Acción")]
         public string Accion { get; set; }

         public int Modulo_Id { get; set; }
         [ForeignKey("Modulo_Id")]
         public virtual Modulo Modulo { get; set; }

         public int Rol_Id { get; set; }
         [ForeignKey("Rol_Id")]
         public virtual Rol Rol { get; set; }

         //public virtual ICollection<AgrupRoles> AgrupRoles { get; set; }

        #region Entity Data Generic

         [NotMappedAttribute]
         [Display(Name = "Fecha última Actualización")]
         public DateTime FechaActualizacion { get; set; }

         [NotMappedAttribute]
         [Display(Name = "Fecha Creacíon")]
         public DateTime FechaCreacion { get; set; }

         [NotMappedAttribute]
         public bool Estado { get; set; }

         [NotMappedAttribute]
         public int? idUsuario { get; set; }
        #endregion
    }
}
