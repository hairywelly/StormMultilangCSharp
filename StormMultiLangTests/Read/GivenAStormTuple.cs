using System;
using NSubstitute;
using NUnit.Framework;
using StormMultiLang;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLangTests.Read
{
    [TestFixture]
    public class GivenAStormTuple
    {
        private JsonProtocolReaderFormat JsonReader()
        {
            return new JsonProtocolReaderFormat(
                Substitute.For<ISetupProcess>());
        }

        private StormTuple Subject(object[] tuple)
        {
            return new StormTuple(JsonReader(), 1, "test", "default", 2, tuple);
        }
        
        [Test]
        public void ShouldHandleAIntTuple()
        {
            var subjectUnderTest = Subject(new[] {"fsdfds", "2"});
            Assert.That(subjectUnderTest.Get<int>(1), Is.EqualTo(2));
        }

        [Test]
        public void ShouldReturnDefaultForInvalidIndex()
        {
            var subjectUnderTest = Subject(new[] {"fsdfds", "2"});
            Assert.That(subjectUnderTest.Get<int>(20), Is.EqualTo(0));
        }

        [Test]
        public void ShouldReturnDefaultForNullTuple()
        {
            var subjectUnderTest = Subject(null);
            Assert.That(subjectUnderTest.Get<int>(0), Is.EqualTo(0));
        }

        [Test]
        public void ShouldReturnDefaultForInvalidTupleValue()
        {
            var subjectUnderTest = Subject(new[] { "" });
            Assert.That(subjectUnderTest.Get<int>(0), Is.EqualTo(0));
        }

        [Test]
        public void ShouldThrowExceptionIfCantConvert()
        {
            var subjectUnderTest = Subject(new[] { "fsdfds", "2" });
            Assert.That(
                () => subjectUnderTest.Get<int>(0),
                Throws.InstanceOf<FormatException>());
        }

        [Test]
        public void ShouldNotBeAbleToHandleASpout()
        {
            var spout = Substitute.For<ISpout>();
            var subjectUnderTest = new StormTuple();
            Assert.That(() => subjectUnderTest.BeProcessesBy(spout),
                Throws.InstanceOf<NotImplementedException>());
        }

        [Test]
        public void ShouldHandleTupleInBolt()
        {
            var subjectUnderTest = new StormTuple();

            var bolt = Substitute.For<IBolt>();
           
            subjectUnderTest.BeProcessesBy(bolt);

            bolt.Received().Process(subjectUnderTest);
        }
    }
}