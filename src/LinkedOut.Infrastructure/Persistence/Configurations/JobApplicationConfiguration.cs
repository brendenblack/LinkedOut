using LinkedOut.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Infrastructure.Persistence.Configurations
{
    public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
    {
        public void Configure(EntityTypeBuilder<JobApplication> builder)
        {
            builder.Ignore(a => a.CanSubmit);

            builder.HasMany(a => a.Notes)
                .WithOne(e => e.Application)
                .HasForeignKey(e => e.ApplicationId);

            builder.OwnsOne(a => a.Location);

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
