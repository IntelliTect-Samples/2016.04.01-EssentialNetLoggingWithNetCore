using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Logging.Tests.CustomLogger
{

    public class CustomLogger : ILogger
    {
        public Queue<string> LogDataQueue = new Queue<string>();

        public IDisposable BeginScopeImpl(object state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            string message = string.Empty;

            if (formatter != null)
            {
                message = formatter(state, exception);
            }
            else
            {
                message = LogFormatter.Formatter(state, exception);
            }

            LogDataQueue.Enqueue(message);

        }

      }
}
