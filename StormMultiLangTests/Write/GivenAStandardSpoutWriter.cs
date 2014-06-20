using System;
using NSubstitute;
using NUnit.Framework;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLangTests.Write
{
    [TestFixture]
    public class GivenAStandardSpoutWriter
    {
        [Test]
        public void ShouldSendSync()
        {
            var sync = JsonStrings.CommandSyncOut();
            var format = Substitute.For<IProtocolWriterFormat>();
            format.Sync().Returns(sync);

            var writer = Substitute.For<IWriteNext>();
            var reader = Substitute.For<IStormReader>();

            var subjectUnderTest = new StandardSpoutWriter(writer, format, reader);
            subjectUnderTest.Sync();

            writer.Received().Write(sync);
        }
        
        [Test]
        public void ShouldLogInfo()
        {
            var log = JsonStrings.CommandLogOut();

            var format = Substitute.For<IProtocolWriterFormat>();
            format.LogInfo("hello world!").Returns(log);

            var writer = Substitute.For<IWriteNext>();
            var reader = Substitute.For<IStormReader>();

            var subjectUnderTest = new StandardSpoutWriter(writer, format, reader);
            subjectUnderTest.LogInfo("hello world!");

            writer.Received().Write(log);
        }

        [Test]
        public void ShouldLogError()
        {
            var format = Substitute.For<IProtocolWriterFormat>();
            var writer = Substitute.For<IWriteNext>();
            var reader = Substitute.For<IStormReader>();

            var subjectUnderTest = new StandardSpoutWriter(writer, format, reader);
            subjectUnderTest.LogError("ERROR!!!!!!");

            writer.Received().AlternativeErrorStream("ERROR!!!!!!");
            format.DidNotReceiveWithAnyArgs().LogError("");
        }

        [Test]
        public void ShouldEmitAndReturnTaskIdsBecauseNotDirect()
        {
            var tuple = new object[0];
            var emit = JsonStrings.CommandEmitAll();

            var format = Substitute.For<IProtocolWriterFormat>();
            format.EmitCommand(tuple, 11, null, "1").Returns(emit);

            var writer = Substitute.For<IWriteNext>();
            var reader = Substitute.For<IStormReader>();
            reader.ReadTaskIds().Returns(new long[] {1, 2, 3});

            var subjectUnderTest = new StandardSpoutWriter(writer, format, reader);
            Assert.That(subjectUnderTest.EmitTuple(tuple, 11, "1"), Is.EquivalentTo(new long[] { 1, 2, 3 }));

            writer.Received().Write(emit);
        }

        [Test]
        public void ShouldEmitWithTaskIdBecauseDirect()
        {
            var tuple = new Object[0];
            var emit = JsonStrings.CommandEmitAll();

            var format = Substitute.For<IProtocolWriterFormat>();
            format.EmitCommand(tuple, 11, 22, "1").Returns(emit);

            var writer = Substitute.For<IWriteNext>();
            var reader = Substitute.For<IStormReader>();

            var subjectUnderTest = new StandardSpoutWriter(writer, format, reader);
            subjectUnderTest.EmitTupleDirect(tuple, 22, 11, "1");

            writer.Received().Write(emit);
            reader.DidNotReceive().ReadTaskIds();
        }
    
    }
}