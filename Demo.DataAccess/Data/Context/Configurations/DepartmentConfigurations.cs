
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataAccess.Data.Context.Configurations
{
    internal class DepartmentConfigurations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(D => D.Id)
                .UseIdentityColumn(10, 10);

            builder.Property(D => D.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(D => D.CreatedOn).HasDefaultValueSql("GETDATE()");

            builder.Property(D => D.LastModifiedOn).HasComputedColumnSql("GETDATE()");
        }
    }
}
