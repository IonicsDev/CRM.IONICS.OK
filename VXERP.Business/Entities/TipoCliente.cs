using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.DAL;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table ("TiposCliente")]
    public class TipoCliente: BaseEntity
    {

         public TipoCliente() 
        {
            this.Clientes = new HashSet<Cliente>();
    
        
        }

       
        [Display(Name = "Parent")]
        public Nullable<int> Parent_Id { get; set; }

        [ForeignKey("Parent_Id")]
        public virtual TipoCliente Parent { get; set; }


        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Descripción corta")]
        public string DescripcionCorta { get; set; }



       

        [NotMappedAttribute]
        [Display(Name = "Estado")]
        public string DescripcionEstado
        {
            get
            {
                return (Estado ? "Activo" : "Inactivo");
            }
        }

        [NotMappedAttribute]
        [Display(Name = "Agupaciónes")]
        public string DescripcionPadres
        {
            get
            {
                return (new TipoClienteRepository()).GetAgrupacionCompleta(this.Id);
            }
        }

      

        #region Entity Data Generic
 
       // public virtual Proveedor Proveedor { get; set; }
        [ForeignKey("idUsuario")]
        public virtual Usuario Usuario{ get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
        
        #endregion
    }
}
