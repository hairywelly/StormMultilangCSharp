using System.Collections.Generic;
using StormMultiLang.Write;

namespace StormMultiLang.Read
{
    public class StormHandshake : IStormCommandIn
    {
        private readonly ISetupProcess _processSetup;

        public StormHandshake(ISetupProcess processSetup)
        {
            _processSetup = processSetup;
        }

        public Dictionary<string, string> Conf { get; set; }
        public Dictionary<string, string> TaskComponent { get; set; }
        public long TaskId { get; set; }
        public string PidDir{get; set; }
        
        public void BeProcessesBy(IBolt bolt)
        {
            _processSetup.Setup(PidDir);
            bolt.Initialise(this);
        }

        public void BeProcessesBy(ISpout spout)
        {
            _processSetup.Setup(PidDir);
            spout.Initialise(this);
        }
    }
}