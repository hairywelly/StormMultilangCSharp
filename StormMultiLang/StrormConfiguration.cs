using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLang
{
    public class StrormConfiguration
    {
        private readonly IStormReader _reader;
        private readonly IWriteNext _outputToParent;
        private readonly IProtocolWriterFormat _writerFormat;

        public StrormConfiguration(
            IStormReader reader, 
            IWriteNext outputToParent, 
            IProtocolWriterFormat writerFormat)
        {
            _reader = reader;
            _outputToParent = outputToParent;
            _writerFormat = writerFormat;
        }

        public IStormReader Reader()
        {
            return _reader;
        }

        public ISpoutWriter SpoutWriter()
        {
            return new StandardSpoutWriter(_outputToParent, _writerFormat, _reader);
        }

        public IBoltWriter BoltWriter()
        {
            return new StandardBoltWriter(_outputToParent, _writerFormat, _reader);
        }
    }
}