using LinkedOut.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<JobApplication> JobApplications { get; }

        public DbSet<JobSearch> JobSearches { get; }
    }
}
