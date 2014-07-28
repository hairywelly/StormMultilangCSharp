using System;
using System.Collections.Concurrent;
using System.Threading;
using StormMultiLang.Write;

namespace StormMultiLang
{
    public class ThreadSafeBoltWriter : IBoltWriter, IShutDown
    {
        private readonly IBoltWriter _proxy;
        private readonly BlockingCollection<Action> _writerQueue = new BlockingCollection<Action>(); 
        private readonly ManualResetEvent _endWait = new ManualResetEvent(false);

        public ThreadSafeBoltWriter(IBoltWriter proxy)
        {
            _proxy = proxy;
            new Thread(() =>
            {
                try
                {
                    foreach (var action in _writerQueue.GetConsumingEnumerable())
                    {
                        if (action != null)
                        {
                            action();
                        }
                        else {break;}
                    }
                }
                finally
                {
                    _endWait.Set();
                }
                
            }){IsBackground = true}.Start();
        }

        public void Acknowledge(long tupleId)
        {
            _writerQueue.Add(()=> _proxy.Acknowledge(tupleId));
        }

        public void Fail(long tupleId)
        {
            _writerQueue.Add(() => _proxy.Fail(tupleId));
        }

        public void LogInfo(string infoMessage)
        {
            _writerQueue.Add(() => _proxy.LogInfo(infoMessage));
        }

        public void LogError(string errorMessage)
        {
            _writerQueue.Add(() => _proxy.LogError(errorMessage));
        }

        public long[] EmitTuple(object[] tuple, long[] anchors, string streamId = null)
        {
            _writerQueue.Add(() => _proxy.EmitTuple(tuple, anchors, streamId));
            return new long[0];
        }

        public void EmitTupleDirect(object[] tuple, long[] anchors, long taskId, string streamId = null)
        {
            _writerQueue.Add(() => _proxy.EmitTupleDirect(tuple, anchors, taskId, streamId));
        }

        public void Shutdown()
        {
            _writerQueue.Add(null);
            _endWait.WaitOne();
        }
    }
}