using StormMultiLang;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLangTests.TestDoubles
{
    public class TestBolt : Bolt
    {
        public TestBolt(IStormReader reader, IBoltWriter writer) : base(reader, writer){}

        public override void Initialise(StormHandshake stormHandshake){}

        public override void Process(StormTuple stormTuple){}
    }
}