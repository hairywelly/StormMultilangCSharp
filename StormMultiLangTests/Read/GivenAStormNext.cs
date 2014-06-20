using System;
using NSubstitute;
using NUnit.Framework;
using StormMultiLang;
using StormMultiLang.Read;

namespace StormMultiLangTests.Read
{
    [TestFixture]
    public class GivenAStormNext
    {
        [Test]
        public void ShouldBeAbleToHandleASpout()
        {
            var spout = Substitute.For<ISpout>();
            var subjectUnderTest = new StormNext();
            subjectUnderTest.BeProcessesBy(spout);
            spout.Received().Next(subjectUnderTest);
        }

        [Test]
        public void ShouldNotHandleTupleInBolt()
        {
            var subjectUnderTest = new StormNext();
            var bolt = Substitute.For<IBolt>();
            Assert.That(() => subjectUnderTest.BeProcessesBy(bolt),
                Throws.InstanceOf<NotImplementedException>());
        }
    }
}