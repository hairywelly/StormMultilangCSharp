namespace StormMultiLang.Read
{
    public struct StormFail : IStormCommandIn
    {
        public void BeProcessesBy(IBolt bolt)
        {
            throw new System.NotImplementedException();
        }

        public void BeProcessesBy(ISpout spout)
        {
            spout.Fail(this);
        }

        public long TupleId { get; set; }
    }
}