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
     [Table("Mails")]
    public class Mail : BaseEntity
    {
         public Mail()
         {
             
         }

         [InvisibleAttribute]
         public string Remitente { get; set; }

         [ColumnNameGridView(Name = "Remitente")]
         [Display(Name = "Remitente")]
         public string NombreRemitente { get; set; }

         [Required (ErrorMessage = "El Asunto es requerido")]
         [MaxLength(200)]
         public string Asunto { get; set; }

         [InvisibleAttribute]
         public string Cuerpo { get; set; }

         [InvisibleAttribute]
         [ForeignKey("idUsuario")]
         public virtual Usuario Usuario { get; set; }

         [NotMappedAttribute]
         [InvisibleAttribute]
         public string Destinatarios
         {
             get
             {
                return (new MailDestinatarioRepository().GetDestinatariosMail(this.Id));
             }
         }

         [NotMappedAttribute]
         public string Fecha
         {
             get
             {
                 return base.FechaActualizacion.ToString("dd/MM/yyyy hh:MM:ss");
             }
         }


         [NotMappedAttribute]
         [Display(Name = "Archivos")]
         [InvisibleAttribute]
         public ArchivoModulo[] ArchivosModulo { get; set; }

    }
}
