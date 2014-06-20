namespace StormMultiLang.Read
{
    public struct StormAcknowledge : IStormCommandIn
    {
        public void BeProcessesBy(IBolt bolt)
        {
            throw new System.NotImplementedException();
        }

        public void BeProcessesBy(ISpout spout)
        {
            spout.Acknowledge(this);
        }

        public long TupleId { get; set; }
    }
}