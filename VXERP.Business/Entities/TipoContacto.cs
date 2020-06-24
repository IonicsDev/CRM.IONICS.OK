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
    [Table("TiposContacto")]

    public class TipoContacto : BaseEntity
    {
         public TipoContacto()
        {
           
            this.Contactos = new HashSet<Contacto>();
           
        }

        [Required]
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


        #region Entity Data Generic


        [ForeignKey("idUsuario")]
        public virtual Usuario Usuario { get; set; }

        #endregion

        public virtual ICollection<Contacto> Contactos { get; set; }





    }
}