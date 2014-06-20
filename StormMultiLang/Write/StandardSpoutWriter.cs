using StormMultiLang.Read;

namespace StormMultiLang.Write
{
    public class StandardSpoutWriter :ISpoutWriter
    {
        private readonly IWriteNext _writeNext;
        private readonly IProtocolWriterFormat _format;
        private readonly IStormReader _reader;
        
        public StandardSpoutWriter(
            IWriteNext writeNext, 
            IProtocolWriterFormat format, 
            IStormReader reader)
        {
            _writeNext = writeNext;
            _format = format;
            _reader = reader;
        }

        public void Sync()
        {
            var lineToSend = _format.Sync();
            _writeNext.Write(lineToSend);
        }

        public void LogInfo(string infoMessage)
        {
            var toSend = _format.LogInfo(infoMessage);
            _writeNext.Write(toSend);
        }

        public void LogError(string errorMessage)
        {
            _writeNext.AlternativeErrorStream(errorMessage);
        }

        public long[] EmitTuple(object[] tuple, long? id = null, string streamId = null)
        {
            var toSend = _format.EmitCommand(tuple, id, null, streamId);
            _writeNext.Write(toSend);
            return _reader.ReadTaskIds();
        }

        public void EmitTupleDirect(object[] tuple, long taskId, long? id = null, string streamId = null)
        {
            var toSend = _format.EmitCommand(tuple, id, taskId, streamId);
            _writeNext.Write(toSend);
        }
    }
}