using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using StormMultiLang;

namespace StormMultiLangTests
{
    [TestFixture]
    public class GivenAStdInOutCommunition
    {
        [Test]
        public void ShouldReadFromInStream()
        {
            Console.SetIn(new StringReader("hello"));

            var subjectUnderTest = new StdInOutCommunication();
            var readLine = subjectUnderTest.ReadLine();

            Assert.That(readLine, Is.EqualTo("hello"));
        }

        [Test]
        public void ShouldOutputOutStrem()
        {
            var emptyString = new StringBuilder();
            Console.SetOut(new StringWriter(emptyString));

            var subjectUnderTest = new StdInOutCommunication();
            subjectUnderTest.WriteLine("hello");
            subjectUnderTest.Flush();

            Assert.That(emptyString.ToString(), Is.EqualTo("hello\r\n"));
        }

        [Test]
        public void ShouldOutputToError()
        {
            var emptyString = new StringBuilder();
            Console.SetError(new StringWriter(emptyString));

            var subjectUnderTest = new StdInOutCommunication();
            subjectUnderTest.Error("error");

            Assert.That(emptyString.ToString(), Is.EqualTo("error\r\n"));
        }
    }
}