using System;
using System.IO;
using StormMultiLang;
using StormMultiLangTests.TestData;

namespace StormMultiLangTests.TestDoubles
{
    public class ProvidedStreamCommunication : ICommunication
    {
        private readonly Stream _captureStream = new MemoryStream();
        
        private readonly TestBoltDataSource _dataSource;
        private readonly TextWriter _writer;

        public ProvidedStreamCommunication(TestBoltDataSource dataSource)
        {
            _writer = new StreamWriter(_captureStream);
            _dataSource = dataSource;
        }

        public string ReadLine()
        {
            var line = _dataSource.NextLine();
            if (line == null)
            {
                throw new Exception("Finished reading");
            }
            return line;
        }

        public void WriteLine(string line)
        {
            _writer.WriteLine(line);
        }

        public void Flush()
        {
            _writer.Flush();
        }

        public void Error(string line)
        {
            throw new System.NotSupportedException("Test doesnt support errors");
        }

        public Stream StreamWithOutput
        {
            get
            {
                _captureStream.Position = 0;
                return _captureStream;
            }
        }
    }
}