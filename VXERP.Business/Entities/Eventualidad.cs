using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("Eventualidades")]
    public class Eventualidad : BaseEntity
    {
        public Eventualidad()
        { 
            
        }

        
        [ColumnNameGridView(Name = "ID Cliente")]
        [InvisibleAttribute]
        [Required(ErrorMessage = "El Cliente es requerido")]
        public int Cg_Clie { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Cliente")]
        public string Nombre_Cliente { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Usuario Alta")]
        public string Usuario_Alta { get; set; }


        [Display(Name = "Pedido")]
        [Required(ErrorMessage = "El Pedido es requerido")]
        [ColumnNameGridView(Name = "Pedido")]
        public int? Pedido_Id { get; set; }

        [ForeignKey("Pedido_Id")]
        [InvisibleAttribute]
        public virtual Pedido Pedido { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        [Display(Name = "Tipo Eventualidad")]
        public int? TipoEventualidad_Id { get; set; }

        [InvisibleAttribute]
        [Display(Name = "Sub Tipo Eventualidad")]
        [Required(ErrorMessage = "El Sub Tipo Eventualidad es requerido")]
        public int SubTipoEventualidad_Id { get; set; }

        [ForeignKey("SubTipoEventualidad_Id")]
        [InvisibleAttribute]
        public virtual SubTipoEventualidad SubTipoEventualidad { get; set; }

        [NotMappedAttribute]
        [ColumnNameGridView(Name = "Tipo Eventualidad")]
        public string NombreTipoEventualidad { get { return GetNombreTipoEventualidad(); } }

        [NotMappedAttribute]
        [ColumnNameGridView(Name = "Sub Tipo Eventualidad")]
        public string NombreSubTipoEventualidad { get { return GetNombreSubTipoEventualidad(); } }

      

        private string GetNombreSubTipoEventualidad()
        {
            DAL.SubTipoEventualidadRepository subTipoEventualidadRepository = new DAL.SubTipoEventualidadRepository();
            Entities.SubTipoEventualidad subTipo = subTipoEventualidadRepository.Get(SubTipoEventualidad_Id).FirstOrDefault();

            return subTipo.Descripcion;
        }

        private string GetNombreTipoEventualidad()
        {
            DAL.SubTipoEventualidadRepository subTipoEventualidadRepository = new DAL.SubTipoEventualidadRepository();
            Entities.SubTipoEventualidad subTipo = subTipoEventualidadRepository.Get(SubTipoEventualidad_Id, s=>s.TipoEventualidad).FirstOrDefault();

            return subTipo.TipoEventualidad.Descripcion;
        }

        [ColumnNameGridView(Name = "Descripción y Causas")]
        [Display(Name = "Descripción:")]
        [Required(ErrorMessage = "La Descripción es requerida")]
        public string Descripcion { get; set; }

         [InvisibleAttribute]
        [ColumnNameGridView(Name = "Observaciones")]
        [Display(Name = "Observaciones:")]
        public string Observaciones { get; set; }

         [InvisibleAttribute]
        [ColumnNameGridView(Name = "Causas")]
        [Display(Name = "Causas:")]
        public string Causas { get; set; }

        [ColumnNameGridView(Name = "Acciones Inmediatas")]
        [Display(Name = "Acciones Inmediatas:")]
        public string AccionesInmediatas { get; set; }

        [Display(Name = "Fecha Ocurrencia")]
        [Required(ErrorMessage = "La Fecha Ocurrencia es requerida")]
        public DateTime? FechaOcurrencia { get; set; }

        [Display(Name = "Fecha Apertura")]
        public DateTime FechaApertura { get; set; }

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
        public virtual Usuario Usuario { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        public string Remito { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        [Display(Name = "Producto")]
        public string NombreProducto { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        [Display(Name = "Cod. Producto")]
        public string OrdenFabricacion { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        [Display(Name = "Lote de Cliente")]
        public string LoteCliente { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        public string NotificarUsuarios { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        public string NotificarUsuarios_Id { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        public string NotificarContactos { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        public string NotificarContactos_Id { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Archivos")]
        [InvisibleAttribute]
        public ArchivoModulo[] ArchivosModulo { get; set; }
    }
}
