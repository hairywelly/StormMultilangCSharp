

namespace StormMultiLang.Read
{
    public interface IProtocolReaderFormat
    {
        StormHandshake Handshake(string handshakeRaw);

        bool IsTaskIdList(string rawStuff);

        long[] TaskIds(string rawStuff);

        IStormCommandIn Command(string rawStuff);

        T Get<T>(object toBePArsed);
    }
}