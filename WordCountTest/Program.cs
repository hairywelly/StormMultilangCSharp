using StormMultiLang;

namespace WordCountTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new StrormConfigurationBuilder().DontBotherWithTaskIds();
            var bolt = new SplitSentence(config.Reader(), config.BoltWriter());
            bolt.Run();
        }
    }
}
