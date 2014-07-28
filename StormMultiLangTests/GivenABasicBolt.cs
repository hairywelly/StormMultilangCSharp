using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using NSubstitute;
using NUnit.Framework;
using StormMultiLang.Read;
using StormMultiLang.Write;
using StormMultiLangTests.TestDoubles;

namespace StormMultiLangTests
{
    [TestFixture]
    public class GivenABasicBolt
    {
        private JsonProtocolReaderFormat JsonReader()
        {
            return new JsonProtocolReaderFormat(
                Substitute.For<ISetupProcess>());
        }

        private StormTuple StormTuple(long tupleId)
        {
            return new StormTuple(JsonReader(), tupleId, "test", "default", 2, null);
        }
        
        [Test]
        public void ShouldInitialiseThenReadCommandThenEndBecauseOfException()
        {
            var tupleToReturn = new object[] {1, 2, 3};
            
            var handshake = Substitute.For<IStormCommandIn>();
            var command = Substitute.For<IStormCommandIn>();
            var reader = Substitute.For<IStormReader>();
            var writer = Substitute.For<IBoltWriter>();
            
            reader.ReadInitialHandshakeMessage().Returns(handshake);
            reader.ReadCommand().Returns(_ => command, _ => { throw new Exception(); });

            var subjectUnderTest = new TestBasicBolt(reader, writer, tupleToReturn);
            command.When(_ => _.BeProcessesBy(subjectUnderTest)).Do(_ => subjectUnderTest.Process(StormTuple(22)));
            subjectUnderTest.Run();
            
            handshake.Received().BeProcessesBy(subjectUnderTest);
            command.Received().BeProcessesBy(subjectUnderTest);
            writer.Received().Acknowledge(22);
            writer.Received().EmitTuple(tupleToReturn, Arg.Is<long[]>(_=>_.SequenceEqual(new long[]{22})));
            writer.ReceivedWithAnyArgs().LogError("");
        }
    }
}