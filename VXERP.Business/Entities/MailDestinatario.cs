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
     [Table("Mails_Destinatarios")]
    public class MailDestinatario : BaseEntity
    {
         public MailDestinatario()
         {
           
         }

         public int Mail_Id { get; set; }

         [ForeignKey("Mail_Id")]
         public virtual Mail Mail { get; set; }

         public string DestinatarioMail { get; set; }

         public string Destinatario { get; set; }

         public bool Visto { get; set; }

         public bool Hilo { get; set; }

         #region Entity Data Generic

         [NotMappedAttribute]
         [Display(Name = "Fecha Última Actualización")]
         public DateTime FechaActualizacion { get; set; }

         [NotMappedAttribute]
         [Display(Name = "Fecha Creación")]
         [ColumnNameGridView(Name = "Fecha Creación")]
         public DateTime FechaCreacion { get; set; }

         #endregion
            
    }
}
