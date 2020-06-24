using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("Identificacion_NoConformidades")]
    public class Identificacion_NoConformidades : BaseEntity
    {
        public Identificacion_NoConformidades()
        { 
            
        }

        [Required(ErrorMessage = "La identificacion es requerida")]
        [Display(Name = "Descripcion")]
        [ColumnNameGridView(Name = "Descripcion")]
        public string Descripcion { get; set; }


        [NotMappedAttribute]
        [Display(Name = "Activo")]
        [ColumnNameGridView(Name = "Activo")]
        public string DescripcionActivo
        {
            get
            {
                return (Estado ? "Activo" : "Inactivo");
            }
        }



    }
}
