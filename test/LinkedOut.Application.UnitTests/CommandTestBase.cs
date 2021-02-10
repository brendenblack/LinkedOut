using LinkedOut.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace LinkedOut.Application.UnitTests
{
    public class CommandTestBase : IDisposable
    {
        public CommandTestBase()
        {
            Context = ApplicationDbContextFactory.Create();
        }

        public IApplicationDbContext Context { get; }

        public void Dispose()
        {
            ApplicationDbContextFactory.Destroy((DbContext)Context);
        }
    }
}
