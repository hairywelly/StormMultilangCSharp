using StormMultiLang.Read;

namespace StormMultiLang
{
    public interface IBolt
    {
        void Process(StormTuple stormTuple);
        void Initialise(StormHandshake stormHandshake);
    }
}