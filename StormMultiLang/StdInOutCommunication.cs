using System;
using System.IO;

namespace StormMultiLang
{
    public class StdInOutCommunication : ICommunication
    {
        private readonly TextReader _in = Console.In;
        private readonly TextWriter _out = Console.Out;
        
        public string ReadLine()
        {
            return _in.ReadLine();
        }

        public void WriteLine(string line)
        {
            _out.WriteLine(line);
        }

        public void Flush()
        {
            _out.Flush();
        }

        public void Error(string line)
        {
            Console.Error.WriteLine(line);
            Console.Error.Flush();
        }
    }
}