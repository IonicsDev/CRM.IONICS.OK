using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("Clientes")]
    public class Cliente : BaseEntity
    {
        public Cliente()
        {
           
        }

        [Required]
        [Display(Name = "Nombre Comercial")]
        [ColumnNameGridView(Name = "Nombre Comercial")]
        public string NombreComercial { get; set; }

        [Display(Name = "Descripción Corta")]
        [ColumnNameGridView(Name = "Descripción Corta")]
        public string DescripcionCorta { get; set; }

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
        
        [ForeignKey("idUsuario")]
        [InvisibleAttribute]
        public virtual Usuario Usuario { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        public bool Deleted { get; set; }
    }
}
