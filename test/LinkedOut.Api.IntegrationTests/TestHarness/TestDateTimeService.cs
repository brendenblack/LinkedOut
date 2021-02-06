using LinkedOut.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Api.IntegrationTests.TestHarness
{
    public class TestDateTimeService : IDateTime
    {
        public DateTime Now => throw new NotImplementedException();
    }
}
