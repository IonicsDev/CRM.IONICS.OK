using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("TiposEventualidad")]
    public class TipoEventualidad : BaseEntity
    {
        public TipoEventualidad()
        { 
            
        }

        [Required(ErrorMessage = "El Nombre de Tipo de Eventualidad es requerido")]
        [Display(Name = "Descripción")]
        [ColumnNameGridView(Name = "Descripción")]
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

        [InvisibleAttribute]
        public virtual ICollection<SubTipoEventualidad> SubTiposEventualidad { get; set; }
    }
}
