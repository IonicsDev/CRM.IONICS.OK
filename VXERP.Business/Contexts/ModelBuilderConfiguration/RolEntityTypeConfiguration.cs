using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using CRM.Business.Entities;

namespace CRM.Business.Contexts.ModelBuilderConfiguration
{
    public class RolEntityTypeConfiguration : EntityTypeConfiguration<Rol>
    {
        public RolEntityTypeConfiguration()
        {
            this.Property(p => p.Id).HasColumnName("Id");

            //this.HasMany(r => r.Modulos).WithMany( p => p.Roles).Map(m =>
            //    {
            //        m.ToTable("RolModulos");
            //        m.MapLeftKey("Rol_id");
            //        m.MapLeftKey("Modulo_Id");
                    
            //    });
           
            this.Ignore(t => t.idUsuario);

            this.ToTable("Roles");
        }
    }
}
