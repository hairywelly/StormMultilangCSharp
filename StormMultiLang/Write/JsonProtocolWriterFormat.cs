using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using ServiceStack;
using ServiceStack.Text;

namespace StormMultiLang.Write
{
    public class JsonProtocolWriterFormat : IProtocolWriterFormat
    {
        public string ProcessId(long pid)
        {
            return new Dictionary<string, long>
            {
                {WellKnownStrings.ProcessId, pid}
            }.ToJson();
        }
        
        public string Sync()
        {
            return new Dictionary<string, string>
            {
                {WellKnownStrings.Command, WellKnownStrings.Synchronize}
            }.ToJson();
        }

        public string LogInfo(string logMessage)
        {
            return new Dictionary<string, string>
            {
                {WellKnownStrings.Command, WellKnownStrings.Log},
                {WellKnownStrings.Message, logMessage},
            }.ToJson();
        }

        public string LogError(string errorMessage)
        {
            return new Dictionary<string, string>
            {
                {WellKnownStrings.Command, WellKnownStrings.Error},
                {WellKnownStrings.Message, errorMessage},
            }.ToJson();
        }

        public string Acknowledge(long tupleId)
        {
            return new Dictionary<string, string>
            {
                {WellKnownStrings.Command, WellKnownStrings.Acknowledge},
                {WellKnownStrings.Id, tupleId.ToString(CultureInfo.InvariantCulture)},
            }.ToJson();
        }

        public string Fail(long tupleId)
        {
            return new Dictionary<string, string>
            {
                {WellKnownStrings.Command, WellKnownStrings.Fail},
                {WellKnownStrings.Id, tupleId.ToString(CultureInfo.InvariantCulture)},
            }.ToJson();
        }

        public string EmitCommand(object[] tupleValues, long[] anchors, long? taskId = null, string streamid = null)
        {
            var result = new OrderedDictionary
            {
                {WellKnownStrings.Command, WellKnownStrings.Emit},
                {WellKnownStrings.Anchors, anchors.Select(_=>_.ToString(CultureInfo.InvariantCulture))},
            };

            if (!string.IsNullOrEmpty(streamid))
            {
                result.Add(WellKnownStrings.Stream, streamid);
            }

            if (taskId.HasValue)
            {
                result.Add(WellKnownStrings.Task, taskId.Value);
            }

            result.Add(WellKnownStrings.Tuple, tupleValues);
            return result.ToJson();
        }

        public string EmitCommand(object[] tupleValues, long? tupleId= null, long? taskId = null, string streamid = null)
        {
            var result = new OrderedDictionary
            {
                {WellKnownStrings.Command, WellKnownStrings.Emit},
            };

            if (tupleId.HasValue)
            {
                result.Add(WellKnownStrings.Id, tupleId.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (!string.IsNullOrEmpty(streamid))
            {
                result.Add(WellKnownStrings.Stream, streamid);
            }

            if (taskId.HasValue)
            {
                result.Add(WellKnownStrings.Task, taskId.Value);
            }

            result.Add(WellKnownStrings.Tuple, tupleValues);
            return result.ToJson();
        }
    }
}