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
     [Table("Modulos")]
    public class Modulo : BaseEntity
    {
         public Modulo()
         {
           
         }

         [Required]
         [Display(Name = "Descripción")]
         public string Descripcion { get; set; }


         public string URL { get; set; }
         public int? Orden { get; set; }
         public string Class { get; set; }
         public bool Visible { get; set; }

         [ForeignKey("Parent_Id")]
         public virtual Modulo Parent { get; set; }
         public Nullable<int> Parent_Id { get; set; }

         //[NotMappedAttribute]
         //public  List<Modulo> GetParentsList
         //{
         //    get
         //    {
         //        return (new ModuloRepository()).GetParentsList(this);
         //    }
         //}

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
