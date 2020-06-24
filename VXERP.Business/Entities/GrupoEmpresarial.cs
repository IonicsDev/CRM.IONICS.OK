using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;
using CRM.Common.EntityDomain;

namespace CRM.Business.Entities
{
    [Table("GruposEmpresarial")]
    public class GrupoEmpresarial : BaseEntity

    {
         public GrupoEmpresarial()
        {
            this.Companias = new HashSet<Compania>();
            this.RolEmpresas = new HashSet<RolEmpresa>();
        
        }

        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }


        [Display(Name = "Descripción corta")]
        public string DescripcionCorta { get; set; }

        
        [Display(Name = "Responsable")]
        public string Responsable { get; set; }

        
        [Display(Name = "Cedula Responsable")]
        public string CedulaResponsable { get; set; }



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

       
        [ForeignKey("idUsuario")]
        public virtual Usuario Usuario { get; set; }

        #endregion
        
        public virtual ICollection<Compania> Companias { get; set; }
        public virtual ICollection<RolEmpresa> RolEmpresas { get; set; }


    }
}
