using SharedServices;
using SharedServices.V1;

namespace Services.Files
{
    public interface IFileService : IService
    {
        string ReadAllText(string path);
        void WriteAllText(string path, string contents);
        FileMonitor StartMonitoring(string path);
        void StopMonitoring(FileMonitor fileMonitor);
        T ReadJson<T>(string path);
    }
}