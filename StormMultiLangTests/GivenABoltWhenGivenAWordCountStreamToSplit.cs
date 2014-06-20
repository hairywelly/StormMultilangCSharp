using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using ServiceStack.Text;
using StormMultiLang.Read;
using StormMultiLang.Write;
using StormMultiLangTests.TestData;
using StormMultiLangTests.TestDoubles;

namespace StormMultiLangTests
{
    [TestFixture]
    public class GivenABoltWhenGivenAWordCountStreamToSplit
    {
        private ProvidedStreamCommunication _communication;
        private SplitSentence _bolt;
      
        [SetUp]
        public void SetUp()
        {
            var dataSource = new TestBoltDataSource();
            _communication = new ProvidedStreamCommunication(dataSource);

            var writerFormat = new JsonProtocolWriterFormat();
            var outputToParent = new WriteStringWithEndTerminator(_communication);

            var osStuff = Substitute.For<IOsSpecific>();
            osStuff.GetProcessId().Returns(1234);
            var readerFormat = new JsonProtocolReaderFormat(new WritePidFileAndSendIdToParent(writerFormat, osStuff, outputToParent));
            var readNext = new ReadUntillEndRecieved(_communication);
            var stormReader = new StormReader(readNext, readerFormat);
            var stormWriter = new StandardBoltWriter(outputToParent, writerFormat, stormReader);
            _bolt = new SplitSentence(stormReader, stormWriter);
        }

        private IEnumerable<string> ReadAllLines(StreamReader reader)
        {
            var line = reader.ReadLine();
            while (line!=null)
            {
                yield return line;
                line = reader.ReadLine();
            }
        }
        
        [Test]
        public void ShouldSendInitialPidAs1234()
        {
            _bolt.Run();
            var firstLine = new StreamReader(_communication.StreamWithOutput).ReadLine();
            Assert.That(firstLine.TrimStuffForCompare(), Is.EqualTo(JsonStrings.PidOut().TrimStuffForCompare()));
        }

        [Test]
        public void ShouldSendEndAfterEachLine()
        {
            _bolt.Run();
            var allLines = ReadAllLines(new StreamReader(_communication.StreamWithOutput)).ToList();
            var countThatBeginWithEnd = allLines.Count(_ => _ == "end");
            Assert.That(countThatBeginWithEnd, Is.EqualTo(allLines.Count/2));
        }

        [Test]
        public void ShouldSetErrorAtEnd()
        {
            _bolt.Run();
            var allLines = ReadAllLines(new StreamReader(_communication.StreamWithOutput)).ToList();
            var lastButOneLine = allLines[allLines.Count - 2];
            Assert.IsTrue(lastButOneLine.StartsWith(@"{""command"":""error"""));
        }

        [Test]
        public void ShouldDo6EmitsOnSameTupleIdAndThenAckThatTupleId()
        {
            _bolt.Run();
            var firstSetToAck = ReadAllLines(new StreamReader(_communication.StreamWithOutput)).Where(_ => _ != "end").Skip(1).Take(7).ToList();
            var lastCommand = JsonObject.Parse(firstSetToAck.Last());
            var otherCommands = firstSetToAck.Take(6).Select(JsonObject.Parse).ToList();

            Assert.That(otherCommands,
                Has.All.Matches<JsonObject>(
                _ => _["command"] == "emit" && _["anchors"] == "[438480253287684408]"));

            Assert.That(lastCommand["command"], Is.EqualTo("ack"));
            Assert.That(lastCommand["id"], Is.EqualTo("438480253287684408"));
        }
    }
}