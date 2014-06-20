
using System.Diagnostics;
using System.IO;
using System.Text;

namespace StormMultiLang.Read
{
    public class ReadUntillEndRecieved : IReadNext
    {
        private readonly ICommunication _lineReader;
        private TextWriter _loggingFile;

        public ReadUntillEndRecieved(ICommunication lineReader)
        {
            _lineReader = lineReader;
            _loggingFile = File.CreateText("C:\\Temp\\" + Process.GetCurrentProcess().Id + "log.txt");
        }

        public string Next()
        {
            var nextThing = new StringBuilder();
            
            while (true)
            {
                var line = _lineReader.ReadLine();
                if (line != WellKnownStrings.End)
                {
                    nextThing.AppendLine(line);
                }
                else
                {
                    break;
                }
            }
            //_loggingFile.WriteLine(nextThing.ToString(0, nextThing.Length - 2));
            return nextThing.ToString(0,nextThing.Length-2);
        }
    }
}