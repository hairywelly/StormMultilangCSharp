using System;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLang
{
    public abstract class Bolt : IBolt
    {
        private readonly IStormReader _reader;
        private readonly IBoltWriter _writer;

        protected Bolt(IStormReader reader, IBoltWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public IBoltWriter Writer
        {
            get { return _writer; }
        }

        public abstract void Initialise(StormHandshake stormHandshake);
        public abstract void Process(StormTuple stormTuple);

        public void Run()
        {
            var handshake = _reader.ReadInitialHandshakeMessage();
            handshake.BeProcessesBy(this);
            try
            {
                while (true)
                {
                    var command = _reader.ReadCommand();
                    command.BeProcessesBy(this);
                }
            }
            catch (Exception e)
            {
                _writer.LogError(e.ToString());
            }
        }
    }
}