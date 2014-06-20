using StormMultiLang;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLangTests.TestDoubles
{
    public class TestSpout :Spout
    {
        public TestSpout(IStormReader reader, ISpoutWriter writer) : base(reader, writer){ }

        public override void Initialise(StormHandshake handshake){}

        public override void Next(StormNext stormNext){}
    }
}