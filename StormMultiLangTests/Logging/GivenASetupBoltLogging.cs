using log4net;
using NSubstitute;
using NUnit.Framework;
using StormMultiLang.Logging;
using StormMultiLang.Write;

namespace StormMultiLangTests.Logging
{
    [TestFixture]
    public class GivenASetupBoltLogging
    {
        [Test]
        public void ShouldRegisterAppenderAndLogToWriter()
        {
            var writer = Substitute.For<IBoltWriter>();

            SetupLog4NetBoltLogging.UsingWriter(writer);
            var logger = LogManager.GetLogger("Test Logger");
            logger.Info("hello");

            writer.ReceivedWithAnyArgs().LogInfo("");
        }
    }
}