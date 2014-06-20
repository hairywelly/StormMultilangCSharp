using System;
using System.IO;

namespace StormMultiLang
{
    public class StdInOutCommunication : ICommunication
    {
        private static readonly TextReader In = Console.In;
        private static readonly TextWriter Out = Console.Out;
        
        public string ReadLine()
        {
            return In.ReadLine();
        }

        public void WriteLine(string line)
        {
            Out.WriteLine(line);
        }

        public void Flush()
        {
            Out.Flush();
        }

        public void Error(string line)
        {
            Console.Error.WriteLine(line);
            Console.Error.Flush();
        }
    }
}