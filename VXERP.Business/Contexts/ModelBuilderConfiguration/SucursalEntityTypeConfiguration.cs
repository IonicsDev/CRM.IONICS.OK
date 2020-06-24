using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using CRM.Business.Entities;

namespace CRM.Business.Contexts.ModelBuilderConfiguration
{
    public class SucursalEntityTypeConfiguration : EntityTypeConfiguration<Sucursal>
    {
        public SucursalEntityTypeConfiguration()
        {
            this.Property(p => p.Id).HasColumnName("Id");

            this.ToTable("Sucursales");
        }
    }
}
