using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("PropiedadesNavegacionListados")]
    public class PropiedadNavegacionListado : BaseEntity
    {
        public PropiedadNavegacionListado()
        { 
            
        }

        public string CampoFK { get; set; }

        public int? Listado_Id { get; set; }

        [ForeignKey("Listado_Id")]
        [InvisibleAttribute]
        public virtual Listado Listado { get; set; }

        #region Entity Data Generic

        [NotMappedAttribute]
        [Display(Name = "Fecha Última Actualización")]
        public new DateTime FechaActualizacion { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Fecha Creación")]
        [ColumnNameGridView(Name = "Fecha Creación")]
        public new DateTime FechaCreacion { get; set; }

        [NotMappedAttribute]
        public new bool Estado { get; set; }

        [NotMappedAttribute]
        public new int? idUsuario { get; set; }

        #endregion
    }
}
