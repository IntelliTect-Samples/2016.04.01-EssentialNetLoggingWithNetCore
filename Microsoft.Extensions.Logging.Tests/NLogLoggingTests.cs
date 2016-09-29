using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Tests.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog.Targets;
using static Microsoft.Extensions.Logging.Tests.Constants;
using NLog.Extensions.Logging;

namespace Microsoft.Extensions.Logging.Tests
{
    [TestClass]
    public class NLogLoggingTests
    {
        ILogger Logger {get;} 
            = ApplicationLogging.CreateLogger<NLogLoggingTests>();

        [TestMethod]
        public void LogInformation_UsingMemoryTarget_LogMessageAppears()
        {
            // Add NLog provider
            ApplicationLogging.LoggerFactory.AddNLog();

            // Configure target
            MemoryTarget target = new MemoryTarget();
            target.Layout = "${message}";
            global::NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(
                target, global::NLog.LogLevel.Info);

            Logger.LogInformation(Message);

            Assert.AreEqual<string>(
                Message, target.Logs.FirstOrDefault<string>());
        }
    }
}
