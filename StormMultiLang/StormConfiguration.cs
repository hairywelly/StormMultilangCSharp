using System;
using System.IO;
using StormMultiLang.Logging;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLang
{
    public class StormConfiguration
    {
        private readonly IStormReader _reader;
        private readonly IWriteNext _outputToParent;
        private readonly IProtocolWriterFormat _writerFormat;

        public StormConfiguration(
            IStormReader reader, 
            IWriteNext outputToParent, 
            IProtocolWriterFormat writerFormat)
        {
            _reader = reader;
            _outputToParent = outputToParent;
            _writerFormat = writerFormat;
        }

        public void ExclusiveAccessToStdInAndOut()
        {
            Console.SetOut(new StreamWriter(Stream.Null));
            Console.SetIn(new StreamReader(Stream.Null));
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
            var boltWriter = new StandardBoltWriter(_outputToParent, _writerFormat, _reader);
            SetupLog4NetBoltLogging.UsingWriter(boltWriter);
            return boltWriter;
        }

        public IBoltWriter ThreadSafeBoltWriter()
        {
            var boltWriter = new StandardBoltWriter(_outputToParent, _writerFormat, _reader);
            var result = new ThreadSafeBoltWriter(boltWriter);
            SetupLog4NetBoltLogging.UsingWriter(result);
            return result;
        }
    }
}