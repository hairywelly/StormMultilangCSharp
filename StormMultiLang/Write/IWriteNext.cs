using System.Security.Cryptography.X509Certificates;

namespace StormMultiLang.Write
{
    public interface IWriteNext
    {
        void Write(string thingToWrite);

        void AlternativeErrorStream(string error);
    }
}