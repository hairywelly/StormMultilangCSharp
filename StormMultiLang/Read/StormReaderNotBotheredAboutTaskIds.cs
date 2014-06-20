namespace StormMultiLang.Read
{
    public class StormReaderNotBotheredAboutTaskIds : IStormReader
    {
        private readonly IReadNext _reader;
        private readonly IProtocolReaderFormat _readerFormat;

        public StormReaderNotBotheredAboutTaskIds(
            IReadNext reader,
            IProtocolReaderFormat readerFormat)
        {
            _reader = reader;
            _readerFormat = readerFormat;
        }
        
        public IStormCommandIn ReadInitialHandshakeMessage()
        {
            var handshakeRaw = _reader.Next();
            return _readerFormat.Handshake(handshakeRaw);
        }

        public IStormCommandIn ReadCommand()
        {
            var nextThing = _reader.Next();
            while (_readerFormat.IsTaskIdList(nextThing))
            {
                nextThing = _reader.Next();
            }
            return _readerFormat.Command(nextThing);
        }

        public long[] ReadTaskIds()
        {
            return new long[0];
        }
    }
}