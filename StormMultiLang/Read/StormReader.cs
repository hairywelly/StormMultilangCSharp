using System.Collections.Generic;
using System.Linq;

namespace StormMultiLang.Read
{
    public class StormReader : IStormReader
    {
        private readonly IReadNext _reader;
        private readonly IProtocolReaderFormat _readerFormat;
        private readonly Queue<IStormCommandIn> _commands = new Queue<IStormCommandIn>();
        private readonly Queue<long[]> _taskIds = new Queue<long[]>();

        public StormReader(
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
            if (_commands.Any())
            {
                return _commands.Dequeue();
            }
            var nextThing = _reader.Next();
            while (_readerFormat.IsTaskIdList(nextThing))
            {
                _taskIds.Enqueue(_readerFormat.TaskIds(nextThing));
                nextThing = _reader.Next();
            }
            return _readerFormat.Command(nextThing);
        }

        public long[] ReadTaskIds()
        {
            if (_taskIds.Any())
            {
                return _taskIds.Dequeue();
            }
            var nextThing = _reader.Next();
            while (!_readerFormat.IsTaskIdList(nextThing))
            {
                _commands.Enqueue(_readerFormat.Command(nextThing));
                nextThing = _reader.Next();
            }
            return _readerFormat.TaskIds(nextThing);
        }
    }
}