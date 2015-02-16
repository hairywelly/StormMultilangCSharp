using StormMultiLang.Read;

namespace StormMultiLang.Write
{
    public class StandardBoltWriter : IBoltWriter
    {
        private readonly IWriteNext _writeNext;
        private readonly IStormReader _stormReader;
        private readonly IProtocolWriterFormat _format;

        public StandardBoltWriter(
            IWriteNext writeNext, 
            IProtocolWriterFormat format, 
            IStormReader stormReader)
        {
            _writeNext = writeNext;
            _format = format;
            _stormReader = stormReader;
        }

        public void Acknowledge(long tupleId)
        {
            var toSend = _format.Acknowledge(tupleId);
            _writeNext.Write(toSend);
        }

        public void Fail(long tupleId)
        {
            var toSend = _format.Fail(tupleId);
            _writeNext.Write(toSend);
        }

        public void Sync()
        {
            var toSend = _format.Sync();
            _writeNext.Write(toSend);
        }

        public void LogInfo(string infoMessage)
        {
            var toSend = _format.LogInfo(infoMessage);
            _writeNext.Write(toSend);
        }

        public void LogError(string errorMessage)
        {
            var toSend = _format.LogError(errorMessage);
            _writeNext.Write(toSend);
        }

        public long[] EmitTuple(object[] tuple, long[] anchors, string streamId = null)
        {
            var toSend = _format.EmitCommand(tuple, anchors, null, streamId);
            _writeNext.Write(toSend);
            return _stormReader.ReadTaskIds();
        }

        public void EmitTupleDirect(object[] tuple, long[] anchors, long taskId, string streamId = null)
        {
            var toSend = _format.EmitCommand(tuple, anchors, taskId, streamId);
            _writeNext.Write(toSend);
        }
    }
}