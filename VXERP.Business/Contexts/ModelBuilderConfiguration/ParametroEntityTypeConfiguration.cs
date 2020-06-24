using CRM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace CRM.Business.Contexts.ModelBuilderConfiguration
{
    public class ParametroEntityTypeConfiguration : EntityTypeConfiguration<Parametro>
    {
        public ParametroEntityTypeConfiguration()
        {
            this.Property(p => p.Id).HasColumnName("Id");

            //this.HasMany(r => r.Modulos).WithMany( p => p.Roles).Map(m =>
            //    {
            //        m.ToTable("RolModulos");
            //        m.MapLeftKey("Rol_id");
            //        m.MapLeftKey("Modulo_Id");

            //    });

            this.Ignore(t => t.idUsuario);

            this.ToTable("Parametros");
        }
    }
}
