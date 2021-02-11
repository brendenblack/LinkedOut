using Microsoft.Extensions.Logging;
using System;
using Xunit.Abstractions;

namespace LinkedOut.Application.IntegrationTests
{
    public class XUnitLogger<T> : ILogger<T>, IDisposable
    {
        private ITestOutputHelper _output;

        public XUnitLogger(ITestOutputHelper output)
        {
            _output = output;
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                _output.WriteLine($"{typeof(T).Name} [{logLevel}] {formatter(state, exception)}");
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}
