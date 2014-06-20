using NSubstitute;
using NUnit.Framework;
using StormMultiLang.Write;

namespace StormMultiLangTests.Write
{
    [TestFixture]
    public class GivenAWritePidFileAndSendIdToParent
    {
        [Test]
        public void ShouldGetPidSendToPArentThenWriteFile()
        {
            var osStuff = Substitute.For<IOsSpecific>();
            osStuff.GetProcessId().Returns(1234);
           
            var jsonStringToSend = JsonStrings.PidOut();
            var writer = Substitute.For<IWriteNext>();
            
            var format = Substitute.For<IProtocolWriterFormat>();
            format.ProcessId(1234).Returns(jsonStringToSend);

            var subjectUnderTest = new WritePidFileAndSendIdToParent(
                format, osStuff, writer);

            subjectUnderTest.Setup("C:\\temp");

            osStuff.Received().WritePidFile(1234, "C:\\temp");
            writer.Received().Write(jsonStringToSend);
        }
    }
}