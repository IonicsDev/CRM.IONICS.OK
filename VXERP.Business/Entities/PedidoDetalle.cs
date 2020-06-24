using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("Pedidos_Detalle")]
    public class PedidoDetalle : BaseEntity
    {
        public PedidoDetalle()
        {
            this.Cantidad = 0;
        }

        public int PedidoID { get; set; }

        [ForeignKey("PedidoID")]
        [InvisibleAttribute]
        public virtual Pedido Pedido { get; set; }

        public string Cg_Prod { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Producto")]
        public string NombreProducto { get; set; }

        public int Cantidad { get; set; }

        [MaxLength(4)]
        [Display(Name = "Tipo de Unidad")]
        public string Unid_Fac { get; set; }

        [MaxLength(100)]
        [Display(Name = "Lotes")]

        public string Lotes { get; set; }

        [MaxLength(10)]
        [Display(Name = "OC Cliente")]
        [StringLength(10, ErrorMessage = "la Orden de Compra debe tener máximo 10 caracteres")]
        public string Orco { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Dosis")]
        public string Dosis { get; set; }

        [NotMappedAttribute]
        public bool Deleted { get; set; }

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
