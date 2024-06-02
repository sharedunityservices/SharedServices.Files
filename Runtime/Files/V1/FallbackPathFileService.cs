using System.IO;
using UnityEngine;

namespace SharedServices.Files.V1
{
    public class FallbackPathFileService : IFileService
    {
        private static FileMonitorMonoBehaviour _fileMonitorMonoBehaviour;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            _fileMonitorMonoBehaviour = GameObjectUtil.CreateDDOLWith<FileMonitorMonoBehaviour>("FileService");
        }
        
        public string ReadAllText(string path)
        {
            return ReadTextFromFallbackSources(path);
        }

        public void WriteAllText(string path, string contents)
        {
            var persistentDataPath = Application.persistentDataPath;
            path = Path.Combine(persistentDataPath, path);
            File.WriteAllText(path, contents);
        }

        public FileMonitor StartMonitoring(string path)
        {
            var persistentDataPath = Application.persistentDataPath;
            path = Path.Combine(persistentDataPath, path);
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
        
        public void WriteJson<T>(string path, T obj)
        {
            var persistentDataPath = Application.persistentDataPath;
            path = Path.Combine(persistentDataPath, path);
            var json = JsonUtil.ToJson(obj);
            WriteAllText(path, json);
        }

        private string ReadTextFromFallbackSources(string path)
        {
            var persistentDataPath = Application.persistentDataPath;
            if (File.Exists(persistentDataPath))
            {
                return File.ReadAllText(persistentDataPath);
            }

            // if (IsAddressableInstalled() && AddressableExists(path))
            // {
            //     return ReadTextFromAddressable(path); 
            // }

            var textResource = Resources.Load<TextAsset>(path);
            if (textResource)
            {
                return textResource.text;
            }
            
            var pathWithoutExt = Path.ChangeExtension(path, null);
            textResource = Resources.Load<TextAsset>(pathWithoutExt);
            if (textResource)
            {
                return textResource.text;
            }
            
            return null;
        }
    }
}