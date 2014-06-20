using System;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLang
{
    public abstract class Spout : ISpout
    {
        private readonly IStormReader _reader;
        private readonly ISpoutWriter _writer;

        protected Spout(
            IStormReader reader, 
            ISpoutWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

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
                    _writer.Sync();
                }
            }
            catch (Exception e)
            {
                _writer.LogError(e.ToString());
            }
        }

        public ISpoutWriter Writer
        {
            get { return _writer; }
        }

        public abstract void Initialise(StormHandshake handshake);
        public abstract void Next(StormNext stormNext);
        public virtual void Acknowledge(StormAcknowledge stormAcknowledge){}
        public virtual void Fail(StormFail stormFail){}
    }
}