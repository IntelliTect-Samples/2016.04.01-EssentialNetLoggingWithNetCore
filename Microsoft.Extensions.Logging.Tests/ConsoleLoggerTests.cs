using System;
using IntelliTect.ConsoleView;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Tests.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.Extensions.Logging.Tests.Constants;


namespace Microsoft.Extensions.Logging.Tests
{
    [TestClass]
    public class ConsoleLoggerTests
    {
        ILogger Logger = ApplicationLogging.CreateLogger<ConsoleLoggerTests>();

        public TestContext TestContext { get; set; }

        [TestMethod]
        [Ignore] // Currently not working until RC-2 is released.
        public void LogInformation_UsingLocalLogFactoryWithLocalLogger_Success()
        {
            Tester.Test(Message, () =>
            {
                ILoggerFactory loggerFactory = new LoggerFactory().AddConsole();

                ILogger logger = loggerFactory.CreateLogger<ConsoleLoggerTests>();

                logger.LogInformation(Message);
            }
            );
        }

        [TestMethod]
        public void CallLoggingInSampleWebApplicationProcess()
        {
            Tester.ExecuteProcess(
                $@"info: SampleWebConsoleApp.Program[[]0[]]
      {Message}
", // + "\r\n",
                "dnx.exe", "SampleWebConsoleApp.dll",
                System.IO.Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location)
                );
        }
        public void CallLoggingInSampleWebApplicationProcess_WithCallChain()
        {
            Tester.ExecuteProcess(
                $@"info: SampleWebConsoleApp.Program[[]0[]]
      {Message}
", // + "\r\n",
                "dnx.exe", $"SampleWebConsoleApp.dll emergency {LogLevel.Debug.ToString()}",
                System.IO.Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location)
                );
        }
    }
}
