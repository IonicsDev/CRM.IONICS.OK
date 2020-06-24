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
    [Table("Companias")]
    public class Compania : BaseEntity
    {
         public Compania()
        {
            this.Divisiones = new HashSet<Division>();
            this.Contactos = new HashSet<Contacto>();
          
            this.RolEmpresas = new HashSet<RolEmpresa>();
         
            this.Clientes = new HashSet<Cliente>();

  
        }

        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Descripción corta")]
        public string DescripcionCorta { get; set; }

        [Display(Name = "CUIT")]
        [RegularExpression("[0-9]+", ErrorMessage = "Debe ingresar solamente numeros")]
        [StringLength(1000, MinimumLength = 13, ErrorMessage = "El Nro. RUC debe tener 13 caracteres")]
        [Required(ErrorMessage = "Complete el RUC")]
        public string RUC { get; set; }

        [Display(Name = "Calle")]
        public string Calle { get; set; }

        [Display(Name = "Número")]
        public string Numero { get; set; }

     

        [Display(Name = "Teléfono PBX")]
        public string TelefonoPBX { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono2 { get; set; }

        [Display(Name = "Responsable Legal")]
        public string ResponsableLegal { get; set; }

       

        [NotMappedAttribute]
        [Display(Name = "Estado")]
        public string DescripcionEstado
        {
            get
            {
                return (Estado ? "Activo" : "Inactivo");
            }
        }

        [Required(ErrorMessage = "Debe seleccionar un grupo empresarial")]
        [Range(1, 5000, ErrorMessage = "Debe seleccionar un grupo empresarial")]
        public int IdGrupoEmpresarial { get; set; }
        [ForeignKey("IdGrupoEmpresarial")]
        public virtual GrupoEmpresarial GrupoEmpresarial { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una ciudad")]
        [Range(1, 5000, ErrorMessage = "Debe seleccionar una ciudad")]
        public int IdCiudad { get; set; }
        [ForeignKey("IdCiudad")]
        public virtual Ciudad Ciudad { get; set; }

        [ForeignKey("idUsuario")]
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<Division> Divisiones { get; set; }
        public virtual ICollection<Contacto> Contactos { get; set; }
   
        public virtual ICollection<RolEmpresa> RolEmpresas { get; set; }
   
        public virtual ICollection<Cliente> Clientes { get; set; }
   
    }
}
