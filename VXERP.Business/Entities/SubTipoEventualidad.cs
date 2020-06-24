using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("SubTiposEventualidad")]
    public class SubTipoEventualidad : BaseEntity
    {
        public SubTipoEventualidad()
        { 
            
        }

        [Display(Name = "Tipo Eventualidad")]
        [Required(ErrorMessage = "El Tipo de Novedad es requerido")]
        [InvisibleAttribute]
        public int TipoEventualidad_Id { get; set; }

        [ForeignKey("TipoEventualidad_Id")]
        [InvisibleAttribute]
        public virtual TipoEventualidad TipoEventualidad { get; set; }

        [NotMappedAttribute]
        [ColumnNameGridView(Name = "Tipo Eventualidad")]
        public string NombreTipoEventualidad { get { return GetNombreTipoEventualidad(); } }

        private string GetNombreTipoEventualidad()
        {
            DAL.TipoEventualidadRepository tipoEventualidadRepository = new DAL.TipoEventualidadRepository();
            Entities.TipoEventualidad tipo = tipoEventualidadRepository.Get(TipoEventualidad_Id).FirstOrDefault();

            return tipo.Descripcion;
        }

        [Required(ErrorMessage = "El Nombre Sub Tipo Eventualidad es requerido")]
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
