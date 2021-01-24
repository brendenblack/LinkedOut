using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LinkedOut.Application.IntegrationTests
{
    public class XUnitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper output;

        public XUnitLoggerProvider(ITestOutputHelper output)
        {
            this.output = output;
        }

        public ILogger CreateLogger<T>() => new XUnitLogger<T>(output);

        public ILogger CreateLogger(string categoryName)
        {
            return CreateLogger<XUnitLoggerProvider>();
        }

        public void Dispose() { }
    }
}
