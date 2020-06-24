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

    [Table("ParametrosGenerales")]
    public class ParametrosGenerales : BaseEntity
    {
        [Display(Name = "Compañia")]
        [Required(ErrorMessage = "Debe seleccionar una compañia")]
        [Range(1, 5000, ErrorMessage = "Debe seleccionar una compañia")]
        public int Compania_Id { get; set; }

       
        [ForeignKey("Compania_Id")]
        public virtual Compania Compania { get; set; }

  

       


       
        

        [NotMappedAttribute]
        [Display(Name = "Estado")]
        public string DescripcionEstado
        {
            get
            {
                return (Estado ? "Activo" : "Inactivo");
            }
        }


        #region Entity Data Generic
 
       // public virtual Proveedor Proveedor { get; set; }
        [ForeignKey("idUsuario")]
        public virtual Usuario Usuario{ get; set; }
        
        #endregion

    }
}
