using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("SubTipo")]
    public class SubTipo : BaseEntity
    {
        public SubTipo()
        { 
            
        }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "El Tipo es requerido")]
        [InvisibleAttribute]
        public int Tipo_Id { get; set; }

        [ForeignKey("Tipo_Id")]
        [InvisibleAttribute]
        public virtual Tipos_NoConformidades Tipo { get; set; }

        [NotMappedAttribute]
        [ColumnNameGridView(Name = "Tipo")]
        public string NombreTipo { get { return GetNombreTipo(); } }

        private string GetNombreTipo()
        {
            DAL.Tipos_NoConformidadesRepository tipoRepository = new DAL.Tipos_NoConformidadesRepository();
            Entities.Tipos_NoConformidades tipo = tipoRepository.Get(Tipo_Id).FirstOrDefault();

            return tipo.Descripcion;
        }

        [Required(ErrorMessage = "El Nombre Sub Tipo  es requerido")]
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
    }
}
