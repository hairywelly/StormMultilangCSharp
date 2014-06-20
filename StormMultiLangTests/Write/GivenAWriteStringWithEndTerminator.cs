using NSubstitute;
using NUnit.Framework;
using StormMultiLang;
using StormMultiLang.Write;

namespace StormMultiLangTests.Write
{
    [TestFixture]
    public class GivenAWriteStringWithEndTerminator
    {
        [Test]
        public void ShouldWriteStuffWithAnEnd()
        {
            var json = JsonStrings.CommandAckOut();
            var rawWriter = Substitute.For<ICommunication>();

            var subjectUnderTest = new WriteStringWithEndTerminator(rawWriter);
            subjectUnderTest.Write(json);

            rawWriter.Received().WriteLine(json);
            rawWriter.Received().WriteLine("end");
            rawWriter.Received().Flush();
        }

        [Test]
        public void ShouldWriteToError()
        {
            const string error = "error!";
            var rawWriter = Substitute.For<ICommunication>();

            var subjectUnderTest = new WriteStringWithEndTerminator(rawWriter);
            subjectUnderTest.AlternativeErrorStream(error);

            rawWriter.Received().Error(error);
        }
    }
}