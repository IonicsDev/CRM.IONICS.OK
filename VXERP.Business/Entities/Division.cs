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
    [Table("Divisiones")]
    public class Division : BaseEntity

    {
        public Division()
        {
          this.Sucursales= new HashSet<Sucursal>();
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

       

        [Required(ErrorMessage = "Debe seleccionar una compañia")]
        [Range(1, 5000, ErrorMessage = "Debe seleccionar una compañia")]
        public int IdCompania { get; set; }
        [ForeignKey("IdCompania")]
        public virtual Compania Compania { get; set; }

        [ForeignKey("idUsuario")]
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<Sucursal> Sucursales { get; set; }
        public virtual ICollection<RolEmpresa> RolEmpresas { get; set; }
    }
}
