using StormMultiLang.Read;

namespace StormMultiLang
{
    public interface ISpout
    {
        void Acknowledge(StormAcknowledge stormAcknowledge);
        void Fail(StormFail stormFail);
        void Next(StormNext stormNext);
        void Initialise(StormHandshake stormHandshake);
    }
}