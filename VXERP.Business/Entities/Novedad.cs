using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("Novedades")]
    public class Novedad : BaseEntity
    {
        public Novedad()
        {
            this.UsuariosNotificados = new List<NovedadUsuarios>();
        }

        [Required(ErrorMessage = "El Título es requerido")]
        [Display(Name = "Título")]
        [ColumnNameGridView(Name = "Título")]
        public string Titulo { get; set; }

        [Display(Name = "Descripción")]
        [InvisibleAttribute]
        public string Descripcion { get; set; }

        [Display(Name = "Tipo de Novedad")]
        [Required(ErrorMessage = "El Tipo de Novedad es requerido")]
        [InvisibleAttribute]
        public int? TipoNovedad_Id { get; set; }

        [ForeignKey("TipoNovedad_Id")]
        [InvisibleAttribute]
        public virtual TipoNovedad TipoNovedad { get; set; }

        [NotMappedAttribute]
        [ColumnNameGridView(Name = "Tipo Novedad")]
        public string NombreTipoNovedad { get { return GetNombreTipoNovedad(); } }

        private string GetNombreTipoNovedad()
        {
            DAL.TipoNovedadRepository tipoNovedadRepository = new DAL.TipoNovedadRepository();
            Entities.TipoNovedad tipo = tipoNovedadRepository.Get(TipoNovedad_Id.Value).FirstOrDefault();

            return tipo.Descripcion;
        }

        [InvisibleAttribute]
        [NotMappedAttribute]
        public virtual ICollection<NovedadUsuarios> UsuariosNotificados { get; set; }

        [NotMappedAttribute]
        [ColumnNameGridView(Name = "Fecha Creación")]
        public DateTime Fecha_Creacion { get { return FechaCreacion; } }

        [NotMappedAttribute]
        [Display(Name = "Estado")]
        [ColumnNameGridView(Name = "Estado")]
        public string DescripcionEstado
        {
            get
            {
                return (Estado ? "Activo" : "Inactivo");
            }
        }

        [ForeignKey("idUsuario")]
        [InvisibleAttribute]
        public virtual Usuario Usuario { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Archivos")]
        [InvisibleAttribute]
        public ArchivoModulo[] ArchivosModulo { get; set; }
    }
}
