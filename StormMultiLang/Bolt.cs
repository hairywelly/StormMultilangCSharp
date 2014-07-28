using System;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLang
{
    public abstract class Bolt : IBolt
    {
        private volatile Exception _exception;
        private volatile bool _keepRunning = true;
        private readonly IStormReader _reader;
        private readonly IBoltWriter _writer;

        protected Bolt(IStormReader reader, IBoltWriter writer)
        {
            _reader = reader;
            _writer = writer;

            AppDomain.CurrentDomain.UnhandledException += (sender , exception) =>
            {
                _keepRunning = false;
                _exception = (Exception)exception.ExceptionObject;
            };
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
                while (_keepRunning)
                {
                    var command = _reader.ReadCommand();
                    command.BeProcessesBy(this);
                }
                if (_exception != null)
                {
                    OnException(_exception);
                }
            }
            catch (Exception e)
            {
                _writer.LogError(e.ToString());
                OnException(e);
            }
        }

        public virtual void OnException(Exception exception){}
    }
}