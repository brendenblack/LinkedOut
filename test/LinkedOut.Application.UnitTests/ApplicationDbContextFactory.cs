using FakeItEasy;
using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Application.UnitTests
{
    /// <summary>
    /// Allows the creation of an in memory database for testing. 
    /// </summary>
    /// <remarks>
    /// This class requires that we create a dependency to the <see cref="LinkedOut.Infrastructure"/>
    /// project so that we can access <see cref="SqlServerApplicationDbContext"/>. This is not ideal, but does provide
    /// a huge convenience when it comes to creating tests.
    /// </remarks>
    public static class ApplicationDbContextFactory
    {
        public static SqlServerApplicationDbContext Create(string id = "")
        {
            var options = new DbContextOptionsBuilder<SqlServerApplicationDbContext>()
                .UseInMemoryDatabase(string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id)
                .Options;

            var logger = A.Fake<ILogger<SqlServerApplicationDbContext>>();

            var dateTimeMock = A.Fake<IDateTime>();
            A.CallTo(() => dateTimeMock.Now)
                .Returns(new DateTime(3001, 1, 1));

            var currentUserServiceMock = A.Fake<ICurrentUserService>();
            A.CallTo(() => currentUserServiceMock.UserId)
                .Returns("00000000-0000-0000-0000-000000000000");

            var context = new SqlServerApplicationDbContext(logger, options, currentUserServiceMock, dateTimeMock);

            //SeedSampleData(context);

            return context;
        }

        //public static void SeedSampleData(ApplicationDbContext context)
        //{

        //}

        public static void Destroy(DbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}
