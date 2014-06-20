using System.Globalization;
using NUnit.Framework;
using StormMultiLang.Write;

namespace StormMultiLangTests.Write
{
    [TestFixture]
    public class GivenAJsonProtocolWriterFormat
    {
        [Test]
        public void ShouldHaveCorrectSyncString()
        {
            var subjectUnderTest = new JsonProtocolWriterFormat();
            Assert.That(subjectUnderTest.Sync().TrimStuffForCompare(), 
                Is.EqualTo(JsonStrings.CommandSyncOut().TrimStuffForCompare()));
        }

        [Test]
        public void ShouldHaveCorrectPidString()
        {
            var subjectUnderTest = new JsonProtocolWriterFormat();
            Assert.That(subjectUnderTest.ProcessId(1234).TrimStuffForCompare(),
                Is.EqualTo(JsonStrings.PidOut().TrimStuffForCompare()));
        }

        [Test]
        public void ShouldHaveCorrectFailString()
        {
            var subjectUnderTest = new JsonProtocolWriterFormat();
            Assert.That(subjectUnderTest.Fail(1231231).TrimStuffForCompare(),
                Is.EqualTo(JsonStrings.CommandFailOut().TrimStuffForCompare()));
        }

        [Test]
        public void ShouldHaveCorrectAcknowledgeString()
        {
            var subjectUnderTest = new JsonProtocolWriterFormat();
            Assert.That(subjectUnderTest.Acknowledge(1231231).TrimStuffForCompare(),
                Is.EqualTo(JsonStrings.CommandAckOut().TrimStuffForCompare()));
        }

        [Test]
        public void ShouldHaveCorrectLogString()
        {
            var subjectUnderTest = new JsonProtocolWriterFormat();
            Assert.That(subjectUnderTest.LogInfo("hello world!").TrimStuffForCompare(),
                Is.EqualTo(JsonStrings.CommandLogOut().TrimStuffForCompare()));
        }

        [Test]
        public void ShouldHaveCorrectErrorString()
        {
            var subjectUnderTest = new JsonProtocolWriterFormat();
            Assert.That(subjectUnderTest.LogError("ERROR!!!!!!").TrimStuffForCompare(),
                Is.EqualTo(JsonStrings.CommandErrorOut().TrimStuffForCompare()));
        }

        [Test]
        public void ShouldEmitWithAnchorsAll()
        {
            var subjectUnderTest = new JsonProtocolWriterFormat();
            var emitJson =
                subjectUnderTest.EmitCommand(new object[] {"field1", 2, 3}, new long[] {1231231, -234234234}, 9, 1.ToString(CultureInfo.InvariantCulture));

            Assert.That(
                emitJson,
                Is.EqualTo(JsonStrings.CommandEmitWithAnchorAll().TrimStuffForCompare()));
        }

        [Test]
        public void ShouldEmitWithAnchorsNoStream()
        {
            var subjectUnderTest = new JsonProtocolWriterFormat();
            var emitJson =
                subjectUnderTest.EmitCommand(new object[] { "field1", 2, 3 }, new long[] { 1231231, -234234234 }, 9);

            Assert.That(
                emitJson,
                Is.EqualTo(JsonStrings.CommandEmitWithAnchorNoStream().TrimStuffForCompare()));
        }

        [Test]
        public void ShouldEmitWithAnchorsNoStreamNoTask()
        {
            var subjectUnderTest = new JsonProtocolWriterFormat();
            var emitJson =
                subjectUnderTest.EmitCommand(new object[] { "field1", 2, 3 }, new long[] { 1231231, -234234234 });

            Assert.That(
                emitJson,
                Is.EqualTo(JsonStrings.CommandEmitWithAnchorNoStreamNoTask().TrimStuffForCompare()));
        }

        [Test]
        public void ShouldEmitAll()
        {
            var subjectUnderTest = new JsonProtocolWriterFormat();
            var emitJson =
                subjectUnderTest.EmitCommand(new object[] { "field1", 2, 3 }, 1231231, 9, 1.ToString(CultureInfo.InvariantCulture));

            Assert.That(
                emitJson,
                Is.EqualTo(JsonStrings.CommandEmitAll().TrimStuffForCompare()));
        }

        [Test]
        public void ShouldEmitNoStream()
        {
            var subjectUnderTest = new JsonProtocolWriterFormat();
            var emitJson =
                subjectUnderTest.EmitCommand(new object[] { "field1", 2, 3 }, 1231231, 9);

            Assert.That(
                emitJson,
                Is.EqualTo(JsonStrings.CommandEmitNoStream().TrimStuffForCompare()));
        }

        [Test]
        public void ShouldEmitNoStreamNoTask()
        {
            var subjectUnderTest = new JsonProtocolWriterFormat();
            var emitJson =
                subjectUnderTest.EmitCommand(new object[] { "field1", 2, 3 }, 1231231);

            Assert.That(
                emitJson,
                Is.EqualTo(JsonStrings.CommandEmitNoStreamNoTask().TrimStuffForCompare()));
        }

        [Test]
        public void ShouldEmitNoStreamNoTaskNoId()
        {
            var subjectUnderTest = new JsonProtocolWriterFormat();
            var emitJson =
                subjectUnderTest.EmitCommand(new object[] { "field1", 2, 3 });

            Assert.That(
                emitJson,
                Is.EqualTo(JsonStrings.CommandEmitNoStreamNoTaskNoId().TrimStuffForCompare()));
        }
    }
}