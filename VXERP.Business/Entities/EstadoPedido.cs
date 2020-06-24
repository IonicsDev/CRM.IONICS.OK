using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("EstadoPedido")]
    public class EstadoPedido : BaseEntity
    {
        public override int Id { get; set; }
        public string Descripcion { get; set; }
    }
}
