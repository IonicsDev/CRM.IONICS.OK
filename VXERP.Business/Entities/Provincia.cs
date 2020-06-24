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
    [Table("Provincias")]
    public class Provincia : BaseEntity
    {
         public Provincia()
        {
            this.Ciudades = new HashSet<Ciudad>();
            //this.Proveedores = new HashSet<Proveedores>();
        }
       
       [Required(ErrorMessage="Debe seleccionar un país")]
       [Range(1, 5000, ErrorMessage="Debe seleccionar un país")]
        public int IdPais { get; set; }

       [ForeignKey("IdPais")]
        public virtual Pais Pais { get; set; }

        [ForeignKey("idUsuario")]
        public virtual Usuario Usuario { get; set; }
        //public virtual ICollection<Proveedores> Proveedores { get; set; }
        //public virtual ICollection<Ciudades> Ciudades { get; set; }
        

        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }


        [Display(Name = "Descripción corta")]
        public string DescripcionCorta { get; set; }

        [Display(Name = "Estado")]
        public string DescripcionEstado
        {
            get
            {
                return (Estado ? "Activo" : "Inactivo");
            }
        }
        //[ForeignKey("Ciudades")]
        public virtual ICollection<Ciudad> Ciudades { get; set; }

    }
}
