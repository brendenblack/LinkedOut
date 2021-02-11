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
    public class JobSearchConfiguration : IEntityTypeConfiguration<JobSearch>
    {
        public void Configure(EntityTypeBuilder<JobSearch> builder)
        {
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
