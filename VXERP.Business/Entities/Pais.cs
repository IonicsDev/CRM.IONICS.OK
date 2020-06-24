using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.DAL;
using CRM.Business.Entities.BaseEntities;
using CRM.Common.EntityDomain;

namespace CRM.Business.Entities
{
    [Table("Paises")]
    public class Pais : BaseEntity
    {
        public Pais()
        {
           this.Provincias = new HashSet<Provincia>();
        
        }

        [Required(ErrorMessage="La Descripción es requerida")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }


        [Display(Name = "Descripción corta")]
        public string DescripcionCorta { get; set; }


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

        //[ForeignKey("Provincias")]
        public virtual ICollection<Provincia> Provincias { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Archivos")]
        public ArchivoModulo[] ArchivosModulo { get; set; }
    }
}
