using System;
using System.IO;
using System.Text;

namespace StormMultiLang.Read
{
    public class ReadUntillEndRecieved : IReadNext
    {
        private readonly ICommunication _lineReader;

        public ReadUntillEndRecieved(ICommunication lineReader)
        {
            _lineReader = lineReader;
        }

        public string Next()
        {
            var errorLineCount = 0;
            var nextThing = new StringBuilder();
            while (true)
            {
                var line = _lineReader.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    errorLineCount++;
                }
                else if (line != WellKnownStrings.End)
                {
                    nextThing.AppendLine(line);
                }
                else
                {
                    break;
                }

                if (errorLineCount >= 10)
                {
                    throw new InvalidDataException("Invalid Lines read from input stream");
                }
            }
            return nextThing.ToString(0,nextThing.Length-2);
        }
    }
}