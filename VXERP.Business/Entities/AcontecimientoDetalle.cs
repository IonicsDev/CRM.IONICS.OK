using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("Acontecimientos_Detalle")]
    public class AcontecimientoDetalle : BaseEntity
    {
        public AcontecimientoDetalle()
        {
            this.FechaApertura = null;
            this.FechaOcurrencia = null;
            FechaEvaluacion = null;
            FechaImplementacion = null;
            FechaCierreAccion = null;
        }

        public int Acontecimiento_ID { get; set; }

        [ForeignKey("Acontecimiento_ID")]
        [InvisibleAttribute]
        public virtual Acontecimiento Acontecimiento { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Apertura")]
        [Required(ErrorMessage = "La Fecha de Apertura es requerida")]
        public DateTime? FechaApertura { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Ocurrencia")]
        [Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public DateTime? FechaOcurrencia { get; set; }

        [Display(Name = "Auditoría de Clientes")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool AuditoriaClientes { get; set; }

        [Display(Name = "Auditoría de Certiificación")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool AuditoriaCertificacion { get; set; }

        [Display(Name = "Inspecciones ARN")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool InspeccionesArn { get; set; }

        [Display(Name = "Otras Inspecciones")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool OtrasInspecciones { get; set; }

        [Display(Name = "Accidentes Laborales")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool AccidentesLaborales { get; set; }

        [Display(Name = "Eventualidades")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool Eventualidades { get; set; }

        [Display(Name = "Reclamo de Clientes")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool ReclamoClientes { get; set; }

        [Display(Name = "Novedades")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool Novedades { get; set; }

        [Display(Name = "Informe de Revision")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool InformeRevision { get; set; }

        [Display(Name = "Encuenta de Clientes")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool EncuentaClientes { get; set; }

        [Display(Name = "Analisis Varios efectuados por Ionics")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool AnalisisVariosIonics { get; set; }

        [Display(Name = "Otras")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool Otras { get; set; }

        [Display(Name = "Acciones Inmediatas")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool AccionesInmediatas { get; set; }

        [Display(Name = "Acciones Correctivas")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool AccionesCorrectivas { get; set; }

        [Display(Name = "Acciones de Mejora")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool AccionesMejora { get; set; }

        [Display(Name = "Otras Acciones")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool AccionesOtras { get; set; }

        [Display(Name = "Sin Acciones")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool SinAcciones { get; set; }

        [Display(Name = "Descripción Acciones")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public string DescripcionAcciones { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Implementación: ")]
        public DateTime? FechaImplementacion { get; set; }

        [Display(Name = "Evaluación de la eficacia de la Acción tomada")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public string DescripcionEvaluacion { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Error FechaEvaluacion")]
        [Display(Name = "Fecha de Evaluación")]
        public DateTime? FechaEvaluacion { get; set; }

        [Display(Name = "Resultado de la Evaluación")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public string ResultadoEvaluacion { get; set; }

        [Display(Name = "Nueva Acción")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public string NuevaAccion { get; set; }

        [Display(Name = "Observaciones")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public string Observaciones { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Cierre de la Acción")]
        public DateTime? FechaCierreAccion { get; set; }


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
