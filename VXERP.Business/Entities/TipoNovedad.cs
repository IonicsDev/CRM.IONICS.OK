using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("TiposNovedades")]
    public class TipoNovedad : BaseEntity
    {
        public TipoNovedad()
        { 
            
        }

        [Required(ErrorMessage = "El Nombre es requerido")]
        [Display(Name = "Nombre")]
        [ColumnNameGridView(Name = "Nombre")]
        public string Descripcion { get; set; }

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

        [ForeignKey("idUsuario")]
        [InvisibleAttribute]
        public virtual Usuario Usuario { get; set; }
    }
}
