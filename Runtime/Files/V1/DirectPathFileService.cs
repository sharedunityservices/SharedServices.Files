using System.IO;
using UnityEngine;

namespace SharedServices.Files.V1
{
    public class DirectPathFileService : IFileService
    {
        private static FileMonitorMonoBehaviour _fileMonitorMonoBehaviour;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            _fileMonitorMonoBehaviour = GameObjectUtil.CreateDDOLWith<FileMonitorMonoBehaviour>("FileService");
        }
        
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public FileMonitor StartMonitoring(string path)
        {
            var fileMonitor = new FileMonitor(path);
            _fileMonitorMonoBehaviour.AddFileMonitor(fileMonitor);
            return fileMonitor;
        }
        
        public void StopMonitoring(FileMonitor fileMonitor)
        {
            _fileMonitorMonoBehaviour.RemoveFileMonitor(fileMonitor);
        }

        public T ReadJson<T>(string path)
        {
            var json = ReadAllText(path);
            return JsonUtil.FromJson<T>(json);
        }
    }
}