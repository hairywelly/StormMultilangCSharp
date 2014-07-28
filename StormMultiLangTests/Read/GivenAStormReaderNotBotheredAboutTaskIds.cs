using NSubstitute;
using NUnit.Framework;
using StormMultiLang.Read;

namespace StormMultiLangTests.Read
{
    [TestFixture]
    public class GivenAStormReaderNotBotheredAboutTaskIds
    {
        [Test]
        public void ShouldReturnEmptyTaskIdList()
        {
            var readNext = Substitute.For<IReadNext>();
            var format = Substitute.For<IProtocolReaderFormat>();
            var subjectUnderTest = new StormReaderNotBotheredAboutTaskIds(readNext, format);

            Assert.That(subjectUnderTest.ReadTaskIds(),Has.Length.EqualTo(0));
        }

        [Test]
        public void ShouldReadCommand()
        {
            var command = JsonStrings.TupleIn().WithoutEnd().ToSingleString();
            var taskIds = JsonStrings.TaskIdsIn().WithoutEnd().ToSingleString();

            var readNext = Substitute.For<IReadNext>();
            readNext.Next().Returns(command);

            var format = GivenAStormReader.FormatSubsitute(command, taskIds);

            var subjectUnderTest = new StormReaderNotBotheredAboutTaskIds(readNext, format);

            Assert.That(subjectUnderTest.ReadCommand(), Is.Not.Null);
        }

        [Test]
        public void ShouldReturnCommandWhenPrecededByTaksIds()
        {
            var command = JsonStrings.TupleIn().WithoutEnd().ToSingleString();
            var taskIds = JsonStrings.TaskIdsIn().WithoutEnd().ToSingleString();

            var readNext = Substitute.For<IReadNext>();
            readNext.Next().Returns(taskIds, taskIds, taskIds, taskIds, taskIds, command);

            var format = GivenAStormReader.FormatSubsitute(command, taskIds);
            
            var subjectUnderTest = new StormReaderNotBotheredAboutTaskIds(readNext, format);
            
            Assert.That(subjectUnderTest.ReadCommand(), Is.Not.Null);
        }
    }
}