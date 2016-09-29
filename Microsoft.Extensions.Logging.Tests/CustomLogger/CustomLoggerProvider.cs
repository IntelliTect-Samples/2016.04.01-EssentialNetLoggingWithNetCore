using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging.Tests.CustomLogger
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        public CustomLoggerProvider() { }
        public CustomLoggerProvider(EventHandler<CustomLoggerProviderEventArgs> onCreateLogger)
        {
            OnCreateLogger = onCreateLogger;
        }
        public ConcurrentDictionary<string, CustomLogger> Loggers { get; set; } = new ConcurrentDictionary<string, CustomLogger>();

        public ILogger CreateLogger(string categoryName)
        {
            CustomLogger customLogger = Loggers.GetOrAdd(categoryName, new CustomLogger());
            OnCreateLogger?.Invoke(this, new CustomLoggerProviderEventArgs(customLogger));
            return customLogger;
        }

        public void Dispose() { }

        public event EventHandler<CustomLoggerProviderEventArgs> OnCreateLogger = delegate { };

    }
}
