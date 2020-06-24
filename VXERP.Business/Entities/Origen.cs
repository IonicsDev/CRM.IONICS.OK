using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Business.Entities
{
    [Table("Origen")]
    public class Origen : BaseEntities.BaseEntity
    {
        public Origen()
        {

        }

        public string Descripcion { get; set; }
    }
}
