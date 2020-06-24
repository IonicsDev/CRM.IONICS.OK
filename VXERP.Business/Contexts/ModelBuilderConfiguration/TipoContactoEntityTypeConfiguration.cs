using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using CRM.Business.Entities;

namespace CRM.Business.Contexts.ModelBuilderConfiguration
{
    public class TipoContactoEntityTypeConfiguration : EntityTypeConfiguration<TipoContacto>
    {
        public TipoContactoEntityTypeConfiguration()
        {
            this.Property(p => p.Id).HasColumnName("Id");

            this.ToTable("TiposContacto");
        }
    }
}
