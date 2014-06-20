using System;
using NSubstitute;
using NUnit.Framework;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLangTests.Write
{
    [TestFixture]
    public class GivenAStandardBoltWriter
    {
        [Test]
        public void ShouldAcknowledge()
        {
            var acknowledge = JsonStrings.CommandAckOut();
            
            var format = Substitute.For<IProtocolWriterFormat>();
            format.Acknowledge(123123).Returns(acknowledge);

            var writer = Substitute.For<IWriteNext>();
            var reader = Substitute.For<IStormReader>();

            var subjectUnderTest = new StandardBoltWriter(writer, format, reader);
            subjectUnderTest.Acknowledge(123123);

            writer.Received().Write(acknowledge);
        }

        [Test]
        public void ShouldFail()
        {
            var fail = JsonStrings.CommandFailOut();
            
            var format = Substitute.For<IProtocolWriterFormat>();
            format.Fail(123123).Returns(fail);

            var writer = Substitute.For<IWriteNext>();
            var reader = Substitute.For<IStormReader>();

            var subjectUnderTest = new StandardBoltWriter(writer, format, reader);
            subjectUnderTest.Fail(123123);

            writer.Received().Write(fail);
        }

        [Test]
        public void ShouldLogInfo()
        {
            var log = JsonStrings.CommandLogOut();
            
            var format = Substitute.For<IProtocolWriterFormat>();
            format.LogInfo("hello world!").Returns(log);

            var writer = Substitute.For<IWriteNext>();
            var reader = Substitute.For<IStormReader>();

            var subjectUnderTest = new StandardBoltWriter(writer, format, reader);
            subjectUnderTest.LogInfo("hello world!");

            writer.Received().Write(log);
        }

        [Test]
        public void ShouldLogError()
        {
            var logError = JsonStrings.CommandErrorOut();

            var format = Substitute.For<IProtocolWriterFormat>();
            format.LogError("ERROR!!!!!!").Returns(logError);

            var writer = Substitute.For<IWriteNext>();
            var reader = Substitute.For<IStormReader>();

            var subjectUnderTest = new StandardBoltWriter(writer, format, reader);
            subjectUnderTest.LogError("ERROR!!!!!!");

            writer.Received().Write(logError);
        }

        [Test]
        public void ShouldEmitAndReturnTaskIdsBecauseNotDirect()
        {
            var tuple = new Object[0];
            var anchors = new long[0];
            var emit = JsonStrings.CommandEmitWithAnchorAll();

            var format = Substitute.For<IProtocolWriterFormat>();
            format.EmitCommand(tuple, anchors, null, "1").Returns(emit);

            var writer = Substitute.For<IWriteNext>();
            var reader = Substitute.For<IStormReader>();
            reader.ReadTaskIds().Returns(new long[] {1, 2, 3});

            var subjectUnderTest = new StandardBoltWriter(writer, format, reader);

            Assert.That(subjectUnderTest.EmitTuple(tuple, anchors, "1"), Is.EquivalentTo(new long[] { 1, 2, 3 }));

            writer.Received().Write(emit);
        }

        [Test]
        public void ShouldEmitWithTaskIdBecauseDirect()
        {
            var tuple = new Object[0];
            var anchors = new long[0];
            var emit = JsonStrings.CommandEmitAll();

            var format = Substitute.For<IProtocolWriterFormat>();
            format.EmitCommand(tuple, anchors, 22, "1").Returns(emit);

            var writer = Substitute.For<IWriteNext>();
            var reader = Substitute.For<IStormReader>();

            var subjectUnderTest = new StandardBoltWriter(writer, format, reader);
            subjectUnderTest.EmitTupleDirect(tuple, anchors, 22, "1");

            writer.Received().Write(emit);
            reader.DidNotReceive().ReadTaskIds();
        }
    }
}