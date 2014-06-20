namespace StormMultiLang.Write
{
    public interface IProtocolWriterFormat
    {
        string Sync();
        string ProcessId(long pid);
        string LogInfo(string logMessage);
        string LogError(string errorMessage);
        string Acknowledge(long tupleId);
        string Fail(long tupleId);

        string EmitCommand(object[] tupleValues, long[] anchors, long? taskId = null, string streamid = null);
        string EmitCommand(object[] tupleValues, long? tupleId, long? taskId = null, string streamid = null);
    }
}