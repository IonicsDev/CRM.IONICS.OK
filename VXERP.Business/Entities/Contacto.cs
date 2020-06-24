using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;
using System.Web;

namespace CRM.Business.Entities
{
    public class Contacto : BaseEntity
    {
        public Contacto()
        { 
            
        }

        [Required(ErrorMessage = "El Cliente es requerido")]
        [Display(Name = "Cliente")]
        public int CodigoCliente { get; set; }

        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Apellido es requerido")]
        public string Apellido { get; set; }

        public string Cargo { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        public int? Interno { get; set; }

        [Display(Name = "Móvil")]
        public string Movil { get; set; }


        [Required(ErrorMessage = "El Email es requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El DNI es requerido")]
        [Display(Name = "DNI")]
        public int? Dni { get; set; }

        public HttpPostedFileBase Foto { get; set; }

        public byte[] FotoByte { get; set; }

        [Display(Name = "Archivos")]
        public ArchivoModulo[] ArchivosModulo { get; set; }

        #region Entity Data Generic

        [NotMappedAttribute]
        [Display(Name = "Fecha Última Actualización")]
        public DateTime FechaActualizacion { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Fecha Creación")]
        [ColumnNameGridView(Name = "Fecha Creación")]
        public DateTime FechaCreacion { get; set; }

        [NotMappedAttribute]
        public bool Estado { get; set; }

        [NotMappedAttribute]
        public int? idUsuario { get; set; }

        #endregion
    }
}
