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
    [Table("Sucursales")]
    public class Sucursal : BaseEntity
    {
        public Sucursal()
        {
            this.RolEmpresas = new HashSet<RolEmpresa>();   
        }

        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Descripción corta")]
        public string DescripcionCorta { get; set; }

        [Display(Name = "Calle")]
        public string Calle { get; set; }

        [Display(Name = "Número")]
        public string Numero { get; set; }

        [Display(Name = "Interseccion")]
        public string Interseccion { get; set; }

        [Display(Name = "Télefono PBX")]
        public string TelefonoPBX { get; set; }

        [Display(Name = "Télefono")]
        public string Telefono2 { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Estado")]
        public string DescripcionEstado
        {
            get
            {
                return (Estado ? "Activo" : "Inactivo");
            }
        }

       

        [Required(ErrorMessage = "Debe seleccionar una división")]
        [Range(1, 5000, ErrorMessage = "Debe seleccionar una división")]
        public int IdDivision { get; set; }
        [ForeignKey("IdDivision")]
        public virtual Division Division { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una ciudad")]
        [Range(1, 5000, ErrorMessage = "Debe seleccionar una ciudad")]
        public int IdCiudad { get; set; }
        [ForeignKey("IdCiudad")]
        public virtual Ciudad Ciudad { get; set; }

        [ForeignKey("idUsuario")]
        public virtual Usuario Usuario { get; set; }


        public virtual ICollection<RolEmpresa> RolEmpresas { get; set; }
    }
}
