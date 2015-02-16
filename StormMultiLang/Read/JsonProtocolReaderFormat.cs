using System.Linq;
using ServiceStack.Text;
using ServiceStack.Text.Json;
using StormMultiLang.Write;

namespace StormMultiLang.Read
{
    public class JsonProtocolReaderFormat : IProtocolReaderFormat
    {
        private readonly ISetupProcess _setupProcess;

        public JsonProtocolReaderFormat(ISetupProcess setupProcess)
        {
            JsConfig.DateHandler = JsonDateHandler.ISO8601; 
            JsConfig.AssumeUtc = true;
            _setupProcess = setupProcess;
        }

        public StormHandshake Handshake(string handshakeRaw)
        {
            var jsonObject = JsonObject.Parse(handshakeRaw);

            var result = new StormHandshake(_setupProcess)
            {
                Conf = jsonObject.Object(WellKnownStrings.Configuration),
                PidDir = jsonObject[WellKnownStrings.ProcessDirectory],
            };

            var context = jsonObject.Object(WellKnownStrings.Context);
            if (context != null && context.Count > 0)
            {
                result.TaskComponent = context.Object(WellKnownStrings.TaskComponent);
                result.TaskId = context.Get<long>(WellKnownStrings.TaskId);
            }
            return result;
        }

        public bool IsTaskIdList(string rawStuff)
        {
            return JsonUtils.IsJsArray(rawStuff);
        }

        public long[] TaskIds(string rawStuff)
        {
            return JsonSerializer.DeserializeFromString<long[]>(rawStuff);
        }

        public IStormCommandIn Command(string rawStuff)
        {
            var jsonObject = JsonObject.Parse(rawStuff);
            switch (jsonObject.Get<string>(WellKnownStrings.Command))
            {
                case WellKnownStrings.Next:
                    return Next(jsonObject);
                case WellKnownStrings.Acknowledge:
                    return Acknowledge(jsonObject);
                case WellKnownStrings.Fail:
                    return Fail(jsonObject);    
            }

            if (jsonObject.Get<string>(WellKnownStrings.Stream) == WellKnownStrings.HeartBeat)
            {
                return HeartBeat(jsonObject);
            }

            return Tuple(jsonObject);
        }

        public T Get<T>(object toBeParsed)
        {
            var asString = toBeParsed as string;
            if (!string.IsNullOrEmpty(asString))
            {
                return (T)JsonReader<T>.Parse(toBeParsed as string);
            }
            return default(T);
        }

        private IStormCommandIn Acknowledge(JsonObject json)
        {
            return new StormAcknowledge
            {
                TupleId = json.Get<long>(WellKnownStrings.Id)
            };
        }

        private IStormCommandIn Fail(JsonObject json)
        {
            return new StormFail
            {
                TupleId = json.Get<long>(WellKnownStrings.Id)
            };
        }

        private IStormCommandIn Next(JsonObject json)
        {
            return new StormNext();
        }

        private IStormCommandIn Tuple(JsonObject json)
        {
            var result = new StormTuple(
                this,
                json.Get<long>(WellKnownStrings.Id),
                json.Get<string>(WellKnownStrings.Component),
                json.Get<string>(WellKnownStrings.Stream),
                json.Get<long>(WellKnownStrings.Task),
                json.Get<string[]>(WellKnownStrings.Tuple).Cast<object>().ToArray());
           
            return result;
        }

        private IStormCommandIn HeartBeat(JsonObject json)
        {
            var result = new StormHeartBeat(
                this,
                json.Get<long>(WellKnownStrings.Id),
                json.Get<string>(WellKnownStrings.Component),
                json.Get<string>(WellKnownStrings.Stream),
                json.Get<long>(WellKnownStrings.Task),
                json.Get<string[]>(WellKnownStrings.Tuple).Cast<object>().ToArray());

            return result;
        }
    }
}