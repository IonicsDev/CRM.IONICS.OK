using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("Pedidos")]
    public class Pedido : BaseEntity
    {
        public Pedido()
        {
            this.PedidoDetalles = new List<PedidoDetalle>();
            this.Fe_Ped = null;
            this.Conf_Fecha = null;
            this.Fe_Retiro = null;
        }
        [MaxLength(500)]
        [StringLength(500, ErrorMessage = "Máximo debe tener 500 caracteres")]
        public string Observaciones { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Cliente")]
        [ColumnNameGridView(Name = "Cliente")]
        public string Nombre_Cliente { get; set; }

        [Required(ErrorMessage = "El Cliente seleccionado no posee CUIT")]
        [MaxLength(30)]
        [InvisibleAttribute]
        public string Cuit { get; set; }

        [Required(ErrorMessage = "La Fecha Solicitada de Ingreso a Planta es requerida")]
        [Display(Name = "Fecha Solicitada Ingreso a Planta")]
        [ColumnNameGridView(Name = "Fecha Pedido")]
        [DataType(DataType.Date)]
        public DateTime? Fe_Ped { get; set; }

        [Display(Name = "Fecha Autorizada para Recepción en Planta")]
        [DataType(DataType.Date)]
        [ColumnNameGridView(Name = "Fecha Confirmación")]
        public DateTime? Conf_Fecha { get; set; }

        [Required(ErrorMessage = "El Cliente es requerido")]
        [InvisibleAttribute]
        public int Cg_Cli { get; set; }

        [InvisibleAttribute]
        public int Conf_Usu { get; set; }

        [DataType(DataType.Date)]
        [ColumnNameGridView(Name = "Fecha Retiro")]
        public DateTime? Fe_Retiro { get; set; }

        [NotMappedAttribute]
        [InvisibleAttribute]
        public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; }

    }
}
