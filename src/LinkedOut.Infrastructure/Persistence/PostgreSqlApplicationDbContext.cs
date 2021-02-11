using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Domain.Common;
using LinkedOut.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Infrastructure.Persistence
{
    public class PostgreSqlApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public PostgreSqlApplicationDbContext(
            DbContextOptions<PostgreSqlApplicationDbContext> options,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<JobSearch> JobSearches { get; set; }

        public DbSet<Note> Notes { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                    // prevent physical deletion of AuditableEntities, but still allow the use of Remove and RemoveRange
                    // here, we first set the state to be unchanged, and then modify the IsDeleted field before allowing the save
                    // https://www.ryansouthgate.com/2019/01/07/entity-framework-core-soft-delete/
                    // https://docs.microsoft.com/en-us/ef/core/querying/filters#disabling-filters
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        entry.Entity.IsDeleted = true;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
