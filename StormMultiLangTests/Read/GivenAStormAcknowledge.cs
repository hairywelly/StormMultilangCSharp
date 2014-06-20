using System;
using NSubstitute;
using NUnit.Framework;
using StormMultiLang;
using StormMultiLang.Read;

namespace StormMultiLangTests.Read
{
    [TestFixture]
    public class GivenAStormAcknowledge
    {
        [Test]
        public void ShouldBeAbleToHandleASpout()
        {
            var spout = Substitute.For<ISpout>();
            var subjectUnderTest = new StormAcknowledge();
            subjectUnderTest.BeProcessesBy(spout);
            spout.Received().Acknowledge(subjectUnderTest);
        }

        [Test]
        public void ShouldNotHandleTupleInBolt()
        {
            var subjectUnderTest = new StormAcknowledge();
            var bolt = Substitute.For<IBolt>();
            Assert.That(() => subjectUnderTest.BeProcessesBy(bolt),
                Throws.InstanceOf<NotImplementedException>());
        }
    }
}