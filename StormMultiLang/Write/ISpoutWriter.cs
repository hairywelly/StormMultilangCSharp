namespace StormMultiLang.Write
{
    public interface ISpoutWriter
    {
        void Sync();
        
        void LogInfo(string infoMessage);
        void LogError(string errorMessage);

        long[] EmitTuple(object[] tuple, long? id = null, string streamId = null);
        void EmitTupleDirect(object[] tuple, long taskId, long? id = null, string streamId = null);
    }
}