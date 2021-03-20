using Microsoft.Extensions.Logging;
using System;

namespace Kubera.App.IntegrationTests.Mock
{
    public class TestLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            Console.WriteLine($"{DateTime.Now:O}[{typeof(T).Name}] - {logLevel}: ({eventId.Id}, {eventId.Name}) {formatter(state, exception)}");
        }
    }
}
