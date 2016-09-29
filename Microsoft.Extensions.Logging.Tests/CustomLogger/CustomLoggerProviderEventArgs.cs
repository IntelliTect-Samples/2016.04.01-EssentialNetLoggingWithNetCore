namespace Microsoft.Extensions.Logging.Tests.CustomLogger
{
    public class CustomLoggerProviderEventArgs
    {
        public CustomLogger CustomLogger { get; }
        public CustomLoggerProviderEventArgs(CustomLogger logger)
        {
            CustomLogger = logger;
        }
    }
}