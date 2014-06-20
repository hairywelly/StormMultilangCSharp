using System;
using NSubstitute;
using NUnit.Framework;
using StormMultiLang;
using StormMultiLang.Read;

namespace StormMultiLangTests.Read
{
    [TestFixture]
    public class GivenAStormFail
    {
        [Test]
        public void ShouldBeAbleToHandleASpout()
        {
            var spout = Substitute.For<ISpout>();
            var subjectUnderTest = new StormFail();
            subjectUnderTest.BeProcessesBy(spout);
            spout.Received().Fail(subjectUnderTest);
        }

        [Test]
        public void ShouldNotHandleTupleInBolt()
        {
            var subjectUnderTest = new StormFail();
            var bolt = Substitute.For<IBolt>();
            Assert.That(() => subjectUnderTest.BeProcessesBy(bolt),
                Throws.InstanceOf<NotImplementedException>());
        }
    }
}