namespace StormMultiLang.Write
{
    public interface IOsSpecific
    {
        int GetProcessId();
        void WritePidFile(int pid, string directory);
    }
}