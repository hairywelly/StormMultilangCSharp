namespace StormMultiLang.Write
{
    public class WritePidFileAndSendIdToParent : ISetupProcess
    {
        private readonly IProtocolWriterFormat _format;
        private readonly IOsSpecific _osSpecific;
        private readonly IWriteNext _writeNext;

        public WritePidFileAndSendIdToParent(
            IProtocolWriterFormat format,
            IOsSpecific osSpecific, 
            IWriteNext writeNext)
        {
            _format = format;
            _osSpecific = osSpecific;
            _writeNext = writeNext;
        }

        public void Setup(string processDirectory)
        {
            var pid = _osSpecific.GetProcessId();
            var lineToSendToParent = _format.ProcessId(pid);
            _writeNext.Write(lineToSendToParent);
            _osSpecific.WritePidFile(pid,processDirectory);
        }
    }
}