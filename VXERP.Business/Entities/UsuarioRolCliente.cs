using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;

namespace CRM.Business.Entities
{
    [Table("UsuariosRolesClientes")]
    [Serializable]
    public class UsuarioRolCliente : BaseEntities.BaseEntity
    {
        [Required(ErrorMessage = "Debe seleccionar un Rol")]
        [Range(1, 5000, ErrorMessage = "Debe seleccionar un Rol")]
        public int UsuarioRol_Id { get; set; }

        [ForeignKey("UsuarioRol_Id")]
        public virtual RolEmpresa UsuarioRol{ get; set; }

        [Required(ErrorMessage = "Debe seleccionar un Cliente")]
        [Range(1, 5000, ErrorMessage = "Debe seleccionar un Cliente")]
        public int Cliente_Id { get; set; }


        //public int Cliente_Id
        //[ForeignKey("Cliente_Id")]
        //public virtual Cliente Cliente { get; set; }


        [ForeignKey("idUsuario")]
        public virtual Usuario Usuario { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Estado")]
        public string DescripcionEstado
        {
            get
            {
                return (Estado ? "Activo" : "Inactivo");
            }
        }

    }

    
}

