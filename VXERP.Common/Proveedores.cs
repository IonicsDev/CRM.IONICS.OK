//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VXERP.Common
{
    using System;
    using System.Collections.Generic;
    
    public partial class Proveedores
    {
        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string DescripcionCorta { get; set; }
        public string RUC { get; set; }
        public Nullable<bool> isContribuyenteEspecial { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Interseccion { get; set; }
        public int IdTipoProveedor { get; set; }
        public int IdProvincia { get; set; }
        public Nullable<int> IdCiudad { get; set; }
        public Nullable<int> IdCanton { get; set; }
        public Nullable<int> IdParroquia { get; set; }
        public int IdTipoLocal { get; set; }
        public string TelefonoPBX { get; set; }
        public string Telefono2 { get; set; }
        public string Fax { get; set; }
        public string ContactoCompra { get; set; }
        public string CorreoContactoCompra { get; set; }
        public string CelularContacto { get; set; }
        public string ContactoCXP { get; set; }
        public string CorreoContactoCXP { get; set; }
        public string CelularContactoCXP { get; set; }
        public Nullable<int> IdActividad { get; set; }
        public Nullable<System.DateTime> FechaInicioActividad { get; set; }
        public string RepresentanteLegal { get; set; }
        public Nullable<int> IdOcupacion { get; set; }
        public string TelefonoRepresentante { get; set; }
        public string CelularRepresentante { get; set; }
        public string CorreoRepresentante { get; set; }
        public decimal MontoCredito { get; set; }
        public int PlazoCredito { get; set; }
        public bool Estado { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public System.DateTime FechaActualizacion { get; set; }
        public Nullable<int> IdUsuario { get; set; }
    
        public virtual Actividades Actividades { get; set; }
        public virtual Cantones Cantones { get; set; }
        public virtual Ciudades Ciudades { get; set; }
        public virtual Ocupaciones Ocupaciones { get; set; }
        public virtual Parroquias Parroquias { get; set; }
        public virtual Provincias Provincias { get; set; }
        public virtual TiposLocal TiposLocal { get; set; }
        public virtual TiposProveedor TiposProveedor { get; set; }
        public virtual Usuarios Usuarios { get; set; }
    }
}
