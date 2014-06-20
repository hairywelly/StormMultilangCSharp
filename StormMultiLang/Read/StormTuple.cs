using ServiceStack.Text.Json;

namespace StormMultiLang.Read
{
    public struct StormTuple : IStormCommandIn
    {
        public long TupleId { get; set; }
        public string Component { get; set; }
        public string Stream { get; set; }
        public long Task { get; set; }
        public string[] Tuple { get; set; }
        
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
            return (T) JsonReader<T>.Parse(Tuple[index]);
        }

        private bool InvalidInputOrBadTuple(int index)
        {
            return Tuple == null || index < 0 || index >= Tuple.Length || string.IsNullOrEmpty(Tuple[index]);
        }
    }
}