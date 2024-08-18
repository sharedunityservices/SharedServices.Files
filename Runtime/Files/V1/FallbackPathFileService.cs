using System.IO;
using UnityEngine;

namespace SharedServices.Files.V1
{
    [UnityEngine.Scripting.Preserve]
    public class FallbackPathFileService : IFileService
    {
        private static FileMonitorMonoBehaviour _fileMonitorMonoBehaviour;
        
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
            FindCopyOrCreateFile(path);
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
        
        private void FindCopyOrCreateFile(string path)
        {
            if (ExistsInPersistentDataPath(path)) return;
            // else if (ExistsInAddressable(path)) CopyFromAddressableToPersistentDataPath(path);
            else if (ExistsInResources(path)) CopyFromResourcesToPersistentDataPath(path);
            else CreateEmptyFileInPersistentDataPath(path);
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
        
        private bool ExistsInPersistentDataPath(string path)
        {
            var persistentDataPath = Application.persistentDataPath;
            var fullPath = Path.Combine(persistentDataPath, path);
            return File.Exists(fullPath);
        }
        
        private bool ExistsInAddressable(string path) => throw new System.NotImplementedException();

        private bool ExistsInResources(string path) => GetTextAssetResource(path);
        
        private bool IsAddressableInstalled() => throw new System.NotImplementedException();
        private bool AddressableExists(string path) => throw new System.NotImplementedException();
        private string ReadTextFromAddressable(string path) => throw new System.NotImplementedException();
        
        private TextAsset GetTextAssetResource(string path)
        {
            var textResource = Resources.Load<TextAsset>(path);
            if (textResource)
            {
                return textResource;
            }
            
            var pathWithoutExt = Path.ChangeExtension(path, null);
            textResource = Resources.Load<TextAsset>(pathWithoutExt);
            return textResource;
        }
        
        private void CopyFromResourcesToPersistentDataPath(string path)
        {
            CreateDirectoriesIfNotExists(path);
            var textResource = GetTextAssetResource(path);
            var persistentDataPath = Application.persistentDataPath;
            var fullPath = Path.Combine(persistentDataPath, path);
            File.WriteAllText(fullPath, textResource.text);
        }

        private void CreateEmptyFileInPersistentDataPath(string path)
        {
            var persistentDataPath = Application.persistentDataPath;
            var fullPath = Path.Combine(persistentDataPath, path);
            File.WriteAllText(fullPath, string.Empty);
        }

        private void CreateDirectoriesIfNotExists(string path)
        {
            var persistentDataPath = Application.persistentDataPath;
            var directory = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(directory)) return;
            var fullPath = Path.Combine(persistentDataPath, directory);
            if (!Directory.Exists(fullPath)) 
                Directory.CreateDirectory(fullPath);
        }
    }
}