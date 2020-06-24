using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("TiposAcontecimiento")]
    public class TipoAcontecimiento : BaseEntity
    {
        public TipoAcontecimiento()
        {

        }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "La Descripcion es requerida")]
        public string Descripcion { get; set; }
    }
}
