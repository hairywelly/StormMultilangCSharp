using StormMultiLang;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace WordCountTest
{
    public class SplitSentence : BasicBolt
    {
        public SplitSentence(IStormReader reader, IBoltWriter writer) : base(reader, writer){}

        public override void Initialise(StormHandshake stormHandshake){}

        protected override void BasicProcess(StormTuple stormTuple)
        {
            var sentence = stormTuple.Get<string>(0);
            foreach (var word in sentence.Split(' '))
            {
                BasicEmit(new object[]{word});
            }
        }
    }
}