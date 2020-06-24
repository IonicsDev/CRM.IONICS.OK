using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("ContactosCliente")]
    public class ContactoCliente: BaseEntity
    {

        [Required]
        public int Cliente_Id { get; set; }

        [ForeignKey("Cliente_Id")]
        public virtual Cliente Cliente { get; set; }

        [Required]
        public int Contacto_Id { get; set; }

        [ForeignKey("Contacto_Id")]
        public virtual Contacto Contacto { get; set; }

        #region BaseEntity Propertys

        [NotMappedAttribute]
        public bool Estado { get; set; }

        [NotMappedAttribute]
        public int? idUsuario { get; set; }

        [NotMappedAttribute]
        public DateTime FechaActualizacion { get; set; }

        [NotMappedAttribute]
        public DateTime FechaCreacion { get; set; }

        #endregion

    }
}
