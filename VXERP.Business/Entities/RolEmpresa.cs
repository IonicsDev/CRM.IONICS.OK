using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.DAL;
using CRM.Business.Entities.BaseEntities;
using CRM.Common.EntityDomain;
using CRM.Business.Views;
using System.Data;
using System.Xml.Serialization;

namespace CRM.Business.Entities
{

    [Table("UsuarioRols")]
    public class RolEmpresa : BaseEntity
    {
        public RolEmpresa()
        {
           this.UsuarioRolClientes =  new HashSet<UsuarioRolCliente>();
        }

        public int Usuario_Id { get; set; }
        public int Rol_Id { get; set; }

        [ForeignKey("Usuario_Id")]
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("Rol_Id")]
        public virtual Rol Rol { get; set; }

        [XmlIgnore]
        public virtual ICollection<UsuarioRolCliente> UsuarioRolClientes { get; set; }

        [NotMappedAttribute]
        public List<Rol> Roles { get; set; }

        [NotMappedAttribute]
        public vClientes Clientes { get; set; }

        #region Entity Data Generic

        [NotMappedAttribute]
        [Display(Name = "Fecha última Actualización")]
        public DateTime FechaActualizacion { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Fecha Creacíon")]
        public DateTime FechaCreacion { get; set; }

        [NotMappedAttribute]
        public bool Estado { get; set; }

        [NotMappedAttribute]
        public new int? idUsuario { get; set; }

        #endregion
    }
}
