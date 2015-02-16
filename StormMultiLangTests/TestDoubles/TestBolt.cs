using System;
using System.Threading;
using StormMultiLang;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLangTests.TestDoubles
{
    public class TestBolt : Bolt
    {
        private readonly ManualResetEvent _started = new ManualResetEvent(false);
        private bool _exceptionCalled;

        public TestBolt(IStormReader reader, IBoltWriter writer) : base(reader, writer){}

        public ManualResetEvent Started
        {
            get { return _started; }
        }

        public bool ExceptionCalled
        {
            get { return _exceptionCalled; }
        }

        public override void Initialise(StormHandshake stormHandshake){}

        public override void Sync(StormHeartBeat stormHeartBeat){}

        public override void Process(StormTuple stormTuple)
        {
            if (_started.WaitOne(1) == false)
            {
                _started.Set();    
            }
            Thread.Sleep(0);
        }

        public override void OnException(Exception exception)
        {
            _exceptionCalled = true;
        }
    }
}