using NSubstitute;
using NUnit.Framework;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLangTests.Read
{
    [TestFixture]
    public class GivenAStormReader
    {
        public static IProtocolReaderFormat FormatSubsitute(string command, string taskIds)
        {
            var format = Substitute.For<IProtocolReaderFormat>();
            format.IsTaskIdList(command).Returns(false);
            format.IsTaskIdList(taskIds).Returns(true);
            format.Command(command).Returns(Substitute.For<IStormCommandIn>());
            format.TaskIds(taskIds).Returns(new long[0]);
            return format;
        }

        [Test]
        public void ShouldReturnHandshakeMessage()
        {
            var command = JsonStrings.HandshakeMessageIn().WithoutEnd().ToSingleString();

            var reader = Substitute.For<IReadNext>();
            reader.Next().Returns(command);

            var format = Substitute.For<IProtocolReaderFormat>();
            var setupProcess = Substitute.For<ISetupProcess>();
            format.Handshake(command).Returns(new StormHandshake(setupProcess));

            var subjectUnderTest = new StormReader(reader, format);
            
            Assert.That(
                subjectUnderTest.ReadInitialHandshakeMessage(),
                Is.Not.Null);

            format.Received().Handshake(command);
            format.DidNotReceive().Handshake(null);
        }
        
        [Test]
        public void ShouldReadCommand()
        {
            var command = JsonStrings.TupleIn().WithoutEnd().ToSingleString();
            var taskIds = JsonStrings.TaskIdsIn().WithoutEnd().ToSingleString();

            var reader = Substitute.For<IReadNext>();
            reader.Next().Returns(command);

            var format = FormatSubsitute(command, taskIds);

            var subjectUnderTest = new StormReader(reader, format);

            Assert.That(subjectUnderTest.ReadCommand(),Is.Not.Null);
        }

        [Test]
        public void ShouldNotLooseCommandIfTasksReadFirst()
        {
            var command = JsonStrings.TupleIn().WithoutEnd().ToSingleString();
            var taskIds = JsonStrings.TaskIdsIn().WithoutEnd().ToSingleString();

            var reader = Substitute.For<IReadNext>();
            reader.Next().Returns(command, taskIds);

            var format = FormatSubsitute(command, taskIds);

            var subjectUnderTest = new StormReader(reader, format);
            subjectUnderTest.ReadTaskIds();
            
            Assert.That(subjectUnderTest.ReadCommand(), Is.Not.Null);
        }

        

        [Test]
        public void ShouldReadCommandWhenPrecededByTaskIds()
        {
            var command = JsonStrings.TupleIn().WithoutEnd().ToSingleString();
            var taskIds = JsonStrings.TaskIdsIn().WithoutEnd().ToSingleString();

            var reader = Substitute.For<IReadNext>();
            reader.Next().Returns(taskIds, taskIds, command);

            var format = FormatSubsitute(command, taskIds);

            var subjectUnderTest = new StormReader(reader, format);
            Assert.That(subjectUnderTest.ReadCommand(), Is.Not.Null);
        }

        [Test]
        public void ShouldReadTaskIds()
        {
            var command = JsonStrings.TupleIn().WithoutEnd().ToSingleString();
            var taskIds = JsonStrings.TaskIdsIn().WithoutEnd().ToSingleString();

            var reader = Substitute.For<IReadNext>();
            reader.Next().Returns(taskIds);

            var format = FormatSubsitute(command, taskIds);

            var subjectUnderTest = new StormReader(reader, format);
            Assert.That(subjectUnderTest.ReadTaskIds(), Is.Not.Null);
        }

        [Test]
        public void ShouldNotLooseTaskIdsIfCommandReadFirst()
        {
            var command = JsonStrings.TupleIn().WithoutEnd().ToSingleString();
            var taskIds = JsonStrings.TaskIdsIn().WithoutEnd().ToSingleString();

            var reader = Substitute.For<IReadNext>();
            reader.Next().Returns(taskIds, command);

            var format = FormatSubsitute(command, taskIds);
            
            var subjectUnderTest = new StormReader(reader, format);
            subjectUnderTest.ReadCommand();

            Assert.That(subjectUnderTest.ReadTaskIds(), Is.Not.Null);
        }

        [Test]
        public void ShouldReadTaskidsWhenPrecededByCommand()
        {
            var command = JsonStrings.TupleIn().WithoutEnd().ToSingleString();
            var taskIds = JsonStrings.TaskIdsIn().WithoutEnd().ToSingleString();

            var reader = Substitute.For<IReadNext>();
            reader.Next().Returns(command, command, taskIds);

            var format = FormatSubsitute(command, taskIds);

            var subjectUnderTest = new StormReader(reader, format);
            Assert.That(subjectUnderTest.ReadTaskIds(), Is.Not.Null);
        }
        
    }
}