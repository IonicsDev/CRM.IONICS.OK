using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("Pedidos_Estados")]
    public class PedidoEstado : BaseEntity
    {
        public PedidoEstado()
        {

        }

        public int PedidoID { get; set; }

        [ForeignKey("PedidoID")]
        [InvisibleAttribute]
        public virtual Pedido Pedido { get; set; }

        public string Cg_Prod { get; set; }

        [Column("Estado")]
        public int EstadoInt { get; set; }

        public bool Devol { get; set; }

        public int Cantidad { get; set; }

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
