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
    [Table("Acontecimientos")]
    public class Acontecimiento : BaseEntity
    {
        public Acontecimiento()
        {
            FechaApertura = null;
            FechaOcurrencia = null;
            FechaEvaluacion = null;
            FechaImplementacion = null;
            FechaCierreAccion = null;
        }

        [Display(Name = "Numeración Especial")]
        public string Id_Custom { get; set; }

        [Display(Name = "Tipo Acontecimiento")]
        [Required(ErrorMessage = "El Tipo Documento es requerido")]
        public int? TipoAcontecimiento_Id { get; set; }

        [Display(Name = "Origen")]
        [Required(ErrorMessage = "Seleccione Origen")]
        public int? Origen_Id { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "La Descripcion es requerida")]
        public string Descripcion { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Error en Fecha")]
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "La Fecha es requerida")]
        public DateTime Fecha { get; set; }

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

        [Display(Name = "Descripcion")]
        public string OrigenOtras { get; set; }

        [Display(Name = "Acciones Inmediatas")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool AccionesInmediatas { get; set; }

        //[Display(Name = "Evaluación de la eficacia de la Acción tomada")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public string DescripcionAccionesInmediatas { get; set; }

        [Display(Name = "Acciones Correctivas")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool AccionesCorrectivas { get; set; }

        //[Display(Name = "Evaluación de la eficacia de la Acción tomada")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public string DescripcionAccionesCorrectivas { get; set; }

        [Display(Name = "Acciones de Mejora")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool AccionesMejora { get; set; }

        //[Display(Name = "Evaluación de la eficacia de la Acción tomada")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public string DescripcionAccionesMejora { get; set; }

        [Display(Name = "Otras Acciones")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public bool AccionesOtras { get; set; }

        //[Display(Name = "Evaluación de la eficacia de la Acción tomada")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public string DescripcionAccionesOtras { get; set; }

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

        [Display(Name = "Descripcion Sin Acciones")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public string DescripcionSinAcciones { get; set; }

        [Display(Name = "Nueva Acción")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public string NuevaAccion { get; set; }

        [Display(Name = "Observaciones")]
        //[Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public string Observaciones { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Cierre de la Acción")]
        public DateTime? FechaCierreAccion { get; set; }


        [Display(Name = "Causas Raíz, Origen de la Mejora, Origen del Cambio")]
        public string Raiz_Mejora_Cambio { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        public virtual TipoAcontecimiento TipoAcontecimiento { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        public virtual Origen Origen { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Archivos")]
        [InvisibleAttribute]
        public ArchivoModulo[] ArchivosModulo { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        public bool FirmaGteGral { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        public string SelectedAnswer
        {
            get
            {
                if (AuditoriaClientes) return "AuditoriaClientes";
                if (AuditoriaCertificacion) return "AuditoriaCertificacion";
                if (InspeccionesArn) return "InspeccionesArn";
                if (OtrasInspecciones) return "OtrasInspecciones";
                if (AccidentesLaborales) return "AccidentesLaborales";
                if (Eventualidades) return "Eventualidades";
                if (ReclamoClientes) return "ReclamoClientes";
                if (Novedades) return "Novedades";
                if (InformeRevision) return "InformeRevision";
                if (EncuentaClientes) return "EncuentaClientes";
                if (AnalisisVariosIonics) return "AnalisisVariosIonics";
                if (Otras) return "Otras";
                return null; // or you can set a default here
            }
            set
            {
                switch (value)
                {
                    case "AuditoriaClientes":
                        AuditoriaClientes = true;
                        break;
                    case "AuditoriaCertificacion":
                        AuditoriaCertificacion = true;
                        break;
                    case "InspeccionesArn":
                        InspeccionesArn = true;
                        break;
                    case "OtrasInspecciones":
                        OtrasInspecciones = true;
                        break;
                    case "AccidentesLaborales":
                        AccidentesLaborales = true;
                        break;
                    case "Eventualidades":
                        Eventualidades = true;
                        break;
                    case "ReclamoClientes":
                        ReclamoClientes = true;
                        break;
                    case "Novedades":
                        Novedades = true;
                        break;
                    case "InformeRevision":
                        InformeRevision = true;
                        break;
                    case "EncuentaClientes":
                        EncuentaClientes = true;
                        break;
                    case "AnalisisVariosIonics":
                        AnalisisVariosIonics = true;
                        break;
                    case "Otras":
                        Otras = true;
                        break;
                }
            }
        }
    }
}
