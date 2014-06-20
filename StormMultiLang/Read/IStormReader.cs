namespace StormMultiLang.Read
{
    public interface IStormReader
    {
        IStormCommandIn ReadInitialHandshakeMessage();
        IStormCommandIn ReadCommand();
        long[] ReadTaskIds();
    }
}