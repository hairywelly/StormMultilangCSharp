namespace StormMultiLang.Read
{
    public interface IStormCommandIn
    {
        void BeProcessesBy(IBolt bolt);
        void BeProcessesBy(ISpout spout);
    }
}