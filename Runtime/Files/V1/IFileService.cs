using SharedServices.V1;

namespace SharedServices.Files.V1
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