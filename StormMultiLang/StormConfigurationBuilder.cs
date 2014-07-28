using System;
using System.Runtime.InteropServices;
using StormMultiLang.Read;
using StormMultiLang.Write;

namespace StormMultiLang
{
    /// <summary>
    /// Bootstrap the spout or bold stuff 
    /// </summary>
    public class StormConfigurationBuilder
    {
        [Flags]
        internal enum ErrorModes : uint 
        {
            SYSTEM_DEFAULT = 0x0,
            SEM_FAILCRITICALERRORS = 0x0001,
            SEM_NOALIGNMENTFAULTEXCEPT = 0x0004,
            SEM_NOGPFAULTERRORBOX = 0x0002,
            SEM_NOOPENFILEERRORBOX = 0x8000
        }
        [DllImport("kernel32.dll")]
        internal static extern ErrorModes SetErrorMode(ErrorModes mode);

        [DllImport("kernel32.dll")]
        static extern FilterDelegate SetUnhandledExceptionFilter(FilterDelegate lpTopLevelExceptionFilter);
        public delegate bool FilterDelegate(Exception ex);
 
        private readonly IProtocolWriterFormat _writerFormat = new JsonProtocolWriterFormat();
        private readonly IProtocolReaderFormat _readerFormat;
        private readonly IWriteNext _outputToParent;
        private readonly IReadNext _readNext;

        public StormConfigurationBuilder()
        {
            RemoveCrashReportPopup();

            ICommunication communication = new StdInOutCommunication();
            IOsSpecific osStuff = new WindowsPidStuff();
            _outputToParent = new WriteStringWithEndTerminator(communication);
            _readerFormat = new JsonProtocolReaderFormat(new WritePidFileAndSendIdToParent(_writerFormat, osStuff, _outputToParent));
            _readNext = new ReadUntillEndRecieved(communication);
        }

        private static void RemoveCrashReportPopup()
        {
            SetUnhandledExceptionFilter(ex => true);
            SetErrorMode(SetErrorMode(0) |
                         ErrorModes.SEM_NOGPFAULTERRORBOX |
                         ErrorModes.SEM_FAILCRITICALERRORS |
                         ErrorModes.SEM_NOOPENFILEERRORBOX);
        }

        public StormConfiguration HandleTaskIds()
        {
            return new StormConfiguration(
                new StormReader(_readNext, _readerFormat),_outputToParent,_writerFormat);
        }

        public StormConfiguration DontBotherWithTaskIds()
        {
            return new StormConfiguration(
                new StormReaderNotBotheredAboutTaskIds(_readNext, _readerFormat), _outputToParent, _writerFormat);
        }
    }
}