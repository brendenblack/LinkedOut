using LinkedOut.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<JobApplication> JobApplications { get; }

        public DbSet<JobSearch> JobSearches { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
