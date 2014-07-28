namespace StormMultiLang.Read
{
    public struct StormTuple : IStormCommandIn
    {
        private readonly IProtocolReaderFormat _format;

        public StormTuple(
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
            bolt.Process(this);
        }

        public void BeProcessesBy(ISpout spout)
        {
            throw new System.NotImplementedException();
        }

        public T Get<T>(int index)
        {
            if (InvalidInputOrBadTuple(index))
            {
                return default(T);
            }
            return _format.Get<T>(Tuple[index]);
        }

        private bool InvalidInputOrBadTuple(int index)
        {
            return Tuple == null || index < 0 || index >= Tuple.Length || Tuple[index] == null;
        }
    }
}