using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ILogValues values = state as ILogValues;

            if (formatter != null)
            {
                message = formatter(state, exception);
            }
            //else if (values != null)
            //{

            //    message = $"{ logLevel.ToString() } ({eventId}): {exception?.ToString()}";

            //    if (exception != null)
            //    {
            //        message += Environment.NewLine + exception;
            //    }
            //}
            else
            {
                message = LogFormatter.Formatter(state, exception);
            }

            LogDataQueue.Enqueue(message);

        }
    }
}
