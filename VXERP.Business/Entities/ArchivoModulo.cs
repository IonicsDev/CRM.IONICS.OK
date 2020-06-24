using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("ArchivosModulo")]
    public class ArchivoModulo : BaseEntity
    {
        [Required(ErrorMessage = "Debe completar la Descripción")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Debe completar la Descripción Corta")]
        [Display(Name = "Descripción Corta")]
        public string DescripcionCorta { get; set; }

        //[Required(ErrorMessage = "Debe seleccionar un Archivo")]
        [Display(Name = "Archivo")]
        public string Path { get; set; }

        [Display(Name = "Tipo Archivo Módulo")]
        [Required(ErrorMessage = "Debe seleccionar un Tipo Archivo Módulo")]
        public int TipoArchivoModulo_Id { get; set; }

        [ForeignKey("TipoArchivoModulo_Id")]
        public virtual TipoArchivoModulo TipoArchivoModulo { get; set; }

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

        public string Entity_Id { get; set; }

        [NotMappedAttribute]
        public string NombreTipoArchivoModulo { get; set; }

        [NotMappedAttribute]
        public bool Deleted { get; set; }
    }
}
