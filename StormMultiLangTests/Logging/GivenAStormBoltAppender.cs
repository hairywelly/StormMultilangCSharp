using System;
using log4net.Core;
using NSubstitute;
using NUnit.Framework;
using StormMultiLang.Logging;
using StormMultiLang.Write;

namespace StormMultiLangTests.Logging
{
    [TestFixture]
    public class GivenAStormBoltAppender
    {
        private LoggingEventData ErrorLog()
        {
            return new LoggingEventData
            {
                Domain="Test",
                ExceptionString = "Exception", 
                Identity = "Test Source", 
                Level = Level.Error, 
                Message = "ERROR!!", 
                TimeStamp = DateTime.Now
            };
        }


        private LoggingEventData InfoLog()
        {
            return new LoggingEventData
            {
                Domain = "Test",
                ExceptionString = "Exception",
                Identity = "Test Source",
                Level = Level.Info,
                Message = "Info",
                TimeStamp = DateTime.Now
            };
        }
        
        
        [Test]
        public void ShouldLogInfo()
        {
            var boltWriter = Substitute.For<IBoltWriter>();
            var subjectUnderTest = new StormBoltAppender(boltWriter);
            subjectUnderTest.DoAppend(new LoggingEvent(ErrorLog()));

            boltWriter.ReceivedWithAnyArgs().LogError("");
            boltWriter.DidNotReceive().LogInfo("");
        }

        [Test]
        public void ShouldLogError()
        {
            var boltWriter = Substitute.For<IBoltWriter>();
            var subjectUnderTest = new StormBoltAppender(boltWriter);
            subjectUnderTest.DoAppend(new LoggingEvent(InfoLog()));

            boltWriter.DidNotReceive().LogError("");
            boltWriter.ReceivedWithAnyArgs().LogInfo("");
        }
    }
}