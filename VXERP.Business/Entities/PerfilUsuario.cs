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
     [Table("PerfilUsuario")]
    public class PerfilUsuario : BaseEntity
    {
         public PerfilUsuario()
         {
           
         }

         [ForeignKey("Usuario_Id")]
         public Usuario Usuario { get; set; }

         public int Usuario_Id { get; set; }
         
         #region Entity Data Generic

         [NotMapped]
         [Display(Name = "Fecha última Actualización")]
         public DateTime FechaActualizacion { get; set; }
         [NotMapped]
         [Display(Name = "Fecha Creacíon")]
         public DateTime FechaCreacion { get; set; }
         [NotMapped]
         public bool Estado { get; set; }
         [NotMapped]
         public int? idUsuario { get; set; }

         #endregion
    }
}
