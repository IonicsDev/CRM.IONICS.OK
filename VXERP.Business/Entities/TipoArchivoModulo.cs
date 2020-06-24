using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("TiposAchivoModulos")]
    public class TipoArchivoModulo : BaseEntity
    {
        [Required]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Estado")]
        public string DescripcionEstado
        {
            get
            {
                return (Estado ? "Activo" : "Inactivo");
            }
        }

        [ForeignKey("idUsuario")]
        public virtual Usuario Usuario { get; set; }
    }
}
