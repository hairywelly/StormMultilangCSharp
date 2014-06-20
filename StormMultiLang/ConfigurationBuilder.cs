using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLang
{
    /// <summary>
    /// Bootstrap the spout or bold stuff 
    /// </summary>
    public class StrormConfigurationBuilder
    {
        private readonly IProtocolWriterFormat _writerFormat = new JsonProtocolWriterFormat();
        private readonly IProtocolReaderFormat _readerFormat;
        private readonly IWriteNext _outputToParent;
        private readonly IReadNext _readNext;

        public StrormConfigurationBuilder()
        {
            ICommunication communication = new StdInOutCommunication();
            IOsSpecific osStuff = new WindowsPidStuff();
            _outputToParent = new WriteStringWithEndTerminator(communication);
            _readerFormat = new JsonProtocolReaderFormat(new WritePidFileAndSendIdToParent(_writerFormat, osStuff, _outputToParent));
            _readNext = new ReadUntillEndRecieved(communication);
        }

        public StrormConfiguration HandleTaskIds()
        {
            return new StrormConfiguration(
                new StormReader(_readNext, _readerFormat),_outputToParent,_writerFormat);
        }

        public StrormConfiguration DontBotherWithTaksIds()
        {
            return new StrormConfiguration(
                new StormReaderNotBotheredAboutTaskIds(_readNext, _readerFormat), _outputToParent, _writerFormat);
        }
    }
}