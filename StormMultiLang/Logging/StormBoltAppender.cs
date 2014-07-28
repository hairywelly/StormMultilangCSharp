using log4net.Appender;
using log4net.Core;
using StormMultiLang.Write;

namespace StormMultiLang.Logging
{
    public class StormBoltAppender : AppenderSkeleton
    {
        private readonly IBoltWriter _boltWriter;

        public StormBoltAppender(IBoltWriter boltWriter)
        {
            _boltWriter = boltWriter;
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var message = loggingEvent.RenderedMessage;
            if (Layout != null)
            {
                message = RenderLoggingEvent(loggingEvent);
            }
            
            if (SomeKindOfError(loggingEvent))
            {
                _boltWriter.LogError(message);
            }
            else
            {
                _boltWriter.LogInfo(message);
            }
        }

        private static bool SomeKindOfError(LoggingEvent loggingEvent)
        {
            return loggingEvent.Level == Level.Error || loggingEvent.Level == Level.Critical ||
                   loggingEvent.Level == Level.Emergency;
        }
    }
}