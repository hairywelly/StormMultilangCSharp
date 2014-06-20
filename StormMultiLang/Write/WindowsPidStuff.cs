using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace StormMultiLang.Write
{
    public class WindowsPidStuff : IOsSpecific
    {
        public int GetProcessId()
        {
            return Process.GetCurrentProcess().Id;
        }

        public void WritePidFile(int pid, string directory)
        {
            var pidFile = Path.Combine(directory, pid.ToString(CultureInfo.InvariantCulture));
            File.Create(pidFile);
        }
    }
}