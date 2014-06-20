using NSubstitute;
using NUnit.Framework;
using StormMultiLang;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLangTests.Read
{
    [TestFixture]
    public class GivenAStormHandshake
    {
        private const string ProcessDir = @"C:\temp\yay";

        [Test]
        public void ShouldHandleHandshakeInBolt()
        {
            var processSetup = Substitute.For<ISetupProcess>();            
            var subjectUnderTest = new StormHandshake(processSetup) { PidDir = ProcessDir };
            var bolt = Substitute.For<IBolt>();

            subjectUnderTest.BeProcessesBy(bolt);

            bolt.Received().Initialise(subjectUnderTest);
            processSetup.Received().Setup(ProcessDir);
        }

        [Test]
        public void ShouldHandleHandshakeInSpout()
        {
            var processSetup = Substitute.For<ISetupProcess>();
            var subjectUnderTest = new StormHandshake(processSetup) { PidDir = ProcessDir };
            var spout = Substitute.For<ISpout>();

            subjectUnderTest.BeProcessesBy(spout);

            spout.Received().Initialise(subjectUnderTest);
            processSetup.Received().Setup(ProcessDir);
        }
    }
}