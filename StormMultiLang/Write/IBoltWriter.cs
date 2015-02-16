namespace StormMultiLang.Write
{
    public interface IBoltWriter
    {
        void Acknowledge(long tupleId);
        void Fail(long tupleId);
        void Sync();
        void LogInfo(string infoMessage);
        void LogError(string errorMessage);
        
        long[] EmitTuple(object[] tuple, long[] anchors, string streamId = null);
        void EmitTupleDirect(object[] tuple, long[] anchors, long taskId, string streamId = null);
    }
}