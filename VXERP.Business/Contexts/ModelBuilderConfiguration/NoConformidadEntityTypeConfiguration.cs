using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using CRM.Business.Entities;

namespace CRM.Business.Contexts.ModelBuilderConfiguration
{
    public class NoConformidadEntityTypeConfiguration : EntityTypeConfiguration<NoConformidad>
    {
        public NoConformidadEntityTypeConfiguration()
        {
            this.Property(p => p.Id).HasColumnName("Id");

            //this.HasMany(r => r.AgrupRoles).WithMany(u => u.AccionRolModulo).Map(m =>
            //    {
            //        m.ToTable("NoConformidadRols");
            //        m.MapLeftKey("NoConformidad_Id");
            //        m.MapRightKey("Rol_Id");
            //    });
            this.Ignore(t => t.Id);
         


            this.ToTable("NoConformidad");
        }
    }
}
