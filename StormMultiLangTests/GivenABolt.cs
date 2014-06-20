using System;
using NSubstitute;
using NUnit.Framework;
using StormMultiLang.Read;
using StormMultiLang.Write;
using StormMultiLangTests.TestDoubles;

namespace StormMultiLangTests
{
    [TestFixture]
    public class GivenABolt
    {
        [Test]
        public void ShouldInitialiseThenReadCommandThenEndBecauseOfException()
        {
            var handshake = Substitute.For<IStormCommandIn>();
            var command = Substitute.For<IStormCommandIn>();
            
            var reader = Substitute.For<IStormReader>();
            reader.ReadInitialHandshakeMessage().Returns(handshake);
            reader.ReadCommand().Returns(_ => command, _ => { throw new Exception(); });
            
            var writer = Substitute.For<IBoltWriter>();

            var subjectUnderTest = new TestBolt(reader, writer);
            subjectUnderTest.Run();
            
            handshake.Received().BeProcessesBy(subjectUnderTest);
            command.Received().BeProcessesBy(subjectUnderTest);
            writer.ReceivedWithAnyArgs().LogError("");
        }
    }
}