namespace StormMultiLang.Write
{
    public class WriteStringWithEndTerminator : IWriteNext
    {
        private readonly ICommunication _lineWriter;

        public WriteStringWithEndTerminator(ICommunication lineWriter)
        {
            _lineWriter = lineWriter;
        }

        public void Write(string thingToWrite)
        {
            _lineWriter.WriteLine(thingToWrite);
            _lineWriter.WriteLine(WellKnownStrings.End);
            _lineWriter.Flush();
        }

        public void AlternativeErrorStream(string error)
        {
            _lineWriter.Error(error);
        }
    }
}