namespace StormMultiLang.Read
{
    public struct StormNext : IStormCommandIn
    {
        public void BeProcessesBy(IBolt bolt)
        {
            throw new System.NotImplementedException();
        }

        public void BeProcessesBy(ISpout spout)
        {
            spout.Next(this);
        }
    }
}