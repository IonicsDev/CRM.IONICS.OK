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
    [Table("NoConformidad")]
    public class NoConformidad : BaseEntity
    {
        public NoConformidad()
        { 
            
        }

        [ForeignKey("idUsuario")]
        public virtual Usuario Usuario { get; set; }

        [InvisibleAttribute]
        [Display(Name = "Tipo")]
        public int? IDTipo { get; set; }

        [InvisibleAttribute]
        [Display(Name = "Numero")]
        public int? Numero { get; set; }

        [Display(Name = "Fecha Ocurrencia")]
        public DateTime? FechaOcurrencia { get; set; }

        [Display(Name = "Fecha Apertura")]
        public DateTime? FechaApertura { get; set; }

        [InvisibleAttribute]
        [Display(Name = "Origen")]
        public int? IDOrigen { get; set; }

        [InvisibleAttribute]
        [Display(Name = "Identificacion")]
        public int? Identificacion { get; set; }

        [ColumnNameGridView(Name = "Descripción y Causas")]
        [Display(Name = "Descripcion:")]
        public string Descripcion { get; set; }


        [ColumnNameGridView(Name = "Observaciones")]
        [Display(Name = "Observaciones:")]
        public string Observaciones { get; set; }

        [ColumnNameGridView(Name = "AccionInmediata")]
        [Display(Name = "Acciones Inmediatas:")]
        public string AccionInmediata { get; set; }

        [InvisibleAttribute]
        [Display(Name = "Gestion De Accion")]
        public string GestionDeAccion { get; set; }


        [ColumnNameGridView(Name = "Comprobacion de eficacia")]
        [Display(Name = "Comprobacion de eficacia:")]
        public string ComprobacionEficacia { get; set; }


        [Display(Name = "Fecha de cierre")]
        public DateTime? FechaCierre { get; set; }

        [Display(Name = "Estado")]
        [ColumnNameGridView(Name = "Estado")]
        public string DescripcionEstado
        {
            get
            {
                return (Estado ? "Activo" : "Inactivo");
            }
        }

        [NotMappedAttribute]
        [Display(Name = "Archivos")]
        [InvisibleAttribute]
        public ArchivoModulo[] ArchivosModulo { get; set; }
    }
}
