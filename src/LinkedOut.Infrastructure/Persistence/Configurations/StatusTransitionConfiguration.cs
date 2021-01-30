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
    public class StatusTransitionConfiguration : IEntityTypeConfiguration<StatusTransition>
    {
        public void Configure(EntityTypeBuilder<StatusTransition> builder)
        {
            
        }
    }
}
