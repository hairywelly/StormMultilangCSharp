namespace StormMultiLang.Read
{
    public struct StormHeartBeat : IStormCommandIn
    {
        private readonly IProtocolReaderFormat _format;

        public StormHeartBeat(
            IProtocolReaderFormat format,
            long tupleId, 
            string component, 
            string stream, 
            long task, 
            object[] tuple) : this()
        {
            _format = format;
            TupleId = tupleId;
            Component = component;
            Stream = stream;
            Task = task;
            Tuple = tuple;
        }

        public long TupleId { get; private set; }
        public string Component { get; private set; }
        public string Stream { get; private set; }
        public long Task { get; private set; }
        private object[] Tuple { get; set; }

        public void BeProcessesBy(IBolt bolt)
        {
            bolt.Sync(this);
        }

        public void BeProcessesBy(ISpout spout)
        {
            throw new System.NotImplementedException();
        }
    }
}
