using System;
using Microsoft.Extensions.Logging.Tests.CustomLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.Extensions.Logging.Tests.Constants;


namespace Microsoft.Extensions.Logging.Tests
{

    [TestClass]
    public class CustomLoggingTests
    {
        static ILogger Logger { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Logger = ApplicationLogging.CreateLogger<CustomLoggingTests>();
        }

        [TestMethod]
        public void LogInformation_UsingLogFactoryWithAddCustomLoggerExtensionMethod_Success()
        {
            CustomLogger.CustomLogger customLogger = null;
            LoggerFactory loggerFactory = new LoggerFactory();

            // Add provider via extension method, hooking up to create logger event once added.
            CustomLoggerProvider logProvider;
            loggerFactory.AddCustomLogger(out logProvider);
            logProvider.OnCreateLogger += (sender, eventArgs) => customLogger = eventArgs.CustomLogger;

            ILogger logger = loggerFactory.CreateLogger<CustomLoggingTests>();
            Assert.IsNotNull(customLogger,
                "Unexpectedly, the CreateLogger() method on CustomLoggerProvider was not called.");

            logger.LogInformation(Message);

            Assert.AreEqual<string>(Message, customLogger.LogDataQueue.Dequeue());
        }


        [TestMethod]
        public void LogInformation_UsingLogFactoryWithAddProvider_Success()
        {
            CustomLogger.CustomLogger customLogger = null;
            LoggerFactory loggerFactory = new LoggerFactory();

            // Add provider via extension method, hooking up to create logger event once added.
            CustomLoggerProvider logProvider = new CustomLoggerProvider();
            loggerFactory.AddProvider(logProvider);
            logProvider.OnCreateLogger += (sender, eventArgs) => customLogger = eventArgs.CustomLogger;

            ILogger logger = loggerFactory.CreateLogger<CustomLoggingTests>();
            Assert.IsNotNull(customLogger,
                "Unexpectedly, the CreateLogger() method on CustomLoggerProvider was not called.");

            logger.LogInformation(Message);

            Assert.AreEqual<string>(Message, customLogger.LogDataQueue.Dequeue());
        }

        [TestMethod]
        public void LogInformation_UsingApplicationLogFactoryWithClassLogger_Success()
        {
            CustomLogger.CustomLogger customLogger = null;
            CustomLoggerProvider logProvider =
                new CustomLoggerProvider((sender, eventArgs) => customLogger = eventArgs.CustomLogger);
            ApplicationLogging.LoggerFactory.AddProvider(logProvider);
            Assert.IsNotNull(customLogger,
                "Unexpectedly, the CreateLogger() method on CustomLoggerProvider was not called.");

            Logger.LogInformation(Message);

            Assert.AreEqual<string>(Message, customLogger.LogDataQueue.Dequeue());
        }

        [TestMethod]
        //we're catching an AggregateException because the guts of the logger will actually catch the
        //InvalidOperatoinException and rethrow an AggregateException
        [ExpectedException(typeof(AggregateException))]
        public void LogInformation_ThrowException_Throws()
        {
            CustomLogger.CustomLogger customLogger = null;
            CustomLoggerProvider logProvider =
                new CustomLoggerProvider((sender, eventArgs) => customLogger = eventArgs.CustomLogger);
            ApplicationLogging.LoggerFactory.AddProvider(logProvider);
            Logger.Log(LogLevel.Information, 0, "state", null,
                (obj, exception) => { throw new InvalidOperationException(); });
            Assert.AreEqual<string>(Message, customLogger.LogDataQueue.Dequeue());
        }

        [TestMethod]
        public void LogCritical_Exception_Success()
        {
            string message = "The amount of caffeine has reach critical levels.";
            CustomLogger.CustomLogger customLogger = null;
            CustomLoggerProvider logProvider =
                new CustomLoggerProvider((sender, eventArgs) => customLogger = eventArgs.CustomLogger);
            ApplicationLogging.LoggerFactory.AddProvider(logProvider);
            Logger.LogCritical(message, new Exception("Sample exception."));
            Assert.AreEqual<string>($"{message}\r\nSystem.Exception: Sample exception.", customLogger.LogDataQueue.Dequeue());
        }
    }

}
