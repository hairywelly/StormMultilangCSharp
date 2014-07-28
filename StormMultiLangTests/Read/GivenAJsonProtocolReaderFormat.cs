using NSubstitute;
using NUnit.Framework;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLangTests.Read
{
    [TestFixture]
    public class GivenAJsonProtocolReaderFormat
    {
        private JsonProtocolReaderFormat Subject()
        {
            return new JsonProtocolReaderFormat(
                Substitute.For<ISetupProcess>());
        }
        
        [Test]
        public void ShouldReturnAHandshakeCommand()
        {
            var rawJsonData = JsonStrings.HandshakeMessageIn().WithoutEnd().ToSingleString();
            var subjectUnderTest = Subject();

            var handshake = subjectUnderTest.Handshake(rawJsonData);
            
            Assert.That(handshake.TaskId, Is.EqualTo(3));
            Assert.That(handshake.PidDir, Is.EqualTo(@"C:\temp"));
                
            Assert.That(handshake.Conf, Has.Count.EqualTo(2));
            Assert.That(handshake.TaskComponent, Has.Count.EqualTo(3));
        }

        [Test]
        public void ShouldJsonParse()
        {
            var subjectUnderTest = Subject();
            Assert.That(subjectUnderTest.Get<long>("123456"), Is.EqualTo(123456));
        }

        [Test]
        public void ShouldIndicateTaskIdArray()
        {
            var rawJsonData = JsonStrings.TaskIdsIn().WithoutEnd().ToSingleString();
            var subjectUnderTest = Subject();
            Assert.That(subjectUnderTest.IsTaskIdList(rawJsonData), Is.True );
        }

        [Test]
        public void ShouldIndicateNotTaskIdArray()
        {
            var rawJsonData = JsonStrings.TupleIn().WithoutEnd().ToSingleString();
            var subjectUnderTest = Subject();
            Assert.That(subjectUnderTest.IsTaskIdList(rawJsonData), Is.False);
        }

        [Test]
        public void ShouldReturnTaskIdArray()
        {
            var rawJsonData = JsonStrings.TaskIdsIn().WithoutEnd().ToSingleString();
            var subjectUnderTest = Subject();
            Assert.That(subjectUnderTest.TaskIds(rawJsonData), 
                Has.Length.EqualTo(3)
                .And.EquivalentTo(new long[]{100,2000,30000}));
        }

        [Test]
        public void ShouldReturnAStormTuple()
        {
            var rawJsonData = JsonStrings.TupleIn().WithoutEnd().ToSingleString();
            var subjectUnderTest = Subject();
            Assert.That(
                subjectUnderTest.Command(rawJsonData),
                Is.TypeOf<StormTuple>()
                .And.Property("TupleId").EqualTo(-6955786537413359385)
                .And.Property("Component").EqualTo("1")
                .And.Property("Stream").EqualTo("2")
                .And.Property("Task").EqualTo(9)
                .And.Property("Tuple").EquivalentTo(
                    new[] {"snow white and the seven dwarfs", "field2", "3" }));
        }

        [Test]
        public void ShouldReturnAStormNext()
        {
            var rawJsonData = JsonStrings.CommandNextIn().WithoutEnd().ToSingleString();
            var subjectUnderTest = Subject();
            Assert.That(
                subjectUnderTest.Command(rawJsonData),
                Is.TypeOf<StormNext>());
        }

        [Test]
        public void ShouldReturnAStormAcknowledge()
        {
            var rawJsonData = JsonStrings.CommandAckIn().WithoutEnd().ToSingleString();
            var subjectUnderTest = Subject();
            Assert.That(
                subjectUnderTest.Command(rawJsonData),
                Is.TypeOf<StormAcknowledge>()
                .And.Property("TupleId").EqualTo(1231231));
        }

        [Test]
        public void ShouldReturnAStormFail()
        {
            var rawJsonData = JsonStrings.CommandFailIn().WithoutEnd().ToSingleString();
            var subjectUnderTest = Subject();
            Assert.That(
                subjectUnderTest.Command(rawJsonData),
                Is.TypeOf<StormFail>()
                .And.Property("TupleId").EqualTo(1231231));
        }
    }
}