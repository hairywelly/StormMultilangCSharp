using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace StormMultiLangTests.TestData
{
    public class TestBoltDataSource
    {
        private readonly TextReader _fileStream;

        public TestBoltDataSource()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var rawStream = assembly.GetManifestResourceStream("StormMultiLangTests.TestData.8400log.txt");
            _fileStream = new StreamReader(rawStream);
        }

        public IEnumerable<string> All()
        {
            var line = _fileStream.ReadLine();
            while (line!= null)
            {
                yield return line;
                line = _fileStream.ReadLine();
            }
        }

        public string NextLine()
        {
            return _fileStream.ReadLine();
        }

        public TextReader FileStream
        {
            get { return _fileStream; }
        }
    }
}