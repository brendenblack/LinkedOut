using LinkedOut.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Api.IntegrationTests.TestHarness
{
    public class TestCurrentUserService : ICurrentUserService
    {
        public string UserId { get; set; } = "00000000-0000-0000-0000-000000000000";

        public bool IsAuthenticated => throw new NotImplementedException();

        public string FirstName => throw new NotImplementedException();

        public string Email => throw new NotImplementedException();
    }
}
