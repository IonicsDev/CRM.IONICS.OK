using CRM.Business.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CRM.Business.Entities
{
    [Table("Parametros")]
    public class Parametro : BaseEntity
    {
        //public Parametro()
        //{
        //    this.RolesEmpresa = new HashSet<RolEmpresa>();
        //}

        [Display(Name = "Nombre Parámetro")]
        [ColumnNameGridView(Name = "Nombre Parámetro")]
        [Required(ErrorMessage = "El Nombre de Parámetro es requerido")]
        public string ParamName { get; set; }


        [Display(Name = "Valor Parámetro")]
        [ColumnNameGridView(Name = "Valor Parámetro")]
        [Required(ErrorMessage = "El Valor de Parámetro es requerido")]
        public string ParamValue { get; set; }


        [Display(Name = "Descripcion")]
        [ColumnNameGridView(Name = "Descripción")]
        [Required(ErrorMessage = "La Descripción es requerida")]
        public string Descripcion { get; set; }

        [NotMapped]
        [Invisible]
        public new int? idUsuario { get; set; }

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

        //[InvisibleAttribute]
        //public virtual ICollection<RolEmpresa> RolesEmpresa { get; set; }

    }
}
