using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("NovedadUsuarios")]
    public class NovedadUsuarios : BaseEntity
    {
        public NovedadUsuarios()
        { 
            
        }

        public int Novedad_Id { get; set; }

        [ForeignKey("Novedad_Id")]
        public virtual Novedad Novedad { get; set; }

        public int Usuario_Id { get; set; }

        [ForeignKey("Usuario_Id")]
        public virtual Usuario Usuario { get; set; }

        public bool Visto { get; set; }

        #region Entity Data Generic

        [NotMappedAttribute]
        [Display(Name = "Fecha Última Actualización")]
        public DateTime FechaActualizacion { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Fecha Creación")]
        [ColumnNameGridView(Name = "Fecha Creación")]
        public DateTime FechaCreacion { get; set; }

        [NotMappedAttribute]
        public bool Estado { get; set; }

        [NotMappedAttribute]
        public int? idUsuario { get; set; }

        #endregion
    }
}
