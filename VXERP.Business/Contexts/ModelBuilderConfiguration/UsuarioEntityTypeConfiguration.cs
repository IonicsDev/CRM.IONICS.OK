using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using CRM.Business.Entities;

namespace CRM.Business.Contexts.ModelBuilderConfiguration
{
    public class UsuarioEntityTypeConfiguration : EntityTypeConfiguration<Usuario>
    {
        public UsuarioEntityTypeConfiguration()
        {
            this.Property(p => p.Id).HasColumnName("Id");

            //this.HasMany(r => r.AgrupRoles).WithMany(u => u.AccionRolModulo).Map(m =>
            //    {
            //        m.ToTable("UsuarioRols");
            //        m.MapLeftKey("Usuario_Id");
            //        m.MapRightKey("Rol_Id");
            //    });
            this.Ignore(t => t.idUsuario);
            this.Ignore(t => t.Recordarme);
         


            this.ToTable("Usuarios");
        }
    }
}
