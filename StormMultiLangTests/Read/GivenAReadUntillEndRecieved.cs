using System.IO;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using StormMultiLang;
using StormMultiLang.Read;

namespace StormMultiLangTests.Read
{
    [TestFixture]
    public class GivenAReadUntillEndRecieved
    {
        [Test]
        public void ShouldReadAllLinesExceptTheEndString()
        {
            var readLines = Substitute.For<ICommunication>();
            readLines.ReadLine().Returns(
                JsonStrings.HandshakeMessageIn().First(),
                JsonStrings.HandshakeMessageIn().Skip(1).ToArray());
            var expected = JsonStrings.HandshakeMessageIn().WithoutEnd().ToSingleString();
            
            var subjectUnderTest = new ReadUntillEndRecieved(readLines);

            Assert.That(subjectUnderTest.Next(), Is.EqualTo(expected));
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void ShouldThowExceptionIfNullLinesRead()
        {
            var readLines = Substitute.For<ICommunication>();
            readLines.ReadLine().Returns(string.Empty);
            var subjectUnderTest = new ReadUntillEndRecieved(readLines);
            subjectUnderTest.Next();
        }
    }
}