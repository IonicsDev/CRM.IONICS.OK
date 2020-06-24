using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;
using CRM.Common.EntityDomain;

namespace CRM.Business.Entities
{
    [Table("Ciudades")]
    public class Ciudad : BaseEntity
    {
        public Ciudad()
        {
          
           
       
            this.Clientes = new HashSet<Cliente>();
            
           
        }

        [Required(ErrorMessage = "Debe seleccionar una provincia")]
        [Range(1, 5000, ErrorMessage = "Debe seleccionar una provincia")]
        public int IdProvincia { get; set; }

        [ForeignKey("IdProvincia")]
        public virtual Provincia Provincia { get; set; }

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

        //[ForeignKey("Cantones")]
      
        
       
        public virtual ICollection<Cliente> Clientes { get; set; }



       
    }
}
