using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using CRM.Business.Entities;

namespace CRM.Business.Contexts.ModelBuilderConfiguration
{
    public class RolesEmpresaEntityTypeConfiguration : EntityTypeConfiguration<RolEmpresa>
    {
        public RolesEmpresaEntityTypeConfiguration()
        {
            this.Property(p => p.Id).HasColumnName("Id");

            //this.HasMany(r => r.AccionRolModulo).WithMany().Map(m =>
            //    {
            //        m.ToTable("UsuarioRols");
            //        m.MapLeftKey("Rol_Id");
            //    });
          
            this.Ignore(t => t.idUsuario);
         //   this.Ignore(t => t.Recordarme);



            this.ToTable("UsuarioRols");
        }
    }
}
