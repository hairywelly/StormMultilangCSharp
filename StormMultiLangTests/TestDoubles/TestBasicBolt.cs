using StormMultiLang;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLangTests.TestDoubles
{
    public class TestBasicBolt : BasicBolt
    {
        private readonly object[] _tupleToReturn;

        public TestBasicBolt(IStormReader reader, IBoltWriter writer, object[] tupleToReturn) : base(reader, writer)
        {
            _tupleToReturn = tupleToReturn;
        }

        public override void Initialise(StormHandshake stormHandshake){}

        protected override void BasicProcess(StormTuple stormTuple)
        {
            BasicEmit(_tupleToReturn);
        }
    }
}