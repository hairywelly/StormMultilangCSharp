namespace StormMultiLang
{
    public interface ICommunication
    {
        string ReadLine();
        void WriteLine(string line);
        void Flush();
        void Error(string line);
    }
}