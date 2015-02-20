using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLang
{
    public abstract class BasicBolt : Bolt
    {
        private long _currentTupleId;
        protected BasicBolt(IStormReader reader, IBoltWriter writer) : base(reader, writer)
        {}

        public override void Process(StormTuple stormTuple)
        {
            _currentTupleId = stormTuple.TupleId;
            BasicProcess(stormTuple);
            Writer.Acknowledge(stormTuple.TupleId);
        }

        public override void Sync(StormHeartBeat stormHeartBeat)
        {
            Writer.Sync();
        }

        protected void BasicEmit(object[] tuple)
        {
            Writer.EmitTuple(tuple, new[] {_currentTupleId});
        }

        protected abstract void BasicProcess(StormTuple stormTuple);
    }
}