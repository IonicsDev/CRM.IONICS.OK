using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CRM.Common.EntityDomain;

namespace CRM.Business.Entities.BaseEntities
{
    [Serializable]
    public abstract class BaseEntity : EntityTypedId<int>,IDelEntity
    {
        [InvisibleAttribute]
        [Display(Name = "Fecha Última Actualización")]
        public DateTime FechaActualizacion { get; set; }

        [Display(Name = "Fecha Creación")]
        [InvisibleAttribute]
        public DateTime FechaCreacion { get; set; }

        [InvisibleAttribute]
        public bool Estado { get; set; }

        [InvisibleAttribute]
        public int? idUsuario { get; set; }
    }
}
