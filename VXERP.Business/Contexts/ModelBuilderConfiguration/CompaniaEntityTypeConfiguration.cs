using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using CRM.Business.Entities;

namespace CRM.Business.Contexts.ModelBuilderConfiguration
{
    public class CompaniaEntityTypeConfiguration : EntityTypeConfiguration<Compania>
    {
        public CompaniaEntityTypeConfiguration()
        {
            this.Property(p => p.Id).HasColumnName("Id");

            this.ToTable("Companias");
        }
    }
}
