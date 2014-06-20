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
            return nextThing.ToString(0,nextThing.Length-2);
        }
    }
}